using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

// See base class for pause/resumer etc .....
public class PlayStreamingMovie : PlayHardwareMovieClassPro {
	public GUIText pauseIndictator; // need to display something if movie pauses with stalled stream
    public GameObject messageReceiver = null;
	bool doneReady; //check done textures
	bool streamEarly; // if streamReady before movieReady -- stream most probaly was already a file (or short stream)
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}

	protected override void Start ()
	{
		base.Start();
		movieIndex=0; // only one stream and at index 0.
	}
	
	public override void FinishedMovie(string str)
	{
        messageReceiver.SendMessage("MovieFinished");
		StopMovie();
//		OpenGLMovieRewindIndex(movieIndex);
//		Debug.Log("Half volume");
//		OpenGLMovieVolumeIndex(movieIndex,0.5f); // half volume;
	}
	
	[DllImport("__Internal")]
    protected static extern void UnityMovieInitStream(string url);
	
	public override void PlayMovie (string movie)
	{
		currentMovie=movie;
		doneReady=false; 
		streamEarly=false;
		playMovie=false; // wait till texture is ready
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitStream(movie);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie=true;
		}

        NativeDialogs.Instance.ShowProgressDialog("Please wait.", "Loading Video", false, false);
	}
	
	[DllImport("__Internal")]
    protected static extern bool UnityMovieInitPreLoadedStream(string url);
	[DllImport("__Internal")]
    public static extern bool OpenGLMovieWiFiAvailable();
	
	public virtual void PlayPreLoadedStream (string movie, string fallbackMovie)
	{
		currentMovie=movie;
		doneReady=false; 
		streamEarly=false;
		playMovie=false; // wait till texture is ready
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if(!UnityMovieInitPreLoadedStream(movie)) {
				bool audio=true;
				UnityMovieInitIndex (0, fallbackMovie,audio, -1.0f);// use audio, start at beginning.
				streamEarly=true; // there is not stream only a fallback file so pretend stream detected!
				pauseIndictator.text="Fallback";
			} else {
				pauseIndictator.text="Preloaded Stream";
			}
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie=true;
		}
	}
	
	public override void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if(!doneReady) {
				Debug.Log("Ready FirstFrame");
				doneReady=true;
				int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
				int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
				MakeTextures(textureWidth,textureHeight);
				print("Textures " + textureWidth + " " + textureHeight);
				setTiling(1.0f);
			    NativeDialogs.Instance.HideProgressDialog();
			}
		}
		if(streamEarly) {
			playMovie = true; // only play once stream is ready.
		} 
		// do not start to play yet ... wait for streamReady
	}
	
	// Callback function from iOS
	public override void streamReadFail () // only one stream
	{
		pauseIndictator.text="StreamRead Fail";
	}
	
	// Callback function from iOS
	public override void ReadyStream () // only one stream
	{
		if(!doneReady) {
			streamEarly=true;
		} else {
			playMovie = true; // only play once stream is ready.
		}
	}
	
	// Callback function from iOS
	public override void streamPause(bool isPaused) // only one stream
	{
		if(isPaused) {
			pauseIndictator.text="Stalled";
		} else {
			pauseIndictator.text="";
		}
	}
	
	void OnDisable()
	{
		Resources.UnloadUnusedAssets();
	}
}
