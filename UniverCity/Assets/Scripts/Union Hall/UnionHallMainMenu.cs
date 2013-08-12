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
        //topBar = GameObject.Find("TopAnchor").GetComponent<TopBarManager>();
        topBar.gameObject.SetActiveRecursively(false);
    }

    void OnInvitationInboxClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = invitationInbox;
        topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Invitation Inbox";

        invitationInbox.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnBrowseSearchClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = browseSearch;
        topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Browse/Search";

        browseSearch.GetComponent<UnionHallBrowseSearch>().currentCategory = "";
        browseSearch.GetComponent<UnionHallBrowseSearch>().returnTo = gameObject;
        browseSearch.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
    void OnCreateClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = createEvent;
        topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Create Event";

        createEvent.SetActiveRecursively(true);
        createEvent.GetComponent<UnionHallCreateEngagement>().NewEvent();
        gameObject.SetActiveRecursively(false);
    }

    void OnEditClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = editEvent;
        topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Edit";

        editEvent.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnCancelWithdrawClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = cancelWithdraw;
        topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Cancel/Withdraw";

        cancelWithdraw.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnBackClicked()
    {
        DestroyImmediate(GameObject.Find("EventManager"));
        DestroyImmediate(GameObject.Find("BusinessManager"));
        Application.LoadLevel(0);
    }
}