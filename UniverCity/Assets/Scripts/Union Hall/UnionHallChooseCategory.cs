using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System.Collections;

public class UnionHallChooseCategory : MonoBehaviour
{
    public GameObject browseAll = null;
    public Transform buttonTransform = null;
    public Transform catButton = null;
    public Transform scrollPanel = null;
    public UIButton chooseButton = null;

    public UnionHallEvent newEvent = null;
 
    void OnEnable()
    {
        newEvent = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        if (newEvent.interests.Count > 0)
            chooseButton.isEnabled = true;
        else
            chooseButton.isEnabled = false;
    }

    void Awake()
    {
        GetCategories();
    }

    void GetCategories()
    {
        foreach (SocialInterest cat in GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().SocialCategories)
        {
            Transform newCat = Instantiate(catButton, buttonTransform.position, buttonTransform.rotation) as Transform;
            newCat.Find("CategoryName").GetComponent<UILabel>().text = cat.Name;
            newCat.parent = scrollPanel.Find("Grid");
            newCat.localScale = buttonTransform.localScale;
            newCat.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
            newCat.GetComponent<UIButtonMessage>().target = gameObject;
            newCat.gameObject.name = "btn Category";
            scrollPanel.Find("Grid").GetComponent<UIGrid>().Reposition();
            
            //if (newEvent.interests.Contains(cat.Id))
            //    newCat.Find("Background").GetComponent<UISlicedSprite>().color = Color.red;
            //else
                newCat.Find("Background").GetComponent<UISlicedSprite>().color = Color.white;
        }
    }

    void OnChooseClicked()
    {
        browseAll.SetActive(true); 
        browseAll.GetComponent<UnionHallEngagementSettings>().responseSave.isEnabled = true;
        gameObject.SetActive(false);
    }

    void OnCategoryClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "btn Category")
        {
            UILabel categoryName = UICamera.lastHit.collider.gameObject.transform.Find("CategoryName").GetComponent<UILabel>();
            UISlicedSprite background =
                UICamera.lastHit.collider.gameObject.transform.Find("Background").GetComponent<UISlicedSprite>();

            foreach (SocialInterest cat in GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().SocialCategories)
            {
                if (cat.Name == categoryName.text)
                {
                    if (!newEvent.interests.Contains(cat.Id))
                    {
                        newEvent.interests.Add(cat.Id);
                        background.color = new Color(0.49f, 0.78f, 0.20f, 1.0f);
                    }
                    else
                    {
                        newEvent.interests.Remove(cat.Id);
                        background.color = Color.white;
                    }
                }
            }
        }

        if (newEvent.interests.Count > 0)
            chooseButton.isEnabled = true;
        else
            chooseButton.isEnabled = false;
    }
}