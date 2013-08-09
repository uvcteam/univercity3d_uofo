using UnityEngine;
using System.Collections;

public class UnionHallErrorWindow : MonoBehaviour 
{
    public UILabel errorMessage = null;

    public void SetErrorText(string error)
    {
        errorMessage.text = "Error\nFix the following errors:\n" + error;
    }

    void OnOKClicked()
    {
        gameObject.SetActiveRecursively(false);
    }
}