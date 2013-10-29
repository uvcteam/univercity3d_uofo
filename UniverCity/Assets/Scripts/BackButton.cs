using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackButton : MonoBehaviour {

	// Use this for initialization
    private BusinessAd businessAd;
    private Stack<Page> prevPages = new Stack<Page>();

	void Start () 
    {
        businessAd = GameObject.Find("BusinessAd").GetComponent<BusinessAd>();
        Debug.Log(businessAd.name);
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoBack();
	}

    void OnDisable()
    {
        prevPages.Clear();
    }

    public void GoBack()
    {
        if (prevPages.Count != 0)
        {
            Debug.Log("Poped " + prevPages.Peek().adPage.Title);
            Page prevPage = prevPages.Pop();
            prevPage.pageBtn.GetComponent<PageButton>().cachePage = false;
            prevPage.pageBtn.GetComponent<PageButton>().GoToPage();
            prevPage.pageBtn.GetComponent<PageButton>().cachePage = true;
        }

    }

    public void CacheCurrentPage()
    {
        if (businessAd.pageGrid.GetComponent<UICenterOnChild>().centeredObject != null)
        {
            Debug.Log("Cached");
            prevPages.Push(businessAd.pageGrid.GetComponent<UICenterOnChild>().centeredObject.GetComponent<Page>());
        }
    }
}
