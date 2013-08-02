using UnityEngine;
using System.Collections;

public class UnionHallWithdrawSuccess : MonoBehaviour 
{
    public GameObject returnTo = null;

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}