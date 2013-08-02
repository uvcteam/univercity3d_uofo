using UnityEngine;
using System.Collections;

public class PCBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTouched()
    {
         MallAd mallad = transform.parent.GetComponent<MallAd>();
         Application.OpenURL("http://www.univercity3d.com/univercity/playad?b=" + mallad.AdOwner.id.ToString());
    }
}
