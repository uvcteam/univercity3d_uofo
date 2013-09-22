using UnityEngine;
using System.Collections;

public class VideoHandler : MonoBehaviour 
{
    public GameObject playButton;
    public string URL;

    public void PlayVideoFromURL()
    {
        Handheld.PlayFullScreenMovie(URL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
    }
}
