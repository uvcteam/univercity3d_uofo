using UnityEngine;
using System.Collections;

public class AutoScroll : MonoBehaviour 
{
    private bool _pressed = false;
    private float _lastTimeTouched = 0.0f;

    public Vector3 start = new Vector3(0.0f, 0.0f, 0.0f);
    public float waitTime = 5.0f;
	
	// Update is called once per frame
    void LateUpdate()
    {
        if (Camera.current != null && transform.localPosition.y > 4000.0f)
        {
            transform.localPosition = start;
            Debug.Log(transform.localPosition);
        }

        if ((Time.time - _lastTimeTouched > waitTime) && !_pressed)
        {
            transform.Translate(new Vector3(0.0f, 0.25f * Time.deltaTime, 0.0f));
        }
    }

    void OnPressed()
    {
        _pressed = true;
    }

    void OnReleased()
    {
        _lastTimeTouched = Time.time;
        _pressed = false;
    }
}
