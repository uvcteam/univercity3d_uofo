using UnityEngine;
using System.Collections;

public class AllEntries : MonoBehaviour 
{
    public GameObject PreviousPanel;
    public GameObject entryPanel;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Private Journal";
    }

    void OnAllEntriesClicked()
    {
        Debug.Log("This button doesn't need to be here.");
    }

    void OnNewEntryClicked()
    {
        entryPanel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
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