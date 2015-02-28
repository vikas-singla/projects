//
//  AddBookmarkController.m
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import "AddBookmarkController.h"
#import "SettingsConstants.h"
#import "SelectBookmarkFolderController.h"

@implementation AddBookmarkController

@synthesize expectedInterfaceOrientation;
@synthesize isInLandscapeMode;
@synthesize inputTable;
@synthesize bookmarkLink;
@synthesize bookmarkName;
@synthesize bookmarkFolder;
@synthesize bookmarkNameTextField;
@synthesize bookmarkURLTextField;
@synthesize bookmarkDescTextField;
@synthesize bookmarkFoldersDict;

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

- (void)keyboardWillShow:(NSNotification *)notif
{
    // The keyboard will be shown. If the user is editing the author, adjust the display so that the
    // author field will not be covered by the keyboard.
	if(isInLandscapeMode)
	{
		inputTable.frame = CGRectMake(0, 44, 480, 114);
	}
}

- (void)keyboardWillHide:(NSNotification *)notif
{
    // The keyboard will be shown. If the user is editing the author, adjust the display so that the
    // author field will not be covered by the keyboard.
	if(isInLandscapeMode)
	{
		inputTable.frame = CGRectMake(0, 64, 480, 276);
	}
}

- (void) loadBookmarks {
	NSDictionary* bookmarkFoldersDictTemp = [[NSUserDefaults standardUserDefaults] dictionaryForKey:kIBrowserBookmarkFolders];
	
	bookmarkFoldersDict = [[NSMutableDictionary alloc] init];
	[bookmarkFoldersDict setDictionary:bookmarkFoldersDictTemp];
}


- (void)textFieldDidEndEditing:(UITextField *)textField {
	if(textField == bookmarkNameTextField)
	{
		bookmarkName = [[NSString alloc] initWithString: bookmarkNameTextField.text];
	}
	else if(textField == bookmarkURLTextField)
	{		
		bookmarkLink = [[NSString alloc] initWithString: bookmarkURLTextField.text];
	}
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	[self loadBookmarks];
	
	bookmarkFolder = kIBrowserParentBookmarkFolder;
	
	inputTable.delegate = self;
	inputTable.dataSource = self;
}

- (void)viewWillDisappear:(BOOL)animated
{
    // unregister for keyboard notifications while not visible.
    [[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillShowNotification object:nil]; 
	
	[[NSNotificationCenter defaultCenter] removeObserver:self name:UIKeyboardWillHideNotification object:nil]; 
}

- (void)viewWillAppear:(BOOL)animated {
	// watch the keyboard so we can adjust the user interface if necessary.
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillShow:) 
												 name:UIKeyboardWillShowNotification object:self.view.window]; 
	
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(keyboardWillHide:) 
												 name:UIKeyboardWillHideNotification object:self.view.window]; 
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
    return 2;
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	
	if(section == 0)
	{
		return 3;
	}
	
	return 1;
}


// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
	
    UITableViewCell *cell = nil;
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
	
	if(indexPath.section == 0)
	{	
		if([indexPath row] == 0)
		{
			if(bookmarkName == nil)
			{
				bookmarkName = @"";
			}
			else
			{
				bookmarkName = [bookmarkName stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
			}
			
			if(isInLandscapeMode)
			{
				bookmarkNameTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 450, 30)];
			}
			else
			{
				bookmarkNameTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 290, 30)];
			}
			
			bookmarkNameTextField.placeholder = @"Bookmark Name";
			bookmarkNameTextField.clearsOnBeginEditing = NO;
			bookmarkNameTextField.font = [UIFont systemFontOfSize:17];
			bookmarkNameTextField.text = bookmarkName;
			bookmarkNameTextField.clearButtonMode = UITextFieldViewModeWhileEditing;
			
			bookmarkNameTextField.delegate = self;
			
			[cell addSubview:bookmarkNameTextField];
			
			[bookmarkNameTextField release];
		}
		else if([indexPath row] == 1)
		{
			if(isInLandscapeMode)
			{
				bookmarkURLTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 450, 30)];
			}
			else
			{
				bookmarkURLTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 290, 30)];
			}
			
			bookmarkURLTextField.placeholder = @"Bookmark Link";
			bookmarkURLTextField.clearsOnBeginEditing = NO;
			bookmarkURLTextField.font = [UIFont systemFontOfSize:17];
			bookmarkURLTextField.text = bookmarkLink;
			bookmarkURLTextField.clearButtonMode = UITextFieldViewModeWhileEditing;
			
			bookmarkURLTextField.delegate = self;
			
			[cell addSubview:bookmarkURLTextField];
			
			[bookmarkURLTextField release];
		}
		else if([indexPath row] == 2)
		{
			if(isInLandscapeMode)
			{
				bookmarkDescTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 450, 30)];
			}
			else
			{
				bookmarkDescTextField = [[UITextField alloc] initWithFrame:CGRectMake(15, 10, 290, 30)];
			}
			
			bookmarkDescTextField.placeholder = @"Bookmark Description";
			bookmarkDescTextField.clearsOnBeginEditing = NO;
			bookmarkDescTextField.font = [UIFont systemFontOfSize:17];
			bookmarkDescTextField.clearButtonMode = UITextFieldViewModeWhileEditing;
			
			[cell addSubview:bookmarkDescTextField];
			
			[bookmarkDescTextField release];
		}
	}
	else
	{
		UIImage* iconImage = nil;
		
		UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(60, 0, 200, 18)];
		cellTextLabel.highlightedTextColor = [UIColor whiteColor];
		cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
		cellTextLabel.backgroundColor = [UIColor clearColor];
		
		iconImage = [UIImage imageNamed:@"FolderIcon.png"];
		
		cellTextLabel.frame = CGRectMake(60, 0, 250, 40);
		
		cellTextLabel.text = bookmarkFolder;
		
		cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
		
		
		UIImageView* iconImageView = [[UIImageView alloc]initWithImage:iconImage];
		iconImageView.frame = CGRectMake(10, 2, 40, 40);
		[cell.contentView addSubview:iconImageView];
		
		[cell.contentView addSubview: cellTextLabel];
		
		[cellTextLabel release];
		
		[iconImageView release];
		
	}
	
    return cell;
}

- (void)alertView:(UIAlertView *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
	if(buttonIndex == 1)
	{
		NSArray* bookmarkLinks = [bookmarkFoldersDict objectForKey:bookmarkFolder];

		NSMutableArray* newBookmarkLinks = [[NSMutableArray alloc] initWithCapacity:([bookmarkLinks count] + 1)];
		[newBookmarkLinks addObjectsFromArray:bookmarkLinks];
		
		if([bookmarkLinks count] > 0)
		{
			for(int i = 0; i < [bookmarkLinks count]; ++i)
			{
				NSDictionary* bookmark = [bookmarkLinks objectAtIndex:i];
				
				NSString* name = [bookmark objectForKey:kIBrowserLinkName];
				
				if([name compare:bookmarkNameTextField.text options: NSCaseInsensitiveSearch] == NSOrderedSame)
				{
					[newBookmarkLinks removeObject:bookmark];
					
					break;
				}
			}
		}
		
		NSMutableDictionary* newBookmark = [[NSMutableDictionary alloc] init];
		[newBookmark setObject:bookmarkNameTextField.text forKey:kIBrowserLinkName];
		[newBookmark setObject:bookmarkURLTextField.text forKey:kIBrowserLinkURL];
		
		if(bookmarkDescTextField.text == nil)
		{
			[newBookmark setObject:@"" forKey:kIBrowserLinkDesc];
		}
		else
		{
			[newBookmark setObject:bookmarkDescTextField.text forKey:kIBrowserLinkDesc];
		}
		
		[newBookmarkLinks addObject:newBookmark];
		
		[bookmarkFoldersDict setObject:newBookmarkLinks forKey:bookmarkFolder];
		
		[[NSUserDefaults standardUserDefaults] setObject:bookmarkFoldersDict forKey:kIBrowserBookmarkFolders];
		
		[[NSUserDefaults standardUserDefaults] synchronize];
		
		[newBookmarkLinks release];
		
		[self.parentViewController dismissModalViewControllerAnimated:YES];		
	}
}

- (IBAction) cancelAction: (id) sender {
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (IBAction) saveAction: (id) sender {
	NSArray* bookmarkLinks = [bookmarkFoldersDict objectForKey:bookmarkFolder];
	
	if(bookmarkNameTextField.text == nil || bookmarkNameTextField.text.length <= 0)
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Please input a valid bookmark name."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
		
		return;
	}
	
	if(bookmarkURLTextField.text == nil || bookmarkURLTextField.text.length <= 0)
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Please input a valid bookmark URL."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
		
		return;
	}
	
	if([bookmarkLinks count] > 0)
	{
		for(int i = 0; i < [bookmarkLinks count]; ++i)
		{
			NSDictionary* bookmark = [bookmarkLinks objectAtIndex:i];
			
			NSString* name = [bookmark objectForKey:kIBrowserLinkName];
			
			if([name compare:bookmarkNameTextField.text options: NSCaseInsensitiveSearch] == NSOrderedSame)
			{
				UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Overwrite" message:@"A bookmark with that name already exists. Do you want to overwrite?"
															   delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
				[alert show];
				[alert release];
				
				return;
			}
		}
	}
	
	NSMutableDictionary* newBookmark = [[NSMutableDictionary alloc] init];
	[newBookmark setObject:bookmarkNameTextField.text forKey:kIBrowserLinkName];
	[newBookmark setObject:bookmarkURLTextField.text forKey:kIBrowserLinkURL];
	
	if(bookmarkDescTextField.text == nil)
	{
		[newBookmark setObject:@"" forKey:kIBrowserLinkDesc];
	}
	else
	{
		[newBookmark setObject:bookmarkDescTextField.text forKey:kIBrowserLinkDesc];
	}
	
	NSMutableArray* newBookmarkLinks = [[NSMutableArray alloc] initWithCapacity:([bookmarkLinks count] + 1)];
	[newBookmarkLinks addObjectsFromArray:bookmarkLinks];
	[newBookmarkLinks addObject:newBookmark];
	
	[bookmarkFoldersDict setObject:newBookmarkLinks forKey:bookmarkFolder];
	
	[[NSUserDefaults standardUserDefaults] setObject:bookmarkFoldersDict forKey:kIBrowserBookmarkFolders];
	
	[[NSUserDefaults standardUserDefaults] synchronize];
	
	[newBookmarkLinks release];
	
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

- (void) setBookmarkSaveFolder: (NSString*) folder {
	self.bookmarkFolder = folder;
	
	[inputTable reloadData];
}


- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    // Navigation logic may go here. Create and push another view controller.
	// AnotherViewController *anotherViewController = [[AnotherViewController alloc] initWithNibName:@"AnotherView" bundle:nil];
	// [self.navigationController pushViewController:anotherViewController];
	// [anotherViewController release];
	
	[inputTable deselectRowAtIndexPath:indexPath animated:YES];
	
	if(indexPath.section == 1)
	{
		SelectBookmarkFolderController* folderController = nil;
		
		if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			folderController = [[SelectBookmarkFolderController alloc] initWithNibName:@"BookmarksFolderListView" bundle:nil];
		}
		else
		{
			folderController = [[SelectBookmarkFolderController alloc] initWithNibName:@"BookmarksFolderListViewLandscape" bundle:nil];
		}
		
		folderController.saveBookmarkController = self;
		
		folderController.expectedInterfaceOrientation = self.interfaceOrientation;
		
		[self presentModalViewController:folderController animated:YES];
		
		[folderController release];
	}
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (void)dealloc {
	[bookmarkFoldersDict release];
	
    [super dealloc];
}


@end
