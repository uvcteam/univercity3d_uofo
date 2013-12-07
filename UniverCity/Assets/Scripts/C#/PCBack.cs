using UnityEngine;
using System.Collections;

public class PCBack : MonoBehaviour 
{
#if USE_STAGING_SERVER
    private static string serverURL = "http://app2.univercity3d.com/univercity/";
#else
    private static string serverURL = "http://www.univercity3d.com/univercity/";
#endif

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTouched()
    {
         MallAd mallad = transform.parent.GetComponent<MallAd>();
         Application.OpenURL(serverURL + "playad?b=" + mallad.AdOwner.id.ToString());
    }
}