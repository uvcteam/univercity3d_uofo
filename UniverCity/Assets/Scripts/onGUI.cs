using UnityEngine;
using System.Collections;

public class onGUI : MonoBehaviour {
	bool toggle;
	Rect r;
	public PlayHardwareMovieClassPro movie;
	// Use this for initialization
	void Start () {
		r = new Rect(0,0,100,30);
		toggle=false;
	}
	
	
	void OnGUI() {
		if (GUI.Button(r, "toggle Audio")) {
			if(toggle) {
           		movie.AudioLevel(1.0f);
			} else {
				movie.AudioLevel(0.0f);
			}
			toggle= !toggle;
		}
    }
}
