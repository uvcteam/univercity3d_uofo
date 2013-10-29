using UnityEngine;
using System.Collections;

public class TopBarManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject currentPanel;
    public GameObject prevPanel;

    void OnBackClicked()
    {
        transform.Find("Panel").Find("Back").GetComponentInChildren<UILabel>().text = "Go Back";
        if (prevPanel == homePanel)
            gameObject.SetActive(false);
        
        currentPanel.SetActive(false);
        prevPanel.SetActive(true);
    }
}