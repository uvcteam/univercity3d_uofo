using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour
{
    public GameObject [] Panels = new GameObject[3];
    public GameObject MemoryBank = null;

	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (MemoryBank.active == true)
	            Application.LoadLevel(0);
	        else
	        {
	            foreach (GameObject go in Panels)
	                go.SetActiveRecursively(false);
	            MemoryBank.SetActiveRecursively(true);
	        }
	    }
	}
}