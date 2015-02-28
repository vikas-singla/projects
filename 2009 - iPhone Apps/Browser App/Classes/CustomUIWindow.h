//
//  CustomUIWindow.h
//  iBrowser
//
//  Created by Vikas on 8/7/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class iBrowserViewController;

@interface CustomUIWindow : UIWindow {
	IBOutlet iBrowserViewController* browserViewController;
	
	UIView* previousTouchView;
	CGPoint previousTouchLocation;
	
	CGPoint origTouchLocation;
	UIView* origTouchView;
	
	NSTimer* tapAndHoldTimer;
}

@property (nonatomic, assign) IBOutlet iBrowserViewController* browserViewController;
@property (nonatomic, retain) UIView* previousTouchView;
@property (nonatomic, assign) CGPoint previousTouchLocation;

@end
