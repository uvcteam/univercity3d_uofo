using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BusinessAd : MonoBehaviour
{
    public GameObject pageGrid;
    public AdManager adManager;
    public GameObject background;
    public GameObject detailsBtn;
    public GameObject loadingDialog;
    public GameObject MegaDealPage;
    public GameObject MegaDealBtn;
    public GameObject MembersOnlyBtn;
    public GameObject BusinessCard;
	public GameObject MoviePlayer;
    public GameObject[] pageBtns;
    public GameObject[] objectsToHide;
	public UITexture[] texturesToPurge;
	public GameObject narrator;
    public GameObject RotateIconPanel;
    public GameObject backBtn;
    public GameObject rateBtn;
    public GameObject facebookBtn;
    public UILabel businessName;
    public bool hasMegaDeal = false;
    public bool hasMembersOnly = false;
    public long sessionId = 0;
    public IDictionary<AdPageType, string> pageDictionary = new Dictionary<AdPageType, string>();

    public List<GameObject> Pages = new List<GameObject>();

    private int numPateBtn = 4;
    private UIAnchor.Side side = UIAnchor.Side.Left;


    void SetUpDictionary()
    {
        pageDictionary.Add(AdPageType.One, "SingleImagePage");
        pageDictionary.Add(AdPageType.Two, "DoubleImagePage");
        pageDictionary.Add(AdPageType.Three, "TripleImagePage");
        pageDictionary.Add(AdPageType.Many, "ScrollableGridPage");
    }

    void OnEnable()
    {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;

        transform.localPosition = Vector3.zero;
        side = GameObject.Find("Anchor").GetComponent<UIAnchor>().side;
        GameObject.Find("Anchor").GetComponent<UIAnchor>().side = UIAnchor.Side.Center;
        GameObject.Find("TheMovie").transform.localScale = new Vector3(2256.0f, 1256.0f, 0);
    }

    void Awake()
    {
		narrator = GameObject.Find("Narrator");
        transform.parent = GameObject.Find("Anchor").transform;
        transform.localScale = new Vector3(1, 1, 1);
        if (Application.loadedLevel != 1)
        {
            transform.localPosition = new Vector3(0, -1386, -500);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
        adManager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
        SetUpDictionary();

        foreach (GameObject btn in pageBtns)
            btn.SetActive(true);
        detailsBtn.SetActive(true);

    }

    public void LateUpdate()
    {
        if ((Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            && (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown))
            RotateIconPanel.SetActive(true);
        else
            RotateIconPanel.SetActive(false);
    }

	public void OnExitClicked()
	{

        ShowObjects();

        foreach (GameObject page in Pages)
		{
			page.GetComponent<Page>().Purge();
            DestroyImmediate(page);
		}

		Pages.Clear();
		
        foreach (GameObject page in GameObject.FindGameObjectsWithTag("Page"))
        {
            page.SetActive(false);
        }
		
		foreach (UITexture tex in texturesToPurge)
		{
			DestroyImmediate(tex.mainTexture, true);
		}

        MegaDealPage.SetActive(false);
        pageGrid.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(false);
        BusinessCard.SetActive(false);
		Resources.UnloadUnusedAssets();

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;

        UnivercityTools.TrackUserAction(adManager.BusinessID, "", "close", sessionId.ToString());

        GameObject.Find("Anchor").GetComponent<UIAnchor>().side = side;
	}

    public IEnumerator SetUpAd(int businessID = 16, string businessName = "")
    {

        AdData adInfo;
        int pageCount = 1;
        this.businessName.text = businessName;
		
		bool isDesktop = Application.platform == RuntimePlatform.WindowsWebPlayer || 
			Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXEditor;

        if (isDesktop || Screen.orientation == ScreenOrientation.Landscape)
            loadingDialog.SetActive(true);

        StartCoroutine(adManager.GetAd(businessID));

        while (!adManager.adReady)
            yield return new WaitForSeconds(0.1f);
		Debug.Log ("Getting business.");

        loadingDialog.SetActive(false);


        if (adManager.hasAd == false)
        {
            BusinessCard.GetComponent<UITexture>().mainTexture = adManager.BusinessCard;
            BusinessCard.SetActive(true);
            MegaDealBtn.SetActive(false);
            MembersOnlyBtn.SetActive(false);
            backBtn.SetActive(false);
            rateBtn.SetActive(false);
            facebookBtn.SetActive(false);

            foreach (GameObject btn in pageBtns)
                btn.SetActive(false);
            detailsBtn.SetActive(false);
			narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(false);
			narrator.SetActive(false);
        }
        else
        {
            backBtn.SetActive(true);
            rateBtn.SetActive(true);
            facebookBtn.SetActive(true);

			if(narrator != null)
			{
				narrator.SetActive(true);
				narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(true);
			}
			
            MembersOnlyBtn.SetActive(true);
            MembersOnlyBtn.GetComponent<UIButton>().isEnabled = false;

            adInfo = adManager.AdInfo;


            foreach (GameObject btn in pageBtns)
            {
                btn.SetActive(true);
            }

            for (int i = 0; i < adInfo.Pages.Count; ++i)
            {
                SetUpPage(adInfo.Pages[i], pageCount);
                ++pageCount;
            }

            if (adInfo.Mega != null)
            {
                SetUpMegaDeal(adInfo);
                MegaDealBtn.SetActive(false);
                StartCoroutine(GameObject.Find("Creature").GetComponent<VirtualMallCreature>().Present());
            }
            else
            {
                GameObject.Find("Creature").GetComponent<VirtualMallCreature>().IsDone = true;
                MegaDealBtn.SetActive(false);
                hasMegaDeal = false;
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
                detailsBtn.SetActive(false);

            UnivercityTools.ScaleImage(narrator.GetComponent<Narrator>().texture, adInfo.Pages[0].Expert.Image);
            narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(true);
            narrator.GetComponent<Narrator>().speechBubbleObject.GetComponentInChildren<UILabel>().text = adInfo.Pages[0].Narrative;
            pageGrid.GetComponent<UIGrid>().repositionNow = true;
            pageGrid.GetComponent<UIGrid>().Reposition();
            pageBtns[0].GetComponent<PageButton>().GoToPage();

        }
        HideObjects();
        sessionId = DateTime.Now.Ticks;

        UnivercityTools.TrackUserAction(adManager.BusinessID, "", "start", sessionId.ToString());
    }
    private void SetUpPage(AdPage adPage, int pageCount)
    {
        GameObject pageObject = (GameObject)Instantiate(Resources.Load("Prefabs/Ad Player/" + pageDictionary[adPage.Type], typeof(GameObject)));

        pageObject.transform.parent = pageGrid.transform;

        Page page = pageObject.GetComponent<Page>();

        page.InitComponents(adPage, this, pageCount);

        SetUpNarratorForPage(page, adPage);

        Pages.Add(pageObject);
    }

    private void SetUpMegaDeal(AdData adInfo)
    {
        MegaDealPage.GetComponent<Page>().businessAd = this;
        hasMegaDeal = true;
        MegaDealBtn.SetActive(true);
        MegaDealBtn.GetComponent<UIButton>().isEnabled = true;
        MegaDeal megaDeal = MegaDealPage.GetComponent<MegaDeal>();
        megaDeal.InitComponents(adInfo);
        MegaDealPage.GetComponent<Page>().pageBtn = MegaDealBtn;
    }

    public void SetUpNarratorForPage(Page page, AdPage adPage)
    {
        page.narratorTexture = adPage.Expert.Image;
        page.speechBubbleText = adPage.Narrative;
    }
	

    public void MovieFinished()
    {
        string trackURL = "http://www.univercity3d.com/univercity/track?id=";

        trackURL += adManager.BusinessID;
        trackURL += "&title=";
        trackURL += "&event=media";
        trackURL += "&play_id=" + sessionId;

        if (PlayerPrefs.GetInt("loggedIn") == 1)
            trackURL += "&token=" +
                        GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;

        Debug.Log("Sending: " + trackURL);
        WWW track = new WWW(trackURL);
    }

    public void HideObjects()
    {
        foreach (GameObject obj in objectsToHide)
            obj.SetActive(false);
    }

    public void ShowObjects()
    {
        foreach (GameObject obj in objectsToHide)
            obj.SetActive(true);
    }

}