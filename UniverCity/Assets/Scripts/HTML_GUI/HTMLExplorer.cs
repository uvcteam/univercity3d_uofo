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

public class HTMLExplorer : MonoBehaviour
{

#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif

    private CoherentUIView _view;
    private AdManager _adManager;
    private bool _viewReady;
    private Vector2 _trans;
    public GameObject World;
    public TweenTransform myTween;
    private GameObject oldPos = null;
    private GameObject _floatingBubble;
    public GameObject leftStick;
    public GameObject rightStick;
    private string _businessID;

    // Use this for initialization
    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        myTween = gameObject.GetComponent<TweenTransform>();
        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("LoadAdPlayer", (System.Action<string>)LoadAdPlayer);
            _view.View.BindCall("OnAdPlayerWasClosed", (System.Action)OnAdPlayerWasClosed);
            _view.View.BindCall("OnBusinessWasSelected", (System.Action)OnBusinessWasSelected);
            _view.View.BindCall("AddBusinesses", (System.Action)AddBusinesses);
            _view.View.BindCall("CloseBusinessList", (System.Action)CloseBusinessList);
            _view.View.BindCall("LoadAdData", (System.Action)LoadAdData);
            _view.View.BindCall("SetBusinessIDForCard", (System.Action<string>)SetBusinessIDForCard);
            _view.View.BindCall("LoadBusinessCard", (System.Action)LoadBusinessCard);
            _view.View.BindCall("MenuClosed", (System.Action)MenuClosed);
        };
       
    }

    void OnViewReady(View view)
    {
        _viewReady = true;

    }

    public void AddBusiness(Business business)
    {
        _view.View.TriggerEvent("PopulateCategory", business.name, business.desc, business.id,
                       Convert.ToBase64String(business.logo.EncodeToPNG()), business.hasAd);
    }

    public void AddClickEventsToBusinesses()
    {
        Debug.Log("takeall");
        _view.InputState = CoherentUIView.CoherentViewInputState.TakeAll;
        _view.View.TriggerEvent("AttachEventToBusinesses");
    }

    public void LoadAdPlayer(string businessid)
    {
        _businessID = businessid;
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/adplayer.html");
    }
    void LoadAdData()
    {
        _view.View.TriggerEvent("LoadAdPlayer", _businessID, serverURL);
    }

    public void OnBusinessWasSelected()
    {
        World.SetActive(false);
    }
    public void OnAdPlayerWasClosed()
    {
        World.SetActive(true);
    }

    public void SetPosition(Vector2 trans, GameObject myBubble)
    {
        _trans = trans;
        _floatingBubble = myBubble;
        rightStick.SetActive(false);
        leftStick.SetActive(false);
        AddBusinesses();
    }

    public void AddBusinesses()
    {
        if (_trans == null)
            return;
        BusinessManager manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        // Add all of the new businesses.
        if (!manager.busByCoord.ContainsKey(_trans)) return;

        foreach (Business bus in manager.busByCoord[_trans])
        {
            this.AddBusiness(bus);
        }
        this.AddClickEventsToBusinesses();
    }

    public void CloseBusinessList()
    {
        _view.View.TriggerEvent("ClearBusinessList");
        OnCancelClicked();
        _view.InputState = CoherentUIView.CoherentViewInputState.TakeNone;
    }

    public void SetReturnPosition()
    {
        oldPos = GameObject.Find("Placeholder");
    }

    void SetBusinessIDForCard(string businessid)
    {
        Debug.Log(businessid);
        _businessID = businessid;
        _view.View.Load("coui://HTML_UI/VirtualMall/businesscard.html");
    }

    void LoadBusinessCard()
    {
        _view.View.TriggerEvent("LoadBusinessCard", _businessID);
    }

    public void OpenMenu()
    {
        _view.View.TriggerEvent("OpenMenu", _businessID);
        _view.InputState = CoherentUIView.CoherentViewInputState.TakeAll;
    }

    public void MenuClosed()
    {
        Debug.Log("close");
        _view.InputState = CoherentUIView.CoherentViewInputState.TakeNone;
    }

    public void OnCancelClicked()
    {
        myTween.eventReceiver = gameObject;
        myTween.to = oldPos.transform;
        myTween.duration = 1.0f;
        myTween.Reset();
        myTween.Toggle();

		Resources.UnloadUnusedAssets();
	}
    void OnTweenFinished(UITweener tweener)
    {
        gameObject.rigidbody.isKinematic = false;
        _floatingBubble.SetActive(true);
        rightStick.SetActive(true);
        leftStick.SetActive(true);
        Destroy(oldPos);
        FloatingBubble.HasAdUp = false;
    }
}

