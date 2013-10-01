using MiniJSON;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnionHallCancelDetail : MonoBehaviour 
{
    public GameObject returnTo = null;
    public GameObject cancelSuccess = null;

    public UILabel eventName = null;
    public UILabel eventDate = null;

    public EventManager manager = null;

    void OnEnable()
    {
        manager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventName.text = manager.currentEvent.Title;
        eventDate.text = manager.currentEvent.GetEventDateTime();
    }

    void OnBackClicked()
    {
        returnTo.SetActive(true);
        //gameObject.SetActive(false);
    }

    void OnContinueClicked()
    {
        StartCoroutine(CancelEvent());
    }

    IEnumerator CancelEvent()
    {
        string webString = "http://www.univercity3d.com/univercity/CancelEvent?";
        webString += "id=" + manager.currentEvent.Id + "&";
        webString += "token=" + GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;
        Debug.Log(webString);
        WWW page = new WWW(webString);
        yield return page;

        // Create an IList of all of the businesses returned to me.
        Dictionary<string, object> cancel = Json.Deserialize(page.text) as Dictionary<string, object>;

        if ((bool)cancel["s"])
        {
            Debug.Log("Success!");
            StopAllCoroutines();
            cancelSuccess.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("There was an error.");
        }
    }
}