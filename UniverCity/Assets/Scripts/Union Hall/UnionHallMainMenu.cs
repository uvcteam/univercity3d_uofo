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

    private TopBarManager _topBar = null;

    void Start()
    {
        _topBar = GameObject.Find("TopAnchor").GetComponent<TopBarManager>();
        _topBar.gameObject.SetActiveRecursively(false);
    }

    void OnInvitationInboxClicked()
    {
        _topBar.prevPanel = gameObject;
        _topBar.currentPanel = invitationInbox;
        _topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Invitation Inbox";

        invitationInbox.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnBrowseSearchClicked()
    {
        _topBar.prevPanel = gameObject;
        _topBar.currentPanel = browseSearch;
        _topBar.gameObject.SetActiveRecursively(true);

        GameObject.Find("PageName").GetComponent<UILabel>().text = "Browse/Search";

        browseSearch.GetComponent<UnionHallBrowseSearch>().currentCategory = "";
        browseSearch.GetComponent<UnionHallBrowseSearch>().returnTo = gameObject;
        browseSearch.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
    void OnCreateClicked()
    {
        createEvent.SetActiveRecursively(true);
        createEvent.GetComponent<UnionHallCreateEngagement>().NewEvent();
        gameObject.SetActiveRecursively(false);
    }

    void OnEditClicked()
    {
        editEvent.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnCancelWithdrawClicked()
    {
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