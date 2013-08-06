using UnityEngine;
using System.Collections;

public class UnionHallCancelWithdraw : MonoBehaviour
{
    public GameObject returnTo = null;
    public GameObject withdraw = null;
    public GameObject cancel = null;

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnWithdrawClicked()
    {
        withdraw.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnCancelClicked()
    {
        cancel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}