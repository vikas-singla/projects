//
//  BookmarksController.m
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "BookmarksController.h"
#import "SettingsConstants.h"
#import "AddBookmarkFolderController.h"
#import "EditBookmarkController.h"
#import "BookmarkFolderDetailViewController.h"

@implementation BookmarksController

@synthesize expectedInterfaceOrientation;
@synthesize bookmarksTable;
@synthesize userDefaults;
@synthesize bookmarkLinks;
@synthesize bookmarkFolders;
@synthesize topNavigationItem;
@synthesize bottomNavigationItem;
@synthesize bookmarkDict;
@synthesize browserController;

/*
- (id)initWithStyle:(UITableViewStyle)style {
    // Override initWithStyle: if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
    if (self = [super initWithStyle:style]) {
    }
    return self;
}
*/

- (void) loadWebPage: (NSString*) linkURL {
	[browserController loadWebPage: linkURL];
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}


- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (void) loadBookmarks {
	NSDictionary* bookmarkFoldersDict = [userDefaults dictionaryForKey:kIBrowserBookmarkFolders];
	bookmarkDict = [[NSMutableDictionary alloc] init];
	[bookmarkDict setDictionary:bookmarkFoldersDict];
	
	NSArray* bookmarkFoldersTemp = [[bookmarkFoldersDict allKeys] sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
	
	bookmarkFolders = [[NSMutableArray alloc] initWithCapacity:[bookmarkFoldersTemp count]];
	[bookmarkFolders addObjectsFromArray:bookmarkFoldersTemp];
	[bookmarkFolders removeObject:kIBrowserParentBookmarkFolder];
	
	NSArray* bookmarkLinksTemp = [bookmarkFoldersDict objectForKey:kIBrowserParentBookmarkFolder];
	bookmarkLinks = [[NSMutableArray alloc] initWithCapacity:[bookmarkLinksTemp count]];
	[bookmarkLinks addObjectsFromArray:bookmarkLinksTemp];
}

- (void)viewDidLoad {
    [super viewDidLoad];
	
	userDefaults = [NSUserDefaults standardUserDefaults];
	
	bookmarksTable.delegate = self;
	bookmarksTable.dataSource = self;
	bookmarksTable.allowsSelectionDuringEditing = YES;
	
	[self loadBookmarks];
	
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
}


- (void)viewWillAppear:(BOOL)animated {
    [super viewWillAppear:animated];
	
	[bookmarkFolders release];
	[bookmarkLinks release];
	[bookmarkDict release];
	
	[self loadBookmarks];
	
	[bookmarksTable reloadData];
}

/*
- (void)viewDidAppear:(BOOL)animated {
    [super viewDidAppear:animated];
}
*/
/*
- (void)viewWillDisappear:(BOOL)animated {
	[super viewWillDisappear:animated];
}
*/
/*
- (void)viewDidDisappear:(BOOL)animated {
	[super viewDidDisappear:animated];
}
*/

/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/


- (IBAction) addNewFolder: (id) sender {
	AddBookmarkFolderController* newFolderController = nil;
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		newFolderController = [[AddBookmarkFolderController alloc] initWithNibName:@"AddBookmarkFolder" bundle:nil];
	}
	else
	{
		newFolderController = [[AddBookmarkFolderController alloc] initWithNibName:@"AddBookmarkFolderLandscape" bundle:nil];
	}
		
	newFolderController.bookmarkController = self;
	
	newFolderController.expectedInterfaceOrientation = self.interfaceOrientation;
	
	[self presentModalViewController:newFolderController animated:YES];
	
	[newFolderController release];
}

- (void) setNavigationBarEndEditing {
	UIBarButtonItem* doneButton = [[UIBarButtonItem alloc]initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(doneBookmarks:)];
	
	self.topNavigationItem.rightBarButtonItem = doneButton;
	
	[doneButton release];
	
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
	
	UIBarButtonItem* addFolderButton = [[UIBarButtonItem alloc]initWithTitle:@"New Folder" style:UIBarButtonItemStyleBordered target:self action:@selector(addNewFolder:)];
	
	self.bottomNavigationItem.rightBarButtonItem = addFolderButton;
	
	[addFolderButton release];
	
	self.topNavigationItem.rightBarButtonItem = nil;
}

- (IBAction) editBookmarks: (id) sender {
	[bookmarksTable setEditing:YES animated:YES];
	
	[self setNavigationBarBeginEditing];
}

- (IBAction) doneBookmarks: (id) sender {
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (BOOL) checkFolderName : (NSString*) name {
	
	BOOL result = NO;
	
	for(int i = 0; i < [bookmarkFolders count]; ++i)
	{
		if([[bookmarkFolders objectAtIndex:i] compare:name options:NSCaseInsensitiveSearch] == NSOrderedSame)
		{
			result = YES;
			break;
		}
	}
	
	return result;
}

- (void)viewWillDisappear:(BOOL)animated
{
	[userDefaults synchronize];
}

- (BOOL) createFolder: (NSString*) name {
	if([self checkFolderName : name])
	{
		return NO;
	}
	
	NSArray	*bookmarkLinksEmpty = [NSArray arrayWithObjects:nil];
	
	[bookmarkDict setObject:bookmarkLinksEmpty forKey:name];
	
	[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
	
	[userDefaults synchronize];
	
	[self loadBookmarks];
	
	[bookmarksTable reloadData];
	
	return YES;
}

#pragma mark Table view methods

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	
	return ([bookmarkLinks count] + [bookmarkFolders count]);
}

- (NSIndexPath *)tableView:(UITableView *)tableView targetIndexPathForMoveFromRowAtIndexPath:(NSIndexPath *)sourceIndexPath toProposedIndexPath:(NSIndexPath *)proposedDestinationIndexPath {

	if([sourceIndexPath row] < [bookmarkFolders count])
	{
		return sourceIndexPath;
	}
	
	if([proposedDestinationIndexPath row] < [bookmarkFolders count])
	{
		return [NSIndexPath indexPathForRow:[bookmarkFolders count] inSection:sourceIndexPath.section];
	}
	
	return proposedDestinationIndexPath;
}

- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath {

	NSDictionary* bookmark = [bookmarkLinks objectAtIndex:([fromIndexPath row] - [bookmarkFolders count])];
	
	NSMutableDictionary* newBookmark = [[NSMutableDictionary alloc] init];
	[newBookmark addEntriesFromDictionary:bookmark];
	
	[bookmarkLinks removeObjectAtIndex:([fromIndexPath row] - [bookmarkFolders count])];
	
	[bookmarkLinks insertObject:newBookmark atIndex:([toIndexPath row] - [bookmarkFolders count])];
	
	[newBookmark release];
	
	[bookmarkDict setObject:bookmarkLinks forKey:kIBrowserParentBookmarkFolder];
	
	[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
	
	[userDefaults synchronize];
}

- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath {
	if([indexPath row] >= [bookmarkFolders count])
	{
		return YES;
	}
	
	return NO;
}

- (UITableViewCellAccessoryType)tableView:(UITableView *)tableView accessoryTypeForRowWithIndexPath:(NSIndexPath *)indexPath {
	
	if(bookmarksTable.editing)
	{
		if([indexPath row] >= [bookmarkFolders count])
		{
			return UITableViewCellAccessoryDisclosureIndicator;
		}
	}
	else
	{
		if([indexPath row] < [bookmarkFolders count])
		{
			return UITableViewCellAccessoryDisclosureIndicator;
		}		
	}
	
	return UITableViewCellAccessoryNone;
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
	
	if([indexPath row] < [bookmarkFolders count])
	{		
		NSString* folderName = [bookmarkFolders objectAtIndex:[indexPath row]];
		
		if([folderName compare:kIBrowserHistoryFolder] == NSOrderedSame)
		{
			folderName = kIBrowserHistoryWord;
			
			iconImage = [UIImage imageNamed:@"HistoryFolderIcon.png"];
		}
		else
		{
			iconImage = [UIImage imageNamed:@"FolderIcon.png"];
		}
		
		cellTextLabel.text = folderName;
		
		cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
		
		cell.hidesAccessoryWhenEditing = YES;
	}
	else
	{
		cell.hidesAccessoryWhenEditing = NO;
		
		iconImage = [UIImage imageNamed:@"PageIcon.png"];
		
		NSDictionary* bookmarkLinkDict = [bookmarkLinks objectAtIndex:([indexPath row] - [bookmarkFolders count])];
		
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
	}
	
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
	
	if(cellDescLabel != nil)
	{
		[cell.contentView addSubview:cellDescLabel];
	}
	
	[iconImageView release];
	
    return cell;
}


- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    // Navigation logic may go here. Create and push another view controller.
	
	[bookmarksTable deselectRowAtIndexPath:indexPath animated:YES];
	
	if(bookmarksTable.editing)
	{
		if([indexPath row] < [bookmarkFolders count])
		{
		}
		else
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
			
			editController.oldBookmarkFolder = kIBrowserParentBookmarkFolder;
			editController.oldBookmarkSettings = [bookmarkLinks objectAtIndex:([indexPath row] - [bookmarkFolders count])];
			
			editController.expectedInterfaceOrientation = self.interfaceOrientation;
			
			
			[self presentModalViewController:editController animated:YES];
			
			[editController release];
		}
	}
	else
	{	
		if([indexPath row] < [bookmarkFolders count])
		{
			BookmarkFolderDetailViewController* folderDetailController = nil;
			
			if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
			{
				folderDetailController = [[BookmarkFolderDetailViewController alloc] initWithNibName:@"BookmarksFolderDetailsView" bundle:nil];
			}
			else
			{
				folderDetailController = [[BookmarkFolderDetailViewController alloc] initWithNibName:@"BookmarksFolderDetailsViewLandscape" bundle:nil];
			}
			
			folderDetailController.showFolderName = [bookmarkFolders objectAtIndex:[indexPath row]];
			folderDetailController.bookmarkController = self;
			
			folderDetailController.expectedInterfaceOrientation = self.interfaceOrientation;
			
			
			[self presentModalViewController:folderDetailController animated:YES];
			
			[folderDetailController release];
		}
		else
		{
			NSDictionary* bookmark = [bookmarkLinks objectAtIndex:([indexPath row] - [bookmarkFolders count])];
			NSString* url = [bookmark objectForKey:kIBrowserLinkURL];
			
			[self loadWebPage:url];		
		}
	}
	// AnotherViewController *anotherViewController = [[AnotherViewController alloc] initWithNibName:@"AnotherView" bundle:nil];
	// [self.navigationController pushViewController:anotherViewController];
	// [anotherViewController release];
}


/*
// Override to support conditional editing of the table view.
- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath {
    // Return NO if you do not want the specified item to be editable.
    return YES;
}
*/



// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath {
    
    if (editingStyle == UITableViewCellEditingStyleDelete) {
		
		if([indexPath row] < [bookmarkFolders count])
		{
			[bookmarkDict removeObjectForKey:[bookmarkFolders objectAtIndex:[indexPath row]]]; 
		}
		else
		{
			[bookmarkLinks removeObjectAtIndex:([indexPath row] - [bookmarkFolders count])];
			[bookmarkDict setObject:bookmarkLinks forKey:kIBrowserParentBookmarkFolder];
		}
		
		[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
		
		[userDefaults synchronize];
		
		[self loadBookmarks];
		
        // Delete the row from the data source
        [bookmarksTable reloadData];
    }   
}



/*
// Override to support rearranging the table view.
- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath {
}
*/


/*
// Override to support conditional rearranging of the table view.
- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath {
    // Return NO if you do not want the item to be re-orderable.
    return YES;
}
*/


- (void)dealloc {
	[bookmarksTable release];
	[topNavigationItem release];
	[bottomNavigationItem release];
	[bookmarkFolders release];
	[bookmarkLinks release];
	[bookmarkDict release];
	
    [super dealloc];
}


@end

