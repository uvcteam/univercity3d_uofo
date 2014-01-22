using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MiniJSON;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM || UNITY_STANDALONE_WIN
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

public class HTMLUnionHall : MonoBehaviour
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    private CoherentUIView _view;
    private UserManager _userManager;
    private EventManager _eventManager;
    private bool _viewReady;

    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;
		_eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
		_eventManager.RepopulateEvents();
		_view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("GetWeekEvents", (System.Action)GetWeekEvents);
            _view.View.BindCall("PopulateCalendar", (System.Action)PopulateCalendar);
            _view.View.BindCall("CreateEvent", (System.Action<string[]>)CreateEvent);
            _view.View.BindCall("GetEvents", (System.Action<string>)GetEvents);
            _view.View.BindCall("GetCategories", (System.Action)GetCategories);
            _view.View.BindCall("GetMyEvents", (System.Action)GetMyEvents);
            _view.View.BindCall("GetOtherEvents", (System.Action)GetOtherEvents);
            _view.View.BindCall("CancelEvent", (System.Action<string>)CancelEvent);
            _view.View.BindCall("WithdrawEvent", (System.Action<string>)WithdrawEvent);
            _view.View.BindCall("JoinEvent", (System.Action<string>)JoinEvent);
            _view.View.BindCall("GetInvitationEvents", (System.Action)GetInvitationEvents);
        };

        _viewReady = false;
    }

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    IEnumerator SendCreateRequest(string url)
    {
        WWW page = new WWW(url);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> createSuccess = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)createSuccess["s"])
        {
            _view.View.TriggerEvent("CreateSuccess");
            _eventManager.RepopulateEvents();
            NativeDialogs.Instance.ShowMessageBox("Success!", "Event successfully created!",
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
        else
        {
            Debug.Log("There was an error: " + createSuccess["reason"].ToString());
            NativeDialogs.Instance.ShowMessageBox("Error!", createSuccess["reason"].ToString(),
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
    }
    IEnumerator SendCancelRequest(string url)
    {
        WWW page = new WWW(url);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> createSuccess = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)createSuccess["s"])
        {
            _eventManager.RepopulateEvents();
            _view.View.Reload(false);
            NativeDialogs.Instance.ShowMessageBox("Success!", "Event successfully cancelled!",
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
        else
        {
            Debug.Log("There was an error: " + createSuccess["reason"].ToString());
            NativeDialogs.Instance.ShowMessageBox("Error!", createSuccess["reason"].ToString(),
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
    }
    IEnumerator SendJoinRequest(string url, int id)
    {
        WWW page = new WWW(url);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> createSuccess = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)createSuccess["s"])
        {
            _userManager.CurrentUser.AttendedEvents.Add(id);
            _view.View.Reload(false);
            NativeDialogs.Instance.ShowMessageBox("Success!", "Event successfully joined!",
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
        else
        {
            Debug.Log("There was an error: " + createSuccess["reason"].ToString());
            NativeDialogs.Instance.ShowMessageBox("Error!", createSuccess["reason"].ToString(),
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
    }
    IEnumerator SendWithdrawRequest(string url, int id)
    {
        WWW page = new WWW(url);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> createSuccess = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)createSuccess["s"])
        {
            _userManager.CurrentUser.AttendedEvents.Remove(id);
            _view.View.Reload(false);
            NativeDialogs.Instance.ShowMessageBox("Success!", "Event successfully withdrawn from!",
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
        else
        {
            Debug.Log("There was an error: " + createSuccess["reason"].ToString());
            NativeDialogs.Instance.ShowMessageBox("Error!", createSuccess["reason"].ToString(),
                new string[] { "OK" }, false, (string button) =>
                {
                });
        }
    }
    IEnumerator WaitToReport(string ev)
    {
        yield return new WaitForSeconds(0.5f);
        _view.View.TriggerEvent(ev);
    }

    #region CoherentUI Bindings
    public void GetWeekEvents()
    {
        Debug.Log("Getting the week's events!");
        DateTime today = DateTime.Today;
        int dayOfTheWeek = (int) DateTime.Now.DayOfWeek;
        DateTime nextSunday = DateTime.Now.AddDays(7 - dayOfTheWeek).Date;
        bool hasEvents = false;

		if(_eventManager.events != null)
		{
			//foreach (UnionHallEvent ev in _eventManager.events)
            for(int i = 0; i < _eventManager.events.Count; ++i)
            {
                UnionHallEvent ev = _eventManager.events[i];
				if ( ev != null && _userManager.CurrentUser.AttendingEvent(ev.Id))
				{
					if (ev.Start >= today && ev.Start < nextSunday)
					{
						string date = ev.Start.ToString("MMMM dd");
						string time = ev.Start.ToString("hh:mm tt");
						_view.View.TriggerEvent("AddWeekEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
						hasEvents = true;
					}
				}
			}
		}

        if (!hasEvents)
        {
            Debug.Log("No events!");
            StartCoroutine(WaitToReport("NoEvents"));
        }
    }
    public void PopulateCalendar()
    {
        Debug.Log("Adding events.");
        Debug.Log("NUMBER OF EVENTS" + _eventManager.events.Count);
        for(int i = 0; i < _eventManager.events.Count; ++i)
        {
            UnionHallEvent ev = _eventManager.events[i];
			Debug.Log(ev.Start.ToString("MM-dd-yyyy") + ": " + ev.Title);
            _view.View.TriggerEvent("AddEvent", ev.Start.ToString("MM-dd-yyyy"), ev.Title, ev.Start.ToString("hh:mm tt"));
        }
        _view.View.TriggerEvent("EventsFinished");
    }
    public void CreateEvent(string[] inputs)
    {
        string phoneRegEx = @"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$";
        foreach (string s in inputs)
            if (s == "")
            {
                NativeDialogs.Instance.ShowMessageBox("Error!", "All inputs must have a value!",
                    new string[] { "OK" }, false, (string button) =>
                    {
                    });
                return;
            }
        if (!Regex.IsMatch(inputs[7], phoneRegEx))
        {
            NativeDialogs.Instance.ShowMessageBox("Error!", "Invalid phone number! Accepted formats:\n(###) ###-####\n##########\n###-###-####",
                new string[] { "OK" }, false, (string button) =>
                {
                });
            return;
        }

        DateTime start = DateTime.Parse(inputs[4] + " " + inputs[5]);
        Debug.Log(start.ToString("yyyy-MM-dd HH:mm"));

        string createURL = serverURL + "CreateEvent?token=";
        createURL += GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;
        createURL += "&title=" + WWW.EscapeURL(inputs[0]);
        createURL += "&desc=" + WWW.EscapeURL(inputs[2]);
        createURL += "&who=" + WWW.EscapeURL(inputs[1]);
        createURL += "&location=" + WWW.EscapeURL(inputs[3]);
        createURL += "&phone=" + WWW.EscapeURL(inputs[7]);
        createURL += "&min=" + inputs[8];
        createURL += "&max=" + inputs[9];
        createURL += "&start=" + WWW.EscapeURL(
            start.ToString("yyyy-MM-dd HH:mm"));
        createURL += "&end=" + WWW.EscapeURL(
            start.AddHours(1).ToString("yyyy-MM-dd HH:mm"));
        createURL += "&interests=" + _userManager.GetIDForCategory(inputs[6]);

        Debug.Log(createURL);
        StartCoroutine(SendCreateRequest(createURL));
    }
    public void GetEvents(string cat)
    {
        bool hasAddedEvent = false;
        foreach (string key in _eventManager.eventsByCategory.Keys)
            Debug.Log("KEY -- " + key);
        Debug.Log("Getting events for " + cat);

        if (cat == "All Categories" || cat == "")
        {
            //foreach (UnionHallEvent ev in _eventManager.events)
            for(int i = 0; i < _eventManager.events.Count; ++i)
            {
                UnionHallEvent ev = _eventManager.events[i];
                if (ev.Email == _userManager.CurrentUser.Email) continue;
                if (_userManager.CurrentUser.AttendingEvent(ev.Id)) continue;
                string date = ev.Start.ToString("MMMM dd");
                string time = ev.Start.ToString("hh:mm tt");
                Debug.Log("Adding event " + ev.Title + " - " + date + " - " + time + " - " + ev.Desc);
                _view.View.TriggerEvent("CreateEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
                hasAddedEvent = true;
            }
        }
        else if (_eventManager.eventsByCategory.ContainsKey(cat))
        {
            //foreach (UnionHallEvent ev in _eventManager.eventsByCategory[cat])
            for(int i = 0; i < _eventManager.eventsByCategory[cat].Count; ++i)
            {
                UnionHallEvent ev = _eventManager.eventsByCategory[cat][i];
                if (ev.Email == _userManager.CurrentUser.Email) continue;
                if (_userManager.CurrentUser.AttendingEvent(ev.Id)) continue;
                string date = ev.Start.ToString("MMMM dd");
                string time = ev.Start.ToString("hh:mm tt");
                Debug.Log("Adding event " + ev.Title + " - " + date + " - " + time + " - " + ev.Desc);
                _view.View.TriggerEvent("CreateEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
                hasAddedEvent = true;
            }
        }

        if (!hasAddedEvent)
        {
            Debug.Log("No events!");
            _view.View.TriggerEvent("NoEvents");
        }
        return;
    }
    public void GetCategories()
    {
        foreach(SocialInterest si in _userManager.Categories)
            _view.View.TriggerEvent("AddCategory", si.Name);
        _view.View.TriggerEvent("CategoriesFinished");
    }
    public void GetMyEvents()
    {
        Debug.Log("Getting my events.");

        if (_userManager.CurrentUser.LoggedIn)
        {
            //foreach (UnionHallEvent ev in _eventManager.events.Where(x => x.Email == _userManager.CurrentUser.Email))
            for(int i = 0; i < _eventManager.events.Count; ++i)
            {
                UnionHallEvent ev = _eventManager.events[i];
                if (ev.Email != _userManager.CurrentUser.Email) continue;
                string date = ev.Start.ToString("MMMM dd");
                string time = ev.Start.ToString("hh:mm tt");
                Debug.Log("Adding event " + ev.Title + " - " + date + " - " + time + " - " + ev.Desc);
                _view.View.TriggerEvent("CreateMyEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
            }
            return;
        }

        Debug.Log("No events!");
        _view.View.TriggerEvent("NoEvents");
        return;
    }
    public void GetOtherEvents()
    {
        Debug.Log("Getting other events.");

        if (_userManager.CurrentUser.LoggedIn)
        {
            //foreach (UnionHallEvent ev in _eventManager.events.Where(x => _userManager.CurrentUser.AttendingEvent(x.Id)))
            for (int i = 0; i < _eventManager.events.Count; ++i)
            {
                UnionHallEvent ev = _eventManager.events[i];
                if (!_userManager.CurrentUser.AttendingEvent(ev.Id)) continue;
                string date = ev.Start.ToString("MMMM dd");
                string time = ev.Start.ToString("hh:mm tt");
                Debug.Log("Adding event " + ev.Title + " - " + date + " - " + time + " - " + ev.Desc);
                _view.View.TriggerEvent("CreateOtherEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
            }
            return;
        }

        Debug.Log("No events!");
        _view.View.TriggerEvent("NoEvents");
        return;
    }
    public void CancelEvent(string id)
    {
        Debug.Log("Cancel event " + id);
        string cancelURL = serverURL + "CancelEvent?token=";
        cancelURL += _userManager.CurrentUser.Token;
        cancelURL += "&id=" + id;

        StartCoroutine(SendCancelRequest(cancelURL));
    }
    public void WithdrawEvent(string id)
    {
        Debug.Log("Withdraw from event " + id);
        string withdrawURL = serverURL + "WithdrawFromEvent?token=";
        withdrawURL += _userManager.CurrentUser.Token;
        withdrawURL += "&id=" + id;

        StartCoroutine(SendWithdrawRequest(withdrawURL, Convert.ToInt32(id)));
    }
    public void JoinEvent(string id)
    {
        Debug.Log("Join event " + id);
        string joinURL = serverURL + "AttendEvent?token=";
        joinURL += _userManager.CurrentUser.Token;
        joinURL += "&id=" + id;

        StartCoroutine(SendJoinRequest(joinURL, Convert.ToInt32(id)));
    }

    public void GetInvitationEvents()
    {
        if (_eventManager.events != null)
        {
            //foreach (UnionHallEvent ev in _eventManager.events)
            for (int i = 0; i < _userManager.CurrentUser.EventInvitations.Count; ++i)
            {
                UnionHallEvent ev = _eventManager.GetEventForId(_userManager.CurrentUser.EventInvitations[i]);
                if (_userManager.CurrentUser.AttendingEvent(ev.Id)) continue;
                string date = ev.Start.ToString("MMMM dd");
                string time = ev.Start.ToString("hh:mm tt");
                _view.View.TriggerEvent("AddEvent", ev.Title, date, time, ev.Desc, ev.Who, ev.Loc, ev.Id);
            }
        }
    }
    #endregion
}