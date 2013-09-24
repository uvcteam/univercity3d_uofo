using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class GUITextureMovie : PlayHardwareMovieClassPro {
	
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start ()
	{
		playMovie = false;
		MakeTextures(16,16);
		guiTexture.enabled=false;
	}
	
	protected override void MakeTextures(int textWidth, int textHeight) {
		if(textWidth != currentTextureWidth || textHeight != currentTextureHeight ) {
			currentTextureWidth=textWidth;
			currentTextureHeight=textHeight;
			//Debug.Log("Texture Size " + currentTextureWidth + " " + currentTextureHeight);
			// Create texture that will be updated in the plugin
			m_Texture = new Texture2D (currentTextureWidth, currentTextureHeight, TextureFormat.BGRA32, false);
		
			// Assign texture to the guiTexture
			guiTexture.texture = m_Texture;
			
		} else {
			movieColorSet = 0;
			guiTexture.color= movieEnd;
		}
		
	}
	
	// heightfactor can be used to remove black borders 
	protected override void setTiling(float heightFactor) {
		float width=(float) OpenGLMovieWidthIndex(movieIndex);
		float height=(float) OpenGLMovieHeightIndex(movieIndex);
		Rect inset= guiTexture.pixelInset;
		inset.width=width;
		inset.height=height;
		guiTexture.pixelInset = inset;	
	}
	
	// Now we can simply call UpdateTexture which gets routed directly into the plugin
	void Update ()
	{
		if (playMovie) {
			if (Application.platform != RuntimePlatform.OSXEditor) {
					OpenGLMovieUpdateTextureIndex(movieIndex,m_Texture.GetNativeTextureID ());
			}
			if (movieColorSet == 3) {
				// wait a couple of frames as there seems to be a display lag
				guiTexture.color= movieColor;
				guiTexture.enabled=true;
				print("update");
			}
			movieColorSet++;
		}
	}
}
