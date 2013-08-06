using UnityEngine;
using System.Collections;

public class UnionHallErrorWindow : MonoBehaviour 
{
    public UILabel errorMessage = null;

    public void SetErrorText(string error)
    {
        errorMessage.text = "[FF0000]Error:\n[FFFFFF]Fix the following errors:\n" + error;
    }

    void OnOKClicked()
    {
        gameObject.SetActiveRecursively(false);
    }
}