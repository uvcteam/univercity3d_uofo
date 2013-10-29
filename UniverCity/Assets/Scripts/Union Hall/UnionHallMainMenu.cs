using UnityEngine;
using System.Collections;

// This is the class that drives the main menu for the
// Union Hall. It contains the managers for all of the buttons
// on the main menu.

public class UnionHallMainMenu : MonoBehaviour 
{
    public GameObject invitationInbox = null;
    public GameObject createEvent = null;
    public GameObject browseSearch = null;
    public GameObject editEvent = null;
    public GameObject cancelWithdraw = null;

    public TopBarManager topBar = null;

    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            Application.LoadLevel(0);
        //topBar = GameObject.Find("TopAnchor").GetComponent<TopBarManager>();
        topBar.gameObject.SetActive(false);
    }

    void OnInvitationInboxClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = invitationInbox;
        topBar.gameObject.SetActive(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Invitation Inbox";

        invitationInbox.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnBrowseSearchClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = browseSearch;
        topBar.gameObject.SetActive(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Browse/Search";

        browseSearch.GetComponent<UnionHallBrowseSearch>().currentCategory = "";
        browseSearch.GetComponent<UnionHallBrowseSearch>().returnTo = gameObject;
        browseSearch.SetActive(true);
        gameObject.SetActive(false);
    }
    void OnCreateClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = createEvent;
        topBar.gameObject.SetActive(true);
        topBar.transform.Find("Panel").Find("Back").GetComponentInChildren<UILabel>().text = "Cancel";

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Create Event";

        createEvent.SetActive(true);
        createEvent.GetComponent<UnionHallCreateEngagement>().NewEvent();
        gameObject.SetActive(false);
    }

    void OnEditClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = editEvent;
        topBar.gameObject.SetActive(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Edit";

        editEvent.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnCancelWithdrawClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = cancelWithdraw;
        topBar.gameObject.SetActive(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Cancel/Withdraw";

        cancelWithdraw.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnBackClicked()
    {
        DestroyImmediate(GameObject.Find("EventManager"));
        DestroyImmediate(GameObject.Find("BusinessManager"));
        Application.LoadLevel(0);
    }
}