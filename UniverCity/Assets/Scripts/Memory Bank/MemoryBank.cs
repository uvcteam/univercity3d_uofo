using UnityEngine;
using System.Collections;

public class MemoryBank : MonoBehaviour 
{
    public GameObject JournalPanel = null;
    public GameObject NotificationPanel = null;
    public GameObject EntriesPanel = null;

    void OnJournalClicked()
    {
        JournalPanel.SetActiveRecursively(true);
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

    void OnJournalVaultClicked()
    {
        EntriesPanel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
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

    void OnBackClicked()
    {
        gameObject.SetActiveRecursively(false);
        Application.LoadLevel(0);
    }
}