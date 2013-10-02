using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Slowly rotate the object around its X axis at 10 degree/second.
    transform.Rotate(Vector3.one * 10.0f * Time.deltaTime);

	}
}
