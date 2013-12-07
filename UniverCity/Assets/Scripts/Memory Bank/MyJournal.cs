using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;

public class MyJournal : MonoBehaviour
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    public GameObject AllEntries = null;
    public GameObject PreviousPanel;

    public UIInput title;
    public UIInput entry;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "New Entry";
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = PreviousPanel;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
    }

    void OnSaveEntryClicked()
    {
        NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Saving entry.", false, false);
        StartCoroutine(SaveEntry());
    }

    IEnumerator SaveEntry()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        string saveURL = serverURL + "AddJournalEntry?";
        saveURL += "token=" + manager.GetComponent<UserManager>().CurrentUser.Token;
        saveURL += "&pin=" + manager.GetComponent<UserManager>().CurrentUser.PIN;
        saveURL += "&title=" + WWW.EscapeURL(title.text);
        saveURL += "&entry=" + WWW.EscapeURL(entry.text);

        WWW page = new WWW(saveURL);
        yield return page;

        NativeDialogs.Instance.HideProgressDialog();

        Dictionary<string, object> result = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(result["s"]))
        {
            NativeDialogs.Instance.ShowMessageBox("Success!", "Entry saved successfully.\nRe-enter the journal to see it.",
                new string[] { "OK" }, false, (string button) =>
                    {
                        title.text = "";
                        entry.text = "";
                    });

            OnBackClicked();
        }
        else
        {
            NativeDialogs.Instance.ShowMessageBox("Could not save!", "Reason: " + (result["reason"] as string),
                new string[] { "OK" }, false, (string button) => { });
        }
    }

    void OnBackClicked()
    {
        title.text = "";
        entry.text = "";
        gameObject.SetActive(false);
        PreviousPanel.SetActive(true);
    }
}