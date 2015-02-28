//
//  CustomUIWindow.m
//  iBrowser
//
//  Created by Vikas on 8/7/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "CustomUIWindow.h"
#import "iBrowserViewController.h"

@implementation CustomUIWindow

@synthesize browserViewController;
@synthesize previousTouchView;
@synthesize previousTouchLocation;

- (void) processTapAndHold:(NSTimer *)timer {	
	
	if(previousTouchView != nil)
	{
		[self.browserViewController checkTapAndHoldLocation:self.previousTouchLocation];
	}
	
	if([timer isValid])
	{
		[timer invalidate];
		tapAndHoldTimer = nil;
	}
}

- (BOOL) checkLocationWithinView: (CGPoint) location {
	BOOL result = NO;
	
	if(location.x >= [browserViewController.browserParentView frame].origin.x && location.x <= ([browserViewController.browserParentView frame].origin.x + [browserViewController.browserParentView frame].size.width) &&
	   location.y >= [browserViewController.browserParentView frame].origin.y && location.y <= ([browserViewController.browserParentView frame].origin.y + [browserViewController.browserParentView frame].size.height))
	{
		result = YES;
		
		if(browserViewController.tabViewIsVisible)
		{
			if(location.x >= [browserViewController.tabsSelectionView frame].origin.x && location.x <= ([browserViewController.tabsSelectionView frame].origin.x + [browserViewController.tabsSelectionView frame].size.width) &&
			   location.y >= [browserViewController.tabsSelectionView frame].origin.y && location.y <= ([browserViewController.tabsSelectionView frame].origin.y + [browserViewController.tabsSelectionView frame].size.height))
			{
				result = NO;
			}			
		}
		
		if(browserViewController.miniTabViewIsVisible)
		{
			if(location.x >= [browserViewController.miniTabParentView frame].origin.x && location.x <= ([browserViewController.miniTabParentView frame].origin.x + [browserViewController.miniTabParentView frame].size.width) &&
			   location.y >= [browserViewController.miniTabParentView frame].origin.y && location.y <= ([browserViewController.miniTabParentView frame].origin.y + [browserViewController.miniTabParentView frame].size.height))
			{
				result = NO;
			}			
		}
		
		if(browserViewController.downloadImageViewIsVisible)
		{
			if(location.x >= [browserViewController.downloadImageProgressView frame].origin.x && location.x <= ([browserViewController.downloadImageProgressView frame].origin.x + [browserViewController.downloadImageProgressView frame].size.width) &&
			   location.y >= [browserViewController.downloadImageProgressView frame].origin.y && location.y <= ([browserViewController.downloadImageProgressView frame].origin.y + [browserViewController.downloadImageProgressView frame].size.height))
			{
				result = NO;
			}	
		}
		
		if(browserViewController.findTextViewIsVisible)
		{
			if(location.x >= [browserViewController.findTextView frame].origin.x && location.x <= ([browserViewController.findTextView frame].origin.x + [browserViewController.findTextView frame].size.width) &&
			   location.y >= [browserViewController.findTextView frame].origin.y && location.y <= ([browserViewController.findTextView frame].origin.y + [browserViewController.findTextView frame].size.height))
			{
				result = NO;
			}
		}
	}
	
	return result;
}

- (CGPoint) getAdjustedLocation: (CGPoint) location {
	
	int y =  [browserViewController.browserParentView frame].origin.y;
	
	return CGPointMake(location.x, location.y - [browserViewController.browserParentView frame].origin.y);
}

- (void) sendEvent: (UIEvent*) event {
	NSSet *touches = [event allTouches];
	//NSSet* touches = [event touchesForView:browserViewController.browserParentView];
	
	if([touches count] == 1)
	{
		UITouch* touch = [touches anyObject];
		CGPoint location = [touch locationInView:browserViewController.view];
		
		if(touch.phase == UITouchPhaseBegan)
		{				
			origTouchLocation = location;
			origTouchView = touch.view;
			
			if(tapAndHoldTimer != nil && [tapAndHoldTimer isValid])
			{
				[tapAndHoldTimer invalidate];
				tapAndHoldTimer = nil;
				
				previousTouchView = nil;				
			}
			
			
			if([self checkLocationWithinView:location])
			{
				previousTouchView = touch.view;
				
				self.previousTouchLocation = [self getAdjustedLocation: [touch locationInView:browserViewController.view]];
				
				tapAndHoldTimer = [NSTimer scheduledTimerWithTimeInterval:1.0f target:self selector:@selector(processTapAndHold:) userInfo:nil repeats:NO];
			}					
		}
		else if(touch.phase == UITouchPhaseEnded)
		{
			if(tapAndHoldTimer != nil && [tapAndHoldTimer isValid])
			{
				[tapAndHoldTimer invalidate];
				tapAndHoldTimer = nil;
				
				previousTouchView = nil;				
			}
			
			if([touch.view isEqual:origTouchView] == YES && fabsf(origTouchLocation.y - location.y) <= 10) 
			{
				if(![UIApplication sharedApplication].statusBarHidden && origTouchLocation.y <= 40 && origTouchLocation.x <= (browserViewController.urlView.frame.size.width - 30))
				{
					[browserViewController scrollToTop];
				}
			}			
		}
		else if(touch.phase == UITouchPhaseMoved) {
			if([touch.view isEqual:self.previousTouchView] == NO || fabsf(self.previousTouchLocation.y - location.y) > 10) {
				if(tapAndHoldTimer != nil && [tapAndHoldTimer isValid])
				{
					[tapAndHoldTimer invalidate];
					tapAndHoldTimer = nil;
					
					previousTouchView = nil;					
				}
			}
		}
		else if(touch.phase == UITouchPhaseCancelled) {
			if(tapAndHoldTimer != nil && [tapAndHoldTimer isValid])
			{
				[tapAndHoldTimer invalidate];
				tapAndHoldTimer = nil;
				
				previousTouchView = nil;
			}
		}
	}
	else
	{
		if(tapAndHoldTimer != nil && [tapAndHoldTimer isValid])
		{
			[tapAndHoldTimer invalidate];
			tapAndHoldTimer = nil;
			
			previousTouchView = nil;			
		}
	}
	
	[super sendEvent:event];
}

- (void)dealloc {
    [super dealloc];
}


@end
