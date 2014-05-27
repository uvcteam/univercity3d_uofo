using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Facebook;
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
    private bool _loadAdplayerOnStart = false;
    private string _subCat;
    private string _businessID = null;
    private int _flashdealID = -1;
    private Dictionary<int,string> _flashDeals = new Dictionary<int,string>();
    private Business currentBusiness;
    private User _user;
	// Use this for initialization
    void Start()
    {
        _user = GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser;
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
            _view.View.BindCall("ClearFlashDeals", (System.Action)ClearFlashDeals);
            _view.View.BindCall("LoadFlashDeals", (System.Action)LoadFlashDeals);
            _view.View.BindCall("TrackUserAction", (System.Action<string, string, string>)TrackUserAction);
            _view.View.BindCall("OnAdPlayerWasClosed", (System.Action)OnAdPlayerWasClosed);
            _view.View.BindCall("FacebookLike", (System.Action<string>)FacebookLike);
			_view.View.BindCall("FacebookUnLike", (System.Action<string>)FacebookUnLike);
            _view.View.BindCall("CheckIfBusinessIsLiked", (System.Action)CheckIfBusinessIsLiked);
            _view.View.BindCall("SaveBusiness", (System.Action<string>)SaveBusiness);
            _view.View.BindCall("UnsaveBusiness", (System.Action<string>)UnsaveBusiness);
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
        _view.View.Load("coui://HTMLUI/VirtualMall/virtualmallsubcat.html");
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
					Debug.Log (business.discountName);
                    _view.View.TriggerEvent("PopulateCategory", business.name, business.desc, business.id, 
					                        Convert.ToBase64String(business.logo.EncodeToPNG()), i, business.hasAd, business.discountName);
                }
            }
            else
                _view.View.TriggerEvent("CreateEmptyCategory", i);

        }
        _view.View.TriggerEvent("AttachEventToBusinesses");
    }

    public void SetBusinessID(string businessid)
    {
        Debug.Log(businessid);
        _businessID = businessid;
         currentBusiness = _businessManager.businesses.Where(x => x.id.ToString() == _businessID).FirstOrDefault();
        _view.View.Load("coui://HTMLUI/VirtualMall/adplayer.html");       
    }

    public void LoadAdplayerOnStart(string businessId)
    {
        _businessID = businessId;
        _loadAdplayerOnStart = true;

    }

    void SetJsonString(string json, int id)
    {
        Debug.Log(id);
        _flashDeals.Add(id, json);
    }

    void LoadAdData()
    {
		Debug.Log ("LoadAdPlayer");
        TrackUserAction(_businessID, currentBusiness.name, "start");
        _view.View.TriggerEvent("LoadAdPlayer", _businessID, serverURL,  
        	GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token);
    }

    void SetFlashDealID(int flashdealID)
    {
        _flashdealID = flashdealID;
		_view.View.Load("coui://HTMLUI/VirtualMall/flashdeal.html");
    }

    void SetBusinessIDForCard(string businessid)
    {
        _businessID = businessid;
		_view.View.Load("coui://HTMLUI/VirtualMall/businesscard.html");
    }

    void LoadBusinessCard()
    {
        _view.View.TriggerEvent("LoadBusinessCard", _businessID);
    }

    void LoadFlashDeal()
    {
        _view.View.TriggerEvent("LoadFlashPlayer", _flashDeals[_flashdealID], serverURL);
    }
    void LoadFlashDeals()
    {
        _view.View.TriggerEvent("LoadFlashDeals", serverURL);
    }

    void ClearFlashDeals()
    {
        _flashDeals.Clear();
    }

    void OnAdPlayerWasClosed()
    {
        if (currentBusiness != null)
            TrackUserAction(_businessID, currentBusiness.name, "close");

        if(Application.loadedLevel == 1)
        {
            gameObject.GetComponent<HTMLExplorer>().OnAdPlayerWasClosed();
        }
    }

    void FacebookLike(string objectToLike)
    {
        Dictionary<string,string> formData = new Dictionary<string, string>();
        formData.Add("object", objectToLike);

        FB.API("/me/og.likes", HttpMethod.POST, RetrievedInfo, formData);
    }

    void FacebookUnLike(string likeID)
    {
		Debug.Log("Liked: " + likeID); 
        FB.API("/" + likeID, HttpMethod.DELETE, RetrievedInfo);
    }

    void CheckIfBusinessIsLiked()
    {
        Debug.Log("---------------------Getting Likes-----------------------");
        FB.API("/me/og.likes", HttpMethod.GET, RetrievedLikes);
    }

    public void RetrievedInfo(FBResult response)
    {
        Debug.Log(response.Text);
    }

    public void RetrievedLikes(FBResult likes)
    {
        _view.View.TriggerEvent("CheckIfLiked", likes.Text);
    }

    void TrackUserAction(string businessID, string title, string eventName)
    {
        string trackURL = serverURL + "track?id=";
        trackURL += businessID;
        trackURL += "&title=" + title.Replace(" ", string.Empty);
        trackURL += "&event=" + eventName.Replace(" ", string.Empty);
        trackURL += "&play_id=" + DateTime.Now.Ticks;

        if (PlayerPrefs.GetInt("loggedIn") == 1)
            trackURL += "&token=" +
                        GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;

        Debug.Log("Sending: " + trackURL);
        WWW track = new WWW(trackURL);

        //while (!track.isDone) { }

        //Debug.Log("Response: " + track.text);
    }

    void SaveBusiness(string id)
    {
        Debug.Log(id);
        _user.SavedBusinesses.Add(Int32.Parse(id));
    }

    void UnsaveBusiness(string id)
    {
        Debug.Log(id);
        _user.SavedBusinesses.Remove(Int32.Parse(id));
    }
}
