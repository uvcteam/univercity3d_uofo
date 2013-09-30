using UnityEngine;
using System.Collections;

public class CategoryButton : MonoBehaviour 
{
    public GameObject topAnchor;
    public eMallSubcategory category;

    void ActivateTopAnchor()
    {
        topAnchor.gameObject.SetActive(true);
        topAnchor.GetComponent<TopBarManager>().prevPanel.SetActive(false);
        topAnchor.GetComponent<TopBarManager>().currentPanel.SetActive(true);
        topAnchor.GetComponent<TopBarManager>().currentPanel.GetComponent<BusinessCategoryMgr>().Populate(category);
        GameObject.Find("PageName").GetComponent<UILabel>().text = transform.Find("Name").GetComponent<UILabel>().text;

    }
}
