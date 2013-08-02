using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
    public int sceneNumber = 0;

	// Use this for initialization

    void Clicked()
    {
        Application.LoadLevel(sceneNumber);
    }
	
}