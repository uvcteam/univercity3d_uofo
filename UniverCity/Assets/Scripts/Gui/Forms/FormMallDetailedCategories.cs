using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormMallDetailedCategories : MonoBehaviour
{
    // publics
    public static FormMallDetailedCategories mInstance = null;
    public GameObject Anim;
    public UITouch Scroller;
    public Transform Pivot;
    public eMallSubcategory currentCategory;

    // privates
    private Vector3 mPivotStartPos;
    private List<GameObject> mButtonGOs = new List<GameObject>();
    private string[] theCategories = { "" };

    [System.Serializable]
    public class Button
    {
        public eMallCategory Enum;
        public Texture2D Icon;
        public string TopText;
        public string BottomText;
    }

    const float BUTTON_SPACING = 1.525f;

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

    static public void Show(eMallSubcategory inSubcategory, float inOffsetX = 0.0f)
    {
        if (mInstance)
        {
            mInstance.gameObject.SetActiveRecursively(true);
            mInstance.DoShow(inSubcategory, inOffsetX);
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

    void DoShow(eMallSubcategory inSubcategory, float inOffsetX = 0.0f)
    {
        currentCategory = inSubcategory;
        if (inOffsetX != 0.0f)
            Gui.Transition(Anim, inOffsetX);

        Pivot.position = mPivotStartPos;
        Vector3 thePos = Pivot.position;

        // Hardcoded for now, this will change later.
        switch (inSubcategory)
        {
            case eMallSubcategory.AAE_Upscale:
                theCategories = new string[3];
                theCategories[0] = "Art Galleries";
                theCategories[1] = "Museums";
                theCategories[2] = "Performing Arts";
                break;
            case eMallSubcategory.AAE_FunStuff:
                theCategories = new string[6];
                theCategories[0] = "Amusement Parks";
                theCategories[1] = "Arcades";
                theCategories[2] = "Cinema/Movie Theatres";
                theCategories[3] = "Hobbies";
                theCategories[4] = "Indoor Sports/Entertainment";
                theCategories[5] = "Outdoor Sports/Entertainment";
                break;
            case eMallSubcategory.AAE_Howlin:
                theCategories = new string[8];
                theCategories[0] = "Bars/Night Clubs";
                theCategories[1] = "Brew Pubs";
                theCategories[2] = "Casinos";
                theCategories[3] = "Live Music Clubs/Dancing";
                theCategories[4] = "Jazz and Blues";
                theCategories[5] = "Concert/Music Venues";
                theCategories[6] = "Ticket Sales";
                theCategories[7] = "Wineries";
                break;
            case eMallSubcategory.FDD_Restaurants:
                theCategories = new string[15];
                theCategories[0] = "American";
                theCategories[1] = "Cajun/Creole";
                theCategories[2] = "Chinese";
                theCategories[3] = "French";
                theCategories[4] = "Greek";
                theCategories[5] = "Indian";
                theCategories[6] = "Italian";
                theCategories[7] = "Japanese";
                theCategories[8] = "Mediterranean";
                theCategories[9] = "Mexican & South American";
                theCategories[10] = "Middle Eastern";
                theCategories[11] = "Seafood";
                theCategories[12] = "Steaks & Chops";
                theCategories[13] = "Thai";
                theCategories[14] = "Vietnamese";
                break;
            case eMallSubcategory.FDD_SpecialTastes:
                theCategories = new string[9];
                theCategories[0] = "Bakeries";
                theCategories[1] = "BBQ/Barbecue";
                theCategories[2] = "Breakfast & Brunch";
                theCategories[3] = "Buffets";
                theCategories[4] = "Delis";
                theCategories[5] = "Diners";
                theCategories[6] = "Pizza";
                theCategories[7] = "Vegetarian";
                theCategories[8] = "Yogurt";
                break;
            case eMallSubcategory.FDD_NowFood:
                theCategories = new string[3];
                theCategories[0] = "Fast Food";
                theCategories[1] = "Food Courts";
                theCategories[2] = "Take Out & Delivery";
                break;
            case eMallSubcategory.FDD_CoffeeTeaJuice:
                theCategories = new string[2];
                theCategories[0] = "Coffees & Tees";
                theCategories[1] = "Juice Bars";
                break;
            case eMallSubcategory.PTS_BedBreakfast:
                theCategories = new string[1];
                theCategories[0] = "Bed & Breakfast";
                break;
            case eMallSubcategory.PTS_Campgrounds:
                theCategories = new string[1];
                theCategories[0] = "Campgrounds";
                break;
            case eMallSubcategory.PTS_GuestHousesHostels:
                theCategories = new string[1];
                theCategories[0] = "Guest Houses & Hostels";
                break;
            case eMallSubcategory.PTS_HotelsMotels:
                theCategories = new string[1];
                theCategories[0] = "Hotels & Motels";
                break;
            case eMallSubcategory.PTS_Resorts:
                theCategories = new string[1];
                theCategories[0] = "Resorts";
                break;
            case eMallSubcategory.PTS_VacationRentals:
                theCategories = new string[1];
                theCategories[0] = "Vacation Rentals";
                break;
            case eMallSubcategory.TTT_TravelAgencies:
                theCategories = new string[1];
                theCategories[0] = "Travel Agencies";
                break;
            case eMallSubcategory.TTT_Tickets:
                theCategories = new string[3];
                theCategories[0] = "Airline";
                theCategories[1] = "Bus";
                theCategories[2] = "Trains";
                break;
            case eMallSubcategory.TTT_Tours:
                theCategories = new string[2];
                theCategories[0] = "Local Tours";
                theCategories[1] = "Cruises";
                break;
            case eMallSubcategory.TTT_TaxisAndLimos:
                theCategories = new string[2];
                theCategories[0] = "Taxis";
                theCategories[1] = "Limo Services";
                break;
            case eMallSubcategory.HFB_BodyArt:
                theCategories = new string[3];
                theCategories[0] = "Tattoos & Piercing";
                theCategories[1] = "Manicures, Pedicures & Nails";
                theCategories[2] = "Tanning, Waxing, etc";
                break;
            case eMallSubcategory.HFB_HairTreatment:
                theCategories = new string[2];
                theCategories[0] = "Barbers";
                theCategories[1] = "Hair Salons";
                break;
            case eMallSubcategory.HFB_CosmeticPlasticSurgery:
                theCategories = new string[1];
                theCategories[0] = "Cosmetic/Plastic Surgery";
                break;
            case eMallSubcategory.HFB_Fitness:
                theCategories = new string[5];
                theCategories[0] = "Gym/Health Club";
                theCategories[1] = "Yoga";
                theCategories[2] = "Martial Arts";
                theCategories[3] = "Weight Loss Centers";
                theCategories[4] = "Recreation Centers/Facilities";
                break;
            case eMallSubcategory.MED_Accupuncture:
                theCategories = new string[1];
                theCategories[0] = "Accupuncture";
                break;
            case eMallSubcategory.MED_AddictionTreatment:
                theCategories = new string[1];
                theCategories[0] = "Addiction Treatment";
                break;
            case eMallSubcategory.MED_AudiologyHearing:
                theCategories = new string[1];
                theCategories[0] = "Audiology/Hearing";
                break;
            case eMallSubcategory.MED_Chiropractors:
                theCategories = new string[1];
                theCategories[0] = "Chiropractors";
                break;
            case eMallSubcategory.MED_CounselingMentalHealth:
                theCategories = new string[1];
                theCategories[0] = "Counseling & Mental Health";
                break;
            case eMallSubcategory.MED_Dentists:
                theCategories = new string[1];
                theCategories[0] = "Dentists";
                break;
            case eMallSubcategory.MED_OptometryEyeGlassesContacts:
                theCategories = new string[1];
                theCategories[0] = "Optometry, Eye Glasses & Contacts";
                break;
            case eMallSubcategory.MED_HerbalMedicine:
                theCategories = new string[1];
                theCategories[0] = "Herbal Medicine";
                break;
            case eMallSubcategory.MED_Hospitals:
                theCategories = new string[1];
                theCategories[0] = "Hospitals";
                break;
            case eMallSubcategory.MED_LasikSurgery:
                theCategories = new string[1];
                theCategories[0] = "Lasik Surgery";
                break;
            case eMallSubcategory.MED_MassageTherapy:
                theCategories = new string[1];
                theCategories[0] = "Massage Therapy";
                break;
            case eMallSubcategory.MED_Nutritionists:
                theCategories = new string[1];
                theCategories[0] = "Nutritionists";
                break;
            case eMallSubcategory.MED_PhysicalTherapy:
                theCategories = new string[1];
                theCategories[0] = "Physical Therapy";
                break;
            case eMallSubcategory.MED_Physicians:
                theCategories = new string[7];
                theCategories[0] = "Allergies & Asthma";
                theCategories[1] = "Dermatology";
                theCategories[2] = "Ear, Nose & Throat";
                theCategories[3] = "Family Practice";
                theCategories[4] = "Ob/Gyn";
                theCategories[5] = "Orthopedics/Sports Medicine";
                theCategories[6] = "Internal Medicine";
                break;
            case eMallSubcategory.MED_SpeechTherapy:
                theCategories = new string[1];
                theCategories[0] = "Speech Therapy";
                break;
            case eMallSubcategory.MED_UrgentCare:
                theCategories = new string[1];
                theCategories[0] = "Urgent Care";
                break;
            case eMallSubcategory.FLS_MoneyPeople:
                theCategories = new string[5];
                theCategories[0] = "Accountants";
                theCategories[1] = "Banks and Credit Unions";
                theCategories[2] = "Check Cashing/Pay Day Loans";
                theCategories[3] = "Financial Planning Advisors";
                theCategories[4] = "Investment";
                break;
            case eMallSubcategory.FLS_Insurance:
                theCategories = new string[3];
                theCategories[0] = "Health Insurance";
                theCategories[1] = "Automobile Insurance";
                theCategories[2] = "Apartment/House Insurance";
                break;
            case eMallSubcategory.FLS_Attorneys:
                theCategories = new string[2];
                theCategories[0] = "General Practice";
                theCategories[1] = "Accident & Personal Injury";
                break;
            case eMallSubcategory.FLS_TaxPreparation:
                theCategories = new string[1];
                theCategories[0] = "Tax Preparation Services";
                break;
            case eMallSubcategory.HHS_Utilities:
                theCategories = new string[8];
                theCategories[0] = "Electrical Services";
                theCategories[1] = "Gas Services";
                theCategories[2] = "Garbage/Waste Management Services";
                theCategories[3] = "Water Services";
                theCategories[4] = "Television & Cable Services";
                theCategories[5] = "Internet Services";
                theCategories[6] = "Telephone Services";
                theCategories[7] = "Mobile/Cell Phone Services";
                break;
            case eMallSubcategory.HHS_Professional:
                theCategories = new string[15];
                theCategories[0] = "Audio, Video & Home Theatre Install";
                theCategories[1] = "Carpenters";
                theCategories[2] = "Contractors";
                theCategories[3] = "Electricians";
                theCategories[4] = "Handyman";
                theCategories[5] = "Heating and Air Conditioning";
                theCategories[6] = "House Cleaning";
                theCategories[7] = "Keys and Locksmiths";
                theCategories[8] = "Landscaping & Gardeners";
                theCategories[9] = "Painters";
                theCategories[10] = "Plumbers";
                theCategories[11] = "Pool/Spa Services";
                theCategories[12] = "Roofing";
                theCategories[13] = "Security Systems";
                theCategories[14] = "Solar Systems";
                break;
            case eMallSubcategory.H_Rent:
                theCategories = new string[3];
                theCategories[0] = "Apartments";
                theCategories[1] = "Houses";
                theCategories[2] = "Rooms";
                break;
            case eMallSubcategory.H_Realtors:
                theCategories = new string[1];
                theCategories[0] = "Realtors";
                break;
            case eMallSubcategory.AHF_Antiques:
                theCategories = new string[1];
                theCategories[0] = "Antiques";
                break;
            case eMallSubcategory.AHF_ExteriorDecor:
                theCategories = new string[2];
                theCategories[0] = "Outdoor Furniture & Appliances";
                theCategories[1] = "Outdoor Lighting & Accessories";
                break;
            case eMallSubcategory.AHF_InteriorDecor:
                theCategories = new string[8];
                theCategories[0] = "Appliances";
                theCategories[1] = "Bathroom";
                theCategories[2] = "Bedroom";
                theCategories[3] = "Furniture";
                theCategories[4] = "HomeOffice";
                theCategories[5] = "Home Theatre/Entertainment Sys.";
                theCategories[6] = "Kitchen";
                theCategories[7] = "Television/Audio";
                break;
            case eMallSubcategory.CAA_Accessories:
                theCategories = new string[1];
                theCategories[0] = "Accessories";
                break;
            case eMallSubcategory.CAA_ClothingAndApparel:
                theCategories = new string[4];
                theCategories[0] = "Athletic";
                theCategories[1] = "Casual";
                theCategories[2] = "Vintage";
                theCategories[3] = "Wedding and Formal";
                break;
            case eMallSubcategory.CAA_Shoes:
                theCategories = new string[3];
                theCategories[0] = "Athletic Shoes";
                theCategories[1] = "Men's Shoes";
                theCategories[2] = "Women's Shoes";
                break;
            case eMallSubcategory.CAA_VintageClothing:
                theCategories = new string[1];
                theCategories[0] = "Vintage Clothing & Accessories";
                break;
            case eMallSubcategory.CAA_DiscountClothingAndApparel:
                theCategories = new string[3];
                theCategories[0] = "Outlet Stores";
                theCategories[1] = "Thrift Stores";
                theCategories[2] = "Consignment";
                break;
            case eMallSubcategory.MAS_MoversMovingCompanies:
                theCategories = new string[1];
                theCategories[0] = "Movers/Moving Companies";
                break;
            case eMallSubcategory.MAS_MovingBoxesSupplies:
                theCategories = new string[1];
                theCategories[0] = "Moving Boxes and Supplies";
                break;
            case eMallSubcategory.MAS_PodsMobileStorage:
                theCategories = new string[1];
                theCategories[0] = "Pods/Mobile Storage";
                break;
            case eMallSubcategory.MAS_RentalMovingTrucksVansTrailers:
                theCategories = new string[1];
                theCategories[0] = "Rental Moving Trucks/Vans/Trailers";
                break;
            case eMallSubcategory.MAS_SelfStorageFacilities:
                theCategories = new string[1];
                theCategories[0] = "Self-Storage Facilities";
                break;
            case eMallSubcategory.MS_BuildingMaterials:
                theCategories = new string[1];
                theCategories[0] = "Building Materials";
                break;
            case eMallSubcategory.MS_Finishings:
                theCategories = new string[6];
                theCategories[0] = "Carpet and Flooring";
                theCategories[1] = "Windows";
                theCategories[2] = "Curtains, Drapes and Blinds";
                theCategories[3] = "Paint and Wall Coverings";
                theCategories[4] = "Lighting";
                theCategories[5] = "Cabinets and Countertops";
                break;
            case eMallSubcategory.CC_Government:
                theCategories = new string[2];
                theCategories[0] = "DMV";
                theCategories[1] = "Licenses";
                break;
            case eMallSubcategory.CC_Religious:
                theCategories = new string[1];
                theCategories[0] = "Religious Worship";
                break;
            case eMallSubcategory.CC_CommunityPlaces:
                theCategories = new string[1];
                theCategories[0] = "Community Places";
                break;
            case eMallSubcategory.CC_CommunityServices:
                theCategories = new string[1];
                theCategories[0] = "Community Services";
                break;
            case eMallSubcategory.SM_PostOffice:
                theCategories = new string[1];
                theCategories[0] = "Post Office";
                break;
            case eMallSubcategory.SM_OvernightDeliveryService:
                theCategories = new string[1];
                theCategories[0] = "Overnight Delivery Services";
                break;
            case eMallSubcategory.VSS_Sales:
                theCategories = new string[5];
                theCategories[0] = "Autos";
                theCategories[1] = "Motorcycles and Scooters";
                theCategories[2] = "Trucks";
                theCategories[3] = "ATVs";
                theCategories[4] = "Campers and RVs";
                break;
            case eMallSubcategory.VSS_Services:
                theCategories = new string[8];
                theCategories[0] = "Auto Body & Collision Repair";
                theCategories[1] = "Towing Services";
                theCategories[2] = "Mufflers & Exhaust Systems";
                theCategories[3] = "Windshields";
                theCategories[4] = "Tires & Wheel Alignment";
                theCategories[5] = "Gas/Service Stations";
                theCategories[6] = "Transmission";
                theCategories[7] = "Radiator & Cooling Systems";
                break;
            case eMallSubcategory.VSS_TrickingOutRide:
                theCategories = new string[4];
                theCategories[0] = "Audio Systems";
                theCategories[1] = "Wheels";
                theCategories[2] = "Exterior Bling";
                theCategories[3] = "Interior Bling";
                break;
            case eMallSubcategory.SGH_ComputerElectronics:
                theCategories = new string[4];
                theCategories[0] = "Computers/Laptops/Tablets";
                theCategories[1] = "Cameras";
                theCategories[2] = "Television & DVR";
                theCategories[3] = "Audio/Home Theatre Systems";
                break;
            case eMallSubcategory.SGH_PowerTools:
                theCategories = new string[2];
                theCategories[0] = "Shop Tools";
                theCategories[1] = "Yard Tools";
                break;
            case eMallSubcategory.SGH_CardsAndGifts:
                theCategories = new string[1];
                theCategories[0] = "Cards and Gifts";
                break;
            case eMallSubcategory.SGF_OutdoorSports:
                theCategories = new string[1];
                theCategories[0] = "Outdoor Sports";
                break;
            case eMallSubcategory.SGF_IndoorSports:
                theCategories = new string[1];
                theCategories[0] = "Indoor Sports";
                break;
            case eMallSubcategory.SGF_IndoorSportsFacilities:
                theCategories = new string[1];
                theCategories[0] = "Indoor Sports Facilities";
                break;
            case eMallSubcategory.SPS_MallsShoppingCenters:
                theCategories = new string[1];
                theCategories[0] = "Malls & Shopping Centers";
                break;
            case eMallSubcategory.SPS_BigBoxStores:
                theCategories = new string[1];
                theCategories[0] = "Big Box Stores";
                break;
            case eMallSubcategory.SPS_GroceryStores:
                theCategories = new string[1];
                theCategories[0] = "Grocery Stores";
                break;
            case eMallSubcategory.SPS_ClothingStores:
                theCategories = new string[1];
                theCategories[0] = "Clothing Stores";
                break;
            case eMallSubcategory.SPS_PetStores:
                theCategories = new string[1];
                theCategories[0] = "Pet Stores";
                break;
            case eMallSubcategory.SPS_SecondHandConsignment:
                theCategories = new string[1];
                theCategories[0] = "2nd Hand & Consignment";
                break;
            case eMallSubcategory.SPS_LiquorStores:
                theCategories = new string[1];
                theCategories[0] = "Liquor Stores";
                break;
            case eMallSubcategory.SPS_ConvenienceStores:
                theCategories = new string[1];
                theCategories[0] = "Convenience Stores";
                break;
            case eMallSubcategory.ICE_Police:
                theCategories = new string[1];
                theCategories[0] = "Police";
                break;
            case eMallSubcategory.ICE_Fire:
                theCategories = new string[1];
                theCategories[0] = "Fire";
                break;
            case eMallSubcategory.ICE_CampusSecurity:
                theCategories = new string[1];
                theCategories[0] = "Campus Security";
                break;
            case eMallSubcategory.ICE_Ambulance:
                theCategories = new string[1];
                theCategories[0] = "Ambulance";
                break;
            case eMallSubcategory.ICE_PoisonControl:
                theCategories = new string[1];
                theCategories[0] = "Poison Control";
                break;
            case eMallSubcategory.ICE_EmergencyMedical:
                theCategories = new string[1];
                theCategories[0] = "Emergency Medical";
                break;
            default:
                theCategories = new string[1];
                theCategories[0] = "No categories";
                break;
        }        

        for (int i = 0; i < theCategories.Length; i++)
        {
            GameObject theButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/FormMallDetailedCategories/Button"), thePos, Quaternion.identity);
            theButton.GetComponent<MallScrollButton>().Setup(theCategories[i]);
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
            FormMallDetailedCategories.Hide();
            FormMallAppPanel.Show(theCategories[theButtonIndex]);

            //Debug.Log("Detailed Button Pressed...");
        }
    }

    #endregion
}
