using UnityEngine;
using System.Collections;

public class BottomMenu : MonoBehaviour 
{
    public GameObject[] buttons = new GameObject[5];
    int activeScene = 4;

    void Start()
    {
        //foreach(GameObject button in buttons)
        //    NGUITools.AddWidgetCollider(button);
        activeScene = Application.loadedLevel - 1;
        if (activeScene != -1)
            buttons[activeScene].SendMessage("DeActivate");
    }

    void OnArcadeClicked()
    {
        Application.LoadLevel(2);
        
    }

    void OnExplorerClicked()
    {
        Application.LoadLevel(1);
    }

    void OnMemoryBankClicked()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
        {
            Application.LoadLevel(0);
            return;
        }
        Application.LoadLevel(4);
    }

    void OnUnionHallClicked()
    {
        if (!GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().IsSignedIn())
        {
            Application.LoadLevel(0);
            return;
        }
        Application.LoadLevel(3);
    }

    void OnVirtualMallClicked()
    {
        Application.LoadLevel(5);
    }
}