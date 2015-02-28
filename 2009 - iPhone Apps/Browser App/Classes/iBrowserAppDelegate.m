//
//  iBrowserAppDelegate.m
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
//

#import "iBrowserAppDelegate.h"
#import "iBrowserViewController.h"
#import "SettingsConstants.h"

@implementation iBrowserAppDelegate

@synthesize window;
@synthesize viewController;


- (void)applicationDidFinishLaunching:(UIApplication *)application {    
    
	[[UIApplication sharedApplication] setStatusBarHidden:YES animated: NO];
	
    // Override point for customization after app launch    
    [window addSubview:viewController.view];
    [window makeKeyAndVisible];
	
	NSURLCache *sharedCache = [[NSURLCache alloc] initWithMemoryCapacity:4194304 
                                                            diskCapacity:41943040 
                                                                diskPath:nil];
    [NSURLCache setSharedURLCache:sharedCache];
    [sharedCache release];
	
	
	NSUserDefaults* userDefaults = [NSUserDefaults standardUserDefaults];
	NSString* newVal = [NSString stringWithFormat:@"%d", [[userDefaults stringForKey:kIbrowserDiagnosticVal] intValue] + 1];
	[userDefaults setObject:newVal  forKey:kIbrowserDiagnosticVal];
	[userDefaults synchronize];
}


- (void)dealloc {
    [viewController release];
    [window release];
    [super dealloc];
}


@end
