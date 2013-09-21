using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;
    public UILabel UserName = null;

    public TopBarManager topBar = null;

    void OnEnable()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            Application.LoadLevel(0);
        UserName.text = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Name;
        topBar.gameObject.SetActiveRecursively(false);
    }

    void OnJournalClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = EntriesPanel;
        topBar.gameObject.SetActiveRecursively(true);

        EntriesPanel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnNotificationsClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = NotificationPanel;
        topBar.gameObject.SetActiveRecursively(true);

        NotificationPanel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnPhotoVaultClicked()
    {
        Debug.Log("Open Photo Vault");
    }

    void OnVideoVaultClicked()
    {
        Debug.Log("Open Video Vault");
    }

    void OnPreferencesClicked()
    {
        Debug.Log("Open Preferences");
    }

    void OnInviteClicked()
    {
        Debug.Log("Open Invite");
    }

    void OnHelpClicked()
    {
        Debug.Log("Open Help");
    }

    void OnSignOutClicked()
    {
        GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().SignOut();
        Application.LoadLevel(0);
    }
}