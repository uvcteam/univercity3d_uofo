using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormMallSubcategories : MonoBehaviour
{
    // publics
    public static FormMallSubcategories mInstance = null;
    public GameObject Anim;
    public UITouch Scroller;
    public Transform Pivot;
    public Button[] ArtsAndEntertainment;
    public Button[] FoodDrinkAndDining;
    public Button[] PlacesToStay;
    public Button[] TravelToursAndTransportation;
    public Button[] HealthFitnessAndBeauty;
    public Button[] Medical;
    public Button[] FinancialAndLegalServices;
    public Button[] HomeHookupAndServices;
    public Button[] Housing;
    public Button[] AptHomeFurnishings;
    public Button[] ClothingAndAccessories;
    public Button[] MovingAndStorage;
    public Button[] MaterialsAndSupplies;
    public Button[] CommunityConnections;
    public Button[] SnailMail;
    public Button[] VehicleSalesAndService;
    public Button[] StuffYouJustGottaHave;
    public Button[] SportingGoodsAndFacilities;
    public Button[] StoresAndPlacesToShop;
    public Button[] InCaseOfEmergency;

    // privates
    private Vector3 mPivotStartPos;
    private eMallCategory mCategory;
    private Button[] mButtons;
    private List<GameObject> mButtonGOs = new List<GameObject>();

    [System.Serializable]
    public class Button
    {
        public eMallSubcategory Enum;
        public Texture2D Icon;
        public string TopText;
        public string BottomText;
    }

    const float BUTTON_SPACING = 2.0f;

    // ***************************************************
    // Start [Monobehavior]
    // ---------------------------------------------------
	void Start()
    {
        mInstance = this;

        mPivotStartPos = Pivot.position;

        Hide();
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

    static public void Show(eMallCategory inCategory, float inOffsetX = 0.0f)
    {
        if (mInstance)
        {
            mInstance.gameObject.SetActiveRecursively(true);
            mInstance.DoShow(inCategory, inOffsetX);
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

    void DoShow(eMallCategory inCategory, float inOffsetX = 0.0f)
    {
        if (inOffsetX != 0.0f)
            Gui.Transition(Anim, inOffsetX);

        Pivot.position = mPivotStartPos;
        Vector3 thePos = Pivot.position;

        mCategory = inCategory;
        switch(mCategory)
        {
            default:
            case eMallCategory.ArtsAndEntertainment:
                mButtons = ArtsAndEntertainment;
                break;
            case eMallCategory.FoodDrinkAndDining:
                mButtons = FoodDrinkAndDining;
                break;
            case eMallCategory.PlacesToStay:
                mButtons = PlacesToStay;
                break;
            case eMallCategory.TravelToursAndTransportation:
                mButtons = TravelToursAndTransportation;
                break;
            case eMallCategory.HealthFitnessAndBeauty:
                mButtons = HealthFitnessAndBeauty;
                break;
            case eMallCategory.Medical:
                mButtons = Medical;
                break;
            case eMallCategory.FinancialAndLegalServices:
                mButtons = FinancialAndLegalServices;
                break;
            case eMallCategory.HomeHookupAndServices:
                mButtons = HomeHookupAndServices;
                break;
            case eMallCategory.Housing:
                mButtons = Housing;
                break;
            case eMallCategory.AptHomeFurnishings:
                mButtons = AptHomeFurnishings;
                break;
            case eMallCategory.ClothingAndAccessories:
                mButtons = ClothingAndAccessories;
                break;
            case eMallCategory.MovingAndStorage:
                mButtons = MovingAndStorage;
                break;
            case eMallCategory.MaterialsAndSupplies:
                mButtons = MaterialsAndSupplies;
                break;
            case eMallCategory.CommunityConnections:
                mButtons = CommunityConnections;
                break;
            case eMallCategory.SnailMail:
                mButtons = SnailMail;
                break;
            case eMallCategory.VehicleSalesAndService:
                mButtons = VehicleSalesAndService;
                break;
            case eMallCategory.StuffYouJustGottaHave:
                mButtons = StuffYouJustGottaHave;
                break;
            case eMallCategory.SportingGoodsAndFacilities:
                mButtons = SportingGoodsAndFacilities;
                break;
            case eMallCategory.StoresAndPlacesToShop:
                mButtons = StoresAndPlacesToShop;
                break;
            case eMallCategory.InCaseOfEmergency:
                mButtons = InCaseOfEmergency;
                break;
        }


        for (int i = 0; i < mButtons.Length; i++)
        {
            GameObject theButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/FormMallCategories/Button"), thePos, Quaternion.identity);
            theButton.GetComponent<MallScrollButton>().Setup(mButtons[i]);
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
                theButton = mButtons[i];
                break;
            }
        }

        if (theButton != null)
        {
            // reset all buttons and disable button to function like option
            for (int i = 0; i < mButtonGOs.Count; i++)
                mButtonGOs[i].GetComponent<GuiButton>().SetState(global::GuiButton.eState.Normal);
            Gui.FocusControl.GetComponent<GuiButton>().SetState(global::GuiButton.eState.DisabledButActive);

            if (FormMallDetailedCategories.GetVisible())
                FormMallDetailedCategories.Hide();

            if (FormMallAppPanel.GetVisible())
                FormMallAppPanel.Hide();

            FormMallDetailedCategories.Show(theButton.Enum);
        }
    }

    #endregion
}
