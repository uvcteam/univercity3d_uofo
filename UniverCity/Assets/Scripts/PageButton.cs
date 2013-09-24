using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour {

    public GameObject Page;

    public void GoToPage()
    {
        GameObject businessAd = GameObject.Find("BusinessAd");
		if(businessAd == null)
		{
			Debug.Log("BusinessAd not found");
			return;
		}
        foreach(GameObject page in GameObject.FindGameObjectsWithTag("Page"))
        {
            page.SetActive(false);
        }
		
		Debug.Log(businessAd.name);

        foreach (GameObject btn in GameObject.Find("BusinessAd").GetComponent<BusinessAd>().pageBtns)
        {
            if (btn.GetComponentInChildren<UILabel>() != null)
            {
                btn.GetComponentInChildren<UILabel>().color = Color.black;
                btn.GetComponent<UIButton>().isEnabled = true;
            }
        }

        if (businessAd.GetComponent<BusinessAd>().hasMegaDeal && businessAd.GetComponent<BusinessAd>().hasMegaDeal)
            businessAd.GetComponent<BusinessAd>().MegaDealBtn.GetComponent<UIButton>().isEnabled = true;

        if (businessAd.GetComponent<BusinessAd>().MegaDealBtn.activeSelf)
            businessAd.GetComponent<BusinessAd>().MegaDealBtn.GetComponentInChildren<UILabel>().color = Color.black;

        //GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.GetComponent<UIButton>().isEnabled = true;

        //if (GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.activeSelf)
        //    GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.GetComponentInChildren<UILabel>().color = Color.black;


        Page.SetActive(true);
        if (gameObject.GetComponentInChildren<UILabel>() != null && gameObject != businessAd.GetComponent<BusinessAd>().detailsBtn)
        {
            gameObject.GetComponentInChildren<UILabel>().color = Color.white;
            gameObject.GetComponent<UIButton>().isEnabled = false;
        }

        

    }
}
