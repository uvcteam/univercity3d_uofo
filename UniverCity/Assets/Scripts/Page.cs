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
    public string title = "Details";
	public Color pageColor = Color.white;
    public GameObject pageBtn;

	// Use this for initialization
	void Start () 
    {
        transform.localScale = new Vector3(1, 1, 1);

        if (gameObject.name == "Mega Deal")
            transform.localPosition = new Vector3(0, 0, -300);
        else
            transform.localPosition = new Vector3(150, 0, -300);

        GameObject.Find("BusinessAd").GetComponent<BusinessAd>().background.GetComponent<UISlicedSprite>().color = pageColor;
	}

    public void OnPageSwitch()
    {

        GameObject businessAd = GameObject.Find("BusinessAd");
        GameObject detailsBtn;
        GameObject narrator = GameObject.Find("BusinessAd").GetComponent<BusinessAd>().narrator;
        detailsBtn = GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn;

        if (detailsPage != null)
        {
            detailsBtn.SetActive(true);
            detailsBtn.GetComponent<PageButton>().Page = detailsPage;
            detailsBtn.gameObject.GetComponentInChildren<UILabel>().text = detailsPage.GetComponent<Page>().title;
        }
        else
            detailsBtn.SetActive(false);

        if (narratorTexture == null)
            narrator.GetComponentInChildren<UITexture>().mainTexture = narratorTexture;
        else
        {
            businessAd.GetComponent<BusinessAd>().narrator.SetActive(true);
            businessAd.GetComponent<BusinessAd>().ScaleImage(narrator.GetComponent<Narrator>().texture, narratorTexture);
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

    //public void LateUpdate()
    //{
    //    bool touchEnded = false;

    //    for (int i = 0; i < Input.touchCount; ++i)
    //    {
    //        if(Input.touches[i].phase == TouchPhase.Ended)
    //            touchEnded = true;
    //        else
    //            touchEnded = false;
    //    }

    //    if (touchEnded || Input.GetMouseButtonUp(0))
    //    {
    //        GameObject businessAd = GameObject.Find("BusinessAd");
    //        GameObject centerPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject;
    //        if (centerPage == gameObject)
    //            pageBtn.GetComponent<PageButton>().GoToPage();
    //    }
    //}

    public void OnPress(bool pressed)
    {
        if (pressed == false)
        {
            GameObject businessAd = GameObject.Find("BusinessAd");
            businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().Recenter();
            GameObject centerPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject;

            if (centerPage != gameObject)
            {
                centerPage.GetComponent<Page>().pageBtn.GetComponent<PageButton>().GoToPage();
            }
        }
    }

}
