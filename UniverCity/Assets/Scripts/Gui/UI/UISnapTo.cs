//#define USE_UPDATE

using UnityEngine;
using System.Collections;

public enum eAnchor { None, Left, Right};

// NOTE: Currently this is calculated to work with an Orthographic camera only.
// Since this is meant for GUI items, this will work fine.
// Changed Snap AGAIN to Start() from OnEnable()/Awake() because Screen.width on Android isn't correct until Start!!! -VM

public class UISnapTo : MonoBehaviour
{
    public Camera GuiCamera = null;
    public eAnchor firstOffsetDirection;
    public float firstOffsetDistance = 0.0f;

    private bool mFirstRun;
	private Vector2 previousObjectDimensions;
	private Vector2 previousScreenDimensions = new Vector2();

	void Start()
    {
        if (!mFirstRun)
        {
            mFirstRun = true;

            previousScreenDimensions.x = Screen.width;
            previousScreenDimensions.y = Screen.height;

            if (GuiCamera == null)
                GuiCamera = Camera.mainCamera;

            DoSnap();
        }
	}
	
#if USE_UPDATE
	void Update()
    {
		// If screen dimensions change or object has moved, we want to adjust to match.
		if (previousScreenDimensions.x != Screen.width || previousScreenDimensions.y != Screen.height)
        {
			previousScreenDimensions.x = Screen.width;
			previousScreenDimensions.y = Screen.height;
            DoSnap();
		}
	}
#endif //USE_UPDATE

    public void DoSnap()
    {
		Vector2 offset = new Vector2(transform.position.x, transform.position.y);
		
		switch (firstOffsetDirection)
        {
		case eAnchor.Left:
            offset.x = GuiCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f)).x + firstOffsetDistance;
		    break;
		case eAnchor.Right:
			offset.x = GuiCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f)).x + firstOffsetDistance;
		    break;
		}
		
		// Add the Z component to the calculated offset, otherwise the Z component is destroyed and becomes 0.0f
		transform.position = new Vector3(offset.x, offset.y, transform.position.z);
	}
}
