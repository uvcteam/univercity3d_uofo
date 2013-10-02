using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour {

    public GameObject Page;

    public void GoToPage()
    {
        string trackURL = "http://www.univercity3d.com/univercity/track?id=";
        GameObject businessAd = GameObject.Find("BusinessAd");

        if (businessAd == null)
        {
            Debug.Log("BusinessAd not found");
            return;
        }

        trackURL += businessAd.GetComponent<BusinessAd>().adManager.BusinessID;
        if (Page.name == "Mega Deal")
        {
            trackURL += "&title=";
            trackURL += "&event=deal";
        }
        else if (Page.name == "Members Only")
        {
            trackURL += "&title=";
            trackURL += "&event=discount"; 
        }
        else
        {
            trackURL += "&title=" + transform.Find("Label").GetComponent<UILabel>().text;
            trackURL += "&event=click";
        }
        trackURL += "&play_id=" + businessAd.GetComponent<BusinessAd>().sessionId;

        if (PlayerPrefs.GetInt("loggedIn") == 1)
            trackURL += "&token=" +
                        GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;
		
        Debug.Log("Sending: " + trackURL);
        WWW track = new WWW(trackURL);

        foreach(GameObject page in GameObject.FindGameObjectsWithTag("Page"))
        {
            page.SetActive(false);
        }
		
		//Debug.Log(businessAd.name);

        foreach (GameObject btn in GameObject.Find("BusinessAd").GetComponent<BusinessAd>().pageBtns)
        {
            if (btn.GetComponentInChildren<UILabel>() != null)
            {
                btn.GetComponentInChildren<UILabel>().color = Color.black;
                btn.GetComponent<UIButton>().isEnabled = true;
            }
        }

        if (businessAd.GetComponent<BusinessAd>().hasMegaDeal && businessAd.GetComponent<BusinessAd>().hasMegaDeal)
            businessAd.GetComponent<BusinessAd>().MegaDealBtn.GetComponent<UIImageButton>().isEnabled = true;


        //GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.GetComponent<UIButton>().isEnabled = true;

        //if (GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.activeInHierarchySelf)
        //    GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn.GetComponentInChildren<UILabel>().color = Color.black;


        Page.SetActive(true);
        if (gameObject.GetComponentInChildren<UILabel>() != null && gameObject != businessAd.GetComponent<BusinessAd>().detailsBtn)
        {
            gameObject.GetComponentInChildren<UILabel>().color = Color.white;
            gameObject.GetComponent<UIButton>().isEnabled = false;
        }

        

    }
}
