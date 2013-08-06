using UnityEngine;
using System.Collections;

public class UnionHallEditEngagements : MonoBehaviour
{
    public GameObject returnTo = null;

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
            Debug.Log("Editing " + eventName.text);
        }
    }
}