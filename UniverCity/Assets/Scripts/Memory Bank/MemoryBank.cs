using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;
    public GameObject PreferencesPanel = null;
    public UILabel UserName = null;

    public TopBarManager topBar = null;

    void OnEnable()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
            Application.LoadLevel(0);
        UserName.text = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Name;
        topBar.gameObject.SetActive(false);
    }

    void OnJournalClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = EntriesPanel;
        topBar.gameObject.SetActive(true);

        EntriesPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnNotificationsClicked()
    {
        topBar.prevPanel = gameObject;
        topBar.currentPanel = NotificationPanel;
        topBar.gameObject.SetActive(true);

        NotificationPanel.SetActive(true);
        gameObject.SetActive(false);
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
        topBar.prevPanel = gameObject;
        topBar.currentPanel = PreferencesPanel;
        topBar.gameObject.SetActive(true);

        PreferencesPanel.SetActive(true);
        gameObject.SetActive(false);
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