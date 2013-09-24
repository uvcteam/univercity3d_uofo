using UnityEngine;
using System.Collections;

// See base class for pause/resume etc....

// do something special; for the clip movie
public class PlayFileBasedMovieForClip : PlayHardwareMovieClassPro {
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}

	protected override void Start ()
	{
		base.Start();
	}
	
	public override void FinishedMovie(string str)
	{
		OpenGLMovieRewindIndex(movieIndex);
		Debug.Log("Half volume");
		OpenGLMovieVolumeIndex(movieIndex,0.5f); // half volume;
	}
	
	public override void PlayMovie (string movie)
	{
		base.PlayMovie(movie);
	}
	
	public override void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
			int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
			MakeTextures(textureWidth,textureHeight);
			if(currentMovie == "clip512.mp4") {
				setTiling(1.0f);
				//NotificationCenter.postNotification(NotificationType.FlameState,1);
			} else {
				setTiling(1.0f);
				renderer.sharedMaterial.SetColor ("_Color", Color.black);
			}
		}
		playMovie = true;
	}
}
