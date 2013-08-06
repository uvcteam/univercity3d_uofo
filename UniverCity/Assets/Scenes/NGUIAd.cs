using UnityEngine;
using System.Collections;

public class NGUIAd : MonoBehaviour
{
    public UILabel adName = null;
    public UILabel adDesc = null;
    public UITexture adLogo = null;
    public GameObject adDiscount = null;
    public GameObject adDeal = null;
    public Business myBusiness;

    public void SetBusiness(Business bus)
    {
        myBusiness = bus;
        adName.text = bus.name;
        adDesc.text = bus.desc;
        adLogo.mainTexture = bus.logo;
    }

    void OnDiscountClicked()
    {
        Debug.Log("Discount");
    }

    void OnMegaDealClicked()
    {
        Debug.Log("Mega Deal");
    }
}