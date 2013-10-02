using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour 
{
    public UIButton sprite;
    public UILabel label;

    void DeActivate()
    {
        sprite.isEnabled = false;
        label.color = new Color(0.21f, 0.84f, 1.0f, 1.0f);
    }
}