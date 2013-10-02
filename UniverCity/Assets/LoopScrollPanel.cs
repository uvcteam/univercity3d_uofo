using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoopScrollPanel : MonoBehaviour 
{

    public GameObject grid;
    public Vector3 rePos = new Vector3(10000.0f, 0.0f, 0.0f);
  
    // Update is called once per frame
    void Update() 
    {
        if (grid.transform.parent.GetComponent<UIPanel>().IsVisible(transform.position) == true)
        {
            Debug.Log(transform.parent.name + "," + name);
            grid.transform.localPosition = transform.parent.transform.localPosition + rePos;
        }
       
	}
}
