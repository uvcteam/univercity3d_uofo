using UnityEngine;
using System.Collections;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
#endif

public class HTMLMainMenu : MonoBehaviour
{
    private CoherentUIView _view;
    private UserManager _userManager;
    private bool _viewReady;

    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("GoToVirtualMall", (System.Action)GoToVirtualMall);
            _view.View.BindCall("GoToUnionHall", (System.Action)GoToUnionHall);
            _view.View.BindCall("GoToMemoryBank", (System.Action)GoToMemoryBank);
            _view.View.BindCall("GoToExplorer", (System.Action)GoToExplorer);
            _view.View.BindCall("GoToArcade", (System.Action)GoToArcade);
            _view.View.BindCall("CheckLoginInformation", (System.Action<string, string>) CheckLoginInformation);
            _view.View.BindCall("SignOut", (System.Action) SignOut);
        };

        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;
        _viewReady = false;
    }

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    #region CoherentUI Bindings

    public void CheckLoginInformation(string email, string password)
    {
        StartCoroutine(_userManager.SignIn(email, password));
    }

    public void GoToVirtualMall()
    {
        Application.LoadLevel(5);
    }

    public void GoToUnionHall()
    {
        Application.LoadLevel(3);
    }

    public void GoToMemoryBank()
    {
        Application.LoadLevel(4);
    }

    public void GoToExplorer()
    {
        Application.LoadLevel(1);
    }

    public void GoToArcade()
    {
        Application.LoadLevel(2);
    }

    public void SignOut()
    {
        _userManager.SignOut();
    }

    #endregion
}