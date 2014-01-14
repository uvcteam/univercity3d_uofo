using System;
using System.Runtime.Remoting.Messaging;
using Facebook;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM || UNITY_STANDALONE_WIN
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

public class HTMLFacebook : MonoBehaviour
{
    private CoherentUIView _view;
    private bool _viewReady;

    private FBResult _result;

	// Use this for initialization
	void Start ()
    {
        FB.Init(SetInit, OnHideUnity);
	    DontDestroyOnLoad(this);
    }

    private void SetInit()
    {
        enabled = true;
        FacebookLogin("user_photos,publish_actions");
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    FBResult FacebookLogin(string scope)
    {
        FB.Login(scope, result => _result = result);

        Debug.Log("================================================================================");
        Debug.Log((FB.IsLoggedIn) ? "LOGGED IN" : "NOT LOGGED IN!");
        Debug.Log("================================================================================");

        return _result;
    }
}