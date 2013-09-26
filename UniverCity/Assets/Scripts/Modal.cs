using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Modal : MonoBehaviour 
{
    public List<GameObject> objectsToHide = new List<GameObject>();
    void Start()
    {
        gameObject.transform.parent = GameObject.Find("Camera").transform;
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, -500.0f);
    }

    void CloseModal()
    {
        ShowObjects();
        Destroy(gameObject);
    }

    public void HideObjects()
    {
        for (int i = 0; i < objectsToHide.Count; ++i)
        {
            objectsToHide[i].SetActive(false);
        }
    }

    public void ShowObjects()
    {
        for (int i = 0; i < objectsToHide.Count; ++i)
        {
            objectsToHide[i].SetActive(true);
        }
    }

    public void GoToLoginPage()
    {
        Application.LoadLevel(0);
    }
}
