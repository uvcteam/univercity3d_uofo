using UnityEngine;
using System.Collections;

public class IndividualEntry : MonoBehaviour
{
    public GameObject PreviousPanel;

    public UILabel title;
    public UILabel date;
    public UILabel entry;

    public int myId = -1;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Journal";
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = PreviousPanel;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
    }

    public void SetupEntry(int index)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        myId = index;

        title.text = manager.GetComponent<UserManager>().CurrentUser.Journals[index].Title;
        date.text = manager.GetComponent<UserManager>().CurrentUser.Journals[index].TimeStamp.ToString("MMMM dd, yyyy on HH:mm tt");
        entry.text = manager.GetComponent<UserManager>().CurrentUser.Journals[index].Entry;
    }

    void OnBackClicked()
    {
        gameObject.SetActive(false);
        PreviousPanel.SetActive(true);
    }
}