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
        returnTo.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnEventClicked()
    {
        withdrawDetail.SetActive(true);
        //gameObject.SetActive(false);
    }
}