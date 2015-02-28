//
//  BookmarkFolderDetailViewController.m
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "BookmarkFolderDetailViewController.h"
#import "SettingsConstants.h"
#import "EditBookmarkController.h"

@implementation BookmarkFolderDetailViewController

@synthesize expectedInterfaceOrientation;
@synthesize bottomButton;
@synthesize bookmarksTable;
@synthesize userDefaults;
@synthesize bookmarkLinks;
@synthesize bookmarkFolders;
@synthesize topNavigationItem;
@synthesize bottomNavigationItem;
@synthesize showFolderName;
@synthesize bookmarkDict;
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

- (void) loadBookmarks {
	NSDictionary* bookmarkFoldersDict = [userDefaults dictionaryForKey:kIBrowserBookmarkFolders];
	bookmarkDict = [[NSMutableDictionary alloc] init];
	[bookmarkDict setDictionary:bookmarkFoldersDict];	
	
	NSArray* bookmarkFoldersTemp = [[bookmarkFoldersDict allKeys] sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
	
	bookmarkFolders = [[NSMutableArray alloc] initWithCapacity:[bookmarkFoldersTemp count]];
	[bookmarkFolders addObjectsFromArray:bookmarkFoldersTemp];
	[bookmarkFolders removeObject:kIBrowserParentBookmarkFolder];
	
	NSArray* bookmarkLinksTemp = [bookmarkFoldersDict objectForKey:showFolderName];
	bookmarkLinks = [[NSMutableArray alloc] initWithCapacity:[bookmarkLinksTemp count]];
	[bookmarkLinks addObjectsFromArray:bookmarkLinksTemp];
}

- (void)viewWillAppear:(BOOL)animated {
    [super viewWillAppear:animated];
	
	[bookmarkFolders release];
	[bookmarkLinks release];
	[bookmarkDict release];
	
	[self loadBookmarks];
	
	[bookmarksTable reloadData];
}

- (void)viewDidLoad {
    [super viewDidLoad];
	
	userDefaults = [NSUserDefaults standardUserDefaults];
	
	bookmarksTable.delegate = self;
	bookmarksTable.dataSource = self;	
	bookmarksTable.allowsSelectionDuringEditing = YES;
	
	[self loadBookmarks];
	
	NSString* title = showFolderName;
	
	if([title compare:kIBrowserHistoryFolder] == NSOrderedSame)
	{
		title = kIBrowserHistoryWord;
		
		bottomButton.style = UIBarButtonItemStyleDone;
		bottomButton.title = @"Clear";
		bottomButton.action = @selector(clearBookmarks:);
	}
	
	self.topNavigationItem.title = title;
	
	UIBarButtonItem* backButton = [[UIBarButtonItem alloc]initWithTitle:@"Back" style:UIBarButtonItemStyleBordered target:self action:@selector(doneBookmarks:)];
	
	self.topNavigationItem.leftBarButtonItem = backButton;
	
	[backButton release];
	
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
}


- (void) setNavigationBarEndEditing {
	UIBarButtonItem* backButton = [[UIBarButtonItem alloc]initWithTitle:@"Back" style:UIBarButtonItemStyleBordered target:self action:@selector(doneBookmarks:)];
	
	self.topNavigationItem.leftBarButtonItem = backButton;
	
	[backButton release];
	
	UIBarButtonItem* editButton = [[UIBarButtonItem alloc]initWithBarButtonSystemItem:UIBarButtonSystemItemEdit target:self action:@selector(editBookmarks:)];
	
	self.bottomNavigationItem.leftBarButtonItem = editButton;
	
	[editButton release];
	
	self.bottomNavigationItem.rightBarButtonItem = nil;
}

- (IBAction) doneEditBookmarks: (id) sender {
	[bookmarksTable setEditing:NO animated:YES];
	
	[self setNavigationBarEndEditing];
}

- (void) setNavigationBarBeginEditing {
	UIBarButtonItem* doneEditButton = [[UIBarButtonItem alloc]initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(doneEditBookmarks:)];
	
	self.bottomNavigationItem.leftBarButtonItem = doneEditButton;
	
	[doneEditButton release];
		
	self.topNavigationItem.leftBarButtonItem = nil;
}

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
	// the user clicked one of the OK/Cancel buttons
	if (buttonIndex == 0)
	{
		[bookmarkLinks removeAllObjects];
		[bookmarkDict setObject:bookmarkLinks forKey:showFolderName];
		
		[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
		
		[userDefaults synchronize];
		
		[self loadBookmarks];
		
        // Delete the row from the data source
        [bookmarksTable reloadData];
	}
}

- (IBAction) clearBookmarks: (id) sender {
	UIActionSheet *actionSheet = [[UIActionSheet alloc] initWithTitle:@"History"
															 delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Clear History"
													otherButtonTitles: nil];
	actionSheet.actionSheetStyle = UIActionSheetStyleDefault;
	[actionSheet showInView:self.view]; // show from our table view (pops up in the middle of the table)
	[actionSheet release];
}

- (IBAction) editBookmarks: (id) sender {
	[bookmarksTable setEditing:YES animated:YES];
	
	[self setNavigationBarBeginEditing];
}

- (IBAction) doneBookmarks: (id) sender {
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}


/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

#pragma mark Table view methods

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	
	return [bookmarkLinks count];
}

- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath {
	
	NSDictionary* bookmark = [bookmarkLinks objectAtIndex:[fromIndexPath row]];
	
	[bookmarkLinks removeObjectAtIndex:[fromIndexPath row]];
	
	[bookmarkLinks insertObject:bookmark atIndex:[toIndexPath row]];
	
	[bookmarkDict setObject:bookmarkLinks forKey:showFolderName];
	
	[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
	
	[userDefaults synchronize];
}

- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath {

	return YES;
}

- (UITableViewCellAccessoryType)tableView:(UITableView *)tableView accessoryTypeForRowWithIndexPath:(NSIndexPath *)indexPath {
	
	return UITableViewCellAccessoryDisclosureIndicator;
}


// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
	
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
    }
    
	//clear prior content views
	NSArray* cellContentSubviews = cell.contentView.subviews;
	
	if(cellContentSubviews && [cellContentSubviews count] > 0)
	{
		for(int i = 0; i < [cellContentSubviews count]; ++i)
		{
			[[cellContentSubviews objectAtIndex:i] removeFromSuperview];
		}
	}
	
    // Set up the cell...
	// this cell hosts the UISwitch control
	UIImage* iconImage = nil;
	
	UILabel* cellDescLabel = nil;
	
	UILabel* cellTextLabel = nil;
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 4, 250, 18)];
	}
	else
	{
		cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 4, 390, 18)];
	}
	
	cellTextLabel.highlightedTextColor = [UIColor whiteColor];
	cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
	cellTextLabel.backgroundColor = [UIColor clearColor];
	
	iconImage = [UIImage imageNamed:@"PageIcon.png"];
	
	NSDictionary* bookmarkLinkDict = [bookmarkLinks objectAtIndex:[indexPath row]];
	
	NSString* desc = [bookmarkLinkDict objectForKey:kIBrowserLinkDesc];
	
	cellTextLabel.text = [bookmarkLinkDict objectForKey:kIBrowserLinkName];
	
	if(desc.length > 0)
	{			
		cellDescLabel = nil;
		
		if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			cellDescLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 26, 250, 14)];
		}
		else
		{
			cellDescLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 26, 390, 14)];
		}
		
		cellDescLabel.highlightedTextColor = [UIColor whiteColor];
		cellDescLabel.font = [UIFont boldSystemFontOfSize:12];
		cellDescLabel.textColor = [UIColor grayColor];
		cellDescLabel.backgroundColor = [UIColor clearColor];
		
		cellTextLabel.font = [UIFont boldSystemFontOfSize:14];
		
		cellDescLabel.text = desc;
	}
	
	cell.accessoryType = UITableViewCellAccessoryNone;
	
	if(cellDescLabel == nil)
	{
		if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			cellTextLabel.frame = CGRectMake(60, 0, 250, 40);
		}
		else
		{
			cellTextLabel.frame = CGRectMake(60, 0, 390, 40);
		}
	}
	
	UIImageView* iconImageView = [[UIImageView alloc]initWithImage:iconImage];
	iconImageView.frame = CGRectMake(10, 2, 40, 40);
	[cell.contentView addSubview:iconImageView];
	
	[cell.contentView addSubview: cellTextLabel];
	
	cell.hidesAccessoryWhenEditing = NO;
	
	if(cellDescLabel != nil)
	{
		[cell.contentView addSubview:cellDescLabel];
	}
	
	[iconImageView release];
	
    return cell;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    // Navigation logic may go here. Create and push another view controller.
	// AnotherViewController *anotherViewController = [[AnotherViewController alloc] initWithNibName:@"AnotherView" bundle:nil];
	// [self.navigationController pushViewController:anotherViewController];
	// [anotherViewController release];
	
	[bookmarksTable deselectRowAtIndexPath:indexPath animated:YES];
	
	if(bookmarksTable.editing)
	{
		EditBookmarkController* editController = nil;
		
		if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			editController = [[EditBookmarkController alloc] initWithNibName:@"EditBookmarkView" bundle:nil];
		}
		else
		{
			editController = [[EditBookmarkController alloc] initWithNibName:@"EditBookmarkViewLandscape" bundle:nil];
		}
		
		editController.oldBookmarkFolder = showFolderName;
		editController.oldBookmarkSettings = [bookmarkLinks objectAtIndex:[indexPath row]];
		
		editController.bookmarkFolder = showFolderName;
		
		editController.expectedInterfaceOrientation = self.interfaceOrientation;		
		
		[self presentModalViewController:editController animated:YES];
		
		[editController release];
	}
	else
	{	
		NSDictionary* bookmark = [bookmarkLinks objectAtIndex:[indexPath row]];
		NSString* url = [bookmark objectForKey:kIBrowserLinkURL];
	
		[bookmarkController loadWebPage:url];
	}
	
	//[self.parentViewController dismissModalViewControllerAnimated:YES];
}

// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath {
    
    if (editingStyle == UITableViewCellEditingStyleDelete) {
		
		[bookmarkLinks removeObjectAtIndex:[indexPath row]];
		[bookmarkDict setObject:bookmarkLinks forKey:showFolderName];
		
		[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
		
		[userDefaults synchronize];
		
		[self loadBookmarks];
		
        // Delete the row from the data source
        [bookmarksTable reloadData];
    }   
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
	[bookmarksTable release];
	[bottomButton release];
	[topNavigationItem release];
	[bottomNavigationItem release];
	[bookmarkFolders release];
	[bookmarkLinks release];
	[bookmarkDict release];
	[showFolderName release];
	
    [super dealloc];
}


@end
