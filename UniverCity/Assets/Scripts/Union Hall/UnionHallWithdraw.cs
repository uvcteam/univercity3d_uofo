using UnityEngine;
using System.Collections;

public class UnionHallWithdraw : MonoBehaviour
{
    public GameObject returnTo = null;
    public GameObject withdrawDetail = null;

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnEventClicked()
    {
        withdrawDetail.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}