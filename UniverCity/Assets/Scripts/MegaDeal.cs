using UnityEngine;
using System.Collections;

public class MegaDeal : MonoBehaviour 
{
    public GameObject Description;
    public GameObject End;
    public GameObject List;
    public GameObject ListLabel;
    public GameObject Price;
    public GameObject PriceLabel;
    public GameObject Title;
	public Color megaDealColor = Color.white;


    void PurchaseDeal()
    {
        int id = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>().AdInfo.Mega.BusinessId;
        string token = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;
        string url = "https://www.univercity3d.com/univercity/mdpurchase?b=" + id + "&token=" + token;

        Application.OpenURL(url);
    }
}
