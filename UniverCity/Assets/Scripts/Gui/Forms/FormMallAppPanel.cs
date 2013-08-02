using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormMallAppPanel : MonoBehaviour
{
    // publics
    public static FormMallAppPanel mInstance = null;
    public GameObject Anim;
    public UITouch Scroller;
    public Transform Pivot;

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

    const float BUTTON_SPACING = 3.85f;

    // ***************************************************
    // Start [Monobehavior]
    // ---------------------------------------------------
	void Start()
    {
        mInstance = this;
        Hide();

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

    static public void Show(string category, float inOffsetX = 0.0f)
    {
        if (mInstance)
        {
            mInstance.gameObject.SetActiveRecursively(true);
            mInstance.DoShow(category, inOffsetX);
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

    void DoShow(string category, float inOffsetX = 0.0f)
    {
        BusinessManager manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();

        if (inOffsetX != 0.0f)
            Gui.Transition(Anim, inOffsetX);

        Pivot.position = mPivotStartPos;
        Vector3 thePos = Pivot.position;

        //Debug.Log(category.ToString());
        // Error-checking to make sure the key exists.
        if (manager.businessesByCategory.ContainsKey(category.ToString()))
        {
            foreach (Business business in manager.businessesByCategory[category.ToString()])
            {
                GameObject theButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/FormMallAppPanel/Button"), thePos, Quaternion.identity);
                // Set up the information.
                MallAd currentAd = theButton.GetComponent<MallAd>();
                currentAd.TextHeadline.text = business.name;
                // Placeholder until I can fix the text showing too much.
                currentAd.TextDescription.text = ResolveTextSize(business.desc, 30, 150);
         
                currentAd.Icon.material.mainTexture = business.logo;
                currentAd.AdOwner = business;

                if (business.hasDiscount)
                {
                    // Pull the discount from the web server...
                    currentAd.RootMembersOnly.transform.Find("lbHeadline").GetComponent<TextMesh>().text = business.discountName;
                    currentAd.RootMembersOnly.transform.Find("lbDescription").GetComponent<TextMesh>().text = business.discountDesc;
                }
                else
                {
                    // Show that there is no discount...
                    //currentAd.RootMembersOnly.transform.Find("lbHeadline").GetComponent<TextMesh>().text = "Discounts";
                    //currentAd.RootMembersOnly.transform.Find("lbDescription").GetComponent<TextMesh>().text = "There are currently no discounts.";
                    theButton.transform.FindChild("RootInfo").transform.FindChild("bnMembersOnly").gameObject.SetActiveRecursively(false);
                }

                if (business.hasMegaDeal)
                {
                    // Pull the deal from the web server...
                    currentAd.RootMegaDeal.transform.Find("lbHeadline").GetComponent<TextMesh>().text = business.dealName;
                    currentAd.RootMegaDeal.transform.Find("lbDescription").GetComponent<TextMesh>().text = business.dealDesc;
                }
                else
                {
                    // Show that there is no deal...
                    //currentAd.RootMegaDeal.transform.Find("lbHeadline").GetComponent<TextMesh>().text = "Mega Deal";
                    //currentAd.RootMegaDeal.transform.Find("lbDescription").GetComponent<TextMesh>().text = "There are currently no mega deals.";
                    theButton.transform.FindChild("RootInfo").transform.FindChild("bnMegaDeal").gameObject.SetActiveRecursively(false);
                }

                theButton.transform.parent = Pivot;
                mButtonGOs.Add(theButton);

                thePos.y -= BUTTON_SPACING;
            }
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

    private string ResolveTextSize(string input, int lineLength, int maxLength)
    {
        string[] words = input.Split(" "[0]);
        string result = "";
        string line = "";

        foreach (string s in words)
        {
            string temp = line + " " + s;
            if (temp.Length > lineLength)
            {
                result += line + "\n";
                line = s;
            }
            else
                line = temp;

            if (result.Length >= maxLength)
                return result.Substring(1, result.Length - 1);
        }

        result += line;
        return result.Substring(1, result.Length - 1);
    }

    #region GuiStuff

    void GuiButton()
    {
        int theButtonIndex = -1;

        // get index of button
        for (int i = 0; i < mButtonGOs.Count; i++)
        {
            if (mButtonGOs[i].transform == Gui.FocusControl)
            {
                theButtonIndex = i;
                break;
            }
        }

        if (theButtonIndex >= 0)
        {
            //if (FormMallSubcategories.GetVisible())
            //    FormMallSubcategories.Hide();

            //FormMallSubcategories.Show(theButton.Enum);

            Debug.Log("Detailed Button Pressed...");
        }
    }

    #endregion
}
