using System;
using UnityEngine;
using System.Collections;

/* Made simple to use (drag and drop, done) for regular keyboard layout.
 * WASD  : Expected movement.
 * Q, E  : Up and down, respectively.
 * Shift : Makes the camera accelerate.
 */

public class FlyCam : MonoBehaviour
{
    public GUIText HUDText = null;
    public GUIText HUDSubText = null;
    public float mainSpeed = 100.0f; // Regular speed.
    public float shiftAdd = 250.0f; // Multiplied by how long shift is held.
    // Think sprinting.
    public float maxShift = 1000.0f; // Maximum speed.
    public float maxHeight = 85.0f; // The max height the camera can go.
    public float minHeight = 20.0f; // The min height the camera can go.

    //private Vector3 lastMouse = new Vector3(255, 255, 255); // So the camera doesn't jump.
    private float totalRun = 1.0f;
    public float activationDistance = 50.0f;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    public bool isHidden = false;

    Quaternion originalRotation = Quaternion.identity;
    private GameObject LastHitObject = null;

    void Update()
    {
        /*
        if (Application.loadedLevel != 2)
        {
            Screen.lockCursor = true;
            Screen.lockCursor = false;
        }

	    if (Input.GetKeyDown(KeyCode.Escape))
	        Application.LoadLevel(0);

        if (axes == RotationAxes.MouseXAndY)
        {
            // Read the mouse input axis
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }
		//lastMouse = Input.mousePosition;
		
		// Keyboard commands.
		float f = 0.0f;
		Vector3 p = GetBaseInput();
		
		if (Input.GetKey(KeyCode.LeftShift))
		{
			totalRun += Time.deltaTime;
			p *= totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
		}
		else
		{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1.0f, 1000.0f);
			p *= mainSpeed;
		}
		
		p *= Time.deltaTime;
		f = p.y + transform.position.y;
		transform.Translate(p);
        */

        transform.position = new Vector3(transform.position.x,
                                         Mathf.Clamp(transform.position.y, minHeight, maxHeight),
                                         transform.position.z);
        if (!rigidbody.isKinematic)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        if (FloatingBubble.HasAdUp) return;
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main.WorldToViewportPoint(ray.origin).x <= 0.1 &&
                Camera.main.WorldToViewportPoint(ray.origin).y >= 0.9)
            {
                GetComponent<HTMLExplorer>().OpenMenu();
            }
            else if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Blocker")
                {
                    Debug.Log("RETURNING");
                    return;
                }
                if ((hit.collider.tag == "FloatingSphere" || hit.collider.tag == "HoverCollision") &&
                    (Vector3.Distance(
                        GameObject.FindWithTag("MainCamera").transform.position,
                        transform.position) <= activationDistance))
                {
                    hit.collider.gameObject.GetComponent<FloatingBubble>().BubbleClicked();
                }
            }
        }
    }

    Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.W))
            p_Velocity += new Vector3(0, 0, 1.0f);
        if (Input.GetKey(KeyCode.S))
            p_Velocity += new Vector3(0, 0, -1.0f);
        if (Input.GetKey(KeyCode.A))
            p_Velocity += new Vector3(-1.0f, 0, 0);
        if (Input.GetKey(KeyCode.D))
            p_Velocity += new Vector3(1.0f, 0, 0);
        if (Input.GetKey(KeyCode.Q))
            p_Velocity += new Vector3(0, -1.0f, 0);
        if (Input.GetKey(KeyCode.E))
            p_Velocity += new Vector3(0, 1.0f, 0);
        if (Input.GetKeyDown(KeyCode.H) && Application.loadedLevel == 2)
            isHidden = !isHidden;
        if (Input.GetKeyDown(KeyCode.M) && Application.loadedLevel == 2)
            Application.LoadLevel(0);
        rigidbody.velocity = Vector3.zero;
        return p_Velocity;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        int num_business = 0;

        if (HUDText != null)
        {
            if (Physics.Raycast(ray, out hit, activationDistance) && hit.collider.tag == "HoverCollision")
            {
                if (LastHitObject != null && LastHitObject != hit.collider.gameObject)
                    LastHitObject.GetComponent<ExplorerBusiness>().DeactivateHighlight();
                LastHitObject = hit.collider.gameObject;
                hit.collider.GetComponent<ExplorerBusiness>().ActivateHighlight();
                num_business = hit.collider.GetComponent<ExplorerBusiness>().num_businesses;
                HUDText.text = hit.collider.gameObject.name;
                HUDSubText.text = num_business + " Business" + (num_business != 1 ? "es" : "");
            }
            else
            {
                if (LastHitObject)
                {
                    LastHitObject.GetComponent<ExplorerBusiness>().DeactivateHighlight();
                    LastHitObject = null;
                }
                HUDText.text = "";
                HUDSubText.text = "";
            }
        }
    }
}