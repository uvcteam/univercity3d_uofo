using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour {

    public GameObject Page;
    GameObject businessAd;

    public void Awake()
    {
        businessAd = GameObject.Find("BusinessAd");
    }

    public void GoToPage()
    {
        string trackURL = "http://www.univercity3d.com/univercity/track?id=";

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


        SelectButton();
        businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().Recenter(Page);

    }

    public void SelectButton()
    {
        foreach (GameObject btn in businessAd.GetComponent<BusinessAd>().pageBtns)
        {
            if (btn.GetComponentInChildren<UILabel>() != null)
            {
                btn.GetComponentInChildren<UILabel>().color = Color.black;
                btn.GetComponent<UIButton>().isEnabled = true;
            }
        }
		GameObject centerPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject;
		if (centerPage != null && centerPage.GetComponent<VideoHandler>() != null)
			centerPage.GetComponent<VideoHandler>().OnPageLeave();
		
        if (businessAd.GetComponent<BusinessAd>().hasMegaDeal && businessAd.GetComponent<BusinessAd>().hasMegaDeal)
            businessAd.GetComponent<BusinessAd>().MegaDealBtn.GetComponent<UIButton>().isEnabled = true;


        Page.SetActive(true);

        if (gameObject.GetComponentInChildren<UILabel>() != null && gameObject != businessAd.GetComponent<BusinessAd>().detailsBtn)
        {
            gameObject.GetComponentInChildren<UILabel>().color = Color.white;
            gameObject.GetComponent<UIButton>().isEnabled = false;
            businessAd.GetComponent<BusinessAd>().pageGrid.transform.parent.gameObject.SetActive(true);

            GameObject[] pageBtns = businessAd.GetComponent<BusinessAd>().pageBtns;

            for (int i = 0; i < pageBtns.Length; ++i)
            {
                if (pageBtns[i].GetComponent<PageButton>().Page != null && 
                    pageBtns[i].GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage != null)
                    pageBtns[i].GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage.SetActive(false);
            }
        }

        else if (gameObject == businessAd.GetComponent<BusinessAd>().detailsBtn)
        {
            businessAd.GetComponent<BusinessAd>().pageGrid.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            GameObject.Find("BackButton").GetComponent<BackButton>().CacheCurrentPage();
        }

        if (Page.name == "Mega Deal")
        {
            businessAd.GetComponent<BusinessAd>().pageGrid.transform.parent.gameObject.SetActive(false);
            GameObject[] pageBtns = businessAd.GetComponent<BusinessAd>().pageBtns;
            GameObject.Find("BackButton").GetComponent<BackButton>().CacheCurrentPage();
            for (int i = 0; i < pageBtns.Length; ++i)
            {
                if (pageBtns[i].GetComponent<PageButton>().Page != null &&
                    pageBtns[i].GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage != null)
                    pageBtns[i].GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage.SetActive(false);
            }

        }
        else
        {
            businessAd.GetComponent<BusinessAd>().MegaDealPage.SetActive(false);
        }

        Page.GetComponent<Page>().OnPageSwitch();

        if (Page.GetComponent<VideoHandler>())
            Page.GetComponent<VideoHandler>().OnPageSwitch();

    }


}
