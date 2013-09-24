//
//  OpenGLMovie.h
//  OpenGLMovieLib
//
//  Created by Gerard Allan on 10/04/2012.
//  Copyright (c) 2012 Predictions Software. All rights reserved.
//


// Use a delegate if movieFinished callback required.
// See setDelegate to assign a delegate.
@protocol OpenGLMovieDelegate <NSObject>
// called when movie has processed last frame.

@optional
// Called at End of Movie, use to change the movie file. or rewind
-(void) movieFinished;
// Use to signal an ayschronous movie start is ready.
-(void) movieReady;

// These will be called as well, if they exist, with the index indicating which movie was responsible
-(void) movieFinished: (int) index;
-(void) movieReady: (int) index;

// called when movie, streaming movie fails, with a reason - if known
-(void) movieFailed:(NSString *) reason;

// This will be called as well, if it exist, with the index indicating which movie was responsible
// called when movie, streaming movie fails, with a reason - if known
-(void) movieFailed:(NSString *) reason index: (int) index;

// STREAM DELEGATE FUNCTIONS

// Filename with path (defaults to NSTemporaryDirectory) 
-(void) streamFileName:(NSString *) fileNamePath;

//Stream Finished download -- after this call, file is complete. This file could be copied etc.
-(void) streamSuccess: (NSString *) fileNamePath;; 

// called when a stream movie that is playing is paused/resumed because data is not avaiable/become available again
// Only called if a frame was required and could not be delivered
// if movie is on pause already this is not called
-(void) streamPause:(BOOL) isPaused;

// called when a stream is starting to play for first time
-(void) streamReady;

// called when a stream fails while reading -- give opportunity to reopen
// Can happen if wifi lost for short time or server closes connection etc.. etc.. 
-(void) streamReadFail;

// called when a stream has been initialised and has first frame ready for display.
// There can be some delay between this and the actual playing of the movie
// as more data needs to be loaded before we can start, without a hardware stall
-(void) streamFirstFrame;

@end

// Movie Functions. There are two versions of each of these functions
//
// OpenGLMovieXXXX(...)  -- use these if you are only going to have a single movie in your app 
//                          Is a short cut to OpenGLMovieXXXXIndex(0, ....)
//
// OpenGLMovieXXXXIndex(int index, ....) -- where index  can be 0 to 3. 
//
// Note there are no checks on index range or indeed if movie has been initalised when these
// functions are called. 


#if defined __cplusplus
extern "C" {
#endif
    // For streaming movies Apple does not like long streaming sessions without WiFi. Check is WiFi is available
    BOOL OpenGLMovieWiFiAvailable(void);
    
    // Start a streaming movie (must be a faststart movie). 
    void OpenGLMovieInitStream(NSString *url);
    
    // Reopen failed stream -- try to continue
    BOOL OpenGLMovieStreamReopen(void);
    
    // If streaming stalls use stall to signal as end of movie.
    // Can use this, if read fails, to play as much of movie as downloaded and then finish.
    void OpenGLMovieStreamFinishOnStall(BOOL finishOnStall);
    
    // Start new movie with a remote url, only if we have already completely written this file to disk already
    // returns the filename used on success, nil on failure.
    // Use this to test if can run url with copy of streamed file saved to disk.
    extern NSString *OpenGLMovieInitStreamIfFile(NSString *url);
    
    // Remove files associated with the url.
    // If files are not removed will continue loading where streaming finished last time.
    
    // If contents of url are expected to change need to remove files related to the url
    // to avoid corruption/failure to reload. 
    // Use this function rather than simply deleting returned file as other files store url expected size.
    extern void OpenGLMovieRemoveStreamCache(NSString *url);
    
    // Set EAGLView context to allow hardware texture copying.
    extern void OpenGLMovieFastTexture(EAGLContext *context);
    
    // Initialise movie accepts any movie format iphone decodes
    // -- movie will play (at least sound will start) as soon as movie is ready to play
    // Use OpenGLMoviePause after this call if immediate startup is not required
    // Assumes file is in Resources
    extern void OpenGLMovieInit(NSString *file);
    
    // Initialise movie accepts any movie format iphone decodes
    // -- movie will play (at least sound will start) as soon as movie is ready to play
    // Use OpenGLMoviePause after this call if immediate startup is not required
    // Can only use local files
    extern void OpenGLMovieInitWithNSURL(NSURL *fileURL);
    
    // Same as OpenGLMovieInit but start movie seek seconds into movie
    extern void OpenGLMovieInitAtTime(NSString *file, float seek);
    
    // Same as OpenGLMovieInitWithNSURL but start movie seek seconds into movie
    extern void OpenGLMovieInitAtTimeWithNSURL(NSURL *file, float seek);

    // Test if movie is actually playing
    // Movie can be paused or waiting for movie to load and start playing
    // The playing state responds to asychronous events so can change at any time.
    extern BOOL OpenGLMovieIsPlaying(void);
    
    // Pause/Resume calls increment/decrement the pauseLevel. Only when pauseLevel is 0 (or less!) will 
    // the movie be active. Note that even with a 0 pauseLevel the movie may not have started 
    // or finished so that no movie will be playing.
    // To test is something is playing see OpenGLMovieIsPlaying.
    extern int OpenGLMoviePauseLevel(void);
    
    // Pause the movie (stop sound)  The movie texture still exists and can be diplayed as a still frame.
    // This increments a pauseLevel, so that for every pause a corresponding resume is required to 
    // decrement the pauseLevel. Only when pauseLevel is zero will movie actually play/resume.
    extern void OpenGLMoviePause(void);
    
    // When set the movie will display as a still frame when paused (default false).
    // Note, that when movie is finished it is "paused" as well even though the PauseLevel is zero. 
    // Even with immediate rewind there can be 
    // a short period when movie is "paused" and may flash on/off unless this is set true;
    extern void OpenGLMovieDisplayOnPause(BOOL set);
    
    // Resume paused movie. At least decrement the pauseLevel and if zero resume.
    extern float OpenGLMovieResume(void);
    
    // Set the movie delegate that will handle MovieFinished
    extern void OpenGLMovieSetDelegate(id<OpenGLMovieDelegate> del);
    
    // Set volume of this movie.
    // The Volume is normally set by app user. 
    // This function should only be used where you have more than one movie running and 
    // need to set the relative volume of each.
    extern void OpenGLMovieVolume(float volume);
    
    // The movie is positioned at 0,0 on a power of two texture.
    // To size the textureCoords the size of both the texture and movie are required.
    // Note, if that are black bands (top/bottom) in your movie these will be included in height and will show unless 
    // adjustment is made to your texcoords to remove them
    
    // Movie width
    extern int OpenGLMovieWidth(void);
    // Movie height
    extern int OpenGLMovieHeight(void);
    // The width of the texture used by this movie.
    extern int OpenGLMovieTextureWidth(void);
    // The height of the texture used by this movie.
    extern int OpenGLMovieTextureHeight(void);
    
    // The duration of this movie in seconds. -- Duration is only available after movie has loaded and can play at least one frame
    extern float OpenGLMovieDuration(void);
    
    // The current play time/position of this movie in seconds. Including seekToTime if used.
    extern float OpenGLMovieCurrentPosition(void);
    
    // Called to update the movie texture. Should be called every frame. The texture will bind and will if
    // required updated to the next required movie frame.
    // Returns true if current frame is valid.
    extern BOOL OpenGLMovieUpdate(void);
    
    // Rewind the movie to the start and play -- If Init was AtTime, will rewind to seek time.
    extern void OpenGLMovieRewind(void);
    
    // Release, stop movie, release memory.
    extern void OpenGLMovieRelease(void);
    
    // Is this a stream/or originally started as a stream
    extern BOOL OpenGLMovieIsStreaming(void);
    
    // Index > 0 only available in Pro Version
    // Equivalent functions for movies at index 0 to 3.
    extern void OpenGLMovieInitIndex(int index, NSString *file);
    extern void OpenGLMovieInitAtTimeIndex(int index, NSString *file,float seek);
    extern void OpenGLMovieInitWithNSURLIndex(int index, NSURL *file);
    extern void OpenGLMovieInitAtTimeWithNSURLIndex(int index, NSURL *file,float seek);

    extern BOOL OpenGLMovieIsPlayingIndex(int index);
    extern void OpenGLMoviePauseIndex(int index);
    extern int OpenGLMoviePauseLevelIndex(int index);
    extern float OpenGLMovieResumeIndex(int index);
    extern float OpenGLMovieHardwareResumeIndex(int index);
    extern void OpenGLMovieSetDelegateIndex(int index,id<OpenGLMovieDelegate> del);
    extern void OpenGLMovieVolumeIndex(int index,float volume);
    extern int OpenGLMovieTextureWidthIndex(int index);
    extern int OpenGLMovieTextureHeightIndex(int index);
    extern int OpenGLMovieWidthIndex(int index);
    extern int OpenGLMovieHeightIndex(int index);
    extern float OpenGLMovieDurationIndex(int index);
    extern float OpenGLMovieCurrentPositionIndex(int index);

    extern BOOL OpenGLMovieUpdateIndex(int index);
    extern void OpenGLMovieRewindIndex(int index);
    extern void OpenGLMovieDisplayOnPauseIndex(int index,BOOL set);
    extern void OpenGLMovieReleaseIndex(int index);

    // returns the internal texture id that movie uses
    extern GLuint OpenGLMovieTextureID(void);
    extern GLuint OpenGLMovieTextureIDIndex(int index);
    extern BOOL OpenGLMovieIsStreamingIndex(int index);
    
    // Used by Unity3D
    // Initialise Unity Movie (of Unity3D plugin) using index (0,1,2,3) Only up to 4 movies are possible.
    // bool audio, if true play (and sync movie to) movie track. If false play movie based on time elapsed
    // Start the movie at seek time seconds into track.
    extern void _UnityMovieInit(int index, char *file, BOOL audio, float seek);
    
    extern void _UnityMovieInitWithNSURL(int index, NSURL *file, BOOL audio, float seek);
    
    extern void _UnityMovieInitStream(NSString *url);
    
    // called to update the texture associated with textureID. Might not update if does not need new texture contents.
    extern BOOL _UnityMovieUpdateTexture(int index, int textureID);
    // sync movie -- return after all currently running operations are stopped. 
    extern void OpenGLMovieSync(int index);
    // has this movie slot already been initialized
    extern BOOL OpenGLMovieExists(int index);

#if defined __cplusplus
}
#endif
