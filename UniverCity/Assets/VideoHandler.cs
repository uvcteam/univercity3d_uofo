using UnityEngine;
using System.Collections;

public class VideoHandler : MonoBehaviour 
{
    public GameObject MoviePlayer;
    public GameObject VideoButton;
    public GameObject PlayButton;
    public GameObject PauseButton;
    public string URL;
    private bool _playVideo = false;
	
	
	public void OnEnable()
	{
		if(MoviePlayer != null)
		{
			MoviePlayer.SetActive(true);
            VideoButton.SetActive(true);
			PlayVideoFromURL();
		}
	}
	
	public void OnDisable()
	{
        if (MoviePlayer != null)
        {
            MoviePlayer.SetActive(false);
            VideoButton.SetActive(false);
            MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().StopMovie();
            _playVideo = false;
        }
	}
    public void PlayVideoFromURL()
    {
        //Handheld.PlayFullScreenMovie(URL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        _playVideo = !_playVideo;
        if (_playVideo)
        {
            MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PlayMovie(URL);
            PauseButton.SetActive(true);
            PlayButton.SetActive(false);
        }
        else
        {
            MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PauseMovie();
            PlayButton.SetActive(true);
            PauseButton.SetActive(false);
        }
    }
}
