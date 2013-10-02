using UnityEngine;
using System.Collections;

public class UnionHallWithdrawDetail : MonoBehaviour 
{
    public GameObject returnTo = null;
    public GameObject withdrawSuccess = null;

    void OnBackClicked()
    {
        //returnTo.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnContinueClicked()
    {
        withdrawSuccess.SetActive(true);
        gameObject.SetActive(false);
    }
}