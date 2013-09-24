using UnityEngine;
using System.Collections;

public class PlayFileBaseMovieAlpha : PlayFileBasedMovieDefault {
	
	// we are going to scale the gameObject to account for half the height being alpha data 
	Vector3 scale;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		scale=transform.localScale;
	}

	protected override void Start ()
	{
		base.Start();
	}
	
	public override void FinishedMovie(string str)
	{
		OpenGLMovieRewindIndex(movieIndex);
	}
	
	public override void PlayMovie (string movie)
	{
		base.PlayMovie(movie);
	}
	
	
	// The alpha movie has half the texture for alpha channel info
	protected override void setTiling(float heightFactor) {
		float width=(float) OpenGLMovieWidthIndex(movieIndex);
		float height=(float) OpenGLMovieHeightIndex(movieIndex);
		height=0.5f*height;// half the width is alpha data
		//Debug.Log("tiling Size " + width + " " + height);
		float scaleW=width/currentTextureWidth;
		float scaleH=(height*heightFactor)/currentTextureHeight;
		float remove=(height - height*heightFactor)/currentTextureHeight;
		float heightAmount=remove/2.0f;
		float widthAmount=(1.0f - scaleW);
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(-widthAmount,heightAmount));
		renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(-scaleW,scaleH));
		//Debug.Log("offset " + heightAmount + " scale " + scaleH);
		
		// set shader 
		renderer.sharedMaterial.SetFloat("_MaskAlphaStart", height/currentTextureHeight);
	}
	
	public override void ReadyMovie (string str)
	{
		float ratio=1.5f;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
			int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
			MakeTextures(textureWidth,textureHeight);
			setTiling(1.0f);
			float width=(float) OpenGLMovieWidthIndex(movieIndex);
			float height=(float) OpenGLMovieHeightIndex(movieIndex);
			ratio=width/(0.5f*height);// half the height is alpha data
		} 
		Vector3 newscale=scale;
		newscale[0]*=ratio;
		transform.localScale=newscale;
		playMovie = true;
	}
}
