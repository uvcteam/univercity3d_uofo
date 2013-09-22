using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public UIPanel adUpdatePanel;
    public UIPanel menuPanel;
    public UIPanel loginPanel;
    public UIPanel signedInPanel;
    public GameObject passwordInput;
    public GameObject usernameInput;
    public GameObject UserManager;
    public GameObject signingInDialog;
    public float tweenTime = 0.33f;

    void Awake()
    {
        UserManager = GameObject.FindGameObjectWithTag("UserManager");
        if (PlayerPrefs.HasKey("loggedIn") && PlayerPrefs.GetInt("loggedIn") == 1)
            gameObject.SetActive(false);
    }

    public void EnableTween()
    {
        foreach (TweenPosition tween in gameObject.GetComponentsInChildren<TweenPosition>())
        {
            if (tween != null)
            {
                tween.Reset();
                tween.Toggle();
            }
        }

        Debug.Log("EnableTween");
    }

    void OnSignInClicked()
    {
        UserManager = GameObject.FindGameObjectWithTag("UserManager");
        UserManager userManager = UserManager.GetComponent<UserManager>();
        string userName = usernameInput.GetComponentInChildren<UILabel>().text;
        string passWord = passwordInput.GetComponentInChildren<UILabel>().text;


        StartCoroutine(userManager.SignIn(userName, passWord));

        passwordInput.GetComponentInChildren<UILabel>().text = "";
    }

}