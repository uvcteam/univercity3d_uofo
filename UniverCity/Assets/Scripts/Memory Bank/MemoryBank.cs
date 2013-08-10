using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;

    public TopBarManager topBar = null;

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("SignedIn") || PlayerPrefs.GetInt("SignedIn") == 0)
            Application.LoadLevel(0);
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
        PlayerPrefs.SetInt("SignedIn", 0);
        Application.LoadLevel(0);
    }
}