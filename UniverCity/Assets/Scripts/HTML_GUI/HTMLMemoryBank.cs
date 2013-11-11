using UnityEngine;
using System.Collections;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

public class HTMLMemoryBank : MonoBehaviour
{
    private CoherentUIView _view;
    private UserManager _userManager;
    private bool _viewReady;

    void Start()
    {
        _view = this.GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("RequestUsername", (System.Action)RequestUsername);
        };

        _viewReady = false;
    }

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    #region CoherentUI Bindings
    public void RequestUsername()
    {
        Debug.Log("Updating username.");
        _view.View.TriggerEvent("UpdateUsername", _userManager.CurrentUser.Name);
    }

    #endregion
}