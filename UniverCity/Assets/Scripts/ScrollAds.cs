using UnityEngine;
using System.Collections;

public class ScrollAds : MonoBehaviour {

    public float tweenTime = 0.25f;
    public float adTime = 5.0f;
    public TweenPosition tweenOut;
    public TweenPosition tweenIn;
    private Vector3 startPos = new Vector3(1000.0f, 0.0f, 0.0f);
    public string[] adTexts;

    private int index = 0;


    // Use this for initialization
    void OnEnable()
    {
        //startPos = gameObject.transform.position;
        StartCoroutine("Scroll");
        tweenOut.eventReceiver = gameObject;
        //TweenPosition.Begin(this.gameObject, tweenTime, new Vector3(transform.position.x, -20f, 0f));
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            index %= adTexts.Length;
            GetComponent<UILabel>().text = adTexts[index++];
            tweenIn.Reset();
            tweenIn.Toggle();
            yield return new WaitForSeconds(adTime);

            tweenOut.Reset();
            tweenOut.Toggle();
            yield return new WaitForSeconds(adTime);
        }
    }

    void OnPositionTweenedOut(UITweener tweener)
    {
        gameObject.transform.position = startPos;
    }
}
