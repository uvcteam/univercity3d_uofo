using UnityEngine;
using System.Collections;

public class SkyboxFollow : MonoBehaviour 
{
	// The target we are following.
	public Transform target;
	
	void LateUpdate()
	{
		// Early out if we don't have a target.
		if (!target)
			return;
		
		transform.position = target.position;
		
		// Always look at the target.
		transform.LookAt(target);
	}
}