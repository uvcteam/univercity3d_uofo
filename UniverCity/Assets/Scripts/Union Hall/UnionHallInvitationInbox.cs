using UnityEngine;
using System.Collections;

public class UnionHallInvitationInbox : MonoBehaviour 
{
    public GameObject mainMenu = null;
    public GameObject browseAll = null;
    public GameObject eventDetail = null;

    void OnBackClicked()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnBrowseAllClicked()
    {
        browseAll.SetActive(true);
        browseAll.GetComponent<UnionHallBrowseSearch>().returnTo = gameObject;
        gameObject.SetActive(false);
    }

    void OnRemoveClicked()
    {
        Debug.Log("Remove clicked.");
    }

    void OnEventClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "Event")
        {
            UILabel eventName = UICamera.lastHit.collider.gameObject.transform.Find("EventName").GetComponent<UILabel>();
            Debug.Log(eventName.text);

            eventDetail.GetComponent<UnionHallEventDetail>().returnTo = gameObject;
            eventDetail.SetActive(true);
            //gameObject.SetActive(false);
        }
    }
}