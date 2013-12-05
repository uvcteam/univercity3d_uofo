using UnityEngine;
using System.Collections;

public class FloatingBubble : MonoBehaviour
{
    public string myLocation = "";
    public float rotateSpeed = 30.0f;
    public GameObject adPanel;
    private BusinessManager manager;
    public Transform tweenTo = null;
    public TweenTransform myTween = null;
	public GameObject ObjectToTween = null;
    public static bool HasAdUp = false;

    void Awake()
    {
		ObjectToTween = GameObject.Find("Main Camera");
        myTween = ObjectToTween.GetComponent<TweenTransform>();
		if (ObjectToTween == null)
			Debug.LogError("CameraBase could not be located!");
        manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        gameObject.SetActive(
            manager.busByCoord.ContainsKey(new Vector2(transform.localPosition.x, transform.localPosition.z)));
    }

	// Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);
    }

    void OnTweenFinished(UITweener tweener)
    {
        adPanel = GameObject.Find("Main Camera");
        adPanel.SetActive(true);
        adPanel.GetComponent<HTMLExplorer>().SetPosition(transform, gameObject);
        adPanel.GetComponent<HTMLExplorer>().SetReturnPosition();
        gameObject.SetActive(false);
    }

    public void BubbleClicked()
    {
        myTween.from = ObjectToTween.transform;
        myTween.to = tweenTo;
        myTween.duration = 1.0f;

        GameObject placeHolder = new GameObject("Placeholder");
        placeHolder.transform.position =
            new Vector3(ObjectToTween.transform.position.x,
                        ObjectToTween.transform.position.y,
                        ObjectToTween.transform.position.z);
        placeHolder.transform.rotation = new Quaternion(ObjectToTween.transform.rotation.x,
                                                        ObjectToTween.transform.rotation.y,
                                                        ObjectToTween.transform.rotation.z,
                                                        ObjectToTween.transform.rotation.w);

        myTween.eventReceiver = gameObject;
        //GameObject.FindWithTag("MainCamera").GetComponent<FlyCam>().enabled = false;
        ObjectToTween.rigidbody.isKinematic = true;
        myTween.Reset();
        myTween.Toggle();
        FloatingBubble.HasAdUp = true;
    }
}