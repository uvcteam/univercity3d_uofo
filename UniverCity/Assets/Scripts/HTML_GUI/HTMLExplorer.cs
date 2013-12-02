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

    private CoherentUIView _view;
    private AdManager _adManager;
    private bool _viewReady;
    private Transform _trans;
    public GameObject World;
    public TweenTransform myTween;
    private GameObject oldPos = null;
    private GameObject _floatingBubble;
    public GameObject leftStick;
    public GameObject rightStick;

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
        };
       
    }

    void OnViewReady(View view)
    {
        _viewReady = true;

    }

    public void AddBusiness(Business business)
    {
        _view.View.TriggerEvent("PopulateCategory", business.name, business.desc, business.id,
                       Convert.ToBase64String(business.logo.EncodeToPNG()));
    }

    public void AddClickEventsToBusinesses()
    {
        _view.View.TriggerEvent("AttachEventToBusinesses");
    }

    public void LoadAdPlayer(string businessid)
    {
        Debug.Log("Loading ad");
        GetComponent<CoherentUIView>().View.Load("coui://HTML_UI/VirtualMall/adplayer.html?id=" + businessid);;
    }

    public void OnBusinessWasSelected()
    {
        World.SetActive(false);
    }
    public void OnAdPlayerWasClosed()
    {
        World.SetActive(true);
    }

    public void SetPosition(Transform trans, GameObject myBubble)
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
        foreach (Business bus in manager.busByCoord[new Vector2(_trans.localPosition.x, _trans.localPosition.z)])
        {
            this.AddBusiness(bus);
        }
        this.AddClickEventsToBusinesses();
    }

    public void CloseBusinessList()
    {
        _view.View.TriggerEvent("ClearBusinessList");
        OnCancelClicked();
    }

    public void SetReturnPosition()
    {
        oldPos = GameObject.Find("Placeholder");
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

