using UnityEngine;
using System.Collections;

public class UnionHallEditEngagements : MonoBehaviour
{
    public GameObject returnTo = null;

    void OnEnable()
    {
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = returnTo;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Edit Engagements";
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
            Debug.Log("Editing " + eventName.text);
        }
    }
}