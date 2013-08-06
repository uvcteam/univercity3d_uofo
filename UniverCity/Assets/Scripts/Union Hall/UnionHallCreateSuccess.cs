using UnityEngine;
using System.Collections;

public class UnionHallCreateSuccess : MonoBehaviour 
{
    public GameObject mainMenu = null;

    public UILabel eventName = null;
    public UILabel eventWho = null;
    public UILabel eventDesc = null;
    public UILabel eventTime = null;
    public UILabel eventLoc = null;

    public UnionHallEvent newEvent = null;

    void OnEnable()
    {
        newEvent = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventName.text = newEvent.Title;
        eventWho.text = newEvent.Who;
        eventDesc.text = newEvent.Desc;
        eventTime.text = newEvent.GetEventDateTime();
        eventLoc.text = newEvent.Loc;
        GameObject.Find("EventManager").GetComponent<EventManager>().RepopulateEvents();
    }

    void OnBackClicked()
    {
        DestroyImmediate(newEvent.gameObject);
        mainMenu.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnEditClicked()
    {
        Debug.Log("Edit later.");
    }
}