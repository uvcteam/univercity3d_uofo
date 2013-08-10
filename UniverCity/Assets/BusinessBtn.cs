using UnityEngine;
using System.Collections;

public class BusinessBtn : MonoBehaviour 
{
    public int businessId;

    public void OnBusinessClicked()
    {
        Application.OpenURL("http://www.univercity3d.com/univercity/playad?b=" + businessId.ToString());
    }
}