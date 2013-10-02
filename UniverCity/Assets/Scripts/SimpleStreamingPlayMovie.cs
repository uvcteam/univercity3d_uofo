using UnityEngine;
using System.Collections;

public class SimpleStreamingPlayMovie : MonoBehaviour
{
	// Keep track of playing video.
	bool isPlaying;
	// is paused
	bool isPlayPaused;
	Matrix4x4 scaledMatrix; // for onGUI
	float retinaScale;
	public PlayStreamingMovie moviePlayer; // The movie playback -- orientation is done using its parent object
	
	public string streamingMovie;
	public string fallbackMovie;// if streaming fails use this instead.
	

	// Use this for initialization
	void Awake ()
	{
		isPlaying = false;
	}
	
	void OnGUI ()
	{
		GenerateGUIMatrix ();
		GUI.matrix = scaledMatrix;
		
		if (!isPlaying) {
			if (GUI.Button (new Rect (5, 10, 100, 25), "Play")) {
				if (PlayStreamingMovie.OpenGLMovieWiFiAvailable ()) {
					moviePlayer.PlayMovie (streamingMovie);
				} else { // see if stream was played fully already or use fallback
					moviePlayer.PlayPreLoadedStream (streamingMovie, fallbackMovie);
				}
				isPlayPaused = false;
				isPlaying = true;
			}
		}
		
		if (isPlaying) {
			if (GUI.Button (new Rect (5, 10, 100, 25), "Stop")) {
				moviePlayer.StopMovie ();
				isPlaying = false;
			}
			
			if (!isPlayPaused) {
				if (GUI.Button (new Rect (110, 10, 100, 25), "Pause")) {
					moviePlayer.PauseMovie ();
					isPlayPaused = true;
				}
			}
			
			if (isPlayPaused) {
				if (GUI.Button (new Rect (110, 10, 100, 25), "Resume")) {
					moviePlayer.ResumeMovie ();
					isPlayPaused = false;
				}
			}
		}

	}
	
	void GenerateGUIMatrix ()
	{
		retinaScale = 1.0f;
		switch (Screen.orientation) {
		case ScreenOrientation.Portrait:
		case ScreenOrientation.PortraitUpsideDown:
			if (Screen.width > 768) { // is ipad
				retinaScale = Screen.width / 768.0f;
			} else { //is iphone/pod
				retinaScale = Screen.width / 320.0f;	
			}	
			break;
		case ScreenOrientation.LandscapeRight:
		case ScreenOrientation.LandscapeLeft:
			if (Screen.height > 768) { // is ipad
				retinaScale = Screen.height / 768.0f;
			} else { //is iphone/pod
				retinaScale = Screen.height / 320.0f;	
			}	
			break;	
		}
		scaledMatrix = Matrix4x4.Scale (new Vector3 (retinaScale, retinaScale, retinaScale));
	}
}

