using UnityEngine;
using System.Collections;
using System.Linq;

public class BusinessCategoryMgr : MonoBehaviour 
{
    private FormMallDetailedCategories _MallCateGories = new FormMallDetailedCategories();
    public GameObject businessCategory;
    private BusinessManager _businessManager;
    private Vector3 startPos;
    private Vector4 clipPos;

    void Awake()
    {
        _businessManager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        startPos = transform.localPosition;
        clipPos = GetComponent<UIPanel>().clipRange;
    }

    void OnDisable()
    {
        transform.localPosition = startPos;
        GetComponent<UIPanel>().clipRange = clipPos;
        Destroy(GetComponent<SpringPanel>());//.enabled = true;
        //Application.LoadLevel(5);
    }

    public void Populate(eMallSubcategory cat)
    {
        Debug.Log(cat);
        _MallCateGories.GetSubCategories(cat);
        Transform grid = transform.Find("Grid");
        GameObject business;

        foreach(string categoryName in _MallCateGories.theCategories.OrderBy(x=>x))
        {
            business = (GameObject)Instantiate(businessCategory, Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f)));
            business.transform.parent = grid;
            business.transform.Find("Label").GetComponent<UILabel>().text = categoryName;

            if (_businessManager.businessesByCategory.ContainsKey(categoryName))
                Destroy(business.transform.Find("NoBusiness").gameObject);

            grid.GetComponent<UIGrid>().Reposition();
            business.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            business.GetComponent<BusinessSubCatMgr>().Populate(categoryName);
        }
        
    }
}
