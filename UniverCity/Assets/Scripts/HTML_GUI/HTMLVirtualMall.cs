using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM || UNITY_STANDALONE_WIN
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

public class HTMLVirtualMall : MonoBehaviour 
{

    private CoherentUIView _view;
    private AdManager _adManager;
    private bool _viewReady;
    private FormMallDetailedCategories _MallCateGories = new FormMallDetailedCategories();
    private BusinessManager _businessManager;
    private bool _loadSubCats = false;
    private string _subCat;
    private int _businessID;
    byte[] foo;
	// Use this for initialization
    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("ReadyForCategories", (System.Action)ReadyForCategories);
            _view.View.BindCall("GetBusinessSubCat", (System.Action<string>)GetBusinessSubCat);
            _view.View.BindCall("SetBusinessID", (System.Action<string>)SetBusinessID);
        };

        _adManager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
        _businessManager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
    }

    void OnViewReady(View view)
    {
        _viewReady = true;

    }

    void GetBusinessSubCat(string subcat)
    {
        _subCat = subcat;
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/virtualmallsubcat.html");
    }

    void ReadyForCategories()
    {
        eMallSubcategory cat = (eMallSubcategory)Enum.Parse(typeof(eMallSubcategory), _subCat);
        _MallCateGories.GetSubCategories(cat);
        Populate();
    }

    void Populate()
    {
        string mallCat = "";
        for (int i = 0; i < _MallCateGories.theCategories.Length; ++i)
        {
            mallCat = _MallCateGories.theCategories[i];
            _view.View.TriggerEvent("CreateCategory", mallCat);

            if (_businessManager.businessesByCategory.ContainsKey(mallCat))
            {
                foreach (Business business in _businessManager.businessesByCategory[mallCat])
                {
                    _view.View.TriggerEvent("PopulateCategory", business.name, business.desc, business.id, 
                        Convert.ToBase64String(business.logo.EncodeToPNG()), i, business.hasAd);
                }
            }
            else
                _view.View.TriggerEvent("CreateEmptyCategory", i);

        }
        _view.View.TriggerEvent("AttachEventToBusinesses");
    }

    void SetBusinessID(string businessid)
    {
        //_view.Page = "coui://HTML_UI/VirtualMall/adplayer.html?id=" + businessid;
        //GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/adplayer.html?id=" + businessid);
        _view.View.Load("coui://HTML_UI/Login/login.html");
    }

}
