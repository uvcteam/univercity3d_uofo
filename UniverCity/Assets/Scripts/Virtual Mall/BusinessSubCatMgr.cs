using UnityEngine;
using System.Collections;
using System.Linq;

public class BusinessSubCatMgr : MonoBehaviour 
{
    private BusinessManager _businessManager;
    public GameObject businessBtn;

    void Awake()
    {
        _businessManager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }

    public void Populate(string category)
    {
        Transform grid = transform.Find("ScrollPanel").Find("Grid");
        GameObject btn;

        if(_businessManager.businessesByCategory.ContainsKey(category))
            foreach (var business in _businessManager.businessesByCategory[category])
            {
                btn = (GameObject)Instantiate(businessBtn);
                btn.transform.parent = grid;
                btn.transform.Find("Logo").gameObject.AddComponent<UITexture>();
                btn.transform.Find("Logo").GetComponent<UITexture>().mainTexture = business.logo;
                //btn.transform.Find("Logo").renderer.material.mainTexture = business.logo;
                btn.transform.Find("Name").GetComponent<UILabel>().text = business.name;
                btn.transform.Find("Description").GetComponent<UILabel>().text = business.desc;
                btn.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                grid.GetComponent<UIGrid>().Reposition();
            }
    }
}
