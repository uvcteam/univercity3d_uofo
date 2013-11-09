using UnityEngine;
using System.Collections;
using Coherent.UI;

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
            _view.View.BindCall("CheckLoginInformation", (System.Action<string, string>) CheckLoginInformation);
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

    #endregion
}