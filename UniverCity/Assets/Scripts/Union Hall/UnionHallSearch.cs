using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;

public class UnionHallSearch : MonoBehaviour
{
    public GameObject browseAll = null;
    public Transform buttonTransform = null;
    public Transform catButton = null;
    public Transform scrollPanel = null;
    public GameObject browseSearch = null;
 
    void Awake()
    {
        GetCategories();
    }

    void GetCategories()
    {
        foreach (SocialInterest cat in GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().Categories)
        {
            Transform newCat = Instantiate(catButton, buttonTransform.position, buttonTransform.rotation) as Transform;
            newCat.Find("CategoryName").GetComponent<UILabel>().text = cat.Name;
            newCat.parent = scrollPanel.Find("Grid");
            newCat.localScale = buttonTransform.localScale;
            newCat.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
            newCat.GetComponent<UIButtonMessage>().target = gameObject;
            newCat.gameObject.name = "btn Category";
            scrollPanel.Find("Grid").GetComponent<UIGrid>().Reposition();
        }
    }

    void OnBackClicked()
    {
        browseAll.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnCategoryClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "btn Category")
        {
            UILabel categoryName = UICamera.lastHit.collider.gameObject.transform.Find("CategoryName").GetComponent<UILabel>();
            browseSearch.GetComponent<UnionHallBrowseSearch>().currentCategory = categoryName.text;
            browseSearch.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}