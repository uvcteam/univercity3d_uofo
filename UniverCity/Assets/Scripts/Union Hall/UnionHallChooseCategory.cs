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

    private List<SocialInterest> categories = new List<SocialInterest>();
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
        StartCoroutine(GetCategories());
    }

    IEnumerator GetCategories()
    {
        string cURL = "http://www.univercity3d.com/univercity/ListSocialInterests";
        int catId = 0;
        string catName = "";

        WWW page = new WWW(cURL);
        yield return page;

        //StreamWriter sw = File.CreateText("businesses.dat");
        //sw.Write(page.text);
        //sw.Close();

        // Create an IList of all of the businesses returned to me.
        IList catInfo = Json.Deserialize(page.text) as IList;

        foreach (Dictionary<string, object> cat in catInfo)
        {
            catId = Convert.ToInt32(cat["id"]);
            catName = cat["int"] as string;
            categories.Add(new SocialInterest(catId, catName));
        }

        foreach (SocialInterest cat in categories)
        {
            Transform newCat = Instantiate(catButton, buttonTransform.position, buttonTransform.rotation) as Transform;
            newCat.Find("CategoryName").GetComponent<UILabel>().text = cat.Name;
            newCat.parent = scrollPanel;
            newCat.localScale = buttonTransform.localScale;
            newCat.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
            newCat.GetComponent<UIButtonMessage>().target = gameObject;
            newCat.gameObject.name = "btn Category";
            scrollPanel.GetComponent<UIGrid>().Reposition();
            
            if (newEvent.interests.Contains(cat.Id))
                newCat.Find("Background").GetComponent<UISlicedSprite>().color = Color.red;
            else
                newCat.Find("Background").GetComponent<UISlicedSprite>().color = Color.white;
        }
    }

    void OnChooseClicked()
    {
        browseAll.SetActiveRecursively(true); 
        browseAll.GetComponent<UnionHallEngagementSettings>().responseSave.isEnabled = true;
        gameObject.SetActiveRecursively(false);
    }

    void OnCategoryClicked()
    {
        if (UICamera.lastHit.collider.gameObject.name == "btn Category")
        {
            UILabel categoryName = UICamera.lastHit.collider.gameObject.transform.Find("CategoryName").GetComponent<UILabel>();
            UISlicedSprite background =
                UICamera.lastHit.collider.gameObject.transform.Find("Background").GetComponent<UISlicedSprite>();

            foreach (SocialInterest cat in categories)
            {
                if (cat.Name == categoryName.text)
                {
                    if (!newEvent.interests.Contains(cat.Id))
                    {
                        newEvent.interests.Add(cat.Id);
                        background.color = Color.red;
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