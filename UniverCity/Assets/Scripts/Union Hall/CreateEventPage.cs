using UnityEngine;
using System.Collections;

public class CreateEventPage : MonoBehaviour
{
    public GameObject prevPage = null;
    public GameObject nextPage = null;

    private void OnPreviousClicked()
    {
        if (prevPage == null) return;
        prevPage.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnNextClicked()
    {
        if (nextPage == null) return;
        nextPage.SetActive(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && prevPage != null)
            OnPreviousClicked();
    }
}