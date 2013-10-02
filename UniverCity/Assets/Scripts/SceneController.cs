using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class SceneController : MonoBehaviour
{
	public PlayHardwareMovieClassPro[] movieClass;
	public string[] movieName;
	public float[] seekTime;
	public bool testStreaming;
	public bool delayStart;
	
	// Use this for initialization
	void Start ()
	{
		if (delayStart) {
			StartCoroutine (DelayStart ());
		} else {
			for (int i=0; i < movieClass.Length; i++) {
				StartCoroutine (RunTest (i, movieName [i], seekTime [i]));
			}
		}
	}
	
	IEnumerator DelayStart ()
	{
		yield return new WaitForSeconds(20.0f);
		for (int i=0; i < movieClass.Length; i++) {
			StartCoroutine (RunTest (i, movieName [i], seekTime [i]));
		}
	}
	
	void Print (string str)
	{
		print (str);
		guiText.text = str;
	}

	[DllImport("__Internal")]
	private static extern void UnityMovieRemoveStreamCache(string url);
		
	IEnumerator RunTest (int index, string movie, float seek)
	{	
		float waitTime = 13.0f;
		if (testStreaming)
			waitTime = 30.0f;
		yield return new WaitForSeconds(waitTime*index);
		print ("Play " + movie);
		// stream if the right class..... as PlayMovieAt is not supported in streaming movies (yet).
		if (typeof(PlayStreamingMovie) == movieClass [index].GetType ()) {
			movieClass [index].PlayMovie (movie);
		} else {
			movieClass [index].PlayMovieAt (movie, seek);
		}
		if (testStreaming) {
			yield return new WaitForSeconds(20.0f);
			print ("pause " + movie);
			movieClass [index].PauseMovie ();
			yield return new WaitForSeconds(3.0f);
			// resume
			print ("resume " + movie);
			movieClass [index].ResumeMovie ();
			yield return new WaitForSeconds(2.0f);
			print ("pause " + movie);
			movieClass [index].PauseMovie ();
			if (typeof(PlayStreamingMovie) == movieClass [index].GetType ()) {
				UnityMovieRemoveStreamCache(movie);
			} 
			if (movieClass.Length - 1 == index) { // last movie
				yield return new WaitForSeconds(3.0f);
				Start();
			}
			/*if (movieClass.Length - 1 == index) { // last movie
				yield return new WaitForSeconds(3.0f);
				// resume
				print ("resume " + movie);
				movieClass [index].ResumeMovie ();
			}*/
		}
	}
    
	// Update is called once per frame
	//void Update () {
	//}
}
