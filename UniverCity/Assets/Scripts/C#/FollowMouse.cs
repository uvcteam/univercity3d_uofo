using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour
{
    RaycastHit hit;
    float range = 100.0f;

	// Update is called once per frame
	void Update () 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            transform.LookAt(hit.collider.gameObject.transform);
        }
	}
}