This project remains the property of Predictions Software Ltd Copyright 2012

This project is for evaluation purposes only. You do NOT have permission to distribute or make copies of this project, except as is reasonable for backup purposes only.

You may not sell or give this projected to anyone else. You are not
permitted to decompile or reverse engineer any of the the binary
elements of this project.

If you have been given this project by other than Predictions Software Ltd please contact us. contact@predictions-software.com

Project requires iOS5 and Unity 3.5 (may work on other version as well).

Use the MovieTest scenes. The MovieController gameObject has a script which controls the demos. The demos give a range of projects which run well on various hardware up to 4 simultaneous movies on ipad3. You may need to modify the controller if you are using other hardware to reduce the number of movie files that are played. 

Movie planes are in MovieObjects (max 4 movies can be played at any one time). Inside each object is a TheMovie with a PlayFileBasedMovie.cs (derived PlayHardwareMovieClassPro.cs) attached. This class controls play/pause/resume etc. See the base class as well.

Communication between the  PlayFileBasedMovie.cs  and xcode is via the iOS gameObject. There are two callbacks related to file based movies. ReadyMovie when movie is ready to play and FinishedMovie when the movie is finished. See attached UnityXcodeMovieTexture.png for details of the interactions between Unity and Xcode

I have included a number of movie files to allow you to test. 

The project does very little if run in the editor. It must be built and run  for iOS to show anything interesting. 

See the OpenGLMovie.h and UnityMovieLibInterface.m in the Plugins/IOS director for some internal details on the link with the OpenGLMovieLib.

Progressive streaming,
Note, in general you can only use progressive streaming with wifi. Unless the movie is small. See apples latest policy before submitting a app to the app store.

Example
See the MovieTestStreaming scene. Streaming is Beta, more features will be added.

The movie plane is in MovieObject0. Only one streaming movie can be played at a time, but file based movies can be played alongside streaming movies. Inside MovieObject0 object is a TheMovie with a PlayStreamingMovie.cs (derived PlayHardwareMovieClassPro.cs) attached. This class controls play/pause/resume etc. See the base class as well.

Movies must be "faststart", if not already, movies can be converted to faststart (google is your friend here). 

Streaming movies are likely to fail, or not be available, so you will need to have a recovery strategy is streaming is unavailable. Also streaming movies may pause/resume if data flow does not keep up. 

As with file based movies communication between iOS and Xcode is done iOS gameObject.  See also the UnityXcodeStreamMovieTexture.png which gives the basic flow of control.
 
Any Questions please contact me.  Gerard.Allan@predictions-software.com

