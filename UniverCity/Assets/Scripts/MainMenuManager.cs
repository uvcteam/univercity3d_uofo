using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public UIPanel adUpdatePanel;
    public UIPanel menuPanel;
    public UIPanel loginPanel;
    public UIPanel signedInPanel;
    public float tweenTime = 0.33f;

    //void Awake()
    //{
    //    if (PlayerPrefs.HasKey("SignedIn") == true && PlayerPrefs.GetInt("SignedIn") == 1)
    //    {
    //        Application.LoadLevel(4);
    //    }
    //    else
    //    {
    //        if (signedInPanel != null)
    //            signedInPanel.GetComponent<TweenPosition>().Play(false);
    //        if (adUpdatePanel != null)
    //            adUpdatePanel.GetComponent<TweenPosition>().Play(false);
    //    }
    //}

    public void EnableTween()
    {
        foreach (TweenPosition tween in gameObject.GetComponentsInChildren<TweenPosition>())
        {
            if (tween != null)
            {
                tween.Reset();
                tween.Toggle();
            }
        }

        Debug.Log("EnableTween");
    }

    void OnSignInClicked()
    {
        foreach (UIInput input in gameObject.GetComponentsInChildren<UIInput>())
        {
            if (input != null)
            {
                input.text = "";
            }
        }

        PlayerPrefs.SetInt("SignedIn", 1);
        Application.LoadLevel(4);

        //signedInPanel.GetComponent<TweenPosition>().Play(true);
        //loginPanel.GetComponent<TweenPosition>().Play(false);
        //adUpdatePanel.GetComponent<TweenPosition>().Play(true);

    }

    void OnSignOutClicked()
    {
        PlayerPrefs.SetInt("SignedIn", 0);
        loginPanel.gameObject.SetActive(true);
        loginPanel.GetComponent<TweenPosition>().Play(true);
        signedInPanel.GetComponent<TweenPosition>().Play(false);
        adUpdatePanel.GetComponent<TweenPosition>().Play(false);
    }

    void LoadingFinished()
    {
        if (PlayerPrefs.HasKey("SignedIn"))
            gameObject.SetActive(false);
    }
}