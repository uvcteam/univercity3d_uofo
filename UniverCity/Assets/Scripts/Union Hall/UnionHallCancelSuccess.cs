using UnityEngine;
using System.Collections;

public class UnionHallCancelSuccess : MonoBehaviour 
{
    public GameObject returnTo = null;

    public UILabel eventName = null;
    public UILabel eventDate = null;

    public EventManager manager = null;

    void OnEnable()
    {
        manager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventName.text = manager.currentEvent.Title;
        eventDate.text = manager.currentEvent.GetEventDateTime();
        manager.RepopulateEvents();
    }

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}