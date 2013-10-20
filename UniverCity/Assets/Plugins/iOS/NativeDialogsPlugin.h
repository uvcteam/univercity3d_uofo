//
//  iOSPlugin.h
//  UnityIOSPlugin
//
//  Created by Yevhen Paschenko on 8/17/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NativeDialogsPlugin : NSObject<UIAlertViewDelegate>
{
    int counter_;
    NSString* gameObject_;
}

@end
