using UnityEngine;
using System.Collections;

public class MyJournal : MonoBehaviour
{
    public GameObject AllEntries = null;
    public GameObject PreviousPanel;

    void Enable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "New Entry";
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = PreviousPanel;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
    }

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