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

    public UIButton create = null;

    public GameObject engagementSettings = null;
    public GameObject legalSettings = null;
    public GameObject mainMenu = null;
    public GameObject errorPanel = null;

    public GameObject newEventObject = null;

    private bool newEvent = true;
    private string errorMessage = "";

    private string dateTime = "";

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
            if (start.text == "24:00")
                start.text = "00:00";

            eventScript.Start = DateTime.Parse(date.text + " " + start.text);
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
        NewObject.name = "NewEvent";

        title.text = "";
        who.text = "";
        desc.text = "";
        loc.text = "";
        start.text = "";
        //date.text = "";
        dateTime = DateTime.Now.ToString("yyyy-MM-dd");

        settingsSet = false;
        newEvent = true;
        create.isEnabled = false;
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

        if (start.text == "")
        {
            errors = true;
            errorMessage += "Must enter a start time.\n";
        }

        if (date.text == "")
        {
            errors = true;
            errorMessage += "Must enter a date.\n";
        }

        if (!settingsSet)
        {
            errors = true;
            errorMessage += "Must set engagement settings.\n";
        }

        return errors;
    }

    void OnDateClicked()
    {
        float width = Screen.width / 2;
        float height = Screen.width / 10;
        Rect drawRect = new Rect((Screen.width - width) / 2, height, width, height);

        NativePicker.Instance.ShowDatePicker(toScreenRect(drawRect), DateTime.Now, (long val) =>
        {
            dateTime = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
        });
    }

    Rect toScreenRect(Rect rect)
    {
        Vector2 lt = new Vector2(rect.x, rect.y);
        Vector2 br = lt + new Vector2(rect.width, rect.height);

        lt = GUIUtility.GUIToScreenPoint(lt);
        br = GUIUtility.GUIToScreenPoint(br);

        return new Rect(lt.x, lt.y, br.x - lt.x, br.y - lt.y);
    }
}