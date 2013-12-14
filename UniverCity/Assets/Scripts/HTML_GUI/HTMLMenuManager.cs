using System;
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

public class HTMLMenuManager : MonoBehaviour
{
    private CoherentUIView _view;
    private UserManager _userManager;
    private bool _viewReady;
    private const string FacebookAppURL = "http://www.univercity3d.com/mobileapp.html";
    private string LocalAppURL = "";

    void Start()
    {
        CoherentUISystem system = Object.FindObjectOfType(typeof (CoherentUISystem)) as CoherentUISystem;
        if (system != null)
        {
            if (system.camera == null)
                system.m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
        }

        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("GoToDestination", (System.Action<string>)GoToDestination);
            _view.View.BindCall("IsFacebookSignedIn", (System.Action)IsFacebookSignedIn);
            _view.View.BindCall("FacebookSignOut", (System.Action)FacebookSignOut);
            _view.View.BindCall("StoreFacebook", (System.Action<string, string, string, string>)StoreFacebook);
        };

        _view.OnViewCreated += (view) => view.InterceptURLRequests(true);
        _view.Listener.URLRequest += OnURLRequestHandler;

        LocalAppURL = _view.Page;

        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;
        _viewReady = false;
    }

    void OnURLRequestHandler(string url, URLResponse response)
    {
        if (url.StartsWith(FacebookAppURL))
        {
            // change the url, keeping all parameters intact
            Debug.Log("Intercepted " + url);
            string redirectURL = LocalAppURL + url.Substring(FacebookAppURL.Length);
            Debug.Log("REDIRECT TO: " + redirectURL);
            response.RedirectRequest(redirectURL);
            return;
        }
        response.AllowRequest();
    }

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    void CheckLogin(int index)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        if (manager.GetComponent<UserManager>().IsSignedIn())
        {
            Application.LoadLevel(index);
            return;
        }

        NativeDialogs.Instance.ShowLoginPasswordMessageBox("Login Required", "", new string[] { "Cancel", "OK" },
                                                           false,
            (string login, string password, string button) =>
            {
                if (button == "OK")
                    StartCoroutine(manager.GetComponent<UserManager>().SignIn(login, password, index));
            });
    }

    #region CoherentUI Bindings

    public void CheckLoginInformation(string email, string password)
    {
        StartCoroutine(_userManager.SignIn(email, password));
    }

    public void GoToDestination(string destination)
    {
        switch (destination)
        {
            case "virtual_mall":
                Application.LoadLevel(5);
                break;
            case "union_hall":
                CheckLogin(3);
                break;
            case "memory_bank":
                CheckLogin(4);
                break;
            case "explorer":
                Application.LoadLevel(1);
                break;
            case "arcade":
                Application.LoadLevel(2);
                break;
            default:
                break;
        }
    }

    public void IsFacebookSignedIn()
    {
        Debug.Log("Is Facebook signed in?");
        if (PlayerPrefs.HasKey("AccessToken") &&
            PlayerPrefs.HasKey("FacebookEmail") &&
            PlayerPrefs.GetString("FacebookEmail") == _userManager.CurrentUser.Email)
        {
            _view.View.TriggerEvent("FacebookAuthorized",
                PlayerPrefs.GetString("AccessToken"),
                PlayerPrefs.GetString("SignedRequest"),
                PlayerPrefs.GetString("ExpiresIn"),
                PlayerPrefs.GetString("Code"));
        }
        else
        {
            _view.View.TriggerEvent("FacebookNotAuthorized");
        }
    }

    public void FacebookSignOut()
    {
        Debug.Log("Sign out of Facebook!");
        PlayerPrefs.DeleteKey("AccessToken");
        PlayerPrefs.DeleteKey("SignedRequest");
        PlayerPrefs.DeleteKey("ExpiresIn");
        PlayerPrefs.DeleteKey("Code");
    }

    public void StoreFacebook(string at, string sr, string ei, string c)
    {
        Debug.Log("Player signed into Facebook: ");
        Debug.Log(at);
        Debug.Log(sr);
        Debug.Log(ei);
        Debug.Log(c);
        PlayerPrefs.SetString("FacebookEmail", _userManager.CurrentUser.Email);
        PlayerPrefs.SetString("AccessToken", at);
        PlayerPrefs.SetString("SignedRequest", sr);
        PlayerPrefs.SetString("ExpiresIn", ei);
        PlayerPrefs.SetString("Code", c);
    }

    #endregion
}