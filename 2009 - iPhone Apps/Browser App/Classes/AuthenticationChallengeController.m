//
//  AuthenticationChallengeController.m
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "AuthenticationChallengeController.h"
#import "iBrowserViewController.h"

@implementation AuthenticationChallengeController

@synthesize browserController;
@synthesize expectedInterfaceOrientation;
@synthesize nameField;
@synthesize passwordField;

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
	
	nameField.font = [UIFont systemFontOfSize:17];
	passwordField.font = [UIFont systemFontOfSize:17];
}


/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (IBAction) logIn: (id) sender {
	if(nameField.text.length > 0 && passwordField.text.length > 0)
	{
		NSString* username = [NSString stringWithString:nameField.text];
		NSString* passw = [NSString stringWithString:passwordField.text];
		
		[browserController useLoginInfo:username :passw];
				
		[self.parentViewController dismissModalViewControllerAnimated:YES];
	}
	else
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Please input a valid name and password."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
	}
}

- (void)viewWillAppear:(BOOL)animated {
	
	[nameField becomeFirstResponder];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (IBAction) cancelAuthentication: (id)sender {
	[browserController cancelAuthentication];
	
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
	[nameField release];
	[passwordField release];
	
    [super dealloc];
}


@end
