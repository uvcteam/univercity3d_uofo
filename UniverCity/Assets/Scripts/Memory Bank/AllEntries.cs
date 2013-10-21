using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AllEntries : MonoBehaviour 
{
    public GameObject PreviousPanel;
    public GameObject entryPanel;
    public GameObject indPanel;

    public GameObject ScrollPanel;
    public GameObject Grid;
    public Transform JournalTransform;

    private List<GameObject> myEntries;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Private Journal";
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = PreviousPanel;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;

        myEntries = new List<GameObject>();
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Private Journal";
        for (int i = 0; i < manager.GetComponent<UserManager>().CurrentUser.Journals.Count; ++i)
        {
            GameObject go =
                Instantiate(Resources.Load("Prefabs/JournalEntry"), JournalTransform.position, JournalTransform.rotation) as
                GameObject;
            go.GetComponent<JournalHolder>().index = i;
            go.transform.Find("Title").GetComponent<UILabel>().text =
                manager.GetComponent<UserManager>().CurrentUser.Journals[i].Title;
            go.transform.parent = Grid.transform;
            go.transform.localScale = Vector3.one;
            go.name = "JournalEntry" + i;
            go.GetComponent<UIButtonMessage>().target = gameObject;
            myEntries.Add(go);
        }
        Grid.GetComponent<UIGrid>().Reposition();
        NativeDialogs.Instance.HideProgressDialog();
    }

    void OnDisable()
    {
        foreach (GameObject go in myEntries)
        {
            Destroy(go);
        }
        myEntries.Clear();
    }

    void OnAllEntriesClicked()
    {
        Debug.Log("This button doesn't need to be here.");
    }
    void OnNewEntryClicked()
    {
        entryPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    void OnEntryClicked()
    {
        GameObject lastHit = UICamera.lastHit.collider.gameObject;
        if (lastHit.name.Contains("JournalEntry"))
        {
            indPanel.SetActive(true);
            indPanel.GetComponent<IndividualEntry>().SetupEntry(lastHit.GetComponent<JournalHolder>().index);
            gameObject.SetActive(false);
        }
    }
    void OnBackClicked()
    {
        gameObject.SetActive(false);
        PreviousPanel.SetActive(true);
    }
}