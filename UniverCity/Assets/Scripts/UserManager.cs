using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MiniJSON;

[Serializable]
public class UserManager : MonoBehaviour
{
    public User CurrentUser;
    public GameObject signingInDialog;
    public GameObject PageToDisable;
    public GameObject exitBtn;
    public GameObject loginPanel;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (GameObject.FindGameObjectWithTag("UserManager") == null)
            gameObject.tag = "UserManager";
        else
            Destroy(gameObject);
    }

    void Awake()
    {
        if (PlayerPrefs.HasKey("loggedIn") && PlayerPrefs.GetInt("loggedIn") == 1)
        {
            CurrentUser = new User(
                PlayerPrefs.GetString("token"),
                PlayerPrefs.GetString("name"),
                PlayerPrefs.GetString("email"),
                PlayerPrefs.GetString("university"));
        }
        else
            CurrentUser = null;
    }

    public IEnumerator SignIn(string email, string password)
    {
        signingInDialog = GameObject.Find("Login Panel").GetComponent<MainMenuManager>().signingInDialog;
        string loginURL = "http://www.univercity3d.com/univercity/DeviceLogin?";

        loginURL += "email=" + WWW.EscapeURL(email);
        loginURL += "&password=" + WWW.EscapeURL(password);

        signingInDialog.SetActive(true);
        signingInDialog.GetComponentInChildren<UILabel>().text = "Signing in...";
        Debug.Log(loginURL);
        WWW page = new WWW(loginURL);
        yield return page;

        Dictionary<string, object> login = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(login["s"]) == false)
        {
            CurrentUser = null;
            signingInDialog.GetComponentInChildren<UILabel>().text = "Wrong username or password!";
            exitBtn.SetActive(true);
        }
        else
        {
            CurrentUser = new User(login, email);

            PlayerPrefs.SetInt("loggedIn", 1);
            PlayerPrefs.SetString("token", CurrentUser.Token);
            PlayerPrefs.SetString("name", CurrentUser.Name);
            PlayerPrefs.SetString("email", CurrentUser.Email);
            PlayerPrefs.SetString("university", CurrentUser.University);

            if (PageToDisable != null)
                PageToDisable.SetActive(false);
            signingInDialog.SetActive(false);
            //GameObject.Find("ExitButton").SetActive(false);
        }

        
    }

    public void SignOut()
    {
        PlayerPrefs.SetInt("loggedIn", 0);
        CurrentUser = null;
        PageToDisable.SetActive(true);
    }

    public bool IsSignedIn()
    {
        return (PlayerPrefs.HasKey("loggedIn") && PlayerPrefs.GetInt("loggedIn") == 1);
    }

    public void ToggleErrorMessage()
    {
        signingInDialog.GetComponentInChildren<UILabel>().text = "Signing in...";
        signingInDialog.SetActive(false);
        exitBtn.SetActive(false);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            loginPanel.SetActive(false);
        }
        else if (CurrentUser != null && CurrentUser.LoggedIn)
        {
            loginPanel.SetActive(false);
        }
        else
            loginPanel.SetActive(true);
    }
}

[Serializable]
public class User
{
    private bool loggedIn = false;
    private string token = "";
    private string name = "";
    private string email = "";
    private string university = "";

    public bool LoggedIn
    {
        get { return loggedIn; }
        set { loggedIn = value; }
    }
    public string Token
    {
        get { return token; }
        set { token = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    public string University
    {
        get { return university; }
        set { university = value; }
    }

    public User(Dictionary<string, object> user, string e)
    {
        loggedIn = Convert.ToBoolean(user["s"]);
        token = user["token"] as string;
        name = user["name"] as string;
        email = e;
        university = user["university"] as string;

        Debug.Log("Logged in as: " + name);
    }

    public User(string t, string n, string e, string u)
    {
        loggedIn = true;
        token = t;
        name = n;
        email = e;
        university = u;
    }
}