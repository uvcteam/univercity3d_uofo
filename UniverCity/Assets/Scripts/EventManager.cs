using System.Globalization;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.IO;

public class EventManager : MonoBehaviour 
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif
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
        if (GameObject.FindGameObjectWithTag("EventManager") == null)
        {
            gameObject.tag = "EventManager";
            StartCoroutine(GetEventInformation());    
        }
        else
            Destroy(gameObject);
        
	}

    public UnionHallEvent GetEventForId(int id)
    {
        UnionHallEvent evnt = events.FirstOrDefault(ev => ev.Id == id);
        return evnt;
    }

    // ***************************************************
    // Initial pull from the UniverCity server for the 
    // event information.
    // ---------------------------------------------------
    public IEnumerator GetEventInformation()
    {
        string eURL = serverURL + "ListEvents";
        int id = 0;
        string phone = "";
        string title = "";
        string desc = "";
        DateTime start;
        string who = "";
        string loc = "";
        string email = "";
        bool goodDownload = false;
        WWW page = null;

        eURL += "?token=" + GameObject.FindGameObjectWithTag("UserManager").GetComponent<UserManager>().CurrentUser.Token;

        Debug.Log(eURL);
        while (!goodDownload)
        {
            page = new WWW(eURL);
            yield return page;

            if (page.error == null && page.text != null)
                goodDownload = true;
        }

        //StreamWriter sw = File.CreateText("businesses.dat");
        //sw.Write(page.text);
        //sw.Close();

        // Create an IList of all of the businesses returned to me.
        Dictionary <string, object> eventInfo = Json.Deserialize(page.text) as Dictionary<string, object>;
        if (Convert.ToBoolean(eventInfo["s"]))
        {
            List<object> eventIt = eventInfo["events"] as List<object>;
            // Iterate through each of the dictionaries in the list.

            //foreach (Dictionary<string, object> uhEvent in eventIt)
			for(int i = 0; i < eventIt.Count; ++i)
            {
				Dictionary<string, object> uhEvent = eventIt[i] as Dictionary<string, object>;
                // Retrieve the ID, Name, and Description of each business.
                id = Convert.ToInt32(uhEvent["id"]);
                title = uhEvent["title"] as string;
                desc = uhEvent["desc"] as string;
                who = uhEvent["who"] as string;
                email = uhEvent["email"] as string;
                loc = uhEvent["location"] as string;
                phone = uhEvent["phone"] as string;
                start = DateTime.Parse(uhEvent["start"] as string);

                Debug.Log(
                    "ID: " + id +
                    "Title: " + title +
                    "Who:" + who +
                    "Desc: " + desc +
                    "Email: " + email +
                    "Loc: " + loc +
                    "Start: " + start.ToString("MMM dd '-' h':'mm tt", CultureInfo.InvariantCulture));

				events.Add(new UnionHallEvent(id, title, desc, who, email, phone, loc, start));
				UnionHallEvent ev = events.Last();
                // All arrays in JSON come in as List<object>.
                //foreach (string category in uhEvent["interests"] as List<object>)
				for(int j = 0; j < (uhEvent["interests"] as List<object>).Count; ++j)
                {
					string category = (uhEvent["interests"] as List<object>)[j] as string;
                    Debug.Log("Event " + title + " in " + category);
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
    }

    public void RepopulateEvents()
    {
        eventsByCategory.Clear();
        events.Clear();
        StartCoroutine(GetEventInformation());
    }
}