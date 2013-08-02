using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gui : MonoBehaviour
{
	//A singleton reference to the Gui class (even though this entire class is a bunch of static function *angry face*) -DAS
	static public Gui Instance;
	
    // publics
    static public bool bInputFrozen;
    static public Transform FocusControl;
		
	public Camera GuiCamera;
	
    // privates
    private GameObject[] mTouchGOs = { null, null, null, null, null, null, null, null, null, null, null, null };
	static public ArrayList theGOCheckArray = new ArrayList();
    private bool[] bTouchOverGOs = { false, false, false, false, false, false, false, false, false, false, false, false };

    // delegates
    public delegate void EmptyEvent();
    public delegate void StringEvent(string inString);
    public delegate void IntBoolEvent(int nNumber, bool bBool);
	
    // constants
    public const float TRANSITION_TIME = 0.5f;
    public const float TRANSITION_OFFSET = 6.0f;

    // ***************************************************
    // Start [Monobehavior]
    // ---------------------------------------------------
    void Start()
    {		
		Instance = this;
		
        // freeze keyboard frame
		#if UNITY_ANDROID || UNITY_IOS
//        TouchScreenKeyboard.autorotateToLandscapeLeft = false;
//        TouchScreenKeyboard.autorotateToLandscapeRight = false;
//        TouchScreenKeyboard.autorotateToPortrait = false;
//        TouchScreenKeyboard.autorotateToPortraitUpsideDown = false;
		
		// disable screen dimming
		Screen.sleepTimeout = 120;
		#endif
    }
	
	public void DoGUITouchUpdate() {}
    // ***************************************************
    // Update [Monobehavior]
    // ---------------------------------------------------
    public bool Update()
    {
        // Touch events to mimick monobehavior events:
        //  OnMouseDown()
        //  OnMouseExit()
        //  OnMouseUp()
		bool theReturn = false;
        //foreach (UnityEngine.Touch touch in Input.touches)
		for(int i = 0; i < Input.touchCount; i++)
        {
			UnityEngine.Touch touch = Input.GetTouch(i);			
            if (touch.phase == TouchPhase.Began)
            {
				//Debug.Log("Begin" + i);
                // do raycast for mousedown
                Ray theRay = GuiCamera.ScreenPointToRay((Vector3)touch.position);
                RaycastHit theRayHit;
                if (Physics.Raycast(theRay, out theRayHit, 100))
                {
                    if (theRayHit.collider.gameObject.GetComponent<GuiButton>())
                    {						
                        // send message and store GO in array bounding to fingerID
                        theRayHit.collider.gameObject.SendMessage("OnMouseDown");
                        mTouchGOs[i] = theRayHit.collider.gameObject;
						theGOCheckArray.Add(mTouchGOs[i]);
                        bTouchOverGOs[i] = true;
						// no hud yet... so comment out VKC
//						if (mTouchGOs[i].transform != FormHud.mWorldCollider)
							theReturn = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
				//Debug.Log("Ended" + i);
                // if fingerID bound to GO then send OnMouseUp message1
                if (mTouchGOs[i])
                {
                    // send message and unbind
                    mTouchGOs[i].SendMessage("OnMouseUp");
					theGOCheckArray.Remove(mTouchGOs[i]);
                    mTouchGOs[i] = null;
                    bTouchOverGOs[i] = false;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {				
                // if fingerID bound to GO then check for when ray NOT hitting GO to send OnMouseExit message
                if (mTouchGOs[i] && bTouchOverGOs[i])
                {
					//Debug.Log("Moved + null pass" + i);
					Ray theRay = GuiCamera.ScreenPointToRay((Vector3)touch.position);
                    RaycastHit theRayHit;
                    if (!Physics.Raycast(theRay, out theRayHit, 100) || theRayHit.collider.gameObject != mTouchGOs[i])
                    {
                        // send message and unbind
                        mTouchGOs[i].SendMessage("OnMouseExit");
                        bTouchOverGOs[i] = false;
                    }
                }
            }
			else if (touch.phase == TouchPhase.Canceled)
			{
				//Debug.Log("Cancel" + i);
				if (mTouchGOs[i])
				{
					// send message and unbind
					mTouchGOs[i].SendMessage("OnMouseUp");
					theGOCheckArray.Remove(mTouchGOs[i]);
					mTouchGOs[i] = null;
					bTouchOverGOs[i] = false;
				}
			}
        }		
		return theReturn;
    }

    static public void Transition(GameObject inGO, float inOffsetX)
    {
        Vector3 thePos = inGO.transform.position;
        thePos.x = inOffsetX;
        iTween.MoveFrom(inGO, iTween.Hash("position", thePos, "time", Gui.TRANSITION_TIME, "delay", 0.01f));
    }
}
