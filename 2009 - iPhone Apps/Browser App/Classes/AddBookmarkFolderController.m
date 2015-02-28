//
//  AddBookmarkFolderController.m
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "AddBookmarkFolderController.h"


@implementation AddBookmarkFolderController

@synthesize expectedInterfaceOrientation;
@synthesize titleField;
@synthesize bookmarkController;

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

- (void)viewWillAppear:(BOOL)animated {
	
	[titleField becomeFirstResponder];
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	titleField.font = [UIFont systemFontOfSize:17];
}


/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (IBAction) saveFolder: (id) sender {
	if(titleField.text.length > 0)
	{
		if([bookmarkController createFolder:titleField.text])
		{
			[self.parentViewController dismissModalViewControllerAnimated:YES];
		}
		else
		{
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"A folder with that name already exists. Please specify a unique folder name."
														   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
			[alert show];	
			[alert release];
		}
	}
	else
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Please input a valid folder name."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
	}
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (IBAction) cancelNewFolder: (id)sender {
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
	[titleField release];
	
    [super dealloc];
}


@end
