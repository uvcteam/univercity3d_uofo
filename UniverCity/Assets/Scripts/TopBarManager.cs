using UnityEngine;
using System.Collections;

public class TopBarManager : MonoBehaviour
{
    public GameObject currentPanel;
    public GameObject prevPanel;

    void OnBackClicked()
    {
        if (prevPanel.name == "pan Main Menu")
            gameObject.SetActiveRecursively(false);
        currentPanel.SetActiveRecursively(false);
        prevPanel.SetActiveRecursively(true);
    }
}