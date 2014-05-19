using UnityEngine;
using System.Collections;

public class ExplorerHighlight : HighlightingController
{
    public Color flashingStartColor = Color.blue;
    public Color flashingEndColor = Color.cyan;
    public float flashingDelay = 2.5f;
    public float flashingFrequency = 0.5f;

    public void Highlight()
    {
        ho.ConstantOn(flashingStartColor);
    }

    public void DeHighlight()
    {
        ho.ConstantOff();
    }
}