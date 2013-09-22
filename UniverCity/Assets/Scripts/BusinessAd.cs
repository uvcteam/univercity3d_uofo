using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BusinessAd : MonoBehaviour
{
    //public int businessID = 0;
    public AdManager adManager;
    public GameObject background;
    public GameObject detailsBtn;
    public GameObject loadingDialog;
    public GameObject MegaDealPage;
    public GameObject MegaDealBtn;
    public GameObject BusinessCard;
    public GameObject[] pageBtns;

    private List<GameObject> _pages = new List<GameObject>();
    private IDictionary<AdPageType, string> pageDictionary = new Dictionary<AdPageType, string>();
    private int numPateBtn = 4;


    void SetUpDictionary()
    {
        pageDictionary.Add(AdPageType.One, "SingleImagePage");
        pageDictionary.Add(AdPageType.Two, "DoubleImagePage");
        pageDictionary.Add(AdPageType.Three, "TripleImagePage");
        pageDictionary.Add(AdPageType.Many, "ScrollableGridPage");
    }

 

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        transform.parent = GameObject.Find("Anchor").transform;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = new Vector3(0, 0, -500);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        adManager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
        SetUpDictionary();

        foreach (GameObject btn in pageBtns)
            btn.SetActive(true);
        detailsBtn.SetActive(true);

    }

	void OnExitClicked()
	{
        foreach (GameObject page in _pages)
        {
            Destroy(page);
        }

        foreach (GameObject page in GameObject.FindGameObjectsWithTag("Page"))
        {
            page.SetActive(false);
        }

        MegaDealPage.SetActive(false);
        gameObject.SetActive(false);
        BusinessCard.SetActive(false);
        Screen.orientation = ScreenOrientation.AutoRotation;
	}

    public IEnumerator SetUpAd(int businessID = 16)
    {
        AdData adInfo;
        GameObject narrator = GameObject.Find("Narrator");
        int pageCount = 1;
        loadingDialog.SetActive(true);
        narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(false);

        StartCoroutine(adManager.GetAd(businessID)); //16 for test reasons

        while (!adManager.adReady)
            yield return new WaitForSeconds(0.1f);

        loadingDialog.SetActive(false);


        if (adManager.hasAd == false)
        {
            BusinessCard.GetComponent<UITexture>().mainTexture = adManager.BusinessCard;
            BusinessCard.SetActive(true);

            foreach (GameObject btn in pageBtns)
                btn.SetActive(false);
            detailsBtn.SetActive(false);
            MegaDealBtn.SetActive(false);
            narrator.GetComponentInChildren<UITexture>().mainTexture = null;
        }
        else
        {
            adInfo = adManager.AdInfo;
            narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(true);

            foreach (GameObject btn in pageBtns)
            {
                btn.SetActive(true);
            }

            foreach (AdPage adPage in adInfo.Pages)
            {
                SetUpPage(adPage, pageCount);
                ++pageCount;
            }

            if (adInfo.Mega != null)
            {
                MegaDealBtn.SetActive(true);
                MegaDeal megaDeal = MegaDealPage.GetComponent<MegaDeal>();
                megaDeal.Description.GetComponent<UILabel>().text = adInfo.Mega.Description;
                megaDeal.End.GetComponent<UILabel>().text = "Hurry! Deal ends " + adInfo.Mega.End;
                megaDeal.List.GetComponent<UILabel>().text = adInfo.Mega.List.ToString();
                megaDeal.Price.GetComponent<UILabel>().text = adInfo.Mega.Price.ToString();
                megaDeal.Title.GetComponent<UILabel>().text = adInfo.Mega.Title;
            }

            //Make first page the defualt
            for (int i = pageCount; i <= numPateBtn; ++i) //disable unused buttons
                GameObject.Find("pageBtn" + i).SetActive(false);

            if (GameObject.Find("pageBtn1").GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage != null)
            {
                detailsBtn.SetActive(true);
                detailsBtn.GetComponent<PageButton>().Page =
                    GameObject.Find("pageBtn1").GetComponent<PageButton>().Page.GetComponent<Page>().detailsPage;
                detailsBtn.gameObject.GetComponentInChildren<UILabel>().text = adInfo.Pages[0].More.Title;
            }
            else
                GameObject.Find("DetailsBtn").SetActive(false);

            //narrator.GetComponentInChildren<UITexture>().mainTexture = adInfo.Pages[0].Expert.Image;
            ScaleImage(narrator.GetComponent<Narrator>().texture, adInfo.Pages[0].Expert.Image);
            narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(true);
            narrator.GetComponent<Narrator>().speechBubbleObject.GetComponentInChildren<UILabel>().text = adInfo.Pages[0].Narrative;
        }

       

    }
    private void SetUpPage(AdPage adPage, int pageCount)
    {
        GameObject pageBtn;
        Page page;
        GameObject pageObject = (GameObject)Instantiate(Resources.Load("Prefabs/Ad Player/" + pageDictionary[adPage.Type], typeof(GameObject)));
        Debug.Log(pageObject.name);
        page = pageObject.GetComponent<Page>();
        page.transform.parent = gameObject.transform;


        for (int i = 0; i < adPage.Parts.Count && i < page.images.Length; ++i)
        {
            //page.images[i].mainTexture = adPage.Parts[i].Image;
            ScaleImage(page.images[i], adPage.Parts[i].Image);
        }

        pageBtn = GameObject.Find("pageBtn" + pageCount);
        pageBtn.SetActive(true);
        pageBtn.GetComponent<PageButton>().Page = pageObject;
        pageBtn.gameObject.GetComponentInChildren<UILabel>().text = adPage.Title;
        page.title = adPage.Title;

        if (adPage.More != null)
        {
            page.detailsPage = (GameObject)Instantiate(Resources.Load("Prefabs/Ad Player/" + pageDictionary[adPage.More.Type], typeof(GameObject)));
            page.detailsPage.transform.parent = gameObject.transform;
            page.detailsPage.GetComponent<Page>().title = adPage.More.Title;

            for (int i = 0; i < adPage.More.Parts.Count && i < page.images.Length; ++i)
            {
                ScaleImage(page.detailsPage.GetComponent<Page>().images[i], adPage.More.Parts[i].Image);
            }

            page.detailsPage.SetActive(false);

            SetUpNarratorForPage(page.detailsPage.GetComponent<Page>(), adPage.More);

            _pages.Add(page.detailsPage);
        }

        SetUpNarratorForPage(page, adPage);

        if (pageCount > 1)
            pageObject.SetActive(false);

        _pages.Add(pageObject);
    }

    public void ScaleImage(GameObject destination, Texture2D source)
    {
        float newWidth = (destination.transform.localScale.y / source.height) * source.width;
        float newHeight = (destination.transform.localScale.x / source.width) * source.height;

        if (source.width > source.height)
            destination.transform.localScale = new Vector3(
                destination.transform.localScale.x,
                newHeight,
                0.0f);
        else
            destination.transform.localScale = new Vector3(
                newWidth,
                destination.transform.localScale.y,
                0.0f);

        destination.GetComponent<UITexture>().mainTexture = source;
    }

    private void SetUpNarratorForPage(Page page, AdPage adPage)
    {
        page.narratorTexture = adPage.Expert.Image;
        page.speechBubbleText = adPage.Narrative;
    }
}