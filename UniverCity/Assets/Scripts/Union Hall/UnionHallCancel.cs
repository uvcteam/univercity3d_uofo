using UnityEngine;
using System.Collections;

public class UnionHallCancel : MonoBehaviour
{
    public GameObject returnTo = null;
    public GameObject cancelDetail = null;

    public Transform scrollPanel = null;
    public Transform buttonTransform = null;
    private EventManager manager;
    public Transform eventButton = null;
    public GameObject Grid = null;

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

        foreach (UnionHallEvent ev in manager.events)
        {
            if (ev.Email == GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Email)
            {
                Transform newEvent =
                    Instantiate(eventButton, buttonTransform.position, buttonTransform.rotation) as Transform;
                newEvent.Find("EventName").GetComponent<UILabel>().text = ev.Title;
                newEvent.Find("EventDateTime").GetComponent<UILabel>().text = ev.GetEventDateTime();
                newEvent.parent = Grid.transform;
                newEvent.localScale = buttonTransform.localScale;
                newEvent.GetComponent<UIDragPanelContents>().draggablePanel =
                    scrollPanel.GetComponent<UIDraggablePanel>();
                newEvent.GetComponent<UIButtonMessage>().target = gameObject;
                newEvent.gameObject.name = "Event";
                Grid.GetComponent<UIGrid>().Reposition();
            }
        }
    }

    void OnBackClicked()
    {
        returnTo.SetActive(true);
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

            cancelDetail.SetActive(true);
            //gameObject.SetActive(false);
        }
    }
}