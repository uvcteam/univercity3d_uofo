using UnityEngine;
using System.Collections;

public class BusinessBtn : MonoBehaviour 
{
    public int businessId;

    public void OnBusinessClicked()
    {
        //Application.OpenURL("http://www.univercity3d.com/univercity/playad?b=" + businessId.ToString());
        //BusinessManager businessmgr = GameObject.FindGameObjectWithTag("BusinessManager").GetComponent<BusinessManager>();
        //Instantiate(businessmgr.businessAdPrefab, new Vector3(0, 0, -500), Quaternion.Euler(new Vector3(0, 0, 270)));

        GameObject businessAd = GameObject.Find("Virtual Mall").GetComponent<VirtualMallManager>().businessAd;
        businessAd.SetActive(true);
        StartCoroutine(businessAd.GetComponent<BusinessAd>().SetUpAd(businessId, transform.Find("Name").GetComponent<UILabel>().text));
    }
}