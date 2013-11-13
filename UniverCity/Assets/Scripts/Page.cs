using UnityEngine;
using System.Collections;
using System;

public class Page : MonoBehaviour 
{
    public Vector2 maxSize = new Vector2(2048.0f, 1636.0f);
    public GameObject[] images;
    public Texture2D narratorTexture = null;
    public string speechBubbleText = "";
    public GameObject detailsPage;
    public GameObject pageBtn;
    public BusinessAd businessAd;
    public AdPage adPage;

	// Use this for initialization
	void Start () 
    {
        transform.localScale = new Vector3(1, 1, 1);

        if (gameObject.name == "Mega Deal")
            transform.localPosition = new Vector3(0, 0, -400);
        else
            transform.localPosition = new Vector3(150, 0, -400);

        if (gameObject != businessAd.MegaDealPage)
            businessAd.background.GetComponent<UISlicedSprite>().color = adPage.Background.TopColor;
        else
            businessAd.background.GetComponent<UISlicedSprite>().color = businessAd.MegaDealPage.GetComponent<MegaDeal>().megaDealColor;
	}

    public void OnPageSwitch()
    {

        GameObject detailsBtn;
        GameObject narrator = businessAd.narrator;
        detailsBtn = businessAd.detailsBtn;

        if (gameObject != businessAd.MegaDealPage)
            businessAd.background.GetComponent<UISlicedSprite>().color = adPage.Background.TopColor;
        else
            businessAd.background.GetComponent<UISlicedSprite>().color = businessAd.MegaDealPage.GetComponent<MegaDeal>().megaDealColor;

        if (detailsPage != null)
        {
            detailsBtn.SetActive(true);
            detailsBtn.GetComponent<PageButton>().Page = detailsPage;
            detailsBtn.gameObject.GetComponentInChildren<UILabel>().text = detailsPage.GetComponent<Page>().adPage.Title;
        }
        else
            detailsBtn.SetActive(false);

        if (narratorTexture == null)
            narrator.GetComponentInChildren<UITexture>().mainTexture = narratorTexture;
        else
        {
            businessAd.narrator.SetActive(true);
            narrator.GetComponent<Narrator>().texture.transform.localScale = new Vector3(400.0f,400.0f,0.0f);
            UnivercityTools.ScaleImage(narrator.GetComponent<Narrator>().texture, narratorTexture);
        }

        if (speechBubbleText == "")
            narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(false);
        else
            narrator.GetComponent<Narrator>().speechBubbleObject.SetActive(true);

        if (narrator.GetComponent<Narrator>().speechBubbleObject.GetComponentInChildren<UILabel>() != null)
            narrator.GetComponent<Narrator>().speechBubbleObject.GetComponentInChildren<UILabel>().text = speechBubbleText;


    }
	
	public void Purge()
	{
		DestroyImmediate(narratorTexture, true);
		foreach (GameObject go in images)
			DestroyImmediate(go.GetComponent<UITexture>().mainTexture, true);
	}

    public void OnPress(bool pressed)
    {
        if (pressed == false)
        {
            businessAd.pageGrid.GetComponent<UICenterOnChild>().Recenter();
            GameObject centerPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject;


            if ( GetComponent<VideoHandler>() != null)
                GetComponent<VideoHandler>().OnPageLeave();

            //When scrolling, the page needs to cache itself so caching is disabled in GoToPage
            pageBtn.GetComponent<PageButton>().backBtn.CachePage(this);
            centerPage.GetComponent<Page>().pageBtn.GetComponent<PageButton>().cachePage = false;
            centerPage.GetComponent<Page>().pageBtn.GetComponent<PageButton>().GoToPage();
            centerPage.GetComponent<Page>().pageBtn.GetComponent<PageButton>().cachePage = true;
        }
    }

    public void InitComponents(AdPage adPage, BusinessAd businessAd, int pageCount)
    {
        this.adPage = adPage;
        this.businessAd = businessAd;
        pageBtn = GameObject.Find("pageBtn" + pageCount);
        pageBtn.SetActive(true);
        pageBtn.GetComponent<PageButton>().Page = gameObject;
        pageBtn.gameObject.GetComponentInChildren<UILabel>().text = adPage.Title;

        for (int i = 0; i < adPage.Parts.Count && i < images.Length; ++i)
        {
            if (adPage.Parts[i].Type == MediaType.Image)
                UnivercityTools.ScaleImage(images[i], adPage.Parts[i].Image);
        }   

        foreach (AdMedia media in adPage.Parts)
        {
            if (media.Type == MediaType.Video)
            {
                GetComponent<VideoHandler>().MoviePlayer = businessAd.MoviePlayer;
                GetComponent<VideoHandler>().URL = media.VideoURL;
                GetComponent<VideoHandler>().videoHeight = media.Height;
                GetComponent<VideoHandler>().videoWidth = media.Width;
                GetComponent<VideoHandler>().VideoButton.SetActive(true);

                if (GetComponentInChildren<UITexture>() != null)
                    GetComponentInChildren<UITexture>().gameObject.SetActive(false);
            }
        }
        if (adPage.More != null)
        {
            detailsPage = (GameObject)Instantiate(Resources.Load("Prefabs/Ad Player/" + businessAd.pageDictionary[adPage.More.Type], typeof(GameObject)));
            detailsPage.transform.parent = businessAd.gameObject.transform;
            detailsPage.GetComponent<Page>().businessAd = businessAd;
            detailsPage.GetComponent<Page>().adPage = adPage.More;
            detailsPage.GetComponent<Page>().pageBtn = businessAd.detailsBtn;

            for (int i = 0; i < adPage.More.Parts.Count && i < images.Length; ++i)
            {
                if (adPage.More.Parts[i].Type == MediaType.Image)
                    UnivercityTools.ScaleImage(detailsPage.GetComponent<Page>().images[i], adPage.More.Parts[i].Image);
            }

            detailsPage.SetActive(false);

            businessAd.SetUpNarratorForPage(detailsPage.GetComponent<Page>(), adPage.More);

            businessAd.Pages.Add(detailsPage);
        }
    }

}
