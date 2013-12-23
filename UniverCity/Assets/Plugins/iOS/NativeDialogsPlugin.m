//
//  iOSPlugin.m
//  UnityIOSPlugin
//
//  Created by Yevhen Paschenko on 8/17/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "NativeDialogsPlugin.h"
#import "MBProgressHUD.h"

extern void UnitySendMessage(const char *, const char *, const char *);

@interface NativeDialogsPlugin () {
    UIView* _glView;
}
@end


@implementation NativeDialogsPlugin

#define PROGRESS_ALERT_TAG 0
//#define ALERT_TAG 101

- (id)init {
    self = [super init];
    
    if (self) {
        UIWindow* window = [UIApplication sharedApplication].keyWindow;
        NSArray* views = window.subviews;
        
        for (UIView* view in views) {
            NSString* classString = NSStringFromClass([view class]);
            if ([classString isEqualToString:@"EAGLView"] || [classString isEqualToString:@"MainGLView"] || [classString isEqualToString:@"UnityView"]) {
                NSLog(@"found %@", classString);
                _glView = view;
                break;
            }
        }
        
    }
    
    return self;
}


- (void)showAlert:(UIAlertView*)alert
{
    [alert show];
}

- (void)hideAlert:(UIAlertView*)alert
{
    [alert dismissWithClickedButtonIndex:0 animated:TRUE];   
}

- (void)hideProgress:(UIAlertView*)alert
{
    [alert dismissWithClickedButtonIndex:0 animated:TRUE];
}


- (void)hideMBProgress:(MBProgressHUD*)hud
{
    [MBProgressHUD hideAllHUDsForView:_glView animated:YES];
}


- (int)messageBoxWithCaption:(NSString*)caption andMessage:(NSString*)message andButtons:(NSArray*)buttons andGameObject:(NSString*)gameObject andStyle:(UIAlertViewStyle)style andText1:(NSString*)text1 andText2:(NSString*)text2
{
    gameObject_ = [gameObject copy];
    UIAlertView* alertView = [[UIAlertView alloc] init];
    
    counter_++;
    if(counter_ == 0)
        counter_++;
       
    [alertView setTag:PROGRESS_ALERT_TAG+counter_];
    [alertView setTitle:caption];
    [alertView setMessage:message];
    [alertView setDelegate:self];
    [alertView setAlertViewStyle:style];
    
    if (style == UIAlertViewStyleLoginAndPasswordInput) {
        [[alertView textFieldAtIndex:0] setText:text1];
        [[alertView textFieldAtIndex:1] setText:text2];
    } else if (style == UIAlertViewStylePlainTextInput || style == UIAlertViewStyleSecureTextInput) {
        [[alertView textFieldAtIndex:0] setText:text1];
    }

    for (NSString* button in buttons) {
        [alertView addButtonWithTitle:button];
    }
    
    [self performSelectorOnMainThread:@selector(showAlert:) withObject:alertView waitUntilDone:NO];
    
    return counter_;
}

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{   
    NSString* response = [NSString string];
    
    if (alertView.tag != PROGRESS_ALERT_TAG) {
        NSString* button = [alertView buttonTitleAtIndex:buttonIndex];
        int tag = alertView.tag;
        tag -= PROGRESS_ALERT_TAG;
        
        switch (alertView.alertViewStyle) {
            case UIAlertViewStyleDefault:
                response = [NSString stringWithFormat:@"MessageBox\n%d\n%@", tag, button];
                break;
                
            case UIAlertViewStyleLoginAndPasswordInput: {
                NSString* login = [[alertView textFieldAtIndex:0] text];
                if (login == nil)
                    login = [NSString string];
                
                NSString* password = [[alertView textFieldAtIndex:1] text];
                if (password == nil)
                    password = [NSString string];
                
                response = [NSString stringWithFormat:@"LoginPasswordMessageBox\n%d\n%@\n%@\n%@", tag, login, password, button];
                break;
            }

            case UIAlertViewStylePlainTextInput: {
                NSString* prompt = [[alertView textFieldAtIndex:0] text];
                if (prompt == nil)
                    prompt = [NSString string];
                
                response = [NSString stringWithFormat:@"PromptMessageBox\n%d\n%@\n%@", tag, prompt, button];
                break;
            }
                
            case UIAlertViewStyleSecureTextInput: {
                NSString* prompt = [[alertView textFieldAtIndex:0] text];
                if (prompt == nil)
                    prompt = [NSString string];

                response = [NSString stringWithFormat:@"SecurePromptMessageBox\n%d\n%@\n%@", tag, prompt, button];
                break;
            }
                
            default:
                break;
        }

        UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "MessageBoxButtonClicked", [response cStringUsingEncoding:NSUTF8StringEncoding]);
    }
}


- (UIAlertView*)displayProgressAlertWithCaption:(NSString*)caption andMessage:(NSString *)message {
    UIAlertView* alert = [[UIAlertView alloc] initWithTitle:caption message:message delegate:self cancelButtonTitle:nil otherButtonTitles:nil];
    alert.tag = PROGRESS_ALERT_TAG;
    [self performSelectorOnMainThread:@selector(showAlert:) withObject:alert waitUntilDone:NO];
    
    return alert;
}

- (UIAlertView*)displayProgressWithCaption:(NSString*)caption andMessage:(NSString *)message {
    UIAlertView* alert = [[UIAlertView alloc] initWithTitle:caption message:message delegate:self cancelButtonTitle:nil otherButtonTitles:nil];
    alert.tag = PROGRESS_ALERT_TAG;
    [self performSelectorOnMainThread:@selector(showAlert:) withObject:alert waitUntilDone:NO];

    return alert;
}

- (MBProgressHUD*)displayMBProgressWithCaption:(NSString*)caption andMessage:(NSString *)message {
    MBProgressHUD* hud = [MBProgressHUD showHUDAddedTo:_glView animated:YES];
    hud.userInteractionEnabled = NO;
    hud.labelText = caption;
    hud.detailsLabelText = message;
    return hud;
}


- (void) willPresentAlertView:(UIAlertView *)alertView {
    if (alertView.tag == PROGRESS_ALERT_TAG) {
        UIActivityIndicatorView *indicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
        indicator.center = CGPointMake(alertView.bounds.size.width/2, alertView.bounds.size.height-45);
        [indicator startAnimating];
        [alertView addSubview:indicator];
    }
}

- (void)dismissProgressAlert:(UIAlertView *)alertView {
    [self performSelectorOnMainThread:@selector(hideAlert:) withObject:alertView waitUntilDone:NO];
}

- (void)dismissMBProgress:(MBProgressHUD *)hud {
    [self performSelectorOnMainThread:@selector(hideMBProgress:) withObject:hud waitUntilDone:NO];
}



@end

NSString* nativeDialogsCreateNSString (const char* string);
NSArray* nativeDialogsCreateArray(const char** array, int num);
void nativeDialogsCreatePluginIfNeeded();
int messageBox(const char* caption, const char* message, const char** buttons, int numButtons, const char* gameObject);
void MBprogressDialog(const char* caption, const char* message);
void progressDialog(const char* caption, const char* message);
void hideProgressDialog();

static NativeDialogsPlugin* g_nativeDialogsPlugin = nil;
static UIAlertView* g_nativialogsProgress = nil;
static MBProgressHUD* g_nativeDialogs_mb_progress;

// Converts C style string to NSString
NSString* nativeDialogsCreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

NSArray* nativeDialogsCreateArray(const char** array, int num)
{
    NSMutableArray* retval = [[NSMutableArray alloc] init];
    
    for (int i = 0 ; i < num ; i++)
    {
        [retval addObject:nativeDialogsCreateNSString(array[i])];
    }
    
    return retval;
}

void nativeDialogsCreatePluginIfNeeded()
{
    if (g_nativeDialogsPlugin == nil)
        g_nativeDialogsPlugin = [[NativeDialogsPlugin alloc] init];
}

int messageBox(const char* caption, const char* message, const char** buttons, int numButtons, const char* gameObject)
{
    nativeDialogsCreatePluginIfNeeded();
    
    return [g_nativeDialogsPlugin messageBoxWithCaption:nativeDialogsCreateNSString(caption) 
                                andMessage:nativeDialogsCreateNSString(message)
                                andButtons:nativeDialogsCreateArray(buttons, numButtons)
                             andGameObject:nativeDialogsCreateNSString(gameObject)
                                  andStyle:UIAlertViewStyleDefault
                                  andText1:@""
                                  andText2:@""];
}

int loginPasswordMessageBox(const char* caption, const char* message, const char* login, const char* password, const char** buttons, int numButtons, const char* gameObject)
{
    nativeDialogsCreatePluginIfNeeded();
    
    return [g_nativeDialogsPlugin messageBoxWithCaption:nativeDialogsCreateNSString(caption) 
                                andMessage:nativeDialogsCreateNSString(message)
                                andButtons:nativeDialogsCreateArray(buttons, numButtons)
                             andGameObject:nativeDialogsCreateNSString(gameObject)
                                  andStyle:UIAlertViewStyleLoginAndPasswordInput
                                  andText1:nativeDialogsCreateNSString(login)
                                  andText2:nativeDialogsCreateNSString(password)];
}

int promptMessageBox(const char* caption, const char* message, const char* prompt, const char** buttons, int numButtons, const char* gameObject)
{
    nativeDialogsCreatePluginIfNeeded();
    
    return [g_nativeDialogsPlugin messageBoxWithCaption:nativeDialogsCreateNSString(caption) 
                                andMessage:nativeDialogsCreateNSString(message)
                                andButtons:nativeDialogsCreateArray(buttons, numButtons)
                             andGameObject:nativeDialogsCreateNSString(gameObject)
                                  andStyle:UIAlertViewStylePlainTextInput
                                  andText1:nativeDialogsCreateNSString(prompt)
                                  andText2:@""];
}

int securePromptMessageBox(const char* caption, const char* message, const char* prompt, const char** buttons, int numButtons, const char* gameObject)
{
    nativeDialogsCreatePluginIfNeeded();
    
    return [g_nativeDialogsPlugin messageBoxWithCaption:nativeDialogsCreateNSString(caption) 
                                andMessage:nativeDialogsCreateNSString(message)
                                andButtons:nativeDialogsCreateArray(buttons, numButtons)
                             andGameObject:nativeDialogsCreateNSString(gameObject)
                                  andStyle:UIAlertViewStyleSecureTextInput
                                  andText1:nativeDialogsCreateNSString(prompt)
                                  andText2:@""];
}

void MBProgressDialog(const char* caption, const char* message) {
    nativeDialogsCreatePluginIfNeeded();
    
    if (g_nativeDialogs_mb_progress != nil)
    {
        hideProgressDialog();
    }
    
    g_nativeDialogs_mb_progress = [g_nativeDialogsPlugin displayMBProgressWithCaption:nativeDialogsCreateNSString(caption) andMessage:nativeDialogsCreateNSString(message)];
    [[UIApplication sharedApplication] beginIgnoringInteractionEvents];
}

void progressDialog(const char* caption, const char* message)
{
    nativeDialogsCreatePluginIfNeeded();
 
    if ([[[UIDevice currentDevice] systemVersion] floatValue] < 7.0) {
        if (g_nativialogsProgress != nil)
        {
            hideProgressDialog();
        }
        
        g_nativialogsProgress = [g_nativeDialogsPlugin displayProgressAlertWithCaption:nativeDialogsCreateNSString(caption) andMessage:nativeDialogsCreateNSString(message)];
    } else {
        MBProgressDialog(caption, message);
    }
}

void hideProgressDialog()
{
    nativeDialogsCreatePluginIfNeeded();
    
    if (g_nativialogsProgress != nil)
    {
        [g_nativeDialogsPlugin dismissProgressAlert:g_nativialogsProgress];
        g_nativialogsProgress = nil;
    }
    
    if (g_nativeDialogs_mb_progress != nil)
    {
        [g_nativeDialogsPlugin dismissMBProgress:g_nativeDialogs_mb_progress];
        g_nativeDialogs_mb_progress = nil;

        [[UIApplication sharedApplication] endIgnoringInteractionEvents];
    }
}
