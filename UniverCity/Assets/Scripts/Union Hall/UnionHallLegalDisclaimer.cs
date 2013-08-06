using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;

public class UnionHallLegalDisclaimer : MonoBehaviour 
{
    public GameObject createEngagement = null;
    public GameObject successPage = null;

    public UnionHallEvent newEvent = null;

    private string webString = "";

    void OnEnable()
    {
        newEvent = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
    }

    void OnAcceptClicked()
    {
        string format = "yyyy-MM-dd HH:mm";
        webString = "http://www.univercity3d.com/univercity/CreateEvent?token=&";
        webString += "title=" + WWW.EscapeURL(newEvent.Title) + "&";
        webString += "desc=" + WWW.EscapeURL(newEvent.Desc) + "&";
        webString += "who=" + WWW.EscapeURL(newEvent.Who) + "&";
        webString += "email=" + WWW.EscapeURL(newEvent.Email) + "&";
        webString += "location=" + WWW.EscapeURL(newEvent.Loc) + "&";
        webString += "phone=" + WWW.EscapeURL(newEvent.Phone) + "&";
        webString += "min=" + newEvent.Min + "&";
        webString += "max=" + newEvent.Max + "&";
        webString += "start=" + WWW.EscapeURL(
            newEvent.Start.ToString(format)) + "&";
        webString += "end=" + WWW.EscapeURL(
            newEvent.Start.AddHours(1).ToString(format)) + "&";
        for (int i = 0; i < newEvent.interests.Count; i++)
        {
            webString += "interests=" + newEvent.interests[i];
            if (i < newEvent.interests.Count - 1)
                webString += "&";
        }

        Debug.Log(webString);

        StartCoroutine(CreateEvent());
    }

    IEnumerator CreateEvent()
    {
        WWW page = new WWW(webString);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> createSuccess = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)createSuccess["s"])
        {
            Debug.Log("Success!");
            StopAllCoroutines();
            successPage.SetActiveRecursively(true);
            gameObject.SetActiveRecursively(false);
        }
        else
        {
            Debug.Log("There was an error.");
        }
    }

    void OnCancelClicked()
    {
        createEngagement.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}