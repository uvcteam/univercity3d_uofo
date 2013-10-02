using UnityEngine;
using System.Collections;

public class DestroyTexture : MonoBehaviour 
{
	public bool DestroyOnDisable = false;
	
	void OnDisable()
	{
		if (!DestroyOnDisable) return;
		DestroyImmediate(GetComponent<UITexture>().mainTexture, true);
	}
	
	void OnDestroy()
	{
		DestroyImmediate(GetComponent<UITexture>().mainTexture, true);
	}
}