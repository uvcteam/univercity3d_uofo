using UnityEngine;
using System.Collections;

public class UnionHallBrowseSearch : MonoBehaviour
{
    public GameObject invitationInbox = null;
    public GameObject search = null;
    public GameObject eventDetail = null;
    public GameObject returnTo = null;
    public Transform eventButton = null;

    public Transform scrollPanel = null;
    public Transform buttonTransform = null;
    private EventManager manager;
    public GameObject Grid = null;

    public string currentCategory = "";
    public UILabel pageLabel = null;

    void OnDisable()
    {
        foreach (Transform child in Grid.transform)
            DestroyImmediate(child.gameObject);
    }

    void OnEnable()
    {
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = returnTo;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;

        foreach (Transform child in Grid.transform)
                DestroyImmediate(child.gameObject);

        manager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (currentCategory == "")
        {
            GameObject.Find("PageName").GetComponent<UILabel>().text = "All Engagements";
            foreach (UnionHallEvent ev in manager.events)
            {
                Transform newEvent = Instantiate(eventButton, buttonTransform.position, buttonTransform.rotation) as Transform;
                newEvent.Find("EventName").GetComponent<UILabel>().text = ev.Title;
                newEvent.Find("EventDateTime").GetComponent<UILabel>().text = ev.GetEventDateTime();
                newEvent.parent = Grid.transform;
                newEvent.localScale = buttonTransform.localScale;
                newEvent.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
                newEvent.GetComponent<UIButtonMessage>().target = gameObject;
                newEvent.gameObject.name = "Event";
                Grid.GetComponent<UIGrid>().Reposition();
            }
        }
        else if (manager.eventsByCategory.ContainsKey(currentCategory))
        {
            GameObject.Find("PageName").GetComponent<UILabel>().text = currentCategory;
            foreach (UnionHallEvent ev in manager.eventsByCategory[currentCategory])
            {
                Transform newEvent = Instantiate(eventButton, buttonTransform.position, buttonTransform.rotation) as Transform;
                newEvent.Find("EventName").GetComponent<UILabel>().text = ev.Title;
                newEvent.Find("EventDateTime").GetComponent<UILabel>().text = ev.GetEventDateTime();
                newEvent.parent = scrollPanel;
                newEvent.localScale = buttonTransform.localScale;
                newEvent.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
                newEvent.GetComponent<UIButtonMessage>().target = gameObject;
                newEvent.gameObject.name = "Event";
                scrollPanel.GetComponent<UIGrid>().Reposition();
            }
        }
        else
            GameObject.Find("PageName").GetComponent<UILabel>().text = currentCategory;

        Grid.GetComponent<UIGrid>().Reposition();
    }

    void OnBackClicked()
    {
        returnTo.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnSearchClicked()
    {
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = gameObject;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = search;
        search.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnEventClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "Event")
        {
            UILabel eventName = UICamera.lastHit.collider.gameObject.transform.Find("EventName").GetComponent<UILabel>();

            foreach (UnionHallEvent ev in manager.events)
            {
                if (ev.Title == eventName.text)
                {
                    manager.currentEvent = ev;
                    break;
                }
            }

            eventDetail.GetComponent<UnionHallEventDetail>().returnTo = gameObject;
            eventDetail.SetActive(true);
            eventDetail.GetComponent<UnionHallEventDetail>().UpdateEvent();
            gameObject.SetActive(false);
        }
    }

}