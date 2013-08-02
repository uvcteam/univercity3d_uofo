using UnityEngine;
using System.Collections;

public class AdUpdater : MonoBehaviour {

    public string[] ads;
    private int index = 0;
    public UILabel ad;
    public float transitionTime = .5f;
	// Use this for initialization
	void Start () 
    {
        ad.text = ads[index];
        StartCoroutine("ChangeAd");
	}

    IEnumerator ChangeAd()
    {
        while (true)
        {
            index %= ads.Length;
            ad.text = ads[index++];
            yield return new WaitForSeconds(transitionTime);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
