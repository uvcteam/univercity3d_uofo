/*
 *  UnityMovieLibInterface.m
 *  MovieTextureLib
 *
 *  Created by Gerard Allan on 09/11/2011.
 *  Copyright 2011 Predictions Software. All rights reserved.
 *
 */
#import <Foundation/Foundation.h>
#import "OpenGLMovie.h"

void UnitySendMessage( const char * obj, const char * method, const char * msg );
void _callUnityMovieInitNSURL(int index,NSURL *fileURL , BOOL audio, float seek);

static int streamID=0;// need to know if stream has change when doing delayed calls

@interface MyOpenGLDelegate : NSObject<OpenGLMovieDelegate> {
    int reTry;
    int index;
}

- (void) forIndex:(int) anindex;

- (void) movieReady;

- (void) performBlock:(void (^)(void))block afterDelay:(NSTimeInterval)delay;

- (void) start;

@end

@implementation MyOpenGLDelegate

- (id) init {
    self = [super init];
    if (self) {
        index=0;
        
    }
    return self;
}

-(void) start {
    reTry=0; // stream retry if read fail 
}
- (void) forIndex:(int) anindex {
    index=anindex;
}

- (void) movieFinished {
    NSString *myIndex=[NSString stringWithFormat:@"%d",index];
    UnitySendMessage("iOS", "FinishedMovie", [myIndex UTF8String]);
}

- (void) movieReady {
    NSString *myIndex=[NSString stringWithFormat:@"%d",index];
    UnitySendMessage("iOS", "ReadyMovie", [myIndex UTF8String]);
}

- (void) movieFailed:(NSString *) msg {
    NSString *myIndex=[NSString stringWithFormat:@"%d %@",index,msg];
    UnitySendMessage("iOS", "FailedMovie", [myIndex UTF8String]);
}


- (void) streamReady {
    NSString *myIndex=[NSString stringWithFormat:@"%d",index];
   UnitySendMessage("iOS", "ReadyStream", [myIndex UTF8String]);
}

- (void) streamPause:(BOOL)isPaused {
    NSString *myPause=[NSString stringWithFormat:@"%d",(int) isPaused];
    UnitySendMessage("iOS", "streamPause", [myPause UTF8String]);
}

-(void) streamReadFail {
    if(reTry == 0) {
        reTry++;
        int local_streamID=streamID;
        [self performBlock:^{ // try this in 2 secs
            if(local_streamID != streamID) return; // stream has changed 
            // no wifi or failed reopen
            if(!OpenGLMovieWiFiAvailable() || !OpenGLMovieStreamReopen()) {
            // have not restart download
            OpenGLMovieStreamFinishOnStall(true);
            UnitySendMessage("iOS", "streamReadFail", "Failed");
            }
        } afterDelay:2];
    } else {
        // have failed again -- tell unity
        OpenGLMovieStreamFinishOnStall(true);
        UnitySendMessage("iOS", "streamReadFail", "Failed");
        reTry=0;// reset reTryLevel
    }
}

- (void)performBlock:(void (^)(void))block afterDelay:(NSTimeInterval)delay
{
	int64_t delta = (int64_t)(1.0e9 * delay);
	dispatch_after(dispatch_time(DISPATCH_TIME_NOW, delta), dispatch_get_main_queue(), block);
}
@end

void UnityMovieInitIndex(int index,char *file, BOOL audio, float seek) {
    NSString *name = [NSString stringWithFormat:@"Data/Raw/%s",file];
    NSURL *fileURL = [[NSBundle mainBundle] URLForResource:name withExtension:nil];
    _callUnityMovieInitNSURL(index,fileURL,audio,seek);
}

// If you have copied a movie to documents directory use this
// Note does not need documents path as it appends filename to docs
// directory.
void UnityMovieInitFromDocumentsIndex(int index, char *file, BOOL audio, float seek) {
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0];
    NSString *name = [NSString stringWithFormat:@"%s",file];
    NSString *filePath = [documentsDirectory stringByAppendingPathComponent:name];
    NSURL *fileURL = [NSURL fileURLWithPath:filePath isDirectory:NO];
    _callUnityMovieInitNSURL(index,fileURL,audio,seek);
}

// Access you movie with the full file path.
void UnityMovieInitFromFilePathIndex(int index, char *file, BOOL audio, float seek) {
    
    NSString *filePath = [NSString stringWithFormat:@"%s",file];
    
    NSURL *fileURL = [NSURL fileURLWithPath:filePath isDirectory:NO];
    _callUnityMovieInitNSURL(index,fileURL,audio,seek);
}

void UnityMovieInitFromDocuments(char *file, BOOL audio, float seek) {
    UnityMovieInitFromDocumentsIndex(0,file,audio,seek);
}
   
void UnityMovieInit(char *file, BOOL audio, float seek) {
    UnityMovieInitIndex(0,file,audio,seek);
}

void UnityMovieInitStream(char *url) {
    int index=0;
    streamID++; // unique stream number
    if(!OpenGLMovieExists(index)) {
        MyOpenGLDelegate *delegate=[[MyOpenGLDelegate alloc] init];
        [delegate forIndex:index];
        OpenGLMovieSetDelegateIndex(index,delegate);
    }
    NSString *fileURL = [NSString stringWithFormat:@"%s",url];
    _UnityMovieInitStream(fileURL);
}

// If contents of url are expected to change need to remove files related to the url
// to avoid corruption/failure to reload.
// Use this function rather than simply deleting returned file as other files store url expected size
void UnityMovieRemoveStreamCache(char *url) {
    NSString *fileURL = [NSString stringWithFormat:@"%s",url];
    OpenGLMovieRemoveStreamCache(fileURL);
}

bool UnityMovieInitPreLoadedStream(char *url) {
    int index=0;
    streamID++; // unique stream number
    if(!OpenGLMovieExists(index)) {
        MyOpenGLDelegate *delegate=[[MyOpenGLDelegate alloc] init];
        [delegate forIndex:index];
        OpenGLMovieSetDelegateIndex(index,delegate);
    }
    NSString *fileURL = [NSString stringWithFormat:@"%s",url];
    NSString *ret=OpenGLMovieInitStreamIfFile(fileURL);
    if(ret==nil) return false;
    return true;
}

NSString *OpenGLMovieInitStreamIfFile(NSString *url);

// Most movie are landscape! but movies recorded on iOS devices can be any direction!
float UnityMoviePreferredRotationIndex(int index)
{
    switch(OpenGLMovieOrientationIndex(index)) {
        case UIImageOrientationDown:
            return 0.0f;
            break;
        case UIImageOrientationUp:
            return 180.0f;
            break;
        case UIImageOrientationLeft:
            return 270.0f;
            break;
        case UIImageOrientationRight:
            return 90.0f;
            break;
        default:return 0.0f;
            break;
    }
    return 0.0f;
}

void OpenGLMovieUpdateTexture(int textureID)
{
    _UnityMovieUpdateTexture(0,textureID);
}

void OpenGLMovieUpdateTextureIndex(int index,int textureID)
{
    _UnityMovieUpdateTexture(index,textureID);
}

void _callUnityMovieInitNSURL(int index,NSURL *fileURL , BOOL audio, float seek) {
    if(!OpenGLMovieExists(index)) {
        MyOpenGLDelegate *delegate=[[MyOpenGLDelegate alloc] init];
        [delegate forIndex:index];
        OpenGLMovieSetDelegateIndex(index,delegate);
    }
    _UnityMovieInitWithNSURL(index,fileURL,audio,seek);
}
