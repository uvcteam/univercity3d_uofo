using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class ForwardiOSMessages : MonoBehaviour {
	public PlayHardwareMovieClassPro[] movie;
	
	void Start() {
		Screen.sleepTimeout = 0;	// do not dim screen
	}
	
	public void FinishedMovie(string msg) {
		int index=Convert.ToInt32(msg);
		movie[index].FinishedMovie(msg);
	}
	
	public void ReadyMovie(string msg) {
		int index=Convert.ToInt32(msg);
		print("ready " + index);
		movie[index].ReadyMovie(msg);
	}
	
	public void ReadyStream(string msg) {
		int index=Convert.ToInt32(msg);
		print("readyStream " + index);
		movie[index].ReadyStream();
	}
	
	public void StreamFirstFrame(string msg) {
		int index=Convert.ToInt32(msg);
		print("StreamFirstFrame " + index);
		movie[index].StreamFirstFrame();
	}
	
	public void streamPause(string msg) {
		int index=Convert.ToInt32(msg);
		bool isPause=(index > 0);
		print("streamPause " + index);
		movie[0].streamPause(isPause);
	}
	
	public void streamReadFail(string msg) {
		print("streamReadFail ");
		movie[0].streamReadFail();
	}
	

}
