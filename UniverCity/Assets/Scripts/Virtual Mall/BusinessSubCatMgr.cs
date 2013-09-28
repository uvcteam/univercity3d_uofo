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
                btn = (GameObject)Instantiate(businessBtn, Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
                btn.transform.parent = grid;
                btn.transform.localPosition = new Vector3(0.0f, 0.0f, -100.0f);
                btn.transform.Find("Logo").GetComponent<UITexture>().mainTexture = business.logo;
                btn.transform.Find("Logo").GetComponent<UITexture>().Update();
                btn.transform.Find("Name").GetComponent<UILabel>().text = business.name;
                btn.transform.Find("Description").GetComponent<UILabel>().text = business.desc;
                btn.GetComponent<BusinessBtn>().businessId = business.id;
                btn.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                grid.GetComponent<UIGrid>().Reposition();
            }
    }
}
