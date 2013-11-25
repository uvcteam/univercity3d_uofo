using UnityEngine;
using System.Collections;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM || UNITY_STANDALONE_WIN
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
            _view.View.BindCall("GoToDestination", (System.Action<string>) GoToDestination);
            _view.View.BindCall("CheckLoginInformation", (System.Action<string, string, bool>) CheckLoginInformation);
            _view.View.BindCall("SignOut", (System.Action) SignOut);
        };

        _userManager = Object.FindObjectOfType(typeof(UserManager)) as UserManager;
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

    public void CheckLoginInformation(string email, string password, bool first)
    {
        if (first && email == "" && password == "" &&
            PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password"))
        {
            Debug.Log("Logging in with default values.");
            StartCoroutine(_userManager.SignIn(PlayerPrefs.GetString("email"),
                PlayerPrefs.GetString("password")));
        }
        else
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

    public void SignOut()
    {
        _userManager.SignOut();
    }

    #endregion
}