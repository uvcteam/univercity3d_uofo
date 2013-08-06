using UnityEngine;
using System.Collections;

public class UnionHallEventDetail : MonoBehaviour 
{
    public GameObject returnTo = null;
    public GameObject responseNotification = null;
    public UIButton detailsButton = null;

    public UILabel eventTitle = null;
    public UILabel eventWho = null;
    public UILabel eventDesc = null;
    public UILabel eventTime = null;
    public UILabel eventLoc = null;

    private EventManager manager;

    void Awake()
    {
    }

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnShareClicked()
    {
        Debug.Log("Share clicked!");
    }

    void OnDetailsClicked()
    {
        Debug.Log("Details clicked!");
    }

    void OnJoinNowClicked()
    {
        responseNotification.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    public void UpdateEvent()
    {
        manager = GameObject.Find("EventManager").GetComponent<EventManager>();

        eventTitle.text = manager.currentEvent.Title;
        eventWho.text = manager.currentEvent.Who;
        eventDesc.text = manager.currentEvent.Desc;
        eventTime.text = manager.currentEvent.GetEventDateTime();
        eventLoc.text = manager.currentEvent.Loc;

        detailsButton.isEnabled = false;
    }
}