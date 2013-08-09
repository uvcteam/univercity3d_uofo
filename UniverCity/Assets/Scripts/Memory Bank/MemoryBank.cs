using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("SignedIn") || PlayerPrefs.GetInt("SignedIn") == 0)
            Application.LoadLevel(0);
    }

    void OnJournalClicked()
    {
        EntriesPanel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnNotificationsClicked()
    {
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