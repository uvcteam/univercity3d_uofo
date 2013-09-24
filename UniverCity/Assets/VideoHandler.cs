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
	private bool _videoPaused = false;
	
	
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
			if (MoviePlayer.GetComponentInChildren<PlayStreamingMovie>() != null)
			{
            	MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().StopMovie();
				_videoPaused = false;
			}
			
            MoviePlayer.SetActive(false);
            VideoButton.SetActive(false);
            _playVideo = false;
        }
	}
    public void PlayVideoFromURL()
    {
        //Handheld.PlayFullScreenMovie(URL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        _playVideo = !_playVideo;
        if (_playVideo)
        {
			if (!_videoPaused)
            	MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PlayMovie(URL);
			else
				MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().ResumeMovie();
			_videoPaused = false;
            PauseButton.SetActive(true);
            PlayButton.SetActive(false);
        }
        else
        {
            MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PauseMovie();
			_videoPaused = true;
            PlayButton.SetActive(true);
            PauseButton.SetActive(false);
        }
    }
}
