using UnityEngine;
using System.Collections;

public class CreateEventLastPage : MonoBehaviour
{
    public GameObject prevPage = null;

    public UILabel Title = null;
    public UILabel Who = null;
    public UILabel Desc = null;
    public UILabel Loc = null;
    public UILabel When = null;

    void OnEnable()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        Title.text = eventScript.Title;
        Who.text = eventScript.Who;
        Desc.text = eventScript.Desc;
        Loc.text = eventScript.Loc;
        When.text = eventScript.Start.ToString("MMMM dd, yyyy") + " at " + eventScript.Start.ToString("h:mm tt");
    }

    private void OnFinishClicked()
    {
        SendMessageUpwards("OnCreateClicked");
    }

    private void OnPreviousClicked()
    {
        if (prevPage == null) return;
        prevPage.SetActive(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && prevPage != null)
            OnPreviousClicked();
    }
}