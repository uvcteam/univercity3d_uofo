using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

public class HTMLMemoryBank : MonoBehaviour
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
            _view.View.BindCall("RequestUsername", (System.Action)RequestUsername);
            _view.View.BindCall("CheckPin", (System.Action<string>)OnJournalClicked);
            _view.View.BindCall("OnSaveEntryClicked", (System.Action<string, string>) OnSaveEntryClicked);
        };

        _viewReady = false;
    }

    IEnumerator CheckPIN(string pin)
    {
        NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Checking PIN", false, false);
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        string journalURL = "http://www.univercity3d.com/univercity/ListJournal?";
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
                Debug.Log("Adding entry " + entry.Title + " " + entry.TimeStamp.ToString("MMMM dd, yyyy") + " " + entry.Entry);
                _view.View.TriggerEvent("AddJournal", entry.Title, entry.TimeStamp.ToString("MMMM dd, yyyy"), entry.Entry);
            }

            NativeDialogs.Instance.HideProgressDialog();
        }
    }

    IEnumerator SaveEntry(string title, string content)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        string saveURL = "http://www.univercity3d.com/univercity/AddJournalEntry?";
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

    #endregion
}