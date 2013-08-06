using UnityEngine;
using System.Collections;

public class AllEntries : MonoBehaviour 
{
    public GameObject PreviousPanel;

    void OnAllEntriesClicked()
    {
        Debug.Log("This button doesn't need to be here.");
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

    void OnEntryClicked()
    {
        Debug.Log("Load this entry.");
    }

    void OnBackClicked()
    {
        gameObject.SetActiveRecursively(false);
        PreviousPanel.SetActiveRecursively(true);
    }
}