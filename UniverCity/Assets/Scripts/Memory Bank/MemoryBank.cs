using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;
    public GameObject PreferencesPanel = null;
    public UILabel UserName = null;

    public TopBarManager topBar = null;

    void OnEnable()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            Application.LoadLevel(0);
        UserName.text = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Name;
        topBar.gameObject.SetActive(false);
    }

    void OnJournalClicked()
    {
        NativeDialogs.Instance.ShowSecurePromptMessageBox("PIN", "", new string[] {"Cancel, OK"}, false, (string prompt,
                                                                                                   string button) =>
            {
                if (button == "Cancel") return;
                else StartCoroutine(CheckPIN(prompt));
            });
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
                (string button) => {});
        }
        else
        {
            NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Loading Journals", false, false);
            manager.GetComponent<UserManager>().CurrentUser.PIN = pin;
            manager.GetComponent<UserManager>().CurrentUser.PopulateJournal(
                result["entries"] as List<object>);
            NativeDialogs.Instance.HideProgressDialog();
            topBar.prevPanel = gameObject;
            topBar.currentPanel = EntriesPanel;
            topBar.gameObject.SetActive(true);

            EntriesPanel.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    void OnNotificationsClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = NotificationPanel;
        topBar.gameObject.SetActive(true);

        NotificationPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnPhotoVaultClicked()
    {
        Debug.Log("Open Photo Vault");
    }

    void OnVideoVaultClicked()
    {
        Debug.Log("Open Video Vault");
    }

    void OnPreferencesClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = PreferencesPanel;
        topBar.gameObject.SetActive(true);

        PreferencesPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnInviteClicked()
    {
        Debug.Log("Open Invite");
    }

    void OnHelpClicked()
    {
        Debug.Log("Open Help");
    }

    void OnSignOutClicked()
    {
        GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().SignOut();
        Application.LoadLevel(0);
    }
}