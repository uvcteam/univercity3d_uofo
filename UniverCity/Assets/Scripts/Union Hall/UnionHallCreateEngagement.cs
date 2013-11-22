using System;
using UnityEngine;
using System.Collections;

public class UnionHallCreateEngagement : MonoBehaviour 
{
    public bool settingsSet = false;
    public UIInput title = null;
    public UIInput who = null;
    public UIInput desc = null;
    public UIInput loc = null;
    public UIInput start = null;
    public UIInput date = null;
    public UIInput email = null;
    public UIInput phone = null;
    public UILabel min = null;
    public UILabel max = null;
    public UILabel startTime = null;
    public UILabel startDate = null;
    public UILabel category = null;

    public GameObject welcomePage = null;
    public GameObject[] pagesToDisable;

    public UIButton create = null;

    public GameObject engagementSettings = null;
    public GameObject legalSettings = null;
    public GameObject mainMenu = null;
    public GameObject errorPanel = null;

    public GameObject newEventObject = null;

    private bool newEvent = true;
    private string errorMessage = "";

    private string eventDate = "";
	private string eventTime = "";

    
    public Transform InterestTransform = null;
    public Transform Grid;
    public GameObject scrollPanel;

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Create Engagement";
    }

    void OnEngagementSettingsClicked()
    {
        engagementSettings.SetActive(true);
        if ( newEvent )
            engagementSettings.GetComponent<UnionHallEngagementSettings>().NewSettings();
        newEvent = false;
        gameObject.SetActive(false);
    }

    void OnCancelClicked()
    {
        DestroyImmediate(GameObject.Find("NewEvent"));
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnCreateClicked()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        newEvent = false;
        if (!CheckForErrors())
        {
            eventScript.Title = title.text;
            eventScript.Who = who.text;
            eventScript.Desc = desc.text;
            eventScript.Loc = loc.text;
            if (eventTime == "24:00")
                eventTime = "00:00";

            eventScript.Start = DateTime.Parse(eventDate + " " + eventTime);
            legalSettings.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            errorPanel.SetActive(true);
            errorPanel.GetComponent<UnionHallErrorWindow>().SetErrorText(errorMessage);
            errorMessage = "";
        }
    }

    public void NewEvent()
    {
        GameObject NewObject = Instantiate(newEventObject, Vector3.zero, Quaternion.identity) as GameObject;
        welcomePage.SetActive(true);
        NewObject.name = "NewEvent";

        foreach(GameObject go in pagesToDisable)
            go.SetActive(true);

        title.text = "";
        who.text = "";
        desc.text = "";
        loc.text = "";
        eventDate = DateTime.Now.ToString("yyyy-MM-dd");
		eventTime = DateTime.Now.ToString ("HH:mm");

        startTime.text = DateTime.Now.ToString("HH:mm tt");
        startDate.text = DateTime.Now.ToString("MMMM dd, yyyy");

        foreach (GameObject go in pagesToDisable)
            go.SetActive(false);

        settingsSet = false;
        newEvent = true;

        scrollPanel.SetActive(true);
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        Debug.Log("Awake was called!");
        foreach (SocialInterest cat in GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().Categories)
        {
            GameObject newInt = Instantiate(Resources.Load("Prefabs/SocialInterest"), InterestTransform.position, InterestTransform.rotation) as GameObject;
            newInt.name = "SocialInterest";
            newInt.transform.Find("Name").GetComponent<UILabel>().text = cat.Name;
            newInt.transform.parent = Grid;
            newInt.transform.localScale = InterestTransform.localScale;
            newInt.GetComponent<UIDragPanelContents>().draggablePanel = scrollPanel.GetComponent<UIDraggablePanel>();
            newInt.GetComponent<UIButtonMessage>().target = gameObject;
            newInt.GetComponent<InterestHolder>().id = cat.Id;
            newInt.GetComponent<InterestHolder>().interestName = cat.Name;
            if (eventScript.interests.Contains(cat.Id))
                newInt.transform.Find("Background").GetComponent<UISlicedSprite>().color = new Color(0.8f, 0.91f, 1.0f, 1.0f);
            else
                newInt.transform.Find("Background").GetComponent<UISlicedSprite>().color = Color.white;
            newInt.GetComponent<UIButtonMessage>().target = gameObject;
            Grid.GetComponent<UIGrid>().Reposition();
        }
        scrollPanel.SetActive(false);
        //create.isEnabled = false;
    }

    public void SettingsSet()
    {
        settingsSet = true;
        create.isEnabled = true;
    }

    private bool CheckForErrors()
    {
        bool errors = false;

        if (title.text == "")
        {
            errors = true;
            errorMessage += "Need a title.\n";
        }

        if (who.text == "")
        {
            errors = true;
            errorMessage += "Must enter who.\n";
        }

        if (desc.text == "")
        {
            errors = true;
            errorMessage += "Must enter a description.\n";
        }

        if (loc.text == "")
        {
            errors = true;
            errorMessage += "Must enter a location.\n";
        }

        if (eventTime == "")
        {
            errors = true;
            errorMessage += "Must enter a start time.\n";
        }

        if (eventDate == "")
        {
            errors = true;
            errorMessage += "Must enter a date.\n";
        }

        return errors;
    }

    void OnDateClicked()
    {
        Rect drawRect;
		if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) 
			drawRect = new Rect((Screen.width / 2) - (Screen.width * 0.23f), (Screen.height / 2) - (Screen.height * 0.05f), Screen.height / 3, Screen.width / 4);
		else
			drawRect = new Rect((Screen.width / 2) - (Screen.width * 0.135f), (Screen.height / 2) - (Screen.height * 0.05f), Screen.height / 3, Screen.width / 4);
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        NativePicker.Instance.ShowDatePicker(drawRect, DateTime.Parse(eventDate), (long val) =>
        {
            eventDate = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
            startDate.text = DateTime.Parse(eventDate).ToString("MMMM dd, yyyy");
            eventScript.Start = DateTime.Parse(eventDate + " " + eventTime);
        });
    }
	
	void OnTimeClicked()
	{
        Rect drawRect;
		if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) 
			drawRect = new Rect((Screen.width / 2) - (Screen.width * 0.23f), (Screen.height / 2) - (Screen.height * 0.05f), Screen.height / 3, Screen.width / 4);
		else
			drawRect = new Rect((Screen.width / 2) - (Screen.width * 0.135f), (Screen.height / 2) - (Screen.height * 0.05f), Screen.height / 3, Screen.width / 4);
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        NativePicker.Instance.ShowTimePicker(drawRect, DateTime.Parse(eventTime), (long val) =>
        {
            eventTime = NativePicker.ConvertToDateTime(val).ToString("HH:mm");
            startTime.text = DateTime.Parse(eventTime).ToString("h:mm tt");
            eventScript.Start = DateTime.Parse(eventDate + " " + eventTime);
        });
	}

    Rect toScreenRect(Rect rect)
    {
        Vector2 lt = new Vector2(rect.x, rect.y);
        Vector2 br = lt + new Vector2(rect.width, rect.height);
		
        lt = GUIUtility.GUIToScreenPoint(lt);
        br = GUIUtility.GUIToScreenPoint(br);
		
		Debug.Log ("LT: " + lt);
		Debug.Log ("BR: " + br);
		
        return new Rect(lt.x, lt.y, br.x - lt.x, br.y - lt.y);
    }

    void OnTitleSubmit()
    {
        Debug.Log("Title changed to " + title.text);
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Title = title.text;
    }

    void OnWhoSubmit()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Who = who.text;
    }

    void OnDescSubmit()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Desc = desc.text;
    }

    void OnLocSubmit()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Loc = loc.text;
    }

    void OnEmailSubmit()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Email = email.text;
    }

    void OnPhoneSubmit()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Phone = phone.text;
    }

    void OnMinChanged()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Min = Convert.ToInt32(min.text);

        if (max.GetComponent<NumberManipulator>().min < eventScript.Min)
            max.GetComponent<NumberManipulator>().min = eventScript.Min;
        if (Convert.ToInt32(min.text) >= Convert.ToInt32(max.text))
        {
            max.text = min.text;
            eventScript.Max = eventScript.Min;
        }
    }

    void OnMaxChanged()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        eventScript.Max = Convert.ToInt32(max.text);

        if (min.GetComponent<NumberManipulator>().min < eventScript.Min)
            max.GetComponent<NumberManipulator>().min = eventScript.Min;

        if (Convert.ToInt32(min.text) <= Convert.ToInt32(max.text))
        {
            min.text = max.text;
            eventScript.Min = eventScript.Max;
        }
    }

    void OnInterestClicked()
    {
        UnionHallEvent eventScript = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
        if (UICamera.lastHit.collider.gameObject.name == "SocialInterest")
        {
            GameObject interest = UICamera.lastHit.collider.gameObject;
            InterestHolder hold = interest.GetComponent<InterestHolder>();

            if (!eventScript.interests.Contains(hold.id))
            {
                eventScript.interests.Clear();
                eventScript.interests.Add(hold.id);
            }

            category.text = hold.interestName;

            pagesToDisable[6].SetActive(true);
            scrollPanel.SetActive(false);
        }
    }

    void OnCategoryClicked()
    {
        scrollPanel.SetActive(true);
        pagesToDisable[6].SetActive(false);
    }
}