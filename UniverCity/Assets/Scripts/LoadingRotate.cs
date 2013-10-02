using UnityEngine;
using System.Collections;

public class LoadingRotate : MonoBehaviour 
{
    public float rotateSpeed = 5.0f;
	
	// Update is called once per frame
	void Update () 
    {
        transform.RotateAround(-Vector3.forward, rotateSpeed * Time.deltaTime);
	}
}