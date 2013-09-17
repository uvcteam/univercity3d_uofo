using UnityEngine;
using System.Collections;

public class BusinessAd : MonoBehaviour
{
    public int businessID = 0;

	void OnExitClicked()
	{
	    Destroy(gameObject);
	}
}