//
//  iOSPlugin.m
//  UnityIOSPlugin
//
//  Created by Yevhen Paschenko on 8/17/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "NativePickerPlugin.h"
#import "NativePickerViewController.h"

extern void UnitySendMessage(const char *, const char *, const char *);

@interface NativePickerPlugin () {
    NativePickerViewController* _pickerViewController;
    UIPopoverController* _pickerPopover;
    UIView* _glView;
    UILabel* _debugLabel;
}
@end

@implementation NativePickerPlugin

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
        
        _pickerViewController = [[NativePickerViewController alloc] initPickerViewController];
        _pickerViewController.delegate = self;
    
    }
    
    return self;
}

- (void)showPicker:(int)type withArray:(NSArray*)array andSelectedItem:(int64_t)selectedItem atRect:(CGRect)rect andGameObject:(NSString*)gameObject{
    
    gameObject_ = [gameObject copy];
    
//    if (_debugLabel == nil) {
//        _debugLabel = [[UILabel alloc] initWithFrame:rect];
//        [_debugLabel setBackgroundColor:[UIColor whiteColor]];
//        [_glView addSubview:_debugLabel];
//    } else {
//        [_debugLabel setFrame:rect];
//    }
    
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        __block CGRect pickerFrame = _pickerViewController.view.frame;
        //CGRect thisFrame = self.view.frame;
        int thisHeight = _glView.frame.size.height;
        int thisWidth = _glView.frame.size.width;
        int pickerHeight = _pickerViewController.view.frame.size.height;
        int pickerWidth = _pickerViewController.view.frame.size.width;
        
        UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];
        
        if (UIInterfaceOrientationIsLandscape(orientation)) {
            thisHeight = _glView.frame.size.width;
            thisWidth = _glView.frame.size.height;
        }
        pickerFrame.origin.x = (thisWidth - pickerWidth)/2;
        
        if ([_glView.subviews containsObject:_pickerViewController.view] == NO) {
            _pickerViewController.mode = type;
            _pickerViewController.itemList = array;
            _pickerViewController.selectedItem = selectedItem;
            
            pickerFrame.origin.y = thisHeight;
            _pickerViewController.view.frame = pickerFrame;
            [_glView addSubview:_pickerViewController.view];
        }
        
        
        [UIView animateWithDuration:0.3 delay:0 options:UIViewAnimationOptionCurveEaseOut animations:^{
            pickerFrame.origin.y = thisHeight - pickerHeight;
            _pickerViewController.view.frame = pickerFrame;
        } completion:^(BOOL finished) {
            
        }];


//        __block CGRect frame = _pickerViewController.view.frame;
//        CGRect thisFrame = _glView.frame;
//        
//        if ([_glView.subviews containsObject:_pickerViewController.view] == NO) {
//            _pickerViewController.mode = type;
//            _pickerViewController.itemList = array;
//            _pickerViewController.selectedItem = selectedItem;
//            
//            frame.origin.y = _glView.frame.size.height;
//            _pickerViewController.view.frame = frame;
//            [_glView addSubview:_pickerViewController.view];
//        }
//        
//        [UIView animateWithDuration:0.3 delay:0 options:UIViewAnimationOptionCurveEaseOut animations:^{
//            frame.origin.y = thisFrame.size.height - frame.size.height;
//            _pickerViewController.view.frame = frame;
//        } completion:^(BOOL finished) {
//            
//        }];
    } else {
        if (_pickerPopover == nil) {
            _pickerPopover = [[UIPopoverController alloc] initWithContentViewController:_pickerViewController];
            _pickerPopover.popoverContentSize = _pickerViewController.view.frame.size;
        }
        
        _pickerViewController.mode = type;
        _pickerViewController.itemList = array;
        _pickerViewController.selectedItem = selectedItem;
        
        [_pickerPopover presentPopoverFromRect:rect inView:_glView permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}

- (void)hidePicker {
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        __block CGRect frame = _pickerViewController.view.frame;
        CGRect thisFrame = _glView.frame;
        
        [UIView animateWithDuration:0.3 delay:0 options:UIViewAnimationOptionCurveEaseIn animations:^{
            frame.origin.y = thisFrame.size.height;
            _pickerViewController.view.frame = frame;
        } completion:^(BOOL finished) {
            [_pickerViewController.view removeFromSuperview];
        }];
    }
}

- (BOOL)isIphone {
    return [[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone;
}

- (void)didSelectedValue:(NSString*)value atIndex:(NSInteger)index {
    NSString* selectedIndex = [NSString stringWithFormat:@"%d", index];
    UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "ItemPicked", [selectedIndex cStringUsingEncoding:NSUTF8StringEncoding]);
}

@end

NSString* createNSString (const char* string);
NSArray* createArray(const char** array, int num);
void createPickerPluginIfNeeded();


static NativePickerPlugin* p_plugin = nil;

/*
// Converts C style string to NSString
NSString* createNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

NSArray* createArray(const char** array, int num)
{
    NSMutableArray* retval = [[NSMutableArray alloc] init];
    
    for (int i = 0 ; i < num ; i++)
    {
        [retval addObject:createNSString(array[i])];
    }
    
    return retval;
 }
 */

void createPickerPluginIfNeeded()
{
    if (p_plugin == nil)
        p_plugin = [[NativePickerPlugin alloc] init];
}

void showPicker(int type, const char** items, int numItems, int64_t selectedItem, int x, int y, int w, int h, const char* gameObject)
{
    createPickerPluginIfNeeded();
    
    NSLog(@"selected item: %lli", selectedItem);
    
    float scale = [[UIScreen mainScreen] scale];
    CGRect rect = CGRectMake(x/scale, y/scale, w/scale, h/scale);
    [p_plugin showPicker:type withArray:createArray(items, numItems) andSelectedItem:selectedItem atRect:rect andGameObject:createNSString(gameObject)];
}

void hidePicker()
{
    createPickerPluginIfNeeded();
    
    [p_plugin hidePicker];
}

bool isIphone() {
    createPickerPluginIfNeeded();
    
    return [p_plugin isIphone];
}
