using UnityEngine;
using System.Collections;

public class FloatingIsland : MonoBehaviour 
{
    public int levelIndex = -1;
    public float rotateSpeed = 30.0f;
    public float activationDistance = 25.0f;
    public GameObject ObjectToTween = null;
    public TweenTransform myTween;

    void Awake()
    {
        ObjectToTween = GameObject.Find("Main Camera");
        myTween = ObjectToTween.GetComponent<TweenTransform>();
		
    }

    void Update()
    {
        if (Camera.main == null)
        {
            Debug.Log("Main camera is null.");
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "FloatingIsland" &&
                    hit.collider.gameObject.name == gameObject.name &&
                    Vector3.Distance(
                        GameObject.FindWithTag("MainCamera").transform.position,
                        transform.position) <= activationDistance
                    )
                {
                    if (levelIndex == 3 || levelIndex == 4)
                    {
				        if (Application.platform == RuntimePlatform.WindowsEditor ||
							Application.platform == RuntimePlatform.WindowsWebPlayer ||
							Application.platform == RuntimePlatform.OSXWebPlayer)
				        {
	                        if (!GameObject.FindWithTag("UserManager").GetComponent<UserManager>().CurrentUser.LoggedIn)
	                        {
	                            GameObject modal = Instantiate(Resources.Load("Prefabs/Error Modal", typeof(GameObject))) as GameObject;
	                            modal.GetComponent<Modal>().objectsToHide.Add(GameObject.Find("CameraBase"));
	                            modal.GetComponent<Modal>().HideObjects();
	                        }
	                        else
	                        {
	                            myTween.eventReceiver = gameObject;
	                            myTween.to = transform;
	                            myTween.duration = 1.0f;
	                            myTween.Reset();
	                            myTween.Toggle();
	                        }
						}
						
        				else if (Application.platform == RuntimePlatform.Android ||
                 		Application.platform == RuntimePlatform.IPhonePlayer)
						{
							CheckLogin(levelIndex);
							return;
						}
                    }
                    else
                    {
                        myTween.eventReceiver = gameObject;
                        myTween.to = transform;
                        myTween.duration = 1.0f;
                        myTween.Reset();
                        myTween.Toggle();
                    }
                }
            }
        }
    }

    void CheckLogin(int index)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("UserManager");
        if (manager.GetComponent<UserManager>().IsSignedIn())
		{
			Application.LoadLevel(index);
			return;
		}
		
        NativeDialogs.Instance.ShowLoginPasswordMessageBox("Login Required", "", new string[] { "Cancel", "OK" },
                                                           false,
            (string login, string password, string button) =>
            {
                if (button == "OK")
                    StartCoroutine(manager.GetComponent<UserManager>().SignIn(login, password, index));
            });
    }

    void OnTweenFinished(UITweener tweener)
    {
        Application.LoadLevel(levelIndex);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
    }
}