using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITextbox : GuiButton
{
    // publics
    public string OnComplete;
    public TextMesh TextMesh;
    public GameObject Carot;
    public GameObject Select;
    public int MaxLength;
    public bool Secure;

    // privates
    static private UITextbox FocusedTextbox = null;
    private bool mFocused;
    private int mCarotIndex;
    private bool mSelOn;
    private int[] mSelIndices = { 0,0 };
    private List<float> mCharWidths = new List<float>();
    private string mUnsecureText;
    private bool mCarotInitSaved;
    private Vector3 mCarotInitPos;

#if UNITY_IOS || UNITY_ANDROID
    private TouchScreenKeyboard mKeyboard = null;
#else
    private Transform mTransform;
#endif

    // ***********************************************
    // SetFocus
    // -----------------------------------------------
    public void SetFocus(bool inShowTouchKeyboard = false)
    {
#if UNITY_IOS || UNITY_ANDROID
        
        if (FocusedTextbox)
        {
            FocusedTextbox.mFocused = false;
            if (FocusedTextbox.mKeyboard != null)
                FocusedTextbox.mKeyboard = null;
			FocusedTextbox = null;
        }
		
        mFocused = true;
        if (inShowTouchKeyboard)
            mKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, Secure);
		
#else

        if (!mCarotInitSaved)
        {
            mCarotInitPos = Carot.transform.localPosition;
            mCarotInitSaved = true;
        }

        // select all
        mCarotIndex = 0;
        Carot.transform.localPosition = mCarotInitPos;
        
        if (Carot)
            Carot.SetActiveRecursively(true);

        mFocused = true;
        SelectAll();
     
#endif
		
		// assign in GUI -VM
        Gui.FocusControl = this.transform;
        FocusedTextbox = this;
    }

    // ***********************************************
    // GetText
    // -----------------------------------------------
    public string GetText()
    {
        return mUnsecureText;
    }

    // ***********************************************
    // SetText
    // -----------------------------------------------
    public void SetText(string inText)
    {
        SetText(inText, false);
    }

    // ***********************************************
    // SetText
    // -----------------------------------------------
    public void SetText(string inText, bool inIgnoreSecure)
    {
        if (MaxLength > 0 && inText.Length > MaxLength)
            inText = inText.Substring(0, MaxLength);

        if (mUnsecureText != inText)
        {
            mUnsecureText = inText;
            if (Secure && !inIgnoreSecure)
                TextMesh.text = new string('*', mUnsecureText.Length);
            else
                TextMesh.text = inText;
        }
    }

    // ***********************************************
    // Awake [Monobehavior]
    // -----------------------------------------------
    void Awake()
    {
        useGUILayout = false;
    }

    // ***********************************************
    // OnEnable [Monobehavior]
    // -----------------------------------------------
    void OnEnable()
    {
        if (Carot)
            Carot.SetActiveRecursively(false);
        if (Select)
            Select.SetActiveRecursively(false);
    }

    // ***********************************************
    // OnDisable [Monobehavior]
    // -----------------------------------------------
    void OnDisable()
    {
        if (FocusedTextbox && FocusedTextbox == this)
            FocusedTextbox = null;

#if UNITY_IOS || UNITY_ANDROID
        if (mKeyboard != null)
        {
            // hide mobile keyboard when control is disabled (from hiding form its on)
            mKeyboard.active = false;
            mKeyboard = null;
        }
#endif
    }

    // ***********************************************
    // Start [Monobehavior]
    // -----------------------------------------------
    void Start()
    {
#if !(UNITY_IOS || UNITY_ANDROID)
        mTransform = this.transform;
#endif

        mUnsecureText = TextMesh.text;
    }

    // ***********************************************
    // OnMouseUp
    // -----------------------------------------------
    protected override void OnMouseUp()
    {        
		base.OnMouseUp();
        SetFocus(true);
    }

    // ***********************************************
    // Update [Monobehavior]
    // -----------------------------------------------
    protected void Update()
    {
#if UNITY_IOS || UNITY_ANDROID

        if (mKeyboard != null)
        {
            if (TextMesh != null)
                SetText(mKeyboard.text);
            if (mKeyboard.done)
            {
                mKeyboard = null;
                if (OnComplete.Length > 0 && !Gui.bInputFrozen)
                {
                    // trigger callback
                    if (OverrideForm)
                        OverrideForm.SendMessageUpwards(OnComplete);
                    else
                        SendMessageUpwards(OnComplete);
                }
            }
        }

#else

        // update input when has focus -VM
        if (mFocused)
        {
            if (mTransform == Gui.FocusControl)
                UpdateInput();
            else
            {
                mFocused = false;
                if (Carot)
                    Carot.SetActiveRecursively(false);
                if (Select)
                    Select.SetActiveRecursively(false);
            }
        }

#endif
    }

    // ***********************************************
    // UpdateInput
    // -----------------------------------------------
    protected char UpdateInput()
    {
        char theResult = '\0';

        foreach (char c in Input.inputString)
        {
            theResult = c;

            if (c == '\b')
            {
                if (mUnsecureText.Length != 0)
                {
                    if (mSelOn && mSelIndices[1] > mSelIndices[0])
                    {
                        int theRemoveSize = mSelIndices[1] - mSelIndices[0];
                        int theNextCarotIndex = mSelIndices[0];
                        MoveCarot(theNextCarotIndex - mCarotIndex, false);
                        SetText(mUnsecureText.Remove(theNextCarotIndex, theRemoveSize), false);
                        ComputeCharWidths();
                    }
                    else
                    {
                        if (mCarotIndex > 0)
                        {
                            MoveCarot(-1, false);
                            SetText(mUnsecureText.Remove(mCarotIndex, 1), false);
                            ComputeCharWidths();
                        }
                    }
                }
            }
            else if (c == '\n' || c == '\r')
            {
                if (OnComplete.Length > 0 && !Gui.bInputFrozen)
                {
                    // trigger callback
                    if (OverrideForm)
                        OverrideForm.SendMessageUpwards(OnComplete);
                    else
                        SendMessageUpwards(OnComplete);
                }
            }
            else
            {
                if (mSelOn && mSelIndices[1] > mSelIndices[0])
                {
                    int theRemoveSize = mSelIndices[1] - mSelIndices[0];
                    int theNextCarotIndex = mSelIndices[0];
                    MoveCarot(theNextCarotIndex - mCarotIndex, false);
                    SetText(mUnsecureText.Remove(theNextCarotIndex, theRemoveSize), false);
                }

                if (MaxLength <= 0 || mUnsecureText.Length < MaxLength)
                {
                    SetText(mUnsecureText.Insert(mCarotIndex, c.ToString()), false);
                    ComputeCharWidths();
                    MoveCarot(1, false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCarot(-1, true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCarot(1, true);
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            MoveCarot(-1000, true);
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            MoveCarot(1000, true);
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Select All");
            SelectAll();
        }

        return theResult;
    }

    // ***********************************************
    // MoveCarot
    // -----------------------------------------------
    private void MoveCarot(int inDir, bool inAllowSel)
    {
        bool theSelOn = (inAllowSel && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));
        if (theSelOn && !mSelOn)
            mSelIndices[0] = mSelIndices[1] = mCarotIndex;
        mSelOn = theSelOn;

        if (inDir > 0)
        {
            while (inDir > 0 && mCarotIndex < mCharWidths.Count)
            {
                inDir--;
                if (mSelIndices[0] == mCarotIndex && mSelIndices[1] > mSelIndices[0])
                    mSelIndices[0]++;
                if (mSelIndices[1] == mCarotIndex)
                    mSelIndices[1]++;
                mCarotIndex++;
                if (Carot)
                    Carot.transform.position += TextMesh.transform.right * mCharWidths[mCarotIndex - 1];
            }
        }
        else if (inDir < 0)
        {
            while (inDir < 0 && mCarotIndex > 0)
            {
                inDir++;
                if (mSelOn)
                {
                    if (mSelIndices[1] == mCarotIndex && mSelIndices[1] > mSelIndices[0])
                        mSelIndices[1]--;
                    if (mSelIndices[0] == mCarotIndex)
                        mSelIndices[0]--;
                    
                }
                mCarotIndex--;
                if (Carot)
                    Carot.transform.position -= TextMesh.transform.right * mCharWidths[mCarotIndex];
            }
        }

        UpdateSelectGO();
    }

    // ***********************************************
    // SelectAll
    // -----------------------------------------------
    private void SelectAll()
    {
        // compute character widths
        ComputeCharWidths();

        mSelOn = true;
        mSelIndices[0] = 0;
        mSelIndices[1] = mCharWidths.Count;
        UpdateSelectGO();
    }

    // ***********************************************
    // UpdateSelectGO
    // -----------------------------------------------
    private void UpdateSelectGO()
    {
        if (Select)
        {
            if (mFocused && mSelOn)
            {
                Vector3 thePos = Select.transform.localPosition;
                Vector3 theScale = Select.transform.localScale;
                thePos.x = 0.0f;

                float[] theWidthSel = { 0, 0 };
                float theWidthT = 0.0f;
                for (int i = 0; i <= mSelIndices[1]; i++)
                {
                    if (i == mSelIndices[0])
                        theWidthSel[0] = theWidthT;
                    if (i == mSelIndices[1])
                        theWidthSel[1] = theWidthT;
                    if (i < mCharWidths.Count)
                        theWidthT += mCharWidths[i];
                }

                thePos.x += (theWidthSel[0] + theWidthSel[1]) * 0.5f;
                theScale.x = (theWidthSel[1] - theWidthSel[0]);

                Select.SetActiveRecursively(true);
                Select.transform.localPosition = thePos;
                Select.transform.localScale = theScale;
            }
            else
            {
                Select.SetActiveRecursively(false);
            }
        }
    }

    // ***********************************************
    // ComputeCharWidths
    // -----------------------------------------------
    private void ComputeCharWidths()
    {
        // save string
        string theSavedString = TextMesh.text;

        // spaces are NOT drawn and therefore doesn't change the size of render bounds
        // so use 'i' for every space when computing width computing width
        string theWidthString = TextMesh.text.Replace(' ', 'i');

        mCharWidths.Clear();
        float theLastSizeX = 0.0f;
        for (int i = 0; i < theWidthString.Length; i++)
        {
            TextMesh.text = theWidthString.Substring(0, i + 1);
            //float theSizeX = TextMesh.renderer.bounds.size.x;
            float theSizeX = TextMesh.renderer.bounds.size.y;
            mCharWidths.Add(theSizeX - theLastSizeX);
            theLastSizeX = theSizeX;
        }

        // restore text
        TextMesh.text = theSavedString;
    }
}
