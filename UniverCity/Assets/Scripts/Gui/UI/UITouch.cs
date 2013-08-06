//#define DEBUG_MULTITOUCH
//#define DEBUG_DRAW

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 2012.09.05 - improvements for bounds and bound snapping back with momentum, added ContainsButtons for UIButton interaction -VM
 * 2012.08.27 - renamed UITouch to fix collision with UnityEngine.Touch when targeting for Flash -VM
 * 2012.08.23 - added Debug visuals, editor multitouch support, and improved bounding/clamping -VM
 * 
 * Touch.cs - this script is a multipurpose class for touch/gestures and includes the following
 *
 *   1) callbacks for common touch events
 *   2) ability to translate, rotate, and scale objects based on drag/drop, pinch, and rotate gestures
 *   3) momentum and bounding support
 *  
 * Transformation based gestures manipulate objects in the following process:
 
 *   1) Perform raycasting to collide with objects and construct Plane to base transformations on
 *   2) Create a dummy transform and attach GameObject to (optionally) for transformation purposes
 *   3) Translate/Rotate/Scale based on touch gestures storing momentum
 *   4) Undergo and decay momentum and/or bound transformation based on specified parmeters
 *   5) Destroy transform and restore gameobject to original hierarchy when done
 * 
 * Transformation Options available including:
 * 
 *   TransformCreate - callback sent to MsgReceiver when touch transform is created
 *   TransformDestroy - callback sent to MsgReceiver when touch transform is destroyed
 *   TransformDrag - callback sent to MsgReceiver when touch transform is dragged in either translate, rotate, scale
 * 
 *   Translate {None, XY, X, Y}
 *   Scale {None, Uniform, XY, X, Y}
 *   Rotate {None, Z}
 *   
 *   Origin {GameObject, Touch, Screen}
 *    - this defines where the transform to manipulte the gameobject is created
 *    - GameObject creates at center of GameObject
 *    - Touch centers the transform at where the touch meets the transformation plane
 *    - Screen doesn't utilize rays and is used for CAMERA manipulation
 *    
 *   ScreenSpaceScalar - the multiplier to scale transfomations by when Origin is set to Screen to improve pixel based camera manipulation
 * 
 *   AttachToTransform - this option attaches gameObject to touch transform. Particles and trailrendering might be examples where attach is NOT wanted.
 * 
 *   Momentum - if momentum is desired for Translate, Rotate and Scale
 *   MomentumAmount - the value multiplied thru each frame to decay momentum | 1.0f doesn't decay and = 0.0f decays in one frame
 * 
 *   Bound - boolean to bound
 *   BoundsFirmness - scalar to snap object back into bounds - 1.0f is fully "rigid" where 0.3f is more "elastic"
 *   BoundPosMin, BoundPosMax - 2D min/max bounds for translation - NOTE that must be NONEZERO to affect bounds!
 *   BoundScaleMin, BoundScaleMax - 2D min/max bounds for scale - NOTE that must be NONEZERO to affect bounds!
 * 
*/
public class UITouch : MonoBehaviour
{
    // publics
    public Camera RayCamera;
    public Transform MsgReceiver;
    public int MaxTouchCount = 2;
    public string TouchDown;
    public string TouchUp;
    public string TouchExit;
    public string TouchDrag;
    public string TouchDoubleClick;
    public string TransformCreate;
    public string TransformDestroy;
    public string TransformDrag;
    public eTranslate Translate;
    public eScale Scale;
    public eRotate Rotate;
    public eOrigin Origin = eOrigin.Touch;
    public float ScreenSpaceScalar = 0.02f;
    public bool AttachToTransform = true;
    public bool Momentum;
    public float MomentumAmount = 0.95f;
    public bool Bound;
    public float BoundsFirmness = 0.3f;
    public Vector2 BoundPosMin;
    public Vector2 BoundPosMax;
    public Vector2 BoundPosEpsilon;
    public Vector2 BoundScaleMin;
    public Vector2 BoundScaleMax;
    public Vector2 BoundScaleEpsilon;
    public float DeadZone = 0.0f;

    // privates
    Transform mTransform;
    Collider mCollider;
    List<int> mFingerOverMap = new List<int>();
    int mFingerOverMapCountLast;
    float mDoubleClickTimer;
    Vector3[] mTouchCurPts = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    Vector3[] mTouchLastPts = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    bool[] mTouchCurFlags = { false, false, false, false, false, false, false, false, false, false };
    bool[] mTouchLastFlags = { false, false, false, false, false, false, false, false, false, false };
    bool mTransFlag;
    bool mTransFlagLast;
    Plane mTransPlane;
    Transform mTransTransform;
    Vector3 mTransInitPos;
    Vector3 mTransInitScale;
    Quaternion mTransInitRot;
    Vector3 mTransInitVector;
    Vector3 mTransInitCenter;
    Vector3 mTransLastPos;
    Vector3 mTransLastScale;
    float mTransLastRot;
    Vector3 mTransDeltaPos;
    Vector3 mTransDeltaScale;
    float mTransDeltaRot;
    Vector3 mTransOffset;
    float mMomentumScalar;
    Vector3 mMomentumPos;
    Vector3 mMomentumScale;
    float mMomentumRot;
    float[] mMomentumClampValues = { 0f, 0f, 0f };
    ArrayList mMemoryPos = new ArrayList();
    ArrayList mMemoryScale = new ArrayList();
    ArrayList mMemoryRot = new ArrayList();
    Transform mSavedParent;
    GuiButton mTouchedButton;         // touched button when touch "contains" UIButtons
    Vector3 mTouchButtonViewportPt;  // viewport point button is touched for OnMouseExit purposes
    bool mDeadZoneSnap = true;
    Vector3 mDeadZoneOffset;

#if DEBUG_MULTITOUCH
    int mStickyFingerID = -1;
#endif //DEBUG_MULTITOUCH
#if DEBUG_DRAW
    GameObject[] mDebugMarkers = { null, null, null};
    GameObject mDebugTransform = null;
#endif //DEBUG_DRAW

    // constants
    const float DOUBLECLICK_WINDOW = 0.25f;
    const int MEMORY_SIZE = 5;
    const float EPSILON = 0.01f;
    const string DEBUG_RESOURCE_MARKER = "Prefabs/TouchMarker";
    const string DEBUG_RESOURCE_TRANSFORM = "Prefabs/TouchTransform";
    const float UIBUTTON_VIEWPORT_DIST = 0.01f;

    public enum eTranslate
    {
        None,
        XY,
        X,
        Y,
    };

    public enum eScale
    {
        None,
        Uniform,
        XY,
        X,
        Y,
    };

    public enum eRotate
    {
        None,
        Z,
    };

    public enum eOrigin
    {
        GameObject,
        Touch,
        Screen,
    };

    public enum eMomentum
    {
        No,
        Yes,
    };

    // *****************************************************
    // Start [Monobehavior]
    // -----------------------------------------------------
    void Start()
    {
        mTransform = this.transform;
        mCollider = this.collider;

        // if camera not set then assign it on start
        if (RayCamera == null)
            RayCamera = Camera.main;
    }

    // *****************************************************
    // OnDisable [Monobehavior]
    // -----------------------------------------------------
    void OnDisable()
    {
        Reset();
    }

    // ***************************************************
    // OnApplicationPause [Monobehavior]
    // ---------------------------------------------------
    void OnApplicationPause(bool inPaused)
    {
        if (inPaused)
            Reset();
    }

    // *****************************************************
    // Update [Monobehavior]
    // -----------------------------------------------------
    void Update()
    {
        // early exit if no collider
        if (!mCollider)
            return;

#if (UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE_WIN)

        {
            Vector3 theMousePos = Input.mousePosition;
            TouchPhase theTouchPhase = TouchPhase.Stationary;
            int theFingerID = 0;

            if (Input.GetMouseButtonDown(0))
                theTouchPhase = TouchPhase.Began;
            else if (Input.GetMouseButtonUp(0))
                theTouchPhase = TouchPhase.Ended;
            else if (Input.GetMouseButton(0))
                theTouchPhase = TouchPhase.Moved;

#if DEBUG_MULTITOUCH

            if (mStickyFingerID >= 0)
                theFingerID = 1;
            if (theTouchPhase == TouchPhase.Ended)
            {
                if (mStickyFingerID < 0 && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    mStickyFingerID = theFingerID;
                    return;
                }
                else if (mStickyFingerID >= 0)
                {
                    if (mFingerOverMap.Contains(mStickyFingerID))
                    {
                        mFingerOverMap.Remove(mStickyFingerID);
                        mTouchLastFlags[mStickyFingerID] = mTouchCurFlags[mStickyFingerID];
                        mTouchCurFlags[mStickyFingerID] = false;
                        mStickyFingerID = -1;
                    }
                }
            }

#endif //DEBUG_MULTITOUCH

#elif (UNITY_IOS || UNITY_ANDROID || UNITY_FLASH)

            
        foreach (UnityEngine.Touch touch in Input.touches)
        {
            Vector3 theMousePos = (Vector3)touch.position;
            TouchPhase theTouchPhase = touch.phase;
            int theFingerID = touch.fingerId;

#endif //UNITY_EDITOR

            if (theTouchPhase == TouchPhase.Began)
            {
                Ray theRay = RayCamera.ScreenPointToRay(theMousePos);
                RaycastHit theRayHit;
                if (Physics.Raycast(theRay, out theRayHit, 100) && theRayHit.collider == mCollider && mFingerOverMap.Count < MaxTouchCount)
                {
                    // add fingerID to map
                    mFingerOverMap.Add(theFingerID);

                    // send TouchDown message
                    if (MsgReceiver && TouchDown.Length > 0)
                        MsgReceiver.SendMessage(TouchDown);

                    // setup transform items
                    mTransFlag = true;
                }

                mTouchCurPts[theFingerID] = theMousePos;
                mTouchLastPts[theFingerID] = theMousePos;
                mTouchCurFlags[theFingerID] = true;
                mTouchLastFlags[theFingerID] = false;
            }
            else if (theTouchPhase == TouchPhase.Ended)
            {
                if (mFingerOverMap.Contains(theFingerID))
                {
                    // remove fingerID from map
                    mFingerOverMap.Remove(theFingerID);

                    // send TouchUp message
                    if (MsgReceiver && TouchUp.Length > 0)
                        MsgReceiver.SendMessage(TouchUp);

                    // send TouchDoubleClick message
                    if (MsgReceiver && TouchDoubleClick.Length > 0)
                    {
                        if (mDoubleClickTimer > 0.0f)
                            MsgReceiver.SendMessage(TouchDoubleClick);
                        else
                            mDoubleClickTimer = DOUBLECLICK_WINDOW;
                    }

                    // reset transflag
                    mTransFlag = false;
                }

                mTouchLastPts[theFingerID] = mTouchCurPts[theFingerID];
                mTouchCurPts[theFingerID] = theMousePos;
                mTouchLastFlags[theFingerID] = mTouchCurFlags[theFingerID];
                mTouchCurFlags[theFingerID] = false;
            }
            else if (theTouchPhase == TouchPhase.Moved)
            {
                Ray theRay = RayCamera.ScreenPointToRay(theMousePos);
                RaycastHit theRayHit;
                if (!mCollider.Raycast(theRay, out theRayHit, 100) || theRayHit.collider != mCollider)
                {
                    // remove fingerID from map
                    if (mFingerOverMap.Contains(theFingerID))
                    {
                        //Debug.Log("Touch.removed from move");
                        // send TouchExit message
                        mFingerOverMap.Remove(theFingerID);

                        if (MsgReceiver && TouchExit.Length > 0)
                            MsgReceiver.SendMessage(TouchExit);

                        mTransFlag = false;
                    }
                }
                else
                {
                    if (mFingerOverMap.Contains(theFingerID))
                    {
                        // send TouchDrag message
                        if (MsgReceiver && TouchDrag.Length > 0)
                            MsgReceiver.SendMessage(TouchDrag);
                    }
                }

                mTouchLastPts[theFingerID] = mTouchCurPts[theFingerID];
                mTouchCurPts[theFingerID] = theMousePos;
                mTouchLastFlags[theFingerID] = mTouchCurFlags[theFingerID];
                mTouchCurFlags[theFingerID] = true;
            }
        }

        // update transformations
        UpdateTouch();

        // decrement/clamp mDoubleClickTimer
        if (mDoubleClickTimer > 0.0f)
            mDoubleClickTimer = Mathf.Max(mDoubleClickTimer - Time.deltaTime, 0.0f);

        mFingerOverMapCountLast = mFingerOverMap.Count;

#if DEBUG_DRAW

        for (int i = 0; i < 3; i++)
        {
            if (mTouchCurFlags[i])
            {
                if (mDebugMarkers[i] == null)
                {
                    mDebugMarkers[i] = (GameObject)GameObject.Instantiate(Resources.Load(DEBUG_RESOURCE_MARKER));
                    switch (i)
                    {
                        case 0:
                            mDebugMarkers[i].renderer.material.color = Color.red;
                            break;
                        case 1:
                            mDebugMarkers[i].renderer.material.color = Color.green;
                            break;
                        default:
                            mDebugMarkers[i].renderer.material.color = Color.blue;
                            break;
                    }
                }

                Ray theRay = RayCamera.ScreenPointToRay(mTouchCurPts[i]);
                RaycastHit theRayHit;
                if (Physics.Raycast(theRay, out theRayHit, 100))
                    mDebugMarkers[i].transform.position = theRayHit.point;
            }
            else if (mDebugMarkers[i])
            {
                Destroy(mDebugMarkers[i]);
                mDebugMarkers[i] = null;
            }
        }

        if (mTransTransform)
        {
            if (mDebugTransform == null)
                mDebugTransform = (GameObject)GameObject.Instantiate(Resources.Load(DEBUG_RESOURCE_TRANSFORM));
            mDebugTransform.transform.position = mTransTransform.position;
        }
        else if (mDebugTransform)
        {
            Destroy(mDebugTransform);
            mDebugTransform = null;
        }

#endif //DEBUG_DRAW
    }

    // *****************************************************
    // Reset
    // -----------------------------------------------------
    public void Reset()
    {
        // cleanup
        for (int i = 0; i < mTouchCurFlags.Length; i++)
        {
            mTouchCurFlags[i] = false;
            mTouchLastFlags[i] = false;
        }
        mFingerOverMap.Clear();
        mFingerOverMapCountLast = 0;
        mTransFlag = false;
        mTransFlagLast = false;
        ResetMomentum();

        // reset touched button
        if (mTouchedButton != null)
        {
            mTouchedButton.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
            mTouchedButton = null;
        }
    }

    // *****************************************************
    // TouchTransformUpdate
    // -----------------------------------------------------
    void UpdateTouch()
    {
        if (mTransFlag)
        {
            // destroy existing transform if it 1) is not under touch transform OR 2) going from single to multitouch
            if (mTransTransform && (!mTransFlagLast || (MaxTouchCount > 1 && mFingerOverMap.Count >= 2 && mFingerOverMap.Count != mFingerOverMapCountLast)))
                TouchTransformDestroy();

            if (!mTransTransform)
                TouchTransformCreate();
            else
                TouchTransformUpdate();
        }

        // bound influenced by BoundsFirmness
        if (mTransTransform)
        {
            bool theMoveFlag = false;

            if (!mTransFlag)
            {
                // update momentum
                if (Momentum && TouchTransformMomentum())
                    theMoveFlag = true;

                // update bounds
                if (Bound && TouchTransformBound())
                {
                    theMoveFlag = true;
                    
                    // when bounding (and momentum currently carrying the transform) "snap" momemtum based on bounds firmness -VM
                    if (Momentum)
                        mMomentumScalar *= (1.0f - this.BoundsFirmness);
                }
            }
            else
            {
                if (Bound)
                    TouchTransformBoundClamp();
            }

            if (Translate != eTranslate.None)
            {
                mTransDeltaPos = mTransTransform.position - mTransLastPos;
                mTransLastPos = mTransTransform.position;
            }
            if (Scale != eScale.None && mFingerOverMap.Count >= 2)
            {
                mTransDeltaScale = mTransTransform.localScale - mTransLastScale;
                mTransLastScale = mTransTransform.localScale;
            }
            if (Rotate != eRotate.None && mFingerOverMap.Count >= 2)
            {
                //mTransDeltaRot = theDeltaDegrees;
                //mTransLastRot = theDegrees;
            }

            // when touch transform not moving then destroy it
            if (!mTransFlag && !theMoveFlag)
                TouchTransformDestroy();
        }

        // get array of button components in children -VM
        UpdateTouchedButtons();

        // assign mTransFlagLast
        mTransFlagLast = mTransFlag;
    }

    // *****************************************************
    // UpdateTouchedButtons - update UI buttons behind touch collider
    // -----------------------------------------------------
    private void UpdateTouchedButtons()
    {
        if (mTransFlag && !mTransFlagLast && mTransTransform)
        {
            mTouchedButton = null;

            Vector3 theViewportPt = RayCamera.WorldToViewportPoint(mTransTransform.position);
            Ray theRay = RayCamera.ViewportPointToRay(theViewportPt);
            RaycastHit theRayCastHit;
            mCollider.enabled = false;
            if (Physics.Raycast(theRay, out theRayCastHit, 100))
            {
                GuiButton theButton = theRayCastHit.collider.GetComponent<GuiButton>();
                if (theButton != null)
                    mTouchedButton = theButton;
            }
            mCollider.enabled = true;

            // send OnMouseDown message
            if (mTouchedButton != null)
            {
                mTouchButtonViewportPt = theViewportPt;
                mTouchedButton.SendMessage("OnMouseDown");
            }
        }

        if (mTouchedButton != null)
        {
            if (mTransFlag && mTransTransform)
            {
                Vector3 theDiff = mTouchButtonViewportPt - RayCamera.WorldToViewportPoint(mTransTransform.position);
                if (mFingerOverMap.Count != 1 || theDiff.sqrMagnitude >= (UIBUTTON_VIEWPORT_DIST * UIBUTTON_VIEWPORT_DIST))
                {
                    // send OnMouseExit message -VM
                    mTouchedButton.SendMessage("OnMouseExit");
                    mTouchedButton = null;
                }
            }
            else
            {
                // send OnMouseUp message
                mTouchedButton.SendMessage("OnMouseUp");
                mTouchedButton = null;
            }
        }
    }

    // *****************************************************
    // TouchTransformDestroy - destroy touch transform
    // -----------------------------------------------------
    private void TouchTransformDestroy()
    {
        if (mTransTransform)
        {
            // restore object in hierarchy
            mTransform.parent = mSavedParent;

            // send TransformDestroy message
            if (MsgReceiver && TransformDestroy.Length > 0)
                MsgReceiver.SendMessage(TransformDestroy, this);

            // destroy transform GO
            Destroy(mTransTransform.gameObject);
            mTransTransform = null;
        }
    }

    // *****************************************************
    // TouchTransformCreate - create touch transform
    // -----------------------------------------------------
    private void TouchTransformCreate()
    {
        // create transform plane
        mTransPlane = new Plane(-RayCamera.transform.forward, mTransform.position);

        // save transform parent
        mSavedParent = mTransform.parent;

        // get transform origin
        Vector3 theOriginPt = mTransform.position;
        if (Origin != eOrigin.GameObject)
        {
            Vector3 theTouchCenter = GetTouchCenter();
            theOriginPt = GetTouchPos(theTouchCenter);
        }

        // set init items
        mTransInitPos = theOriginPt;
        mTransInitScale = mTransform.localScale;
        mTransInitRot = mTransform.localRotation;

        // reset momentum items
        ResetMomentum();

        // setup transform GO
        mTransTransform = new GameObject("TouchGO").transform;
        mTransTransform.position = mTransInitPos;
        mTransTransform.localScale = mTransInitScale;
        mTransTransform.localRotation = mTransInitRot;
        mTransTransform.parent = mSavedParent;

        // set offset
        mTransOffset = mTransform.position - mTransTransform.position;

        // parent object to transform GO
        if (AttachToTransform)
            mTransform.parent = mTransTransform;

        // setup init/last items
        mTransInitVector = GetTouchVector();
        mTransInitScale = mTransTransform.localScale;
        mTransInitRot = mTransTransform.localRotation;
        mTransLastPos = mTransInitPos;
        mTransLastScale = mTransInitScale;
        mTransLastRot = 0.0f;
        mTransDeltaPos = Vector3.zero;
        mTransDeltaScale = Vector3.zero;
        mTransDeltaRot = 0.0f;
        mDeadZoneSnap = true;

        // send TransformDestroy message
        if (MsgReceiver && TransformCreate.Length > 0)
            MsgReceiver.SendMessage(TransformCreate, this);
    }

    // *****************************************************
    // TouchTransformUpdate - update touch transform thru touch behaviors
    // -----------------------------------------------------
    private void TouchTransformUpdate()
    {
        bool theMoveFlag = false;

        // translate to touch
        if (Translate != eTranslate.None)
        {
            Vector3 theTouchCenter = GetTouchCenter();
            Vector3 theTouchPos = GetTouchPos(theTouchCenter);
            
            // deadzone snapping
            if (DeadZone > 0.0f)
            {
                if (mDeadZoneSnap && (mTransInitPos - theTouchPos).sqrMagnitude < (DeadZone * DeadZone))
                    theTouchPos = mTransInitPos;
                else
                {
                    if (mDeadZoneSnap)
                        mDeadZoneOffset = (mTransInitPos - theTouchPos).normalized * DeadZone;
                    theTouchPos += mDeadZoneOffset;
                    mDeadZoneSnap = false;
                }
            }

            if (Translate == eTranslate.XY)
            {
                mTransTransform.position = theTouchPos;
            }
            else if (Translate == eTranslate.X)
            {
                Vector3 theTouchPt = theTouchPos;
                Vector3 theTransDir = RayCamera.transform.right;
                float theTransDot = Vector3.Dot(theTouchPt - mTransInitPos, theTransDir);
                mTransTransform.position = mTransInitPos + theTransDir * theTransDot;
            }
            else if (Translate == eTranslate.Y)
            {
                Vector3 theTouchPt = theTouchPos;
                Vector3 theTransDir = RayCamera.transform.up;
                float theTransDot = Vector3.Dot(theTouchPt - mTransInitPos, theTransDir);
                mTransTransform.position = mTransInitPos + theTransDir * theTransDot;
            }

            // mark move
            if (mTransTransform.position != mTransLastPos)
                theMoveFlag = true;

            // momentum bookwork
            mMomentumScalar = 1.0f;
            mMemoryPos.Add(mTransTransform.position - mTransLastPos);
            if (mMemoryPos.Count > MEMORY_SIZE)
                mMemoryPos.RemoveAt(0);

            //mTransDeltaPos = mTransTransform.position - mTransLastPos;
            //mTransLastPos = mTransTransform.position;
        }

        // scale to touch
        if (Scale != eScale.None && mFingerOverMap.Count >= 2)
        {
            if (Scale == eScale.Uniform)
            {
                Vector3 theCurVector = GetTouchVector();
                float theFraction = theCurVector.magnitude / mTransInitVector.magnitude;
                mTransTransform.localScale = mTransInitScale * theFraction;
            }
            else if (Scale == eScale.XY)
            {
                Vector3 theCurVector = GetTouchVector();
                float theInfluenceX = Mathf.Clamp(2.0f * mTransInitVector.x / mTransInitVector.magnitude, 0.0f, 1.0f);
                float theInfluenceY = Mathf.Clamp(2.0f * mTransInitVector.y / mTransInitVector.magnitude, 0.0f, 1.0f);
                float theFractionX = theCurVector.x / mTransInitVector.x;
                float theFractionY = theCurVector.y / mTransInitVector.y;
                theFractionX = Mathf.Lerp(1.0f, theFractionX, theInfluenceX);
                theFractionY = Mathf.Lerp(1.0f, theFractionY, theInfluenceY);
                mTransTransform.localScale = new Vector3(mTransInitScale.x * theFractionX, mTransInitScale.y * theFractionY, mTransInitScale.z);
            }
            else if (Scale == eScale.X)
            {
                Vector3 theCurVector = GetTouchVector();
                float theInfluenceX = Mathf.Clamp(2.0f * mTransInitVector.x / mTransInitVector.magnitude, 0.0f, 1.0f);
                float theFractionX = theCurVector.x / mTransInitVector.x;
                theFractionX = Mathf.Lerp(1.0f, theFractionX, theInfluenceX);
                mTransTransform.localScale = new Vector3(mTransInitScale.x * theFractionX, mTransInitScale.y, mTransInitScale.z);
            }
            else if (Scale == eScale.Y)
            {
                Vector3 theCurVector = GetTouchVector();
                float theInfluenceY = Mathf.Clamp(2.0f * mTransInitVector.y / mTransInitVector.magnitude, 0.0f, 1.0f);
                float theFractionY = theCurVector.y / mTransInitVector.y;
                theFractionY = Mathf.Lerp(1.0f, theFractionY, theInfluenceY);
                mTransTransform.localScale = new Vector3(mTransInitScale.x, mTransInitScale.y * theFractionY, mTransInitScale.z);
            }

            // mark move
            if (mTransTransform.localScale != mTransLastScale)
                theMoveFlag = true;

            // momentum bookwork
            mMomentumScalar = 1.0f;
            mMemoryScale.Add(mTransTransform.localScale - mTransLastScale);
            if (mMemoryScale.Count > MEMORY_SIZE)
                mMemoryScale.RemoveAt(0);

            //mTransDeltaScale = mTransTransform.localScale - mTransLastScale;
            //mTransLastScale = mTransTransform.localScale;
        }

        // rotate to touch
        if (Rotate != eRotate.None && mFingerOverMap.Count >= 2)
        {
            if (Rotate == eRotate.Z)
            {
                Vector3 theCurVector = GetTouchVector();
                float theRadians = Mathf.Atan2(theCurVector.y, theCurVector.x) - Mathf.Atan2(mTransInitVector.y, mTransInitVector.x);
                float theDegrees = theRadians * Mathf.Rad2Deg;
                mTransTransform.localRotation = mTransInitRot * Quaternion.Euler(0, 0, theDegrees);

                // mark move
                if (theDegrees != mTransLastRot)
                    theMoveFlag = true;

                // momentum bookwork
                mMomentumScalar = 1.0f;
                float theDeltaDegrees = theDegrees - mTransLastRot;
                if (theDeltaDegrees > 180.0f)
                    theDeltaDegrees -= 360.0f;
                else if (theDeltaDegrees < -180.0f)
                    theDeltaDegrees += 360.0f;

                mMemoryRot.Add(theDeltaDegrees);
                if (mMemoryRot.Count > MEMORY_SIZE)
                    mMemoryRot.RemoveAt(0);

                mTransDeltaRot = theDeltaDegrees;
                mTransLastRot = theDegrees;
            }
        }

        // send TransformDrag message
        if (theMoveFlag && MsgReceiver && TransformDrag.Length > 0)
            MsgReceiver.SendMessage(TransformDrag, this);
    }

    // *****************************************************
    // GetTouchCenter - get center of all touches over collider
    // -----------------------------------------------------
    private Vector3 GetTouchCenter()
    {
        Vector3 theResult = Vector3.zero;
        for (int i = 0; i < mFingerOverMap.Count; i++)
            theResult += mTouchCurPts[mFingerOverMap[i]];
        if (mFingerOverMap.Count > 1)
            theResult /= (float)mFingerOverMap.Count;

        return theResult;
    }

    // *****************************************************
    // GetTouchVector - get vector of first two touches over collider
    // -----------------------------------------------------
    private Vector3 GetTouchVector()
    {
        if (mFingerOverMap.Count >= 2)
            return mTouchCurPts[mFingerOverMap[1]] - mTouchCurPts[mFingerOverMap[0]];
        return -mTouchCurPts[mFingerOverMap[0]];
    }

    // *****************************************************
    // TouchTransformMomentum - optionally update touch transform "momentum"
    // -----------------------------------------------------
    private bool TouchTransformMomentum()
    {
        if (Translate != eTranslate.None)
        {
            if (mMemoryPos.Count > 0)
            {
                mMomentumPos = ComputeLinVelocity();
                mMemoryPos.Clear();
            }

            mTransTransform.position += (mMomentumPos * mMomentumScalar);
        }

        if (Scale != eScale.None)
        {
            if (mMemoryScale.Count > 0)
            {
                mMomentumScale = ComputeScaleVel();
                mMemoryScale.Clear();
            }

            mTransTransform.localScale += (mMomentumScale * mMomentumScalar);
        }

        if (Rotate != eRotate.None)
        {
            if (mMemoryRot.Count > 0)
            {
                mMomentumRot = ComputeAngVelocity();
                mMemoryRot.Clear();
            }

            mTransTransform.localRotation *= Quaternion.Euler(0, 0, mMomentumRot * mMomentumScalar);
        }

        // decay momentum
        mMomentumScalar *= MomentumAmount;

        return (mMomentumScalar > EPSILON);
    }

    // *****************************************************
    // TouchTransformBound - optionally bound touch transform
    // -----------------------------------------------------
    private bool TouchTransformBound()
    {
        bool theMoveFlag = false;

        // bound pos x
        if (BoundPosMin.x != 0.0f && mTransTransform.localPosition.x + mTransOffset.x < BoundPosMin.x - EPSILON)
        {
            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.x -= (theLocalPos.x + mTransOffset.x - BoundPosMin.x) * BoundsFirmness;
            mTransTransform.localPosition = theLocalPos;
            theMoveFlag = true;
        }
        else if (BoundPosMax.x != 0.0f && mTransTransform.localPosition.x + mTransOffset.x > BoundPosMax.x + EPSILON)
        {
            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.x -= (theLocalPos.x + mTransOffset.x - BoundPosMax.x) * BoundsFirmness;
            mTransTransform.localPosition = theLocalPos;
            theMoveFlag = true;
        }

        // bound pos y
        if (BoundPosMin.y != 0.0f && mTransTransform.localPosition.y + mTransOffset.y < BoundPosMin.y - EPSILON)
        {
            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.y -= (theLocalPos.y + mTransOffset.y - BoundPosMin.y) * BoundsFirmness;
            mTransTransform.localPosition = theLocalPos;
            theMoveFlag = true;
        }
        else if (BoundPosMax.y != 0.0f && mTransTransform.localPosition.y + mTransOffset.y > BoundPosMax.y + EPSILON)
        {
            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.y -= (theLocalPos.y + mTransOffset.y - BoundPosMax.y) * BoundsFirmness;
            mTransTransform.localPosition = theLocalPos;
            theMoveFlag = true;
        }

        // bound scale x
        if (Scale != eScale.None)
        {
            if (Scale == eScale.Uniform)
            {
                if (BoundScaleMin.x != 0.0f && mTransTransform.localScale.x < BoundScaleMin.x - EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMin.x) * BoundsFirmness;
                    theLocalScale.y = theLocalScale.z = theLocalScale.x;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }
                else if (BoundScaleMax.x != 0.0f && mTransTransform.localScale.x > BoundScaleMax.x + EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMax.x) * BoundsFirmness;
                    theLocalScale.y = theLocalScale.z = theLocalScale.x;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }
            }
            else
            {
                if (BoundScaleMin.x != 0.0f && mTransTransform.localScale.x < BoundScaleMin.x - EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMin.x) * BoundsFirmness;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }
                else if (BoundScaleMax.x != 0.0f && mTransTransform.localScale.x > BoundScaleMax.x + EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMax.x) * BoundsFirmness;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }

                // bound scale y
                if (BoundScaleMin.y != 0.0f && mTransTransform.localScale.y < BoundScaleMin.y - EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.y -= (theLocalScale.y - BoundScaleMin.y) * BoundsFirmness;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }
                else if (BoundScaleMax.y != 0.0f && mTransTransform.localScale.y > BoundScaleMax.y + EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.y -= (theLocalScale.y - BoundScaleMax.y) * BoundsFirmness;
                    mTransTransform.localScale = theLocalScale;
                    theMoveFlag = true;
                }
            }
        }

        return theMoveFlag;
    }

    // *****************************************************
    // TouchTransformBoundClamp - optionally bound/clamp touch transform
    // -----------------------------------------------------
    private void TouchTransformBoundClamp()
    {
        // bound pos x
        float theOffsetX = mTransTransform.localPosition.x + mTransOffset.x;
        if (BoundPosMin.x != 0.0f && theOffsetX < BoundPosMin.x)
        {
            theOffsetX -= (theOffsetX - BoundPosMin.x) * 0.5f;
            if (theOffsetX < BoundPosMin.x - BoundPosEpsilon.x)
                theOffsetX = BoundPosMin.x - BoundPosEpsilon.x;
            
            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.x = theOffsetX - mTransOffset.x;
            mTransTransform.localPosition = theLocalPos;
        }
        else if (BoundPosMax.x != 0.0f && theOffsetX > BoundPosMax.x)
        {
            theOffsetX -= (theOffsetX - BoundPosMax.x) * 0.5f;
            if (theOffsetX > BoundPosMax.x + BoundPosEpsilon.x)
                theOffsetX = BoundPosMax.x + BoundPosEpsilon.x;

            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.x = theOffsetX - mTransOffset.x;
            mTransTransform.localPosition = theLocalPos;
        }

        // bound pos y
        float theOffsetY = mTransTransform.localPosition.y + mTransOffset.y;
        if (BoundPosMin.y != 0.0f && theOffsetY < BoundPosMin.y)
        {
            theOffsetY -= (theOffsetY - BoundPosMin.y) * 0.5f;
            if (theOffsetY < BoundPosMin.y - BoundPosEpsilon.y)
                theOffsetY = BoundPosMin.y - BoundPosEpsilon.y;

            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.y = theOffsetY - mTransOffset.y;
            mTransTransform.localPosition = theLocalPos;
        }
        else if (BoundPosMax.y != 0.0f && theOffsetY > BoundPosMax.y)
        {
            theOffsetY -= (theOffsetY - BoundPosMax.y) * 0.5f;
            if (theOffsetY > BoundPosMax.y + BoundPosEpsilon.y)
                theOffsetY = BoundPosMax.y + BoundPosEpsilon.y;

            Vector3 theLocalPos = mTransTransform.localPosition;
            theLocalPos.y = theOffsetY - mTransOffset.y;
            mTransTransform.localPosition = theLocalPos;
        }

        // bound scale x
        if (Scale != eScale.None)
        {
            if (Scale == eScale.Uniform)
            {
                if (BoundScaleMin.x != 0.0f && mTransTransform.localScale.x < BoundScaleMin.x - EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMin.x) * BoundsFirmness;
                    theLocalScale.y = theLocalScale.z = theLocalScale.x;
                    mTransTransform.localScale = theLocalScale;
                }
                else if (BoundScaleMax.x != 0.0f && mTransTransform.localScale.x > BoundScaleMax.x + EPSILON)
                {
                    Vector3 theLocalScale = mTransTransform.localScale;
                    theLocalScale.x -= (theLocalScale.x - BoundScaleMax.x) * BoundsFirmness;
                    theLocalScale.y = theLocalScale.z = theLocalScale.x;
                    mTransTransform.localScale = theLocalScale;
                }
            }
            else
            {
                if (BoundScaleMin.x != 0.0f && mTransTransform.localScale.x < BoundScaleMin.x)
                {
                    Vector3 theLocScale = mTransTransform.localScale;
                    theLocScale.x -= (theLocScale.x - BoundScaleMin.x) * 0.5f;
                    if (theLocScale.x < BoundScaleMin.x - BoundScaleEpsilon.x)
                        theLocScale.x = BoundScaleMin.x - BoundScaleEpsilon.x;
                    mTransTransform.localScale = theLocScale;
                }
                else if (BoundScaleMax.x != 0.0f && mTransTransform.localScale.x > BoundScaleMax.x)
                {
                    Vector3 theLocScale = mTransTransform.localScale;
                    theLocScale.x -= (theLocScale.x - BoundScaleMax.x) * 0.5f;
                    if (theLocScale.x > BoundScaleMax.x + BoundScaleEpsilon.y)
                        theLocScale.x = BoundScaleMax.x + BoundScaleEpsilon.y;
                    mTransTransform.localScale = theLocScale;
                }

                // bound scale y
                if (BoundScaleMin.y != 0.0f && mTransTransform.localScale.y < BoundScaleMin.y)
                {
                    Vector3 theLocScale = mTransTransform.localScale;
                    theLocScale.y -= (theLocScale.y - BoundScaleMin.y) * 0.5f;
                    if (theLocScale.y < BoundScaleMin.y - BoundScaleEpsilon.x)
                        theLocScale.y = BoundScaleMin.y - BoundScaleEpsilon.x;
                    mTransTransform.localScale = theLocScale;
                }
                else if (BoundScaleMax.y != 0.0f && mTransTransform.localScale.y > BoundScaleMax.y)
                {
                    Vector3 theLocScale = mTransTransform.localScale;
                    theLocScale.y -= (theLocScale.y - BoundScaleMax.y) * 0.5f;
                    if (theLocScale.y > BoundScaleMax.y + BoundScaleEpsilon.y)
                        theLocScale.y = BoundScaleMax.y + BoundScaleEpsilon.y;
                    mTransTransform.localScale = theLocScale;
                }
            }
        }
    }

    // *****************************************************
    // ResetMomentum
    // -----------------------------------------------------
    private void ResetMomentum()
    {
        mMomentumScalar = 0.0f;
        mMomentumPos = Vector3.zero;
        mMomentumScale = Vector3.zero;
        mMomentumRot = 0.0f;
        mMemoryPos.Clear();
        mMemoryScale.Clear();
        mMemoryRot.Clear();
    }

    // *****************************************************
    // GetTouchPos
    // -----------------------------------------------------
    private Vector3 GetTouchPos(Vector3 inScreenSpaceTouch)
    {
        // when origin is Screen then return screen space position
        if (Origin == eOrigin.Screen)
            return inScreenSpaceTouch * ScreenSpaceScalar;

        // otherwise get world space position thru ray
        Ray theRay = RayCamera.ScreenPointToRay(inScreenSpaceTouch);
        float thePlaneDist;
        if (mTransPlane.Raycast(theRay, out thePlaneDist))
            return theRay.GetPoint(thePlaneDist);

        return inScreenSpaceTouch;
    }

    // *****************************************************
    // GetTransform
    // -----------------------------------------------------
    public Transform GetTransform()
    {
        return mTransTransform;
    }

    // *****************************************************
    // GetTransformDeltaPos
    // -----------------------------------------------------
    public Vector3 GetTransformDeltaPos()
    {
        return mTransDeltaPos;
    }

    // *****************************************************
    // GetTransformDeltaScale
    // -----------------------------------------------------
    public Vector3 GetTransformDeltaScale()
    {
        return mTransDeltaScale;
    }

    // *****************************************************
    // GetTransformDeltaRot
    // -----------------------------------------------------
    public float GetTransformDeltaRot()
    {
        return mTransDeltaRot;
    }

    // *****************************************************
    // GetTouchPos
    // -----------------------------------------------------
    public Vector3 GetTouchPos(int inIndex = 0)
    {
        return mTouchCurPts[inIndex];
    }

    // *****************************************************
    // DestroyTouchTransform
    // -----------------------------------------------------
    public void DestroyTouchTransform()
    {
        TouchTransformDestroy();
    }

    // *****************************************************
    // ComputeLinVelocity
    // -----------------------------------------------------
    public Vector3 ComputeLinVelocity()
    {
        Vector3 theResult = Vector3.zero;
        for (int i = 0; i < mMemoryPos.Count; i++)
            theResult += (Vector3)mMemoryPos[i];
        if (mMemoryPos.Count > 0)
            theResult /= (float)mMemoryPos.Count;

        // clamp momentum
        if (mMomentumClampValues[0] != 0)
        {
            theResult.x = Mathf.Clamp(theResult.x, -mMomentumClampValues[0], mMomentumClampValues[0]);
            theResult.y = Mathf.Clamp(theResult.y, -mMomentumClampValues[0], mMomentumClampValues[0]);
            theResult.z = Mathf.Clamp(theResult.z, -mMomentumClampValues[0], mMomentumClampValues[0]);
        }

        return theResult;
    }

    // *****************************************************
    // ComputeScaleVel
    // -----------------------------------------------------
    public Vector3 ComputeScaleVel()
    {
        Vector3 theResult = Vector3.zero;
        for (int i = 0; i < mMemoryScale.Count; i++)
            theResult += (Vector3)mMemoryScale[i];
        if (mMemoryPos.Count > 0)
            theResult /= (float)mMemoryScale.Count;

        // clamp momentum	
        if (mMomentumClampValues[1] != 0)
        {
            theResult.x = Mathf.Clamp(theResult.x, -mMomentumClampValues[1], mMomentumClampValues[1]);
            theResult.y = Mathf.Clamp(theResult.y, -mMomentumClampValues[1], mMomentumClampValues[1]);
            theResult.z = Mathf.Clamp(theResult.z, -mMomentumClampValues[1], mMomentumClampValues[1]);
        }

        return theResult;
    }

    // *****************************************************
    // ComputeAngVelocity
    // -----------------------------------------------------
    public float ComputeAngVelocity()
    {
        float theResult = 0.0f;
        for (int i = 0; i < mMemoryRot.Count; i++)
            theResult += (float)mMemoryRot[i];
        theResult /= (float)mMemoryRot.Count;

        // clamp momentum
        if (mMomentumClampValues[2] != 0)
            theResult = Mathf.Clamp(theResult, -mMomentumClampValues[2], mMomentumClampValues[2]);

        return theResult;
    }

    // *****************************************************
    // SetMomentumClampValues
    // -----------------------------------------------------
    public void SetMomentumClampValues(float inPosMomentum, float inScaleMomentum, float inRotMomentum)
    {
        mMomentumClampValues[0] = inPosMomentum;
        mMomentumClampValues[1] = inScaleMomentum;
        mMomentumClampValues[2] = inRotMomentum;
    }

    // *****************************************************
    // SetTouchedButton
    // -----------------------------------------------------
    public void SetTouchedButton(GuiButton inButton)
    {
        mTouchedButton = inButton;
    }

    // *****************************************************
    // GetMomentumPos
    // -----------------------------------------------------
    public Vector3 GetMomentumPos()
    {
        return mMomentumPos * mMomentumScalar;
    }

    // *****************************************************
    // GetMomentumScale
    // -----------------------------------------------------
    public Vector3 GetMomentumScale()
    {
        return mMomentumScale * mMomentumScalar;
    }

    // *****************************************************
    // GetMomentumRot
    // -----------------------------------------------------
    public float GetMomentumRot()
    {
        return mMomentumRot * mMomentumScalar;
    }
}
