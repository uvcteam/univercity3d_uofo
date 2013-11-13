using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MiniJSON;
using Object = UnityEngine.Object;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

[Serializable]
public class UserManager : MonoBehaviour
{
    public User CurrentUser;
    public GameObject signingInDialog;
    public GameObject PageToDisable;
    public GameObject exitBtn;
    public GameObject loginPanel;

    public List<SocialInterest> Categories = new List<SocialInterest>();

    public CoherentUIView _view = null;
    private bool _viewReady = false;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (GameObject.FindGameObjectWithTag("UserManager") == null)
            gameObject.tag = "UserManager";
        else
            Destroy(gameObject);

        _view = GameObject.Find("Main Camera").GetComponent<CoherentUIView>();
        _view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
    }

    void OnViewReady(View view)
    {
        _viewReady = true;
    }

    void Awake()
    {
        StartCoroutine(GetCategories());

        if (PlayerPrefs.HasKey("loggedIn") && PlayerPrefs.GetInt("loggedIn") == 1)
        {
            CurrentUser = new User(
                PlayerPrefs.GetString("token"),
                PlayerPrefs.GetString("name"),
                PlayerPrefs.GetString("email"),
                PlayerPrefs.GetString("university"));
            //StartCoroutine(SignIn(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password")));
        }
        else
            CurrentUser = null;
    }

    IEnumerator GetCategories()
    {
        string cURL = "http://www.univercity3d.com/univercity/GetAllSocialInterests";
        int catId = 0;
        string catName = "";
        WWW page = null;
        bool goodDownload = false;

        while (!goodDownload)
        {
            page = new WWW(cURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }

        Dictionary<string, object> results = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(results["s"]))
        {
            Dictionary<string, object> tree = results["tree"] as Dictionary<string, object>;
            foreach (Dictionary<string, object> cat in tree["children"] as List<object>)
            {
                catId = Convert.ToInt32(cat["id"]);
                catName = cat["name"] as string;
                Categories.Add(new SocialInterest(catId, catName));
            }
        }
    }
	
    public IEnumerator SignIn(string email, string password, int index = -1)
    {
        if (PlayerPrefs.GetInt("loggedIn") == 0)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.OSXWebPlayer)
            {
                //signingInDialog = GameObject.Find("Login Panel").GetComponent<MainMenuManager>().signingInDialog;
                //signingInDialog.SetActive(true);
                //signingInDialog.GetComponentInChildren<UILabel>().text = "Signing in...";
                ;
            }
            else if (Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer)
                NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Signing In", false, false); 
        }

        string loginURL = "http://www.univercity3d.com/univercity/DeviceLogin?";

        loginURL += "email=" + WWW.EscapeURL(email);
        loginURL += "&password=" + WWW.EscapeURL(password);
        Debug.Log(loginURL);
        WWW page = new WWW(loginURL);
        yield return page;

        Dictionary<string, object> login = Json.Deserialize(page.text) as Dictionary<string, object>;

        if (Convert.ToBoolean(login["s"]) == false)
        {
            CurrentUser = null;

            if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.OSXWebPlayer)
            {
                signingInDialog.GetComponentInChildren<UILabel>().text = "Wrong username or password!";
                exitBtn.SetActive(true);
            }
            else if (Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer)
            {
                NativeDialogs.Instance.HideProgressDialog();
                NativeDialogs.Instance.ShowMessageBox("Error", "Invalid username/password.", new string[] {"OK"}, false, (string button) => NativeDialogs.Instance.HideProgressDialog());
            }
        }
        else
        {
            CurrentUser = new User(login, email);

            PlayerPrefs.SetInt("loggedIn", 1);
            PlayerPrefs.SetString("token", CurrentUser.Token);
            PlayerPrefs.SetString("name", CurrentUser.Name);
            PlayerPrefs.SetString("email", CurrentUser.Email);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetString("university", CurrentUser.University);
			Debug.Log (CurrentUser.Token);
            if (PageToDisable != null)
                PageToDisable.SetActive(false);
            if (signingInDialog != null)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.OSXWebPlayer)
                    signingInDialog.SetActive(false);
                else if (Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer)
                    NativeDialogs.Instance.HideProgressDialog();
            }

            Debug.Log("Triggering event on the JavaScript!");
            _view.View.TriggerEvent("LoggedIn", CurrentUser.Name);
            NativeDialogs.Instance.HideProgressDialog();

            StartCoroutine(GetUserCategories());
			if (index != -1)
				Application.LoadLevel(index);
            //GameObject.Find("ExitButton").SetActive(false);
        }

        
    }
    public IEnumerator GetUserCategories()
    {
        string cURL = "http://www.univercity3d.com/univercity/GetSocialInterests?token=" +
            CurrentUser.Token;
        int catId = 0;
        string catName = "";
        WWW page = null;
        bool goodDownload = false;

        while (!goodDownload)
        {
            page = new WWW(cURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }
        
        Dictionary<string, object> results = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (Convert.ToBoolean(results["s"]))
        {
            foreach (object id in results["interests"] as List<object>)
            {
                catId = Convert.ToInt32(id);
                catName = CategoryNameForId(catId);
                CurrentUser.AddInterest(catName, catId);
            }
        }
    }

    public void SignOut()
    {
        PlayerPrefs.SetInt("loggedIn", 0);
        CurrentUser = null;
		if (PageToDisable != null)
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

    private string CategoryNameForId(int id)
    {
        foreach (SocialInterest t in Categories)
            if (t.Id == id) return t.Name;

        return "";
    }

    public SocialInterest GetCategoryById(int id)
    {
        foreach (SocialInterest t in Categories)
            if (t.Id == id) return t;

        return null;
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
    private string pin = "";

    private List<SocialInterest> categories;
    private List<JournalEntry> journals;

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
    public List<SocialInterest> Categories
    {
        get { return categories; }
        set { categories = value; }
    }
    public string PIN
    {
        get { return pin; }
        set { pin = value; }
    }
    public List<JournalEntry> Journals
    {
        get { return journals; }
        set { journals = value; }
    }

    public User(Dictionary<string, object> user, string e)
    {
        loggedIn = Convert.ToBoolean(user["s"]);
        token = user["token"] as string;
        name = user["name"] as string;
        email = e;
        university = user["university"] as string;
        categories = new List<SocialInterest>();
        journals = new List<JournalEntry>();
    }
    public User(string t, string n, string e, string u)
    {
        loggedIn = true;
        token = t;
        name = n;
        email = e;
        university = u;
        categories = new List<SocialInterest>();
        journals = new List<JournalEntry>();
    }

    public void SetCategories(List<SocialInterest> categories)
    {
        this.categories.Clear();
        this.categories.AddRange(categories);
    }
    public void PopulateJournal(List<object> json)
    {
        journals.Clear();
        foreach (Dictionary<string, object> entry in json)
        {
            journals.Add(new JournalEntry(Convert.ToInt32(entry["id"]),
										  entry["title"] as string,
                                          entry["entry"] as string,
                                          entry["ts"] as string));
            NativeDialogs.Instance.HideProgressDialog();
        }
    }
    public bool HasInterest(string name)
    {
        foreach (SocialInterest interest in categories)
            if (interest.Name == name) return true;
        return false;
    }
    public bool HasInterest(int id)
    {
        foreach (SocialInterest interest in categories)
            if (interest.Id == id) return true;
        return false;
    }
    public void AddInterest(string name, int id)
    {
        if (!HasInterest(id))
            categories.Add(new SocialInterest(id, name));
    }
    public void RemoveInterest(string name)
    {
        foreach (SocialInterest interest in categories)
            if (interest.Name == name)
            {
                categories.Remove(interest);
                return;
            }
    }
}

[Serializable]
public class JournalEntry
{
	private int id;
    private string title;
    private string entry;
    private DateTime timeStamp;
	
	public int Id
	{
		get { return id; }
		set { id = value; }
	}
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public string Entry
    {
        get { return entry; }
        set { entry = value; }
    }
    public DateTime TimeStamp
    {
        get { return timeStamp; }
        set { timeStamp = value; }
    }

    public JournalEntry(int i, string t, string e, string ts)
    {
		id = i;
        title = t;
        entry = e;
        timeStamp = DateTime.Parse(ts);
    }
}