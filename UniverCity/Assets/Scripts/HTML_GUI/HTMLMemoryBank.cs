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

        _view.Listener.ReadyForBindings += (frameId, path, isMainFrame) =>
        {
            _view.View.BindCall("GoToDestination", (System.Action<string>)GoToDestination);
        };

        _view.View.TriggerEvent("UpdateUsername", _userManager.CurrentUser.Name);
        _viewReady = false;
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

    #endregion
}