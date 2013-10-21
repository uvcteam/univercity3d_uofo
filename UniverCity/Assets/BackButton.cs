using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	// Use this for initialization
    private GameObject businessAd;
    private Page prevPage;

	void Start () 
    {
        businessAd = GameObject.Find("BusinessAd");
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        GoBack();
	}

    public void GoBack()
    {

        if (prevPage == null)
            businessAd.GetComponent<BusinessAd>().OnExitClicked();
        else
        {
            prevPage.pageBtn.GetComponent<PageButton>().GoToPage();
            prevPage = null;
        }

    }

    public void CacheCurrentPage()
    {
        prevPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject.GetComponent<Page>();
    }
}
