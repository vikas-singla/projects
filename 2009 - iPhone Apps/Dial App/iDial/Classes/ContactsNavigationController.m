//
//  ContactsNavigationController.m
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "ContactsNavigationController.h"
#import "ContactInformationController.h"

@implementation ContactsNavigationController

@synthesize displayAddBarButton;

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
	self.delegate = self;
	self.peoplePickerDelegate = self;
    [super viewDidLoad];
}

- (void)viewWillAppear:(BOOL)animated; {
	// Custom initialization
	
	ContactInformationController *contactInfoController = nil;
	
	if(contactInfoController == nil) {
		contactInfoController = [ContactInformationController getInstance];
		[contactInfoController registerABChangeCallback];
	}
	
	[super viewWillAppear:animated];
}

/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (BOOL)peoplePickerNavigationController:(ABPeoplePickerNavigationController *)peoplePicker shouldContinueAfterSelectingPerson:(ABRecordRef)person {
	ABPersonViewController *personViewController = [[ABPersonViewController alloc] init];
	personViewController.displayedPerson = person;
	personViewController.allowsEditing = YES;
	
	[self pushViewController:personViewController animated:TRUE];
	
	personViewController.personViewDelegate = self;
	
	[personViewController release];
	
	return NO;
}

- (BOOL)peoplePickerNavigationController:(ABPeoplePickerNavigationController *)peoplePicker shouldContinueAfterSelectingPerson:(ABRecordRef)person property:(ABPropertyID)property identifier:(ABMultiValueIdentifier)identifier {
	return NO;
}

- (void)peoplePickerNavigationControllerDidCancel:(ABPeoplePickerNavigationController *)peoplePicker {
}

- (BOOL)personViewController:(ABPersonViewController *)personViewController shouldPerformDefaultActionForPerson:(ABRecordRef)person property:(ABPropertyID)property identifier:(ABMultiValueIdentifier)identifierForValue {
	return YES;
}

- (void)navigationController:(UINavigationController *)navigationController willShowViewController:(UIViewController *)viewController animated:(BOOL)animated {
	
	if([viewController isMemberOfClass: [ABPersonViewController class]]) {
		displayAddBarButton = FALSE;
		
		UIBarButtonItem* backItem = [[UIBarButtonItem alloc] initWithTitle:@"View Contacts" style:UIBarButtonItemStyleBordered target:self action:@selector(cancelPersonViewController:)];									 
		
		viewController.navigationItem.leftBarButtonItem = backItem;
		
		[backItem release];
	}
	else if([viewController isMemberOfClass:[ABNewPersonViewController class]]) {
		displayAddBarButton = FALSE;
	}
	else if([viewController isMemberOfClass:[ABUnknownPersonViewController class]]) {
		displayAddBarButton = FALSE;
	}
	
	[viewController.view setNeedsDisplay];
	
	if(displayAddBarButton) {
		// provide my own Edit button to dismiss the keyboard
		UIBarButtonItem* saveItem = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemAdd
																					  target:self action:@selector(addContactAction:)];
		
		viewController.navigationItem.rightBarButtonItem = saveItem;
		
		[saveItem release];
	}
}

- (void)newPersonViewController:(ABNewPersonViewController *)newPersonViewController didCompleteWithNewPerson:(ABRecordRef)person {

	displayAddBarButton = true;
	
	[self popViewControllerAnimated:YES];
}

- (void)cancelPersonViewController:(id)sender {
	displayAddBarButton = true;
	
	[self popViewControllerAnimated:YES];
}

- (void)addContactAction:(id)sender
{
	ABNewPersonViewController* newPersonController = [[ABNewPersonViewController alloc] init];
	
	[self pushViewController:newPersonController animated:TRUE];
	
	newPersonController.newPersonViewDelegate = self;
	
	[newPersonController release];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
    [super dealloc];
}


@end
