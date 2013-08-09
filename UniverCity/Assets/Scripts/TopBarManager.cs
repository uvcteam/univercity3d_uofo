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
            gameObject.SetActiveRecursively(false);
        currentPanel.SetActiveRecursively(false);
        prevPanel.SetActiveRecursively(true);
    }
}