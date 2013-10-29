using UnityEngine;
using System.Collections;

public class MainMenuNavigation : MonoBehaviour
{
    public GameObject BottomBar = null;

    void OnVirtualMallClicked()
    {
        BottomBar.SendMessage("OnVirtualMallClicked");
    }

    void OnUnionHallClicked()
    {
        BottomBar.SendMessage("OnUnionHallClicked");
    }

    void OnMemoryBankClicked()
    {
        BottomBar.SendMessage("OnMemoryBankClicked");
    }

    void OnExplorerClicked()
    {
        BottomBar.SendMessage("OnExplorerClicked");
    }

    void OnArcadeClicked()
    {
        BottomBar.SendMessage("OnArcadeClicked");
    }
}