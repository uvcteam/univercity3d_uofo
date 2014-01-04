using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour
{
    public ScreenOrientation orientation;
    public EasyJoystick left;
    public EasyJoystick right;
	// Use this for initialization
	void Start()
	{
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeRight;
	}

}