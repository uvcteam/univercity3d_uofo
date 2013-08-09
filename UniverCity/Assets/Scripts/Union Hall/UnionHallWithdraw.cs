using UnityEngine;
using System.Collections;

public class UnionHallWithdraw : MonoBehaviour
{
    public GameObject returnTo = null;
    public GameObject withdrawDetail = null;

    void OnEnable()
    {
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = returnTo;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
    }

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnEventClicked()
    {
        withdrawDetail.SetActiveRecursively(true);
        //gameObject.SetActiveRecursively(false);
    }
}