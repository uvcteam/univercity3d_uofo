using UnityEngine;
using System.Collections;

public class CategoryButton : MonoBehaviour 
{
    public GameObject topAnchor;
    public eMallSubcategory category;

    void OnPress()
    {
        topAnchor.gameObject.SetActiveRecursively(true);
        topAnchor.GetComponent<TopBarManager>().prevPanel.SetActiveRecursively(false);
        topAnchor.GetComponent<TopBarManager>().currentPanel.SetActiveRecursively(true);
        topAnchor.GetComponent<TopBarManager>().currentPanel.GetComponent<BusinessCategoryMgr>().Populate(category);
        GameObject.Find("PageName").GetComponent<UILabel>().text = transform.Find("Name").GetComponent<UILabel>().text;

    }
}
