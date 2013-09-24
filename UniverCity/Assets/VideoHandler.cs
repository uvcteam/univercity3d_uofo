using UnityEngine;
using System.Collections;

public class VideoHandler : MonoBehaviour 
{
    public GameObject MoviePlayer;
    public string URL;
	
	
	public void OnEnable()
	{
		if(MoviePlayer != null)
		{
			MoviePlayer.SetActive(true);
			PlayVideoFromURL();
		}
	}
	
	public void OnDisable()
	{
		if(MoviePlayer != null)
			MoviePlayer.SetActive(false);
	}
    public void PlayVideoFromURL()
    {
        //Handheld.PlayFullScreenMovie(URL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
		
		MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PlayMovie(URL);
    }
}
