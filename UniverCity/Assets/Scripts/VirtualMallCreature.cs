using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class VirtualMallCreature : MonoBehaviour
{
    public float velocity = 10.0f;
    public Vector3 finalPosition = Vector3.zero;
    private Vector3 startPosition;
    public List<GameObject> objectsToShow;
    public UILabel MyUiLabel = null;
    public string MyString = "";
    public bool IsDone = false;
    public GameObject Turtle;
	// Use this for initialization
	void Start ()
	{
	    startPosition = transform.position;
	    //StartCoroutine(Present());
	}

    public IEnumerator Present()
    {
		IsDone = false;
        yield return new WaitForSeconds(1.0f); //Wait for ad to render images to prevent lag.
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        string backup = MyUiLabel.text;

        animation.Play("walk");
        while (transform.position.x > finalPosition.x)
        {
            transform.Translate(Vector3.forward*Time.deltaTime*velocity);
            yield return new WaitForEndOfFrame();
        }

        animation.Play("capture");
        foreach(GameObject go in objectsToShow)
            go.SetActive(true);
        yield return new WaitForSeconds(animation.clip.length);

        while (transform.rotation.eulerAngles.y > 180)
        {
            transform.Rotate(Vector3.up, -90.0f * Time.deltaTime * 3.0f);
            yield return new WaitForEndOfFrame();
        }

        animation.Play("speak");
        MyUiLabel.text = MyString;
        yield return new WaitForSeconds(animation.clip.length * 2.0f);
        animation.Stop();
        MyUiLabel.text = backup;
        while (transform.rotation.eulerAngles.y < 270)
        {
            transform.Rotate(Vector3.up, 90.0f * Time.deltaTime * 3.0f);
            yield return new WaitForEndOfFrame();
        }

        animation.Play("walk");
        while (transform.position.x > -2.0f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * velocity);
            yield return new WaitForEndOfFrame();
        }
        IsDone = true;
        yield return null;
    }
	
	void OnDisable()
	{
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
	}
}