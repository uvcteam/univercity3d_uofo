using UnityEngine;
using System.Collections;

public class GUITextureMovieFileBased : GUITextureMovie {

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
	
	public override void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
			int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
			MakeTextures(textureWidth,textureHeight);
			setTiling(1.0f);
		}
		playMovie = true;
	}
}

