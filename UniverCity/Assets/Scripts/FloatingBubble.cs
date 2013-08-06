using UnityEngine;
using System.Collections;

public class FloatingBubble : MonoBehaviour
{
    public string myLocation = "";
    public float rotateSpeed = 30.0f;
    public float activationDistance = 50.0f;
    public GameObject adPanel;
    private BusinessManager manager;
    public Transform tweenTo = null;
    public TweenTransform myTween = null;
	public GameObject ObjectToTween = null;

    void Awake()
    {
		ObjectToTween = GameObject.Find("CameraBase");
        myTween = ObjectToTween.GetComponent<TweenTransform>();
		if (ObjectToTween == null)
			Debug.LogError("CameraBase could not be located!");
        manager = GameObject.Find("BusinessManager").GetComponent<BusinessManager>();
        gameObject.SetActiveRecursively(
            manager.busByCoord.ContainsKey(new Vector2(transform.localPosition.x, transform.localPosition.z)));
    }

	// Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);

        if (!Input.GetMouseButtonDown(0)) return;
        if (!Physics.Raycast(ray, out hit)) return;
        if (hit.collider.tag != "FloatingSphere" || hit.collider.gameObject.name != gameObject.name ||
            !(Vector3.Distance(
                GameObject.FindWithTag("MainCamera").transform.position,
                transform.position) <= activationDistance)) return;

        myTween.from = ObjectToTween.transform;
        myTween.to = tweenTo;
        myTween.duration = 2.0f;

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
        GameObject.FindWithTag("MainCamera").GetComponent<FlyCam>().enabled = false;
        ObjectToTween.rigidbody.isKinematic = true;
		myTween.Reset();
        myTween.Toggle();
        adPanel.SetActiveRecursively(true);
        adPanel.GetComponent<AdPanelManager>().SetPosition(transform, gameObject);
        adPanel.GetComponent<AdPanelManager>().SetReturnPosition();
    }

    void OnTweenFinished(UITweener tweener)
    {
        gameObject.SetActiveRecursively(false);
    }
}