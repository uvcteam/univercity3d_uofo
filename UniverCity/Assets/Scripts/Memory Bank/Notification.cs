using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour 
{
    public GameObject PreviousPanel;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Notifications";
    }

    void OnSaveClicked()
    {
        Debug.Log("Save.");
    }

    void OnShareClicked()
    {
        Debug.Log("Share.");
    }

    void OnDeleteClicked()
    {
        Debug.Log("Delete.");
    }

    void OnViewSavedNotificationsClicked()
    {
        Debug.Log("View saved notifications.");
    }

    void OnNotificationClicked()
    {
        Debug.Log("Notification was clicked.");
    }

    void OnBackClicked()
    {
        gameObject.SetActiveRecursively(false);
        PreviousPanel.SetActiveRecursively(true);
    }
}