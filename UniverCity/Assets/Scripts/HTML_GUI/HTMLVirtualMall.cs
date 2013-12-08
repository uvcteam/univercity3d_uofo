using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif

    private CoherentUIView _view;
    private AdManager _adManager;
    private bool _viewReady;
    private FormMallDetailedCategories _MallCateGories = new FormMallDetailedCategories();
    private BusinessManager _businessManager;
    private bool _loadSubCats = false;
    private string _subCat;
    private string _businessID = null;
    private int _flashdealID = -1;
    private Dictionary<int,string> _flashDeals = new Dictionary<int,string>();
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
            _view.View.BindCall("LoadAdData", (System.Action)LoadAdData);
            _view.View.BindCall("SetBusinessIDForCard", (System.Action<string>)SetBusinessIDForCard);
            _view.View.BindCall("LoadBusinessCard", (System.Action)LoadBusinessCard);
            _view.View.BindCall("SetJsonString", (System.Action<string, int>)SetJsonString);
            _view.View.BindCall("SetFlashDealID", (System.Action<int>)SetFlashDealID);
            _view.View.BindCall("LoadFlashDeal", (System.Action)LoadFlashDeal);
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
        _businessID = businessid;
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/adplayer.html");       
    }

    void SetJsonString(string json, int id)
    {
        Debug.Log(id);
        _flashDeals.Add(id, json);
    }

    void LoadAdData()
    {
        Debug.Log("LoadAdPlayer");
        _view.View.TriggerEvent("LoadAdPlayer", _businessID, serverURL);
    }

    void SetFlashDealID(int flashdealID)
    {
        _flashdealID = flashdealID;
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/flashdeal.html");
    }

    void SetBusinessIDForCard(string businessid)
    {
        _businessID = businessid;
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/businesscard.html");
    }

    void LoadBusinessCard()
    {
        _view.View.TriggerEvent("LoadBusinessCard", _businessID);
    }

    void LoadFlashDeal()
    {
        Debug.Log("LoadFlashPlayer");
        _view.View.TriggerEvent("LoadFlashPlayer", _flashDeals[_flashdealID]);
    }
}
