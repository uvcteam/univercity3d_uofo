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

    void OnDisable()
    {
        foreach (Transform child in scrollPanel)
            DestroyImmediate(child.gameObject);
    }

    void OnEnable()
    {
        foreach (Transform child in scrollPanel)
            DestroyImmediate(child.gameObject);

        manager = GameObject.Find("EventManager").GetComponent<EventManager>();

        foreach (UnionHallEvent ev in manager.events)
        {
            if (ev.Email == "student")
            {
                Transform newEvent =
                    Instantiate(eventButton, buttonTransform.position, buttonTransform.rotation) as Transform;
                newEvent.Find("EventName").GetComponent<UILabel>().text = ev.Title;
                newEvent.Find("EventDateTime").GetComponent<UILabel>().text = ev.GetEventDateTime();
                newEvent.parent = scrollPanel;
                newEvent.localScale = buttonTransform.localScale;
                newEvent.GetComponent<UIDragPanelContents>().draggablePanel =
                    scrollPanel.GetComponent<UIDraggablePanel>();
                newEvent.GetComponent<UIButtonMessage>().target = gameObject;
                newEvent.gameObject.name = "Event";
                scrollPanel.GetComponent<UIGrid>().Reposition();
            }
        }
    }

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
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

            cancelDetail.SetActiveRecursively(true);
            gameObject.SetActiveRecursively(false);
        }
    }
}