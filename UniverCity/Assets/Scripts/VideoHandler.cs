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
	public int videoHeight = 0;
	public int videoWidth = 0;
	
	
	public void OnEnable()
	{
		if(MoviePlayer != null)
		{
			MoviePlayer.SetActive(true);
            VideoButton.SetActive(true);
			GameObject video = GameObject.Find("TheMovie");
			AdManager adManager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
			video.transform.localScale = new Vector3(1000.0f, 550.0f, 0.0f);
		
		if(video.GetComponent<PlayStreamingMovie>().renderer.material.mainTexture != null)
				GameObject.Find("BusinessAd").GetComponent<BusinessAd>().ScaleVideo(video,
					videoHeight, videoWidth);
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
		Debug.Log ("In PlayVideoFromURL");
        if (_playVideo)
        {
			Debug.Log ("Playing movie.");
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
			Debug.Log ("Pausing movie.");
            MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PauseMovie();
			_videoPaused = true;
            PlayButton.SetActive(true);
            PauseButton.SetActive(false);
        }
    }
}
