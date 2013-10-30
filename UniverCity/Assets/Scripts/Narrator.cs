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

        speechBubbleObject.SetActive(_toggle);

    }
}
