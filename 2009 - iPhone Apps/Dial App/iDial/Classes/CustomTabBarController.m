//
//  CustomTabBarController.m
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "CustomTabBarController.h"
#import "KeypadViewController.h"
#import "KeyboardViewController.h"
#import "ContactInformationController.h"

@implementation CustomTabBarController

@synthesize keypadController;
@synthesize keyboardController;
@synthesize contactInfoController;
@synthesize userDefaults;

+ (void)initialize{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSString	*defaultTabIndex = [[NSString alloc] initWithFormat:@"%d", 0];
    NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
								 defaultTabIndex, kIDialTabIndex, nil];
	
    [defaults registerDefaults:appDefaults];
}

/*
 // The designated initializer. Override to perform setup that is required before the view is loaded.
 - (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
 if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
 // Custom initialization
 }
 return self;
 }
 */

/*
 // Implement loadView to create a view hierarchy programmatically, without using a nib.
 - (void)loadView {
 }
 */


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	self.delegate = self;
	
	if(userDefaults == nil)
	{
		userDefaults = [NSUserDefaults standardUserDefaults];
	}
	
	int tabIndex = ((NSString*)[userDefaults stringForKey:kIDialTabIndex]).intValue;
	
	if(tabIndex >= 0 && tabIndex < [self.viewControllers count])
	{	
		self.selectedViewController = [self.viewControllers objectAtIndex:tabIndex];
	}
}

/*
 // Override to allow orientations other than the default portrait orientation.
 - (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
 // Return YES for supported orientations
 return (interfaceOrientation == UIInterfaceOrientationPortrait);
 }
 */
- (void)tabBarController:(UITabBarController *)tabBarController didEndCustomizingViewControllers:(NSArray *)viewControllers changed:(BOOL)changed {
}

- (void)tabBarController:(UITabBarController *)tabBarController didSelectViewController:(UIViewController *)viewController {
	int tabIndex = [self.viewControllers indexOfObject:viewController];
	
	[userDefaults setObject:[[NSString alloc] initWithFormat:@"%d", tabIndex] forKey:kIDialTabIndex];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
    [super dealloc];
}


@end
