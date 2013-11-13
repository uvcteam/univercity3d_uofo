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
    private bool _autoPlayVideo = true;
	public int videoHeight = 0;
	public int videoWidth = 0;


    public IEnumerator OnPageSwitch()
	{
		if(MoviePlayer != null)
		{

            GameObject video = MoviePlayer.transform.FindChild("TheMovie").gameObject;
			video.transform.localScale = new Vector3(2256.0f, 1256.0f, 0);
			video.GetComponent<MeshRenderer>().enabled = true;
            VideoButton.SetActive(true);


            UnivercityTools.ScaleVideo(video,
                videoHeight, videoWidth);
			
            if (_autoPlayVideo)
            {
                _autoPlayVideo = false;

                VirtualMallCreature creature = GameObject.Find("Creature").GetComponent<VirtualMallCreature>();

                creature.Turtle.SetActive(true);
                while(creature.IsDone == false)
                    yield return null;
                creature.Turtle.SetActive(false);
                PlayVideoFromURL();
            }
            else
            {
                PlayButton.SetActive(true);
                PauseButton.SetActive(false);
            }

            if (Application.platform == RuntimePlatform.WindowsEditor)
                transform.FindChild("Texture").gameObject.SetActive(true);
		}
	}

    public void OnPageLeave()
	{
        if (MoviePlayer != null)
        {
			if (MoviePlayer.GetComponentInChildren<PlayStreamingMovie>() != null)
			{
            	MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PauseMovie();
				_videoPaused = false;
			}
			
			Debug.Log("Moved");
            _playVideo = false;
			MoviePlayer.transform.position = new Vector3(100.0f, transform.position.y, MoviePlayer.transform.position.z);
        }
	}
    public void PlayVideoFromURL()
    {
       
        _playVideo = !_playVideo;

        if (_playVideo)
        {
            if(Application.platform == RuntimePlatform.Android)
                Handheld.PlayFullScreenMovie(URL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("Playing movie.");
                if (!_videoPaused)
                {
                    MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PlayMovie(URL);
                }
                else
                    MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().ResumeMovie();
                _videoPaused = false;
                PauseButton.SetActive(true);
                PlayButton.SetActive(false);
            }
        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().PauseMovie();
			_videoPaused = true;
            PlayButton.SetActive(true);
            PauseButton.SetActive(false);
        }
    }
	
	public void OnDestroy()
	{
		if (MoviePlayer != null && MoviePlayer.GetComponentInChildren<PlayStreamingMovie>() != null)
			MoviePlayer.GetComponentInChildren<PlayStreamingMovie>().StopMovie();
	}

    public void Update()
    {
        if (MoviePlayer != null)
        {
            GameObject businessAd = GameObject.Find("BusinessAd");
            GameObject centerPage = businessAd.GetComponent<BusinessAd>().pageGrid.GetComponent<UICenterOnChild>().centeredObject;

            if (centerPage == gameObject)
            {
                MoviePlayer.transform.position = new Vector3(transform.position.x, transform.position.y, MoviePlayer.transform.position.z); ;
            }
        }
    }

    void OnEnable()
    {
        if (MoviePlayer != null)
		{
            MoviePlayer.transform.FindChild("TheMovie").GetComponent<MeshRenderer>().enabled = true;
		}
    }

    void OnDisable()
    {
        if (MoviePlayer != null)
		{
			MoviePlayer.transform.FindChild("TheMovie").GetComponent<MeshRenderer>().enabled = false;
		}
    }
	
	void Awake()
	{
		if (MoviePlayer != null)
		{
            MoviePlayer.transform.FindChild("TheMovie").GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
