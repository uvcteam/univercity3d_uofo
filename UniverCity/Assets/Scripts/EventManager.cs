using System.Globalization;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;

public class EventManager : MonoBehaviour 
{
    // NOTE:: All of the businesses are being handled by reference, so there are no
    // duplicates in memory. I repeat: All businesses exist only once in memory.
    // Used to sort the businesses by categories.
    public Dictionary<string, List<UnionHallEvent>> eventsByCategory = new Dictionary<string, List<UnionHallEvent>>();
    // Just a straight list of the businesses.
    public List<UnionHallEvent> events = new List<UnionHallEvent>();
    public UnionHallEvent currentEvent = null;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
        StartCoroutine(GetEventInformation());
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    // ***************************************************
    // Initial pull from the UniverCity server for the 
    // event information.
    // ---------------------------------------------------
    public IEnumerator GetEventInformation()
    {
        string eURL = "http://wwww.univercity3d.com/univercity/ListEvents";
        int id = 0;
        string phone = "";
        string title = "";
        string desc = "";
        int confirmed = 0;
        DateTime start;
        string who = "";
        string loc = "";
        string email = "";

        WWW page = new WWW(eURL);
        yield return page;

        //StreamWriter sw = File.CreateText("businesses.dat");
        //sw.Write(page.text);
        //sw.Close();

        // Create an IList of all of the businesses returned to me.
        IList eventInfo = Json.Deserialize(page.text) as IList;
        // Iterate through each of the dictionaries in the list.
        foreach (Dictionary<string, object> uhEvent in eventInfo)
        {
            // Retrieve the ID, Name, and Description of each business.
            id = Convert.ToInt32(uhEvent["id"]);
            title = uhEvent["title"] as string;
            desc = uhEvent["desc"] as string;
            who = uhEvent["who"] as string;
            email = uhEvent["email"] as string;
            loc = uhEvent["location"] as string;
            phone = uhEvent["phone"] as string;
            start = DateTime.Parse(uhEvent["start"] as string);

            //Debug.Log(
            //    "Title: " + title +
            //    "Who:" + who +
            //    "Desc: " + desc +
            //    "Email: " + email +
            //    "Loc: " + loc +
            //    "Start: " + start.ToString("MMM dd '-' h':'mm tt", CultureInfo.InvariantCulture));

            UnionHallEvent ev = new UnionHallEvent(id, title, desc, who, email, phone, loc, start);
            events.Add(ev);

            // All arrays in JSON come in as List<object>.
            foreach (string category in uhEvent["interests"] as List<object>)
            {
                // If the category does not exist, we need to create it...
                if (!eventsByCategory.ContainsKey(category))
                    eventsByCategory.Add(category, new List<UnionHallEvent>());
                // Add the business to the category it belongs to.
                eventsByCategory[category].Add(ev);
            }
        }

        // Debug to make sure the categories were all created properly.
        //foreach (KeyValuePair<string, List<Business>> pair in businesses)
        //{
        //    Debug.Log(pair.Key);
        //}
    }

    public void RepopulateEvents()
    {
        eventsByCategory.Clear();
        events.Clear();
        StartCoroutine(GetEventInformation());
    }
}

//public class Business
//{
//    public int id;
//    public string desc;
//    public string name;
//    public bool hasMegaDeal = false;
//    public bool hasDiscount = false;
//    public string dealName;
//    public string dealDesc;
//    public string discountName;
//    public string discountDesc;
//    public Texture2D logo;

//    public Business(int id, string desc, string name)
//    {
//        this.id = id;
//        this.desc = desc;
//        this.name = name;
//    }

//    public void AddDeal(string name, string desc)
//    {
//        hasMegaDeal = true;
//        dealName = name;
//        dealDesc = desc;
//    }

//    public void AddDiscount(string name, string desc)
//    {
//        hasDiscount = true;
//        discountName = name;
//        discountDesc = desc;
//    }
//}