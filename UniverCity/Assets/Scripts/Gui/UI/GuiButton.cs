using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuTypeForSounds
{
	None,
	Click,
    ClickOpen,
    ClickClose,
	MAX
}

public class GuiButton : MonoBehaviour
{
    // publics
    public MenuTypeForSounds TypeForSounds = MenuTypeForSounds.None;
    public Transform OverrideForm;
    public string MouseUp;
    public string MouseDn;
	public string MouseExit;
    public Transform TransNormal;
    public Transform TransOver;
    public Transform TransPressed;
    public Transform TransDisabled;
    public Transform TransCustom;
    public Transform TransEffect;
    public Transform TransDisabledButActive;

    // protected
    protected bool bPressedFlag = false;
    protected GuiButton.eState eButtonState = eState.Normal;
    protected List<Vector3> mEffectLocalScales = new List<Vector3>();

    public enum eState
    {
        Normal,
        Over,
        Pressed,
        Disabled,
        DisabledButActive,
        Custom,
    }

    // ***********************************************
    // Awake [Monobehavior]
    // -----------------------------------------------
    void Awake()
    {
        useGUILayout = false;
		//if (nSideLen != 0)
		//	this.GetComponent<MeshFilter>().mesh.uv = PxUtil.GenericSquareUV(fAtlasX, fAtlasY, nSideLen);
    }

    // ***********************************************
    // Start [Monobehavior]
    // -----------------------------------------------
    void Start()
    {
        // nothing
    }

    // ***********************************************
    // OnEnable [Monobehavior]
    // -----------------------------------------------
    void OnEnable()
    {
        SetState(eState.Normal);
    }

    // ***********************************************
    // OnDisable [Monobehavior]
    // -----------------------------------------------
    void OnDisable()
    {
        // restore local scale from effect
        if (mEffectLocalScales.Count > 0)
        {
            if (null != TransEffect)
            {
                TransEffect.localScale = mEffectLocalScales[0];
            }
            mEffectLocalScales.Clear();
        }
    }

    // ***********************************************
    // OnMouseDown [Monobehavior]
    // -----------------------------------------------
    protected virtual void OnMouseDown()
    {
        // early exit when button disabled
        if (eButtonState == eState.Disabled)
            return;

        // assign focus control
        Gui.FocusControl = this.transform;

        // toggle normal/pressed
        SetState(eState.Pressed);

        // trigger callback
        if (MouseDn != null && MouseDn.Length > 0 && !Gui.bInputFrozen)
        {
            if (OverrideForm)
                OverrideForm.SendMessageUpwards(MouseDn);
            else
                SendMessageUpwards(MouseDn);
        }

        bPressedFlag = true;
    }

    // ***********************************************
    // OnMouseUp [Monobehavior]
    // -----------------------------------------------
    protected virtual void OnMouseUp()
    {
		// play sounds, but no sounds yet so comment out
        /*
        switch (TypeForSounds)
        {
            case MenuTypeForSounds.Click:
                Sound.PlayOneShot(Sound.eSoundID.BtnClick);
                break;
            case MenuTypeForSounds.ClickOpen:
                Sound.PlayOneShot(Sound.eSoundID.BtnOpen);
                break;
            case MenuTypeForSounds.ClickClose:
                Sound.PlayOneShot(Sound.eSoundID.BtnClose);
                break;
        }
        */
        
        // early exit when button disabled
        if (eButtonState == eState.Disabled)
            return;

        // toggle normal/pressed
        if (TransOver)
            SetState(eState.Over);
        else
            SetState(eState.Normal);
        

        if (bPressedFlag)
        {
            // assign focus control
            Gui.FocusControl = this.transform;

            if (MouseUp.Length > 0 && !Gui.bInputFrozen)
            {
                // trigger callback
                if (OverrideForm)
                    OverrideForm.SendMessageUpwards(MouseUp);
                else
                    SendMessageUpwards(MouseUp);
            }

            bPressedFlag = false;
        }
    }

    // ***********************************************
    // OnMouseEnter [Monobehavior]
    // -----------------------------------------------
    protected virtual void OnMouseEnter()
    {
        // early exit when button disabled
        if (eButtonState == eState.Disabled)
            return;

        if (TransOver)
            SetState(eState.Over);
    }

    // ***********************************************
    // OnMouseExit [Monobehavior]
    // -----------------------------------------------
    protected virtual void OnMouseExit()
    {
        // early exit when button disabled
        if (eButtonState == eState.Disabled)
            return;

        SetState(eState.Normal);

        if (bPressedFlag)
        {
            if (MouseExit.Length > 0 && !Gui.bInputFrozen)
            {
                if (OverrideForm)
                    OverrideForm.SendMessageUpwards(MouseExit);
                else
                    SendMessageUpwards(MouseExit);
            }

            bPressedFlag = false;
        }
	}

    // ***********************************************
    // SetState - set state of button
    // -----------------------------------------------
    public void SetState(eState inState)
    {
        eButtonState = inState;

        if (TransOver)
            TransOver.gameObject.active = (eButtonState == eState.Over);
        if (TransNormal)
            TransNormal.gameObject.active = (eButtonState == eState.Normal);
        if (TransPressed)
            TransPressed.gameObject.active = (eButtonState == eState.Pressed);
        if (TransDisabled)
            TransDisabled.gameObject.active = (eButtonState == eState.Disabled);
        if (TransDisabledButActive)
            TransDisabledButActive.gameObject.active = (eButtonState == eState.DisabledButActive);
        if (TransCustom)
            TransCustom.gameObject.active = (eButtonState == eState.Custom);
    }

    // ***********************************************
    // GetState - get state of button
    // -----------------------------------------------
    public eState GetState()
    {
        return eButtonState;
    }
}
