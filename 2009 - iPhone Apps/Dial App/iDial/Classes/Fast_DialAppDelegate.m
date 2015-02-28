//
//  Fast_DialAppDelegate.m
//  Fast Dial
//
//  Created by Vikas on 12/13/08.
//  Copyright __MyCompanyName__ 2008. All rights reserved.
//

#import "Fast_DialAppDelegate.h"
#import "ContactInformationController.h"

@implementation Fast_DialAppDelegate

@synthesize window;
@synthesize tabBarController;


- (void) loadContactInformationController: (id)anObject {
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
	
	ContactInformationController *controller = [ContactInformationController getInstance];
	
	[controller loadData];
	
	[pool release];
}

- (void)applicationDidFinishLaunching:(UIApplication *)application {
    
    // Add the tab bar controller's current view as a subview of the window
    [window addSubview:tabBarController.view];
	
	ContactInformationController *controller = [ContactInformationController getInstance];
	
	[NSThread detachNewThreadSelector:@selector(loadContactInformationController:) toTarget:self withObject:nil];
}


/*
// Optional UITabBarControllerDelegate method
- (void)tabBarController:(UITabBarController *)tabBarController didSelectViewController:(UIViewController *)viewController {
}
*/

/*
// Optional UITabBarControllerDelegate method
- (void)tabBarController:(UITabBarController *)tabBarController didEndCustomizingViewControllers:(NSArray *)viewControllers changed:(BOOL)changed {
}
*/


- (void)dealloc {
    [tabBarController release];
    [window release];
    [super dealloc];
}

@end

