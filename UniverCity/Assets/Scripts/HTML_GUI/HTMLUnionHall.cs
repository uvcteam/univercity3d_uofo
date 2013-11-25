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

public class HTMLUnionHall : MonoBehaviour
{
    private CoherentUIView _view;
    private UserManager _userManager;
    private bool _viewReady;

    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("CreateEvent", (System.Action<string[]>)CreateEvent);
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

    #region CoherentUI Bindings

    public void CreateEvent(string[] inputs)
    {
        string emailRegEx = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                          + "@"
                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
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
        if (!Regex.IsMatch(inputs[7], emailRegEx))
        {
            NativeDialogs.Instance.ShowMessageBox("Error!", "Invalid email! Accepted format:\nname@domain.info",
                new string[] { "OK" }, false, (string button) =>
                {
                });
            return;
        }
        if (!Regex.IsMatch(inputs[8], phoneRegEx))
        {
            NativeDialogs.Instance.ShowMessageBox("Error!", "Invalid phone number! Accepted formats:\n(###) ###-####\n##########\n###-###-####",
                new string[] { "OK" }, false, (string button) =>
                {
                });
            return;
        }

        DateTime start = DateTime.Parse(inputs[4] + " " + inputs[5]);
        Debug.Log(start.ToString("yyyy-MM-dd HH:mm"));

        string createURL = "http://www.univercity3d.com/univercity/CreateEvent?token=";
        createURL += GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;
        createURL += "&title=" + WWW.EscapeURL(inputs[0]);
        createURL += "&desc=" + WWW.EscapeURL(inputs[2]);
        createURL += "&who=" + WWW.EscapeURL(inputs[1]);
        createURL += "&email=" + WWW.EscapeURL(inputs[7]);
        createURL += "&location=" + WWW.EscapeURL(inputs[3]);
        createURL += "&phone=" + WWW.EscapeURL(inputs[8]);
        createURL += "&min=" + inputs[9];
        createURL += "&max=" + inputs[10];
        createURL += "&start=" + WWW.EscapeURL(
            start.ToString("yyyy-MM-dd HH:mm"));
        createURL += "&end=" + WWW.EscapeURL(
            start.AddHours(1).ToString("yyyy-MM-dd HH:mm"));
        createURL += "&interests=" + _userManager.GetIDForCategory(inputs[6]);

        Debug.Log(createURL);
        // TODO: Add server call and check return.
    }
    #endregion
}