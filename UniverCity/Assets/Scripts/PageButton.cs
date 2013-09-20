using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour {

    public GameObject Page;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToPage()
    {
        foreach(GameObject page in GameObject.FindGameObjectsWithTag("Page"))
        {
            page.SetActive(false);
        }

        Page.SetActive(true);

    }
}
