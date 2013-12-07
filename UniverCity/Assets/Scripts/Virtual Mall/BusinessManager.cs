using System.Net.Security;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;

public class BusinessManager : MonoBehaviour 
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
    // NOTE:: All of the businesses are being handled by reference, so there are no
    // duplicates in memory. I repeat: All businesses exist only once in memory.
    // Used to sort the businesses by categories.
    public Dictionary<string, List<Business>> businessesByCategory = new Dictionary<string, List<Business>>();
    // Just a straight list of the businesses.
    public List<Business> businesses = new List<Business>();
    public Dictionary<Vector2, List<Business>> busByCoord = new Dictionary<Vector2, List<Business>>();
    public GameObject loader;
    public GameObject loginPanel;
    public Transform businessAdPrefab;

	// Use this for initialization
	void Start ()
	{
	    Debug.Log("USING: " + serverURL);
        DontDestroyOnLoad(this);
        if (GameObject.FindGameObjectWithTag("BusinessManager") == null)
        {
            gameObject.tag = "BusinessManager";

            StartCoroutine(GetBusinessInformation());

        }
        else
            Destroy(gameObject);
        
	}

    // ***************************************************
    // Initial pull from the UniverCity servert for the 
    // business information.
    // ---------------------------------------------------
    public IEnumerator GetBusinessInformation()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) ;
            //loader.SetActive(true);
        else if (Application.platform == RuntimePlatform.Android ||
                 Application.platform == RuntimePlatform.IPhonePlayer)
            NativeDialogs.Instance.ShowProgressDialog("Please Wait", "Loading Businesses", false, false);  
        string bURL = serverURL + "BusinessInfo?u=UofO";
        string bLURL = serverURL + "BusinessLogos?b=";
        string bName = "";
        string bDesc = "";
        int id = 0;
        Vector2 busPos;
        WWW page = null;
        bool goodDownload = false;

        // This string will be populated with a list of all business IDs.
        string businessIDs = "";

        while (!goodDownload)
        {
            page = new WWW(bURL);
            yield return page;

            if (page.error == null && page.text != null && page.isDone)
                goodDownload = true;
        }


        // Create an IList of all of the businesses returned to me.
        List<object> businessInfo = Json.Deserialize(page.text) as List<object>;
        // Iterate through each of the dictionaries in the list.
        foreach (Dictionary<string, object> business in businessInfo)
        {
            // Retrieve the ID, Name, and Description of each business.
            id = Convert.ToInt32(business["id"]);
            bName = business["name"] as string;
            bDesc = business["desc"] as string;
            Business bus = new Business(id, bName, bDesc);
            businesses.Add(bus);
            // Check to see if the business has a discount...
            if (business.ContainsKey("discount"))
            {
                Dictionary<string, object> discount = business["discount"] as Dictionary<string, object>;
                bus.AddDiscount(discount["title"] as string, discount["desc"] as string);

            }

            // Check to see if the business has a deal...
            if (business.ContainsKey("megadeal"))
            {
                Dictionary<string, object> deal = business["megadeal"] as Dictionary<string, object>;
                bus.AddDeal(deal["title"] as string, deal["desc"] as string);
            }

            if (business.ContainsKey("hasAd"))
                bus.hasAd = Convert.ToBoolean(business["hasAd"]);

            // Get the X, Z coordinates.
            if (business.ContainsKey("locs"))
            {
                foreach (Dictionary<string, object> coords in (IList)business["locs"])
                {
                    try
                    {
                        bus.xPos = (float) Convert.ToDouble(coords["x"]);
                        bus.zPos = (float) Convert.ToDouble(coords["z"]);
                        busPos.x = bus.xPos;
                        busPos.y = bus.zPos;
                    }
                    catch (KeyNotFoundException)
                    {
                        busPos.x = -1;
                        busPos.y = -1;
                    }

                    if (!busByCoord.ContainsKey(busPos))
                    {
                        busByCoord[busPos] = new List<Business>();
                    }

                    busByCoord[busPos].Add(bus);
                }
            }


            // All arrays in JSON come in as List<object>.
            foreach (string category in business["cats"] as List<object>)
            {
                // If the category does not exist, we need to create it...
                if (!businessesByCategory.ContainsKey(category))
                    businessesByCategory.Add(category, new List<Business>());
                // Add the business to the category it belongs to.
                businessesByCategory[category].Add(bus);
            }
        }

        // Create the list of business IDs.
        for (int i = 0; i < businesses.Count; ++i)
            businessIDs += businesses[i].id + ",";
        // Trim the trailing comma.
        bLURL += businessIDs.Trim(","[0]);

        goodDownload = false;
        while (!goodDownload)
        {
            // Do not touch this next line. If you change it, and the logos
            // no longer load, increment the following counter: 0
            string newURL = bLURL;
            page = new WWW(newURL);
            yield return page;

            if (page.error == null && page.texture != null)
                goodDownload = true;
        }

        // Put the resulting image in a Texture2D to work with...
        Texture2D allLogos = page.texture;
        int imageWidthHeight = allLogos.width;

        // Now to go through and add all of the textures to the businesses...
        for (int i = 0; i < businesses.Count; ++i)
        {
            businesses[i].logo = new Texture2D(imageWidthHeight, imageWidthHeight);
            businesses[i].logo.SetPixels(allLogos.GetPixels(0, imageWidthHeight * (businesses.Count - i - 1),
                                         imageWidthHeight, imageWidthHeight));
            // Apply the new logo.
            businesses[i].logo.Apply();
        }

        if (Application.platform == RuntimePlatform.WindowsEditor) ;
            //loader.SetActive(false);
        else if (Application.platform == RuntimePlatform.Android ||
                 Application.platform == RuntimePlatform.IPhonePlayer)
            NativeDialogs.Instance.HideProgressDialog();
    }
}

[Serializable]
public class Business
{
    public int id;
    public string desc;
    public string name;
    public bool hasMegaDeal = false;
    public bool hasDiscount = false;
    public string dealName;
    public string dealDesc;
    public string discountName;
    public string discountDesc;
    public Texture2D logo;
    public float xPos;
    public float zPos;
    public bool hasAd = false;

    public Business(int id, string name, string desc)
    {
        this.id = id;
        this.desc = desc;
        this.name = name;
    }

    public void AddDeal(string name, string desc)
    {
        hasMegaDeal = true;
        dealName = name;
        dealDesc = desc;
    }

    public void AddDiscount(string name, string desc)
    {
        hasDiscount = true;
        discountName = name;
        discountDesc = desc;
    }
}