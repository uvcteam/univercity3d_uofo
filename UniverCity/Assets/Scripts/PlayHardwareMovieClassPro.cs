using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PlayHardwareMovieClassPro : MonoBehaviour
{
	public int movieIndex;
	protected int currentTextureWidth = 0;
	protected int currentTextureHeight = 0;
	protected Texture2D m_Texture = null;
	protected Color movieColor = Color.white;
	protected Color movieEnd = Color.clear;
	protected string currentMovie;
	protected bool playMovie;
	protected int movieColorSet;
	
	
	// Use this for initialization
	protected virtual void Awake ()
	{
		// so we can see thing in editor
		if (Application.platform == RuntimePlatform.OSXEditor) {
			movieColor = Color.green;
			movieEnd = Color.gray;
		}
	}
	
	// Link to the UnityTexture plugin and call the UpdateTexture function there.
	[DllImport("__Internal")]
	private static extern float UnityMoviePreferredRotationIndex (int index); 
	// Assumes that parent is available to set rotation
	void MoviePreferredRotation ()
	{
//		if (!transform.parent.name.Equals ("MovieProferredRotation")) {
//			if(UnityMoviePreferredRotationIndex (movieIndex) != 180.0f) {
//				Debug.LogError ("Movie needs MovieProferredRotation parent to rotate to preferred rotation of " + UnityMoviePreferredRotationIndex (movieIndex));	
//			} 
//		} else {
//			Vector3 rot = transform.parent.transform.localEulerAngles;
//			if (Application.platform != RuntimePlatform.OSXEditor) {
//				rot.z = UnityMoviePreferredRotationIndex (movieIndex);
//			}
//			print ("PreferredOrient " + rot.z);
//			transform.parent.transform.localEulerAngles = rot;
//		}
	}
	
	// Link to the UnityTexture plugin and call the UpdateTexture function there.
	[DllImport("__Internal")]
	protected static extern void UnityMovieInitIndex (int index, string file, bool audio, float seek);

	protected virtual void Start ()
	{
		playMovie = false;
		MakeTextures (16, 16);
		renderer.enabled = false;
	}
	
	protected virtual void MakeTextures (int textWidth, int textHeight)
	{
		MoviePreferredRotation ();
		if (textWidth != currentTextureWidth || textHeight != currentTextureHeight) {
			currentTextureWidth = textWidth;
			currentTextureHeight = textHeight;
			// Debug.Log ("Texture Size " + currentTextureWidth + " " + currentTextureHeight);
			// Create texture that will be updated in the plugin
			if(m_Texture) {
				Destroy(m_Texture);
			} 
			m_Texture = new Texture2D (currentTextureWidth, currentTextureHeight, TextureFormat.BGRA32, false);
		
			
			// Assign texture to the renderer
			if (renderer) {
				if (Application.platform != RuntimePlatform.OSXEditor) {
					renderer.sharedMaterial.SetTexture ("_MainTex", m_Texture);
				}
				movieColorSet = 0;
				renderer.sharedMaterial.SetColor ("_Color", movieEnd);
			
			} else {
				Debug.Log ("Game object has no renderer to assign the generated texture to!");
			}
		} else {
			movieColorSet = 0;
			renderer.sharedMaterial.SetColor ("_Color", movieEnd);
		}
		
	}
	
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieTextureWidthIndex (int index);
	
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieTextureHeightIndex (int index);
   
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieWidthIndex (int index);
    
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieHeightIndex (int index);
	
	// heightfactor can be used to remove black borders 
	protected virtual void setTiling (float heightFactor)
	{
		float width = (float)OpenGLMovieWidthIndex (movieIndex);
		float height = (float)OpenGLMovieHeightIndex (movieIndex);
		//Debug.Log("tiling Size " + width + " " + height);
		float scaleW = width / currentTextureWidth;
		float scaleH = (height * heightFactor) / currentTextureHeight;
		float remove = (height - height * heightFactor) / currentTextureHeight;
		float heightAmount = remove / 2.0f;
		float widthAmount = (1.0f - scaleW);
		renderer.sharedMaterial.SetTextureOffset ("_MainTex", new Vector2 (-widthAmount, heightAmount));
		renderer.sharedMaterial.SetTextureScale ("_MainTex", new Vector2 (-scaleW, scaleH));
		//Debug.Log("offset " + heightAmount + " scale " + scaleH);
	}

	[DllImport("__Internal")]
	private static extern void OpenGLMoviePauseIndex (int index);

	[DllImport("__Internal")]
	private static extern float OpenGLMovieResumeIndex (int index);
	
	public void ResumeMovie ()
	{
		if (playMovie) {
			//float resumeTime=0.0f;
			if (Application.platform != RuntimePlatform.OSXEditor) {
				//resumeTime=
				OpenGLMovieResumeIndex (movieIndex);
			}
		}
	}
	
	void OnApplicationPause (bool pause)
	{
		if (pause) {
			PauseMovie (); 
		} else {
			ResumeMovie ();// MUST resume after application paase
		}
	}
	
	public void PauseMovie ()
	{
		if (playMovie) {
			if (Application.platform != RuntimePlatform.OSXEditor) {
				OpenGLMoviePauseIndex (movieIndex);
			}
		}
	}
	
	[DllImport("__Internal")]
	protected static extern void OpenGLMovieVolumeIndex (int index, float volume);
	
	public void AudioLevel (float level)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if (level < 0f)
				level = 0f;
			if (level > 1f)
				level = 1f;
			OpenGLMovieVolumeIndex (movieIndex, level);
		}
	}
	
	// Callback from iOS
	[DllImport("__Internal")]
	protected static extern void OpenGLMovieRewindIndex (int index);

	public virtual void FinishedMovie (string str)
	{
		OpenGLMovieRewindIndex (movieIndex);
	}
	
	public virtual void PlayMovie (string movie)
	{
		currentMovie = movie;
		playMovie=false;// wait till texture is ready
		bool audio = true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitIndex (movieIndex, movie, audio, -1.0f);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie = true;
		}
	}
	
	public virtual void PlayMovieAt (string movie, float startTime)
	{
		currentMovie = movie;
		playMovie=false;// wait till texture is ready
		bool audio = true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitIndex (movieIndex, movie, audio, startTime);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie = true;
		}
	}
	
	// Link to the UnityTexture plugin
	[DllImport("__Internal")]
	private static extern void UnityMovieInitFromDocumentsIndex (int index, string file, bool audio, float seek);
	// Use this if movie has be placed/downloaded to Documernts (only the move name is required not the path
	public void PlayMovieFromDocuments (string movie)
	{
		playMovie=false;// wait till texture is ready
		currentMovie = movie;
		bool audio = true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitFromDocumentsIndex (movieIndex, movie, audio, -1.0f);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie = true;
		}
	}
	
	// Link to the UnityTexture plugin
	[DllImport("__Internal")]
	private static extern void UnityMovieInitFromFilePathIndex (int index, string file, bool audio, float seek);
	// Use this if movie has be placed/downloaded somewhere that you know the path to. Must use full path 
	// -- You can use PlayMovie for Resource based files or PlayMovieFromDocuments for Document based files
	public void PlayMovieFromFilePath (string movie)
	{
		playMovie=false;// wait till texture is ready
		currentMovie = movie;
		print ("Play Movie " + movie);
		bool audio = true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitFromFilePathIndex (movieIndex, movie, audio, -1.0f);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie = true;
		}
	}
	
	// Callback function from iOS
	public virtual void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			int textureWidth = OpenGLMovieTextureWidthIndex (movieIndex);
			int textureHeight = OpenGLMovieTextureHeightIndex (movieIndex);
			MakeTextures (textureWidth, textureHeight);
			setTiling (1.0f);
		}
		playMovie = true; // override if default is not to start when ready.
	}
	
	public virtual void FailedMovie(string msg) {
		playMovie=false;
		print("Movie Failed because:" + msg);
	}
	
	// Callback function from iOS
	public virtual void ReadyStream () // only one stream
	{
		// not in this class
	}
	
	// Callback function from iOS
	public virtual void streamReadFail () // only one stream
	{
		// not in this class
	}
	
	// Callback function from iOS
	public virtual void streamPause (bool isPaused) // only one stream
	{
		// not in this class
	}
	
	// Callback function from iOS
	public virtual void StreamFirstFrame ()
	{
		// not in this class
	}
	
	[DllImport("__Internal")]
	protected static extern void OpenGLMovieReleaseIndex (int index);

	public virtual void StopMovie ()
	{
		
		playMovie = false;
		renderer.sharedMaterial.SetColor ("_Color", movieEnd);
		PauseMovie ();
		movieColorSet = 0;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			OpenGLMovieReleaseIndex (movieIndex); // kill and release memory
		}
	}

		
	// Link to the UnityTexture plugin and call the UpdateTexture function there.
	[DllImport("__Internal")]
	protected static extern bool OpenGLMovieUpdateTextureIndex (int index, int textureID);
	
	// Now we can simply call UpdateTexture which gets routed directly into the plugin
	void Update ()
	{
		if (playMovie) {
			
			if (Application.platform != RuntimePlatform.OSXEditor) {
				OpenGLMovieUpdateTextureIndex (movieIndex, m_Texture.GetNativeTextureID ());
			}
			if (movieColorSet == 3) {
				// wait a couple of frames as there seems to be a display lag
				renderer.sharedMaterial.SetColor ("_Color", movieColor);
				renderer.enabled = true;
			}
			movieColorSet++;
		}
	}

		
}
