//
//  SelectBookmarkFolderController.m
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "SelectBookmarkFolderController.h"
#import "SettingsConstants.h"

@implementation SelectBookmarkFolderController

@synthesize expectedInterfaceOrientation;
@synthesize editBookmarkController;
@synthesize folderTable;
@synthesize userDefaults;
@synthesize bookmarkLinks;
@synthesize bookmarkFolders;
@synthesize saveBookmarkController;

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
	
	NSArray* bookmarkFoldersTemp = [[bookmarkFoldersDict allKeys] sortedArrayUsingSelector:@selector(caseInsensitiveCompare:)];
	
	bookmarkFolders = [[NSMutableArray alloc] initWithCapacity:[bookmarkFoldersTemp count]];
	[bookmarkFolders addObjectsFromArray:bookmarkFoldersTemp];
	[bookmarkFolders removeObject:kIBrowserParentBookmarkFolder];
	
	bookmarkLinks = [bookmarkFoldersDict objectForKey:kIBrowserParentBookmarkFolder];
}


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	userDefaults = [NSUserDefaults standardUserDefaults];
		
	[self loadBookmarks];
	
	folderTable.delegate = self;
	folderTable.dataSource = self;
}


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

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	
	return ([bookmarkFolders count] - 1);
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
	
	UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 0, 200, 18)];
	cellTextLabel.highlightedTextColor = [UIColor whiteColor];
	cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
	cellTextLabel.backgroundColor = [UIColor clearColor];
	
	if(([indexPath row] + 1) < [bookmarkFolders count])
	{
		iconImage = [UIImage imageNamed:@"FolderIcon.png"];
		
		cellTextLabel.frame = CGRectMake(60, 0, 250, 40);
		
		cellTextLabel.text = [bookmarkFolders objectAtIndex:([indexPath row] + 1)];
		
		cell.accessoryType = UITableViewCellAccessoryNone;
	}
	
	UIImageView* iconImageView = [[UIImageView alloc]initWithImage:iconImage];
	iconImageView.frame = CGRectMake(10, 2, 40, 40);
	[cell.contentView addSubview:iconImageView];
	
	[cell.contentView addSubview: cellTextLabel];
	
	[iconImageView release];	
	
    return cell;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (IBAction) cancelAction: (id) sender {
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    // Navigation logic may go here. Create and push another view controller.
	// AnotherViewController *anotherViewController = [[AnotherViewController alloc] initWithNibName:@"AnotherView" bundle:nil];
	// [self.navigationController pushViewController:anotherViewController];
	// [anotherViewController release];
	[folderTable deselectRowAtIndexPath:indexPath animated:YES];
	
	if(saveBookmarkController != nil)
	{
		[saveBookmarkController setBookmarkSaveFolder:[bookmarkFolders objectAtIndex:([indexPath row] + 1)]];
	}
	else if(editBookmarkController != nil)
	{
		[editBookmarkController setBookmarkSaveFolder:[bookmarkFolders objectAtIndex:([indexPath row] + 1)]];
	}
	
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void)dealloc {
	[folderTable release];
	[bookmarkFolders release];
	
    [super dealloc];
}


@end
