using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using MiniJSON;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM || UNITY_STANDALONE_WIN
using Coherent.UI;
using Coherent.UI.Binding;
#elif UNITY_IPHONE || UNITY_ANDROID
using Coherent.UI.Mobile;
using Coherent.UI.Mobile.Binding;
#endif

[Serializable]
public class UserManager : MonoBehaviour
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    
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
        //_view.OnViewCreated += new UnityViewListener.CoherentUI_OnViewCreated(this.OnViewReady);
    }
    void OnLevelWasLoaded(int level)
    {
        if (level != 0 && loginPanel != null)
            loginPanel.SetActive(false);
        if (level == 1)
        {
            Destroy(GameObject.Find("VirtualMall(Material)"));
            Debug.Log("HUR");
        }
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
        string cURL = serverURL + "GetAllSocialInterests";
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

            Categories = Categories.OrderBy(o => o.Name).ToList();
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
                if (signingInDialog != null)
                {
                    signingInDialog = GameObject.Find("Login Panel").GetComponent<MainMenuManager>().signingInDialog;
                    signingInDialog.SetActive(true);
                    signingInDialog.GetComponentInChildren<UILabel>().text = "Signing in...";
                }
            }
            else if (Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer)
                NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Signing In", false, false); 
        }

        string loginURL = serverURL + "DeviceLogin?";

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
                if (signingInDialog != null)
                    signingInDialog.GetComponentInChildren<UILabel>().text = "Wrong username or password!";
                if (exitBtn != null)
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

            while (_view == null || _view.View == null)
            {
                Debug.Log("No view...");
                _view = GameObject.Find("Main Camera").GetComponent<CoherentUIView>();
                yield return new WaitForEndOfFrame();
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
        string cURL = serverURL + "GetSocialInterests?token=" +
            CurrentUser.Token;
        string aURL = serverURL + "ListMyEvents?token=" +
            CurrentUser.Token;
        int catId = 0;
        int aeId = 0;
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

        goodDownload = false;
        Debug.Log(aURL);

        while (!goodDownload)
        {
            page = new WWW(aURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }

        results = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (Convert.ToBoolean(results["s"]))
        {
            foreach (object id in results["events"] as List<object>)
            {
                aeId = Convert.ToInt32(id);
                CurrentUser.AttendedEvents.Add(aeId);
                Debug.Log("Attending event: " + aeId);
            }
        }

        string iURL = serverURL + "ListMyInvitations?token=";
        iURL += CurrentUser.Token;

        goodDownload = false;
        Debug.Log(iURL);

        while (!goodDownload)
        {
            page = new WWW(iURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }

        Debug.Log("===============INVITED EVENTS==================");
        Debug.Log(page.text);

        results = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (Convert.ToBoolean(results["s"]))
        {
            foreach (object id in results["invitations"] as List<object>)
            {
                Debug.Log("INVITED TO EVENT " + Convert.ToInt32(id));
                CurrentUser.EventInvitations.Add(Convert.ToInt32(id));
            }
        }

        string bURL = serverURL + "ListSaved?token=";
        bURL += CurrentUser.Token;

        goodDownload = false;
        Debug.Log(bURL);

        while (!goodDownload)
        {
            page = new WWW(bURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }
        
        Debug.Log("===============SAVED BUSINESSESS==================");
        Debug.Log(page.text);

        results = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (Convert.ToBoolean(results["s"]))
        {
            foreach (object id in results["savedBusinesses"] as List<object>)
            {
                Debug.Log("SAVING BUSINESS: " + Convert.ToInt32(id));
                CurrentUser.SavedBusinesses.Add(Convert.ToInt32(id));
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
    public int GetIDForCategory(string cat)
    {
        foreach (SocialInterest c in Categories.Where(c => c.Name == cat))
            return c.Id;
        return -1;
    }

    public void UpdateUser()
    {
        CurrentUser = new User(
            PlayerPrefs.GetString("token"),
            PlayerPrefs.GetString("name"),
            PlayerPrefs.GetString("email"),
            PlayerPrefs.GetString("university"));

        StartCoroutine(GetUserCategories());
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
    private List<int> eventInvitations; 
    private List<int> attendedEvents;
    private List<int> savedBusinesses; 

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
    public List<int> EventInvitations
    {
        get { return eventInvitations; }
        set { eventInvitations = value; }
    }
    public List<int> SavedBusinesses
    {
        get { return savedBusinesses; }
        set { savedBusinesses = value; }
    }
    public List<int> AttendedEvents
    {
        get { return attendedEvents; }
        set { attendedEvents = value; }
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
        attendedEvents = new List<int>();
        eventInvitations = new List<int>();
        savedBusinesses = new List<int>();
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
        attendedEvents = new List<int>();
        eventInvitations = new List<int>();
        savedBusinesses = new List<int>();
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
        return categories.Any(interest => interest.Id == id);
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

    public bool AttendingEvent(int id)
    {
        return AttendedEvents.Any(aid => id == aid);
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