using UnityEngine;
using System.Collections;
using System.Linq;

public class BusinessCategoryMgr : MonoBehaviour 
{
    public GameObject preferredBusinesses;
    public GameObject businessCategory;
    public Camera camera;
    public UISlicedSprite hidePanel;

    private FormMallDetailedCategories _MallCateGories = new FormMallDetailedCategories();
    private BusinessManager _businessManager;
    private Vector3 startPos;
    private Vector4 clipPos;
	private eMallSubcategory _currentCategory;

    void Awake()
    {
        _businessManager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        startPos = transform.localPosition;
        clipPos = GetComponent<UIPanel>().clipRange;
    }

    void OnEnable()
    {
        //preferredBusinesses.SetActive(false);
        preferredBusinesses.transform.localPosition = new Vector3(400.0f, -1100.0f, -600.0f);
        preferredBusinesses.GetComponent<AutoScroll>().start = new Vector3(400.0f, -1100.0f, -600.0f);
        hidePanel.transform.localPosition = new Vector3(700.0f, 4.5f, -600.0f);
		//Populate(_currentCategory);
    }

    void OnDisable()
    {
        transform.localPosition = startPos;
        GetComponent<UIPanel>().clipRange = clipPos;
        Destroy(GetComponent<SpringPanel>());//.enabled = true;
        //preferredBusinesses.SetActive(true);
        preferredBusinesses.transform.localPosition = new Vector3(600.0f, -1100.0f, -600.0f);
        preferredBusinesses.GetComponent<AutoScroll>().start = new Vector3(600.0f, -1100.0f, -600.0f);
        hidePanel.transform.localPosition = new Vector3(900.0f, 4.5f, -600.0f);
        //Application.LoadLevel(5);
    }

    public void Populate(eMallSubcategory cat)
    {
        _currentCategory = cat;
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
