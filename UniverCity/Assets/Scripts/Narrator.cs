using UnityEngine;
using System.Collections;

public class Narrator : MonoBehaviour 
{
    public GameObject speechBubbleObject;
    public GameObject texture;
    public SpeechBubble speechBubble;
    private bool _toggle = true;

    public void ToggleSpeechBubble()
    {
        _toggle = !_toggle;

        //if (_toggle)
        //    speechBubbleObject.transform.localPosition = new Vector3(-330.0f, -550.0f, -500.0f);
        //else
        //    speechBubbleObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        speechBubbleObject.SetActive(_toggle);
    }
}
