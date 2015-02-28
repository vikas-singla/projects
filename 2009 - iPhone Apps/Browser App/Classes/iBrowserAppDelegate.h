//
//  iBrowserAppDelegate.h
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
//

#import <UIKit/UIKit.h>

@class iBrowserViewController;

@interface iBrowserAppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
    iBrowserViewController *viewController;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet iBrowserViewController *viewController;

@end

