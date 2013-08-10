using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;

public class BusinessManager : MonoBehaviour 
{
    // NOTE:: All of the businesses are being handled by reference, so there are no
    // duplicates in memory. I repeat: All businesses exist only once in memory.
    // Used to sort the businesses by categories.
    public Dictionary<string, List<Business>> businessesByCategory = new Dictionary<string, List<Business>>();
    // Just a straight list of the businesses.
    public List<Business> businesses = new List<Business>();
    public Dictionary<Vector2, List<Business>> busByCoord = new Dictionary<Vector2, List<Business>>();
    public GameObject loader;

	// Use this for initialization
	void Start () 
    {
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
        loader.SetActive(true);
        string bURL = "http://www.univercity3d.com/univercity/BusinessInfo?u=UofO";
        string bLURL = "http://www.univercity3d.com/univercity/BusinessLogos?b=";
        string bName = "";
        string bDesc = "";
        int id = 0;
        Vector2 busPos;

        // This string will be populated with a list of all business IDs.
        string businessIDs = "";
        
        WWW page = new WWW(bURL);
        yield return page;

        //StreamWriter sw = File.CreateText("businesses.dat");
        //sw.Write(page.text);
        //sw.Close();

        // Create an IList of all of the businesses returned to me.
        IList businessInfo = Json.Deserialize(page.text) as IList;
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

                // Debug to check out the discount...
                //Debug.Log("Found a discount!");
                //Debug.Log(bus.name + " - " + bus.discountName + " - " + bus.discountDesc);
            }

            // Check to see if the business has a deal...
            if (business.ContainsKey("megadeal"))
            {
                Dictionary<string, object> deal = business["megadeal"] as Dictionary<string, object>;
                bus.AddDeal(deal["title"] as string, deal["desc"] as string);

                // Debug to check out the deal...
                //Debug.Log("Found a discount!");
                //Debug.Log(bus.name + " - " + bus.dealName + " - " + bus.dealDesc);
            }

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
                    catch (KeyNotFoundException e)
                    {
                        Debug.Log(e);
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

            // Debug to see if the companies acually read in properly.
            //Debug.Log(id + " - " + bName + " - " + bDesc);

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

        // Debug to make sure the categories were all created properly.
        //foreach (KeyValuePair<string, List<Business>> pair in businesses)
        //{
        //    Debug.Log(pair.Key);
        //}

        // Create the list of business IDs.
        for (int i = 0; i < businesses.Count; ++i)
            businessIDs += businesses[i].id + ",";
        // Trim the trailing comma.
        bLURL += businessIDs.Trim(","[0]);

        Debug.Log(bLURL);

        // Get the list of images...
        page = new WWW(bLURL);
        yield return page;

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

        loader.SetActive(false);
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