using UnityEngine;
using System.Collections;

public class Page : MonoBehaviour 
{
    public Vector2 maxSize = new Vector2(2048.0f, 1636.0f);
    public UITexture[] images;
    public Texture2D narratorTexture;
    public string speechBubbleText = "";
    public GameObject detailsPage;
    public string title = "Details";

	// Use this for initialization
	void Start () 
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = new Vector3(0, 0, -300);
	}

    void OnEnable()
    {
        GameObject detailsBtn;
        detailsBtn = GameObject.Find("BusinessAd").GetComponent<BusinessAd>().detailsBtn;

        if (detailsPage != null)
        {
            detailsBtn.SetActive(true);
            detailsBtn.GetComponent<PageButton>().Page = detailsPage;
            detailsBtn.gameObject.GetComponentInChildren<UILabel>().text = detailsPage.GetComponent<Page>().title;
        }
        else
            detailsBtn.SetActive(false);

        GameObject.Find("Narrator").GetComponentInChildren<UITexture>().mainTexture = narratorTexture;
        GameObject.Find("Narrator").GetComponent<Narrator>().speechBubbleObject.SetActive(true);
        GameObject.Find("Narrator").GetComponent<Narrator>().speechBubbleObject.GetComponentInChildren<UILabel>().text = speechBubbleText;
        //GameObject.Find("SpeechBubble").GetComponentInChildren<UILabel>().text = speechBubbleText;

        if (speechBubbleText == "")
             GameObject.Find("Narrator").GetComponent<Narrator>().speechBubbleObject.SetActive(false);
        else
            GameObject.Find("Narrator").GetComponent<Narrator>().speechBubbleObject.SetActive(true);
    }
	

}
