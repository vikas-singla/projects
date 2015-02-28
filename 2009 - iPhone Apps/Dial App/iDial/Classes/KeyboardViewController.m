//
//  KeyboardViewController.m
//  Fast Dial
//
//  Created by Vikas on 12/17/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "KeyboardViewController.h"
#import "ContactInformationController.h"
#import "ContactTableViewCell.h"

static NSString* syncKeyboardText = @"KeyboardSyncText";

@implementation KeyboardViewController

@synthesize activityImageView;
@synthesize activityIndicator;

@synthesize mySearchBar;
@synthesize contactListView;
@synthesize contactInfoController;
@synthesize callMechanismInstance;
@synthesize userDefaults;
@synthesize searchBarText;
@synthesize contactListViewData;

+ (void)initialize{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSString	*defaultDigitText = @"";
    NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
								 defaultDigitText, kIDIalKeyboardViewDigitText, nil];
	
    [defaults registerDefaults:appDefaults];
}

- (void)keyboardWillShow:(NSNotification *)notif
{
    // The keyboard will be shown. If the user is editing the author, adjust the display so that the
    // author field will not be covered by the keyboard.
    CGRect contactListFrame = contactListView.frame;
	contactListFrame.size.height = contactListFrame.size.height - 170;
	contactListView.frame = contactListFrame;
}

- (void)keyboardWillHide:(NSNotification *)notif
{
    // The keyboard will be shown. If the user is editing the author, adjust the display so that the
    // author field will not be covered by the keyboard.
    CGRect contactListFrame = contactListView.frame;
	contactListFrame.size.height = contactListFrame.size.height + 170;
	contactListView.frame = contactListFrame;
}

- (void) showActivityIndicator {
	
	if(activityImageView == nil)
	{
		UIImage* activityImage = [UIImage imageNamed:@"ActivityIndicatorBackground.png"];
		activityImageView = [[UIImageView alloc] initWithImage:activityImage];
		activityImageView.alpha = 0.46;
		
		[self.view addSubview:activityImageView];
	}
	
	if(activityIndicator == nil)
	{
		activityIndicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
		activityIndicator.hidesWhenStopped = YES;
		
		activityIndicator.frame = CGRectMake(142, 188, 37, 37);
		
		[self.view addSubview:activityIndicator];
	}
	
	activityImageView.frame = CGRectMake(122, 170, 75, 75);
	
	[activityIndicator startAnimating];
}

- (void) hideActivityIndicator {
	activityImageView.frame = CGRectMake(-124, 170, 75, 75);
	
	[activityIndicator stopAnimating];
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
	
	// don't get in the way of user typing
	mySearchBar.autocorrectionType = UITextAutocorrectionTypeNo;
	mySearchBar.autocapitalizationType = UITextAutocapitalizationTypeNone;
	mySearchBar.showsCancelButton = NO;
	mySearchBar.keyboardType = UIKeyboardTypeNamePhonePad;
	
	if(callMechanismInstance == nil) {
		callMechanismInstance = [CallMechanism getInstance];
	}
	
	if(userDefaults == nil)
	{
		userDefaults = [NSUserDefaults standardUserDefaults];
	}
	
	contactInfoController = [ContactInformationController getInstance];
	
	contactListView.dataSource = self;
	contactListView.delegate = self;
	
	[self showActivityIndicator];
}

- (void)viewWillDisappear:(BOOL)animated
{
    // unregister for keyboard notifications while not visible.
    [[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillShowNotification object:nil]; 
	
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillHideNotification object:nil]; 
}

- (void)viewWillAppear:(BOOL)animated {
	
	if(contactInfoController != nil && !contactInfoController.loadingContacts)
	{
		[contactInfoController resetFilteredContactListToMasterList];
		
		if(mySearchBar.text != nil && [mySearchBar.text length] > 0)
		{
			[NSThread detachNewThreadSelector:@selector(performTextSearch:) toTarget:self withObject: [NSArray arrayWithObjects: [[NSString alloc] initWithString:mySearchBar.text],
																									   [[NSString alloc] initWithString:mySearchBar.text], nil]];
		}
		else if(contactListView != nil)
		{
			//[contactListView reloadData];
			[self refreshContactListView:nil];
		}
	}	
	else
	{
		[self refreshContactListView:nil];
	}
	
	// watch the keyboard so we can adjust the user interface if necessary.
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillShow:) 
												 name:UIKeyboardWillShowNotification object:self.view.window]; 
	
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillHide:) 
												 name:UIKeyboardWillHideNotification object:self.view.window]; 
	
	[mySearchBar becomeFirstResponder];
	
	[super viewWillAppear:animated];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}

- (void) timerFiredEvent:(NSTimer *)timer {
	
	if(!contactInfoController.loadingContacts)
	{
		[timer invalidate];
		
		[contactInfoController resetFilteredContactListToMasterList];
		
		[self hideActivityIndicator];
		
		if(searchBarText != nil)
		{
			[NSThread detachNewThreadSelector:@selector(performTextSearch:) toTarget:self withObject: [NSArray arrayWithObjects: [[NSString alloc] initWithString:searchBarText],
																									   [[NSString alloc] initWithString:searchBarText], nil]];
		}
		
		[contactListView reloadData];
	}
}


- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	if(contactListViewData != nil)
	{
		[contactListViewData release];
		contactListViewData = nil;
	}
	
	if(contactInfoController.loadingContacts)
	{
		[self showActivityIndicator];
		
		[NSTimer scheduledTimerWithTimeInterval:0.5 target:self selector:@selector(timerFiredEvent:) userInfo:nil repeats:YES];
		
		return 0;
	}
	else
	{
		[self hideActivityIndicator];
	}
	
	contactListViewData = [[NSArray alloc] initWithArray:contactInfoController.filteredABContactList];
	
    return [contactListViewData count];
}


// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
	
    ContactTableViewCell *cell = (ContactTableViewCell *)[contactListView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[ContactTableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];		
		cell.accessoryType = UITableViewCellAccessoryNone;
		
		// Set up the cell...
		cell.backgroundColor = [UIColor clearColor];
    }
	else
	{
		[cell reInitialize];
	}
	
	if(([indexPath row] % 2) == 1)
	{
		[cell setGrayColor];
		cell.contentView.backgroundColor = [UIColor colorWithRed:216.0/255 green:216.0/255 blue:216.0/255 alpha:1.0];
		cell.backgroundView.backgroundColor = [UIColor colorWithRed:216.0/255 green:216.0/255 blue:216.0/255 alpha:1.0];
		cell.accessoryView.backgroundColor = [UIColor colorWithRed:216.0/255 green:216.0/255 blue:216.0/255 alpha:1.0];
	}
	else
	{
		[cell setWhiteColor];
		cell.accessoryView.backgroundColor = [UIColor whiteColor];
	}
    
	ContactInformationWrapper* contactInfo = [contactListViewData objectAtIndex:indexPath.row];
	
	[cell setContactInformationWrapper:contactInfo];
	
    return cell;
}

- (void)dialButtonTapped:(id)sender event:(id)event
{
	NSSet *touches = [event allTouches];
	UITouch *touch = [touches anyObject];
	CGPoint currentTouchPosition = [touch locationInView:self.contactListView];
	NSIndexPath *indexPath = [contactListView indexPathForRowAtPoint: currentTouchPosition];
	if (indexPath != nil)
	{
		[self tableView: self.contactListView accessoryButtonTappedForRowWithIndexPath: indexPath];
	}
}


- (void)tableView:(UITableView *)tableView accessoryButtonTappedForRowWithIndexPath:(NSIndexPath *)indexPath
{	
	ContactInformationWrapper* contactInfo = [contactListViewData objectAtIndex:indexPath.row];
	
	[userDefaults setObject:@"" forKey:kIDIalKeyboardViewDigitText];
	
	[callMechanismInstance callNumberWithInfo: contactInfo];
	
	mySearchBar.text = @"";
}


- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
	ContactInformationWrapper* contactInfo = [contactListViewData objectAtIndex:indexPath.row];
	
	[userDefaults setObject:@"" forKey:kIDIalKeyboardViewDigitText];
	
	[callMechanismInstance callNumberWithInfo: contactInfo];
	
	mySearchBar.text = @"";
	
    /*
	 To conform to the Human Interface Guidelines, selections should not be persistent --
	 deselect the row after it has been selected.
	 */
	[contactListView deselectRowAtIndexPath:indexPath animated:YES];
}

#pragma mark UISearchBarDelegate 

- (void)searchBarTextDidBeginEditing:(UISearchBar *)searchBar
{
	// only show the status bar's cancel button while in edit mode
	mySearchBar.showsCancelButton = YES;
	
	if(searchBarText != nil)
	{
		[searchBarText release];
		searchBarText = nil;
	}
	
	searchBarText = [[NSString alloc] initWithString: mySearchBar.text];
	
	[contactInfoController searchTextBeginEditing];
}

- (void)searchBarTextDidEndEditing:(UISearchBar *)searchBar
{
	mySearchBar.showsCancelButton = NO;	
	
	[userDefaults setObject:[[NSString alloc] initWithString: mySearchBar.text] forKey:kIDIalKeyboardViewDigitText];
}

- (void) refreshContactListView: (id) anObject {
	
	[contactListView reloadData];
}

- (void) performTextSearch: (id) anObject {
	
	@synchronized(syncKeyboardText)
	{
		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
		
		[self showActivityIndicator];
		
		NSArray* params = (NSArray*) anObject;
		
		NSString* priorSearchText = [params objectAtIndex:0];
		NSString* newSearchText = [params objectAtIndex:1];
		
		if([priorSearchText length] > [newSearchText length])
		{
			[contactInfoController searchReset];
		}			
		
		[contactInfoController searchTextChange:newSearchText: FALSE];	
		
		[self performSelectorOnMainThread:@selector(refreshContactListView:) withObject:nil waitUntilDone:YES];
		
		[self hideActivityIndicator];
		
		[pool release];
	}
}

- (void)searchBar:(UISearchBar *)searchBar textDidChange:(NSString *)searchText
{
	if(searchBarText != nil)
	{
		if(!contactInfoController.loadingContacts)
		{
			[NSThread detachNewThreadSelector:@selector(performTextSearch:) toTarget:self withObject: [NSArray arrayWithObjects: [[NSString alloc] initWithString:searchBarText],
																									   [[NSString alloc] initWithString:searchText], nil]];
		}
		
		[searchBarText release];
		searchBarText = [[NSString alloc] initWithString: mySearchBar.text];
	}
}

// called when cancel button pressed
- (void)searchBarCancelButtonClicked:(UISearchBar *)searchBar
{
	// if a valid search was entered but the user wanted to cancel, bring back the saved list content
	[contactInfoController searchReset];
	
	searchBar.text = @"";
	
	[userDefaults setObject:@"" forKey:kIDIalKeyboardViewDigitText];
	
	[contactListView reloadData];
	
	[searchBar resignFirstResponder];	
}

// called when Search (in our case "Done") button pressed
- (void)searchBarSearchButtonClicked:(UISearchBar *)searchBar
{
	[searchBar resignFirstResponder];
}

- (void)dealloc {	
	[mySearchBar release];
	[contactListView release];
	
    [super dealloc];
}

@end
