using UnityEngine;
using System.Collections;

public class CameraSpin : MonoBehaviour {

    public float rotationSpeed = .001f;
	
	// Update is called once per frame
	void Update () 
    {
        transform.RotateAround(Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
