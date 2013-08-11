using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour
{
    public ScreenOrientation orientation;
	// Use this for initialization
	void Start()
	{
	    Screen.orientation = orientation;
	}
}