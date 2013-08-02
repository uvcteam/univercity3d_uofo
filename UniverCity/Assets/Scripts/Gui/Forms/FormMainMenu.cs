#define STICKY_LOGIN

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormMainMenu : MonoBehaviour
{
    // publics
    public static FormMainMenu mInstance = null;
    public GameObject Anim;
    public UITextbox TextUsername;
    public UITextbox TextPassword;
    public GameObject BnCheckOff;
    public GameObject BnCheckOn;
    public Transform ScrollSnapPt;

    // privates
    private bool mVisible = true;

    // ***************************************************
    // Start [Monobehavior]
    // ---------------------------------------------------
	void Start()
    {
        mInstance = this;

        Hide();
        Show();
        GuiMall();
	}

    void OnEnable()
    {
        if (!mInstance)
            return;

        TextUsername.SetText("Username");
        TextPassword.SetText("Password", true);
        BnCheckOff.SetActiveRecursively(true);
        BnCheckOn.SetActiveRecursively(false);

#if STICKY_LOGIN
        string theStickyUser = PlayerPrefs.GetString("StickUser", "");
        string theStickyPass = PlayerPrefs.GetString("StickPass", "");
        int theStickyRemember = PlayerPrefs.GetInt("StickRemember", 0);
        if (theStickyUser.Length > 0 && theStickyPass.Length > 0)
        {
            TextUsername.SetText(theStickyUser);
            TextPassword.SetText(theStickyPass);
            BnCheckOff.SetActiveRecursively(theStickyRemember == 0);
            BnCheckOn.SetActiveRecursively(theStickyRemember == 1);
        }
#endif

        // change focus -VM
        TextUsername.SetFocus();
    }

    // ***************************************************
    // OnDestroy [Monobehavior]
    // ---------------------------------------------------
    void OnDestroy()
    {
        mInstance = null;
    }

    static public bool GetVisible()
    {
        if (mInstance)
            return mInstance.gameObject.active;
        return false;
    }

    static public void Show(float inOffsetX = 0.0f)
    {
        if (mInstance)
        {
            mInstance.gameObject.SetActiveRecursively(true);
            mInstance.DoShow(inOffsetX);
        }
    }

    static public void Hide()
    {
        if (mInstance)
        {
            mInstance.gameObject.SetActiveRecursively(false);
            mInstance.DoHide();
        }
    }

    void DoShow(float inOffsetX = 0.0f)
    {
        if (inOffsetX != 0.0f)
            Gui.Transition(Anim, inOffsetX);

        
    }

    void DoHide()
    {
        // todo
    }

    void Update()
    {
        // todo
    }

    #region GuiStuff

    void GuiUsername() { }
    void GuiPass() { }

    void GuiCheckToggle()
    {
        BnCheckOff.SetActiveRecursively(!BnCheckOff.active);
        BnCheckOn.SetActiveRecursively(!BnCheckOn.active);
    }

    void GuiLogin()
    {
        // todo    
    }

    void GuiMemoryBank()
    {
        Application.LoadLevel(4);
    }

    void GuiUnion()
    {
        Application.LoadLevel(3);  
    }

    void GuiExplorer()
    {
        Application.LoadLevel(1);
    }

    void GuiMall()
    {
        iTween.MoveTo(Anim, iTween.Hash("x", ScrollSnapPt.position.x, "time", 0.5f));
        mVisible = false;

        FormMallCategories.Show();
    }

    void GuiScrollBar()
    {
        if (!mVisible)
        {
            //iTween.MoveTo(Anim, iTween.Hash("x", 0.0f, "time", 0.5f));
            //mVisible = true;

            //FormMallCategories.Hide();
            //FormMallSubcategories.Hide();
            //FormMallDetailedCategories.Hide();
            //FormMallAppPanel.Hide();
            Application.LoadLevel(0);
        }
    }

    #endregion
}
