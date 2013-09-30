using UnityEngine;
using System.Collections;

public class TopBarManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject currentPanel;
    public GameObject prevPanel;

    void OnBackClicked()
    {
        if (prevPanel == homePanel)
            gameObject.SetActive(false);
        currentPanel.SetActive(false);
        prevPanel.SetActive(true);
    }
}