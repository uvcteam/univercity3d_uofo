using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public UIPanel adUpdatePanel;
    public UIPanel menuPanel;
    public UIPanel loginPanel;
    public UIPanel signedInPanel;
    public float tweenTime = 0.33f;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SignedIn") == true && PlayerPrefs.GetInt("SignedIn") == 1)
        {
            if (loginPanel != null)
                loginPanel.gameObject.SetActive(false);
            if (adUpdatePanel != null)
                adUpdatePanel.gameObject.SetActive(true);
            //loginPanel.GetComponent<TweenPosition>().Play(true);
            if (signedInPanel != null)
                signedInPanel.gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        foreach (TweenPosition tween in gameObject.GetComponentsInChildren<TweenPosition>())
        {
            if (tween != null)
            {
                tween.Reset();
                tween.Toggle();
            }
        }
        
    }

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

        adUpdatePanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(true);
        signedInPanel.gameObject.SetActive(true);

        signedInPanel.GetComponent<TweenPosition>().Play(true);
        loginPanel.GetComponent<TweenPosition>().Play(false);
        adUpdatePanel.GetComponent<TweenPosition>().Play(true);

    }

    void OnSignOutClicked()
    {
        PlayerPrefs.SetInt("SignedIn", 0);
        loginPanel.gameObject.SetActive(true);
        loginPanel.GetComponent<TweenPosition>().Play(true);
        signedInPanel.GetComponent<TweenPosition>().Play(false);
        adUpdatePanel.GetComponent<TweenPosition>().Play(false);

    }
}