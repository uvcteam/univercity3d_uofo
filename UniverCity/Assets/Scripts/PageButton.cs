using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour 
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    public GameObject Page;
    private GameObject businessAd;
    public BackButton backBtn;
    public bool cachePage = true;

    public void Awake()
    {
        businessAd = GameObject.Find("BusinessAd");
        backBtn = GameObject.Find("BackButton").GetComponent<BackButton>();
    }

    public void GoToPage()
    {
        string trackURL = serverURL + "track?id=";

        if (businessAd == null)
        {
            Debug.Log("BusinessAd not found");
            return;
        }

        trackURL += businessAd.GetComponent<BusinessAd>().adManager.BusinessID;
        if (Page.name == "Mega Deal")
        {
            UnivercityTools.TrackUserAction(businessAd.GetComponent<BusinessAd>().adManager.BusinessID, "", "deal", businessAd.GetComponent<BusinessAd>().sessionId.ToString());
        }
        else if (Page.name == "Members Only")
        {
            UnivercityTools.TrackUserAction(businessAd.GetComponent<BusinessAd>().adManager.BusinessID, "", "discount", businessAd.GetComponent<BusinessAd>().sessionId.ToString());
        }
        else
        {
            UnivercityTools.TrackUserAction(businessAd.GetComponent<BusinessAd>().adManager.BusinessID, transform.Find("Label").GetComponent<UILabel>().text,
                "click", businessAd.GetComponent<BusinessAd>().sessionId.ToString());
        }

        SelectButton();
        businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().Recenter(Page);

    }

    public void SelectButton()
    {
        if (cachePage == true)
        {
            backBtn.CacheCurrentPage();
        }

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
        }

        if (Page.name == "Mega Deal")
        {
            businessAd.GetComponent<BusinessAd>().pageGrid.transform.parent.gameObject.SetActive(false);

            GameObject[] pageBtns = businessAd.GetComponent<BusinessAd>().pageBtns;

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

        if (Page.GetComponent<VideoHandler>() && this.active == true)
            StartCoroutine(Page.GetComponent<VideoHandler>().OnPageSwitch());

    }


}
