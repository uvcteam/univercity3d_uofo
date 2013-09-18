using UnityEngine;
using System.Collections;

public class BusinessAd : MonoBehaviour
{
    public int businessID = 0;

    void Awake()
    {
        transform.parent = GameObject.Find("Anchor").transform;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = new Vector3(0, 0, -500);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
    }

	void OnExitClicked()
	{
	    Destroy(gameObject);
	}
}