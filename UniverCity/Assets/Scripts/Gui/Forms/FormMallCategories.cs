using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormMallCategories : MonoBehaviour
{
    // publics
    public static FormMallCategories mInstance = null;
    public GameObject Anim;
    public UITouch Scroller;
    public Transform Pivot;
    public Button[] ScrollButtons;

    // privates
    private Vector3 mPivotStartPos;
    private List<GameObject> mButtonGOs = new List<GameObject>();

    [System.Serializable]
    public class Button
    {
        public eMallCategory Enum;
        public Texture2D Icon;
        public string TopText;
        public string BottomText;
    }

    const float BUTTON_SPACING = 2.55f;

    // ***************************************************
    // Start [Monobehavior]
    // ---------------------------------------------------
	void Start()
    {
        mInstance = this;
        //Hide();

        mPivotStartPos = Pivot.position;
	}

    void OnEnable()
    {
        mInstance = this;

        mPivotStartPos = Pivot.position;
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
        Debug.Log("Showing.");
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
        Debug.Log("Showing categories.");
        Pivot.position = mPivotStartPos;
        Vector3 thePos = Pivot.position;

        for (int i = 0; i < ScrollButtons.Length; i++)
        {
            GameObject theButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/FormMallCategories/Button"), thePos, Quaternion.identity);
            theButton.GetComponent<MallScrollButton>().Setup(ScrollButtons[i]);
            theButton.transform.parent = Pivot;
            mButtonGOs.Add(theButton);

            thePos.y -= BUTTON_SPACING;
        }
    }

    void DoHide()
    {
        foreach (GameObject go in mButtonGOs)
            Destroy(go);
        mButtonGOs.Clear();
    }

    void Update()
    {
        if (Scroller.GetTransform())
        {
            Pivot.position += Scroller.GetTransformDeltaPos();
            for (int i = 0; i < mButtonGOs.Count; i++)
            {
                Vector3 theButtonPos = mButtonGOs[i].transform.position;
                if (theButtonPos.y < -BUTTON_SPACING * 0.5f * mButtonGOs.Count)
                {
                    theButtonPos.y += BUTTON_SPACING * mButtonGOs.Count;
                    mButtonGOs[i].transform.position = theButtonPos;
                }
                else if (theButtonPos.y > BUTTON_SPACING * 0.5f * mButtonGOs.Count)
                {
                    theButtonPos.y -= BUTTON_SPACING * mButtonGOs.Count;
                    mButtonGOs[i].transform.position = theButtonPos;
                }
            }
        }
    }

    #region GuiStuff

    void GuiButton()
    {
        Button theButton = null;
        // get index of button
        for (int i = 0; i < mButtonGOs.Count; i++)
        {
            if (mButtonGOs[i].transform == Gui.FocusControl)
            {
                theButton = ScrollButtons[i];
                break;
            }
        }

        if (theButton != null)
        {
            // reset all buttons and disable button to function like option
            for (int i = 0; i < mButtonGOs.Count; i++)
                mButtonGOs[i].GetComponent<GuiButton>().SetState(global::GuiButton.eState.Normal);
            Gui.FocusControl.GetComponent<GuiButton>().SetState(global::GuiButton.eState.Disabled);

            if (FormMallSubcategories.GetVisible())
                FormMallSubcategories.Hide();

            if (FormMallDetailedCategories.GetVisible())
                FormMallDetailedCategories.Hide();

            if (FormMallAppPanel.GetVisible())
                FormMallAppPanel.Hide();

            FormMallSubcategories.Show(theButton.Enum);
        }
    }

    #endregion
}
