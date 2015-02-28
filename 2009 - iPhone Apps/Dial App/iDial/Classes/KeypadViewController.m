//
//  KeypadViewController.m
//  Fast Dial
//
//  Created by Vikas on 12/14/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "KeypadViewController.h"
#import "ContactInformationController.h"
#import "ContactTableViewCell.h"

static NSString* syncKeypadText = @"KeypadSyncText";

@implementation KeypadViewController

@synthesize activityImageView;
@synthesize activityIndicator;

@synthesize keypadContactListViewData;

@synthesize	keypadDigitOne;
@synthesize	keypadDigitTwo;
@synthesize keypadDigitThree;
@synthesize keypadDigitFour;
@synthesize keypadDigitFive;
@synthesize keypadDigitSix;
@synthesize keypadDigitSeven;
@synthesize keypadDigitEight;
@synthesize keypadDigitNine;
@synthesize keypadDigitZero;
@synthesize keypadDigitDelete;
@synthesize keypadDigitHideShow;
@synthesize keypadDigitCall;
@synthesize keypadDigitStar;
@synthesize keypadDigitPound;
@synthesize keypadDigitLabel;
@synthesize contactListView;
@synthesize keypadView;
@synthesize contactInfoController;
@synthesize keypadIsHidden;
@synthesize callMechanismInstance;
@synthesize userDefaults;
@synthesize deleteDigitsTimer;

@synthesize keypadDigitOneSound;
@synthesize keypadDigitTwoSound;
@synthesize keypadDigitThreeSound;
@synthesize keypadDigitFourSound;
@synthesize keypadDigitFiveSound;
@synthesize keypadDigitSixSound;
@synthesize keypadDigitSevenSound;
@synthesize keypadDigitEightSound;
@synthesize keypadDigitNineSound;
@synthesize keypadDigitZeroSound;

+ (void)initialize{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSString	*defaultDigitText = @"";
    NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
								 defaultDigitText, kIDIalKeypadViewDigitText, nil];
	
    [defaults registerDefaults:appDefaults];
}

// The designated initializer. Override to perform setup that is required before the view is loaded.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
        
	}
    return self;
}


/**
 // Implement loadView to create a view hierarchy programmatically, without using a nib.
 - (void)loadView {
 
 }
 */

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
		
		activityIndicator.frame = CGRectMake(143, 43, 37, 37);
		
		[self.view addSubview:activityIndicator];
	}
	
	activityImageView.frame = CGRectMake(124, 24, 75, 75);
	
	[activityIndicator startAnimating];
}

- (void) hideActivityIndicator {
	activityImageView.frame = CGRectMake(-124, 24, 75, 75);
	
	[activityIndicator stopAnimating];
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	
	NSString *path = [[NSBundle mainBundle] pathForResource:@"0" ofType:@"aif"];
	keypadDigitZeroSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"1" ofType:@"aif"];
	keypadDigitOneSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"2" ofType:@"aif"];
	keypadDigitTwoSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"3" ofType:@"aif"];
	keypadDigitThreeSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"4" ofType:@"aif"];
	keypadDigitFourSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"5" ofType:@"aif"];
	keypadDigitFiveSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"6" ofType:@"aif"];
	keypadDigitSixSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"7" ofType:@"aif"];
	keypadDigitSevenSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"8" ofType:@"aif"];
	keypadDigitEightSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	path = [[NSBundle mainBundle] pathForResource:@"9" ofType:@"aif"];
	keypadDigitNineSound = [[SoundEffect alloc] initWithContentsOfFile:path];
	
	
	if(userDefaults == nil)
	{
		userDefaults = [NSUserDefaults standardUserDefaults];
	}
	
    [super viewDidLoad];
	
	if(callMechanismInstance == nil) {
		callMechanismInstance = [CallMechanism getInstance];
	}	
	
	if(contactListView == nil)
	{
		if(keypadIsHidden)
		{
			contactListView = [[UITableView alloc] initWithFrame: CGRectMake(0, 0, 320, 130 + 180) style: UITableViewStylePlain];
		}
		else
		{
			contactListView = [[UITableView alloc] initWithFrame: CGRectMake(0, 0, 320, 130) style: UITableViewStylePlain];
		}
		
		[self.view addSubview:contactListView];
		
		contactListView.dataSource = self;
		contactListView.delegate = self;
	}
	
	contactInfoController = [ContactInformationController getInstance];
	
	[self showActivityIndicator];
}

- (void)viewWillAppear:(BOOL)animated; {
	// Custom initialization
	[contactInfoController registerABChangeCallback];
	
	if(contactInfoController != nil && !contactInfoController.loadingContacts) {		
		
		[contactInfoController resetFilteredContactListToMasterList];
		
		if(keypadDigitLabel.text != nil && [keypadDigitLabel.text length] > 0)
		{
			[NSThread detachNewThreadSelector:@selector(performKeypadTextSearch:) toTarget:self withObject: [[NSString alloc] initWithString:keypadDigitLabel.text]];
		}
		else if(contactListView != nil)
		{
			//[contactListView reloadData];
			[self refreshKeypadContactListView:nil];
		}		
	}
	else
	{
		[self refreshKeypadContactListView:nil];
	}
	
	
	[super viewWillAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
}

/*
 // Override to allow orientations other than the default portrait orientation.
 - (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
 // Return YES for supported orientations
 return (interfaceOrientation == UIInterfaceOrientationPortrait);
 }
 */

void keypadViewHide(UIView* keypadView, UITableView* contactListView, UIButton* hideShowButton) {
	[ UIView beginAnimations: @"KeypadAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.5f ]; // Set the duration to 5/10ths of a second.
	CGRect keypadViewFrame = [keypadView frame];
	keypadViewFrame.origin.y = 307;
	keypadView.frame = keypadViewFrame;
	
	CGRect contactListViewFrame = [contactListView frame];
	contactListViewFrame.size.height = contactListViewFrame.size.height + 180;
	contactListView.frame = contactListViewFrame;
	
	[ UIView commitAnimations ]; // Animate!
	
	UIImage* showKeypadImage = [UIImage imageNamed:@"DialPadShowKeypad.png"];
	UIImage* showKeypadHighlightedImage = [UIImage imageNamed:@"DialPadShowKeypadHighlighted.png"];
	
	[hideShowButton setImage:showKeypadImage forState:UIControlStateNormal];
	[hideShowButton setImage:showKeypadHighlightedImage	forState:UIControlStateHighlighted];
	[hideShowButton setImage:showKeypadHighlightedImage	forState:UIControlStateSelected];
}

void keypadViewShow(UIView* keypadView, UITableView* contactListView, UIButton* hideShowButton) {
	[ UIView beginAnimations: @"KeypadAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.5f ]; // Set the duration to 5/10ths of a second.
	CGRect keypadViewFrame = [keypadView frame];
	keypadViewFrame.origin.y = 127;
	keypadView.frame = keypadViewFrame;
	
	CGRect contactListViewFrame = [contactListView frame];
	contactListViewFrame.size.height = contactListViewFrame.size.height - 180;
	contactListView.frame = contactListViewFrame;
	
	[ UIView commitAnimations ]; // Animate!
	
	UIImage* hideKeypadImage = [UIImage imageNamed:@"DialPadHideKeypad.png"];
	UIImage* hideKeypadHighlightedImage = [UIImage imageNamed:@"DialPadHideKeypadHighlighted.png"];
	
	[hideShowButton setImage:hideKeypadImage forState:UIControlStateNormal];
	[hideShowButton setImage:hideKeypadHighlightedImage	forState:UIControlStateHighlighted];
	[hideShowButton setImage:hideKeypadHighlightedImage	forState:UIControlStateSelected];
}

- (void) refreshKeypadContactListView: (id) anObject {
	
	[contactListView reloadData];
	
	[contactListView setNeedsDisplay];
}

- (void) performKeypadTextSearch: (id) anObject {
	
	@synchronized(syncKeypadText)
	{	
		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
		
		[self showActivityIndicator];
		
		NSString* searchDigitText = (NSString*) anObject;	
		
		[contactInfoController searchReset];
		
		[contactInfoController searchTextBeginEditing];
		[contactInfoController searchTextChange:searchDigitText: YES];	
		
		[self performSelectorOnMainThread:@selector(refreshKeypadContactListView:) withObject:nil waitUntilDone:YES];
		
		[self hideActivityIndicator];
		
		[pool release];		
	}
}

- (void) deleteAllDigits: (NSTimer*) timer {
	
	[deleteDigitsTimer invalidate];
	[deleteDigitsTimer release];
	deleteDigitsTimer = nil;
	
	[contactInfoController searchReset];
	
	[NSThread detachNewThreadSelector:@selector(performKeypadTextSearch:) toTarget:self withObject: [[NSString alloc] initWithString:@""]];
	
	keypadDigitLabel.text = @"";
	
	[userDefaults setObject:@"" forKey:kIDIalKeypadViewDigitText];
}

- (IBAction) deleteDigitPressedDown: (id) sender {
	deleteDigitsTimer = [[NSTimer scheduledTimerWithTimeInterval:1.5 target:self selector:@selector(deleteAllDigits:) userInfo:nil repeats:YES] retain];
}

- (IBAction) digitPressed: (id) sender {
	NSString	*digitText = keypadDigitLabel.text;
	
	if(deleteDigitsTimer != nil)
	{
		[deleteDigitsTimer invalidate];
		[deleteDigitsTimer release];
		deleteDigitsTimer = nil;
	}	
	
	if(sender == keypadDigitOne)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitOneSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"1"];
	}
	else if(sender == keypadDigitTwo)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitTwoSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"2"];
	}
	else if(sender == keypadDigitThree)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitThreeSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"3"];
	}
	else if(sender == keypadDigitFour)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitFourSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"4"];
	}
	else if(sender == keypadDigitFive)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitFiveSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"5"];
	}
	else if(sender == keypadDigitSix)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitSixSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"6"];
	}
	else if(sender == keypadDigitSeven)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitSevenSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"7"];
	}
	else if(sender == keypadDigitEight)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitEightSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"8"];
	}
	else if(sender == keypadDigitNine)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitNineSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"9"];
	}
	else if(sender == keypadDigitZero)
	{
		if([[userDefaults stringForKey:kIDialKeypadConfigTones] compare:kIDialTrue] == NSOrderedSame)
		{
			[keypadDigitZeroSound play];
		}
		
		digitText = [keypadDigitLabel.text stringByAppendingString: @"0"];
	}
	else if(sender == keypadDigitStar)
	{
		digitText = [keypadDigitLabel.text stringByAppendingString: @"*"];
	}
	else if(sender == keypadDigitPound)
	{
		digitText = [keypadDigitLabel.text stringByAppendingString: @"#"];
	}
	else if(sender == keypadDigitHideShow)
	{
		digitText = [keypadDigitLabel.text stringByAppendingString: @"+"];
	}
	else if(sender == keypadDigitCall)
	{
		[userDefaults setObject:@"" forKey:kIDIalKeypadViewDigitText];
		
		[callMechanismInstance callNumber: digitText];
		
		[contactInfoController searchReset];
		
		digitText = @"";
	}
	else if(sender == keypadDigitDelete)
	{
		if([keypadDigitLabel.text length] < 1)
		{
			digitText = @"";
		}
		else
		{
			digitText = [keypadDigitLabel.text substringToIndex:([keypadDigitLabel.text length] - 1)];
			[contactInfoController searchReset];
		}
	}
	
	if(!contactInfoController.loadingContacts)
	{
		[NSThread detachNewThreadSelector:@selector(performKeypadTextSearch:) toTarget:self withObject: [[NSString alloc] initWithString:digitText]];
	}
	
	keypadDigitLabel.text = digitText;
	
	[userDefaults setObject:digitText forKey:kIDIalKeypadViewDigitText];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}

- (void) timerFiredEvent:(NSTimer *)timer {
	
	if(!contactInfoController.loadingContacts)
	{
		[timer invalidate];
		
		[contactInfoController resetFilteredContactListToMasterList];
		
		[self hideActivityIndicator];		
		
		NSString* digitText = @"";
		
		if(keypadDigitLabel.text != nil)
		{
			digitText = keypadDigitLabel.text;
		}
		
		[NSThread detachNewThreadSelector:@selector(performKeypadTextSearch:) toTarget:self withObject: [[NSString alloc] initWithString:digitText]];
		
		[contactListView reloadData];
	}
}

// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {	
	
	if(keypadContactListViewData != nil)
	{
		[keypadContactListViewData release];
		keypadContactListViewData = nil;
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
	
	keypadContactListViewData = [[NSArray alloc] initWithArray:contactInfoController.filteredABContactList];
	
    return [keypadContactListViewData count];
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
	}
	else
	{
		[cell setWhiteColor];
	}
    
	ContactInformationWrapper* contactInfo = [keypadContactListViewData objectAtIndex:indexPath.row];
	
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
	ContactInformationWrapper* contactInfo = [keypadContactListViewData objectAtIndex:indexPath.row];
	
	[callMechanismInstance callNumberWithInfo: contactInfo];
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
	ContactInformationWrapper* contactInfo = [keypadContactListViewData objectAtIndex:indexPath.row];
	
	[callMechanismInstance callNumberWithInfo: contactInfo];
	
    /*
	 To conform to the Human Interface Guidelines, selections should not be persistent --
	 deselect the row after it has been selected.
	 */
	[contactListView deselectRowAtIndexPath:indexPath animated:YES];
}



- (void)dealloc {
    
	[keypadDigitOne release];
	[keypadDigitTwo release];
	[keypadDigitThree release];
	[keypadDigitFour release];
	[keypadDigitFive release];
	[keypadDigitSix release];
	[keypadDigitSeven release];
	[keypadDigitEight release];
	[keypadDigitNine release];
	[keypadDigitZero release];
	[keypadDigitLabel release];
	[contactListView release];
	[contactInfoController release];
	
	[keypadDigitZeroSound release];
	[keypadDigitOneSound release];
	[keypadDigitTwoSound release];
	[keypadDigitThreeSound release];
	[keypadDigitFourSound release];
	[keypadDigitFiveSound release];
	[keypadDigitSixSound release];
	[keypadDigitSevenSound release];
	[keypadDigitEightSound release];
	[keypadDigitNineSound release];
	
	[super dealloc];
}


@end
