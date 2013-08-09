using UnityEngine;
using System.Collections;

public class UnionHallCancelWithdraw : MonoBehaviour
{
    public GameObject returnTo = null;
    public GameObject withdraw = null;
    public GameObject cancel = null;

    void OnEnable()
    {
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().prevPanel = returnTo;
        GameObject.Find("TopAnchor").GetComponent<TopBarManager>().currentPanel = gameObject;
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Cancel/Withdraw";
    }

    void OnBackClicked()
    {
        returnTo.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnWithdrawClicked()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Withdraw";

        withdraw.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnCancelClicked()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Cancel";

        cancel.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }
}