using UnityEngine;
using System.Collections;

public class VirtualMallManager : MonoBehaviour 
{
    public GameObject topAnchor;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void ActivateTopAchor()
    {
        topAnchor.gameObject.SetActiveRecursively(true);
        topAnchor.GetComponent<TopBarManager>().prevPanel.SetActiveRecursively(false);
        topAnchor.GetComponent<TopBarManager>().currentPanel.SetActiveRecursively(true);


    }
}
