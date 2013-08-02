using UnityEngine;
using System.Collections;

public class MyJournal : MonoBehaviour
{
    public GameObject AllEntries = null;
    public GameObject PreviousPanel;

    void OnAllEntriesClicked()
    {
        AllEntries.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnNewEntryClicked()
    {
        Debug.Log("New entry.");
    }

    void OnAddPhotoClicked()
    {
        Debug.Log("Add photo.");
    }

    void OnAddVideoClicked()
    {
        Debug.Log("Add video.");
    }

    void OnSaveEntryClicked()
    {
        Debug.Log("Save entry.");
    }

    void OnLastEntryClicked()
    {
        Debug.Log("Last entry.");
    }

    void OnBackClicked()
    {
        gameObject.SetActiveRecursively(false);
        PreviousPanel.SetActiveRecursively(true);
    }
}