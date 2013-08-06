using UnityEngine;
using System.Collections;

public class MallScrollButton : MonoBehaviour
{
    public Renderer Icon;
    public TextMesh TextTop;
    public TextMesh TextBottom;

    public void Setup(FormMallCategories.Button inButton)
    {
        Icon.material.mainTexture = inButton.Icon;
        TextTop.text = inButton.TopText;
        TextBottom.text = inButton.BottomText;
    }

    public void Setup(FormMallSubcategories.Button inButton)
    {
        Icon.material.mainTexture = inButton.Icon;
        TextTop.text = inButton.TopText;
        TextBottom.text = inButton.BottomText;
    }

    public void Setup(string inString)
    {
        TextTop.text = inString;
    }
}
