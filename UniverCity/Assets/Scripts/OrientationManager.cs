using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour
{
    public ScreenOrientation orientation;
	// Use this for initialization

    public void ChangeOrientationToLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        //iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeRight;
    }

    public void ChangeOrientationToAuto()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
    }
}