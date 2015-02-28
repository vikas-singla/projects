//
//  ContactsViewController.m
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "ContactsViewController.h"
#import "ContactsNavigationController.h"

@implementation ContactsViewController

@synthesize tabBarController;

- (void)awakeFromNib {
	
	ContactsNavigationController	*nav = [[ContactsNavigationController alloc] init];
	
	NSMutableArray		*newControllers = [NSMutableArray arrayWithArray: [self.tabBarController viewControllers]];
	int					index = [newControllers indexOfObject: self];
	
	[newControllers replaceObjectAtIndex: index withObject: nav];
	
	nav.tabBarItem = self.tabBarItem;
	
	nav.displayAddBarButton = TRUE;
	
	[self.tabBarController setViewControllers: newControllers animated: NO];
	[nav release];
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

/*
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
}
*/

/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
    [super dealloc];
}


@end
