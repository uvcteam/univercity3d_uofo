using System;
using UnityEngine;
using System.Collections;

public class UnionHallEngagementSettings : MonoBehaviour 
{
    public GameObject categoryChooser = null;
    public UIInput email = null;
    public UIInput phone = null;
    public UIButton responseSave = null;
    public UIInput min = null;
    public UIInput max = null;
    public UIButton attendeesSave = null;
    public UIButton saveSettings = null;
    public GameObject errorPanel = null;
    public GameObject createPanel = null;
    public GameObject chooseCategories = null;

    public UnionHallEvent newEvent = null;

    private string errorMessage = "";

    void OnEnable()
    {
        GameObject.Find("PageName").GetComponent<UILabel>().text = "Create Engagement";
        newEvent = GameObject.Find("NewEvent").GetComponent<UnionHallEvent>();
    }

    void Awake()
    {
        saveSettings.isEnabled = false;
        responseSave.isEnabled = false;
        attendeesSave.isEnabled = false;
    }

    void OnChooseCategoriesClicked()
    {
        chooseCategories.SetActiveRecursively(true);
        gameObject.SetActiveRecursively(false);
    }

    void OnResponseSaveClicked()
    {
        if ( email.text != "" &&
             phone.text != "" )
            attendeesSave.isEnabled = true;
    }

    void OnAttendeesSaveClicked()
    {
        if (min.text == "" || Convert.ToInt32(min.text) < 4)
            min.text = "4";

        saveSettings.isEnabled = true;
    }

    void OnSaveSettingsClicked()
    {
        if (!CheckForErrors())
        {
            newEvent.Email = email.text;
            newEvent.Phone = phone.text;
            newEvent.Min = Convert.ToInt32(min.text);
            if (max.text == "")
                newEvent.Max = 99;
            else
                newEvent.Max = Convert.ToInt32(max.text);
            createPanel.GetComponent<UnionHallCreateEngagement>().SettingsSet();
            createPanel.SetActiveRecursively(true);
            gameObject.SetActiveRecursively(false);
        }
        else
        {
            errorPanel.SetActiveRecursively(true);
            errorPanel.GetComponent<UnionHallErrorWindow>().SetErrorText(errorMessage);
            errorMessage = "";
        }
    }

    public void NewSettings()
    {
        email.text = "";
        phone.text = "";
        min.text = "";
        max.text = "";

        saveSettings.isEnabled = false;
        responseSave.isEnabled = false;
        attendeesSave.isEnabled = false;
    }

    private bool CheckForErrors()
    {
        bool errors = false;

        if (Convert.ToInt32(min.text) < 4)
        {
            errors = true;
            errorMessage += "Minimum attendees is less than 4.\n";
        }

        if (max.text != "" && Convert.ToInt32(max.text) > 99)
        {
            errors = true;
            errorMessage += "Max attendees is 99.\n";
        }

        if (max.text != "" && Convert.ToInt32(min.text) > Convert.ToInt32(max.text))
        {
            errors = true;
            errorMessage += "Min attendees is more than max.\n";
        }

        return errors;
    }
}