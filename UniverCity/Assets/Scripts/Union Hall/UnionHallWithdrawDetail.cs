using UnityEngine;
using System.Collections;

public class UnionHallWithdrawDetail : MonoBehaviour 
{
    public GameObject returnTo = null;
    public GameObject withdrawSuccess = null;

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnContinueClicked()
    {
        withdrawSuccess.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}