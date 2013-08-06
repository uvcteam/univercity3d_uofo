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
        ObjectToTween = GameObject.Find("CameraBase");
        myTween = ObjectToTween.GetComponent<TweenTransform>();
    }

    void Update()
    {
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
                    myTween.eventReceiver = gameObject;
                    myTween.to = transform;
                    myTween.duration = 1.0f;
                    myTween.Reset();
                    myTween.Toggle();
                }
            }
        }
    }

    void OnTweenFinished(UITweener tweener)
    {
        Application.LoadLevel(levelIndex);
    }
}