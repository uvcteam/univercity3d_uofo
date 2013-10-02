using UnityEngine;
using System.Collections;

public class UnionHallResponseNotification : MonoBehaviour 
{
    public GameObject unionHall = null;

    void OnBackClicked()
    {
        //unionHall.SetActive(true);
        gameObject.SetActive(false);
    }
}