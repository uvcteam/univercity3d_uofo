using System;
using System.Collections.Generic;
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

public class HTMLMemoryBank : MonoBehaviour
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif

    private CoherentUIView _view;
    private UserManager _userManager;
    private bool _viewReady;
    private const string FacebookAppURL = "http://www.univercity3d.com/mobileapp.html";
    private string LocalAppURL = "";

    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        LocalAppURL = _view.Page;
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("RequestUsername", (System.Action)RequestUsername);
            _view.View.BindCall("CheckPin", (System.Action<string>)OnJournalClicked);
            _view.View.BindCall("OnSaveEntryClicked", (System.Action<string, string>)OnSaveEntryClicked);
			_view.View.BindCall("DeleteEntry", (System.Action<string>)DeleteEntry);
            _view.View.BindCall("GetCategories", (System.Action)GetCategories);
            _view.View.BindCall("UpdateCategories", (System.Action<string>)UpdateCategories);
            _view.View.BindCall("SignOut", (System.Action)SignOut);
        };
        
        _viewReady = false;
    }
    
    IEnumerator CheckPIN(string pin)
    {
        NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Checking PIN", false, false);
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        string journalURL = serverURL + "ListJournal?";
        journalURL += "token=" + manager.GetComponent<UserManager>().CurrentUser.Token;
        journalURL += "&pin=" + pin;
        journalURL += "&start=" + 0;
        journalURL += "&count=" + 50;

        WWW page = new WWW(journalURL);
        yield return page;

        NativeDialogs.Instance.HideProgressDialog();

        Dictionary<string, object> result = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (!Convert.ToBoolean(result["s"]))
        {
            NativeDialogs.Instance.ShowMessageBox("Invalid PIN", "The PIN you entered was incorrect.", new string[] { "OK" }, false,
                (string button) => { });
        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Loading journal entries.", false, false);
            _view.View.TriggerEvent("PinCorrect");
            _userManager.CurrentUser.PIN = pin;
            _userManager.CurrentUser.PopulateJournal(
                result["entries"] as List<object>);

            foreach (JournalEntry entry in _userManager.CurrentUser.Journals)
            {
                Debug.Log("Adding entry " + entry.Id + " " + entry.Title + " " + entry.TimeStamp.ToString("MMMM dd, yyyy") + " " + entry.Entry);
                _view.View.TriggerEvent("AddJournal", entry.Id, entry.Title, entry.TimeStamp.ToString("MMMM dd, yyyy"), entry.Entry);
            }
			
			_view.View.TriggerEvent("JournalsFinished");
            NativeDialogs.Instance.HideProgressDialog();
        }
    }

    IEnumerator SaveEntry(string title, string content)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        string saveURL = serverURL + "AddJournalEntry?";
        saveURL += "token=" + _userManager.CurrentUser.Token;
        saveURL += "&pin=" + _userManager.CurrentUser.PIN;
        saveURL += "&title=" + WWW.EscapeURL(title);
        saveURL += "&entry=" + WWW.EscapeURL(content);

        WWW page = new WWW(saveURL);
        yield return page;

        NativeDialogs.Instance.HideProgressDialog();

        Dictionary<string, object> result = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(result["s"]))
        {
            NativeDialogs.Instance.ShowMessageBox("Success!", "Entry saved successfully.\nRe-enter the journal to see it.",
                new string[] { "OK" }, false, (string button) =>
                {
                });
            _view.View.TriggerEvent("CreateSuccess");
        }
        else
        {
            NativeDialogs.Instance.ShowMessageBox("Could not save!", "Reason: " + (result["reason"] as string),
                new string[] { "OK" }, false, (string button) => { });
        }
    }
	
	IEnumerator DeleteJournal(int id)
	{
		Debug.Log("Deleting journal " + id);
		string deleteURL = serverURL + "DeleteJournalEntry?";
		deleteURL += "token=" + _userManager.CurrentUser.Token;
		deleteURL += "&pin=" + _userManager.CurrentUser.PIN;
		deleteURL += "&id=" + id;
		
		WWW page = new WWW(deleteURL);
		yield return page;

        Dictionary<string, object> result = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(result["s"]))
        {
            NativeDialogs.Instance.ShowMessageBox("Success!", "Entry deleted successfully.\nRe-enter the journal to see it.",
                new string[] { "OK" }, false, (string button) =>
                {
                });
			Debug.Log("Journal deleted.");
            _view.View.TriggerEvent("DeleteSuccess", id);
        }
        else
        {
            NativeDialogs.Instance.ShowMessageBox("Could not delete!", "Reason: " + (result["reason"] as string),
                new string[] { "OK" }, false, (string button) => { });
        }
	}

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    #region CoherentUI Bindings
    public void RequestUsername()
    {
        Debug.Log("Updating username.");
        _view.View.TriggerEvent("UpdateUsername", _userManager.CurrentUser.Name);
    }

    public void OnJournalClicked(string pin)
    {
        Debug.Log("Check PIN " + pin);
        StartCoroutine(CheckPIN(pin));
    }
    
    public void OnSaveEntryClicked(string title, string content)
    {
        NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Saving entry.", false, false);
        StartCoroutine(SaveEntry(title, content));
    }
	
	public void DeleteEntry(string id)
	{
		Debug.Log("Trying to delete: " + Convert.ToInt32(id));
		StartCoroutine(DeleteJournal(Convert.ToInt32(id)));
	}

    public void GetCategories()
    {
        Debug.Log("Getting the categories");
        foreach (SocialInterest interest in _userManager.Categories)
        {
            Debug.Log("Adding interest " + interest.Id);
            _view.View.TriggerEvent("AddCategory", interest.Id, interest.Name, _userManager.CurrentUser.HasInterest(interest.Id));
        }
        _view.View.TriggerEvent("CategoriesFinished");
    }

    public void UpdateCategories(string newCats)
    {
        string setURL = serverURL + "SetSocialInterests?token=";
        setURL += _userManager.CurrentUser.Token;
        setURL += newCats;

        Debug.Log("Updating categories " + newCats);

        string[] splitCategories = Regex.Split(newCats, "&i=");
        List<SocialInterest> newInterests = new List<SocialInterest>();

        foreach (string split in splitCategories)
        {
            if (split == "") continue;
            if (_userManager.GetCategoryById(Convert.ToInt32(split)) != null)
                newInterests.Add(_userManager.GetCategoryById(Convert.ToInt32(split)));
        }

        _userManager.CurrentUser.SetCategories(newInterests);

        WWW page = new WWW(setURL);
    }

    public void SignOut()
    {
        _userManager.SignOut();
        Application.LoadLevel(0);
    }

    #endregion
}