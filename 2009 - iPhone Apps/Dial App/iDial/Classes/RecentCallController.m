//
//  RecentCallController.m
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "RecentCallController.h"
#import "CallLogsView.h"

@implementation RecentCallController

@synthesize contactListView;
@synthesize callMechanismInstance;

/*
// The designated initializer. Override to perform setup that is required before the view is loaded.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
        // Custom initialization
    }
    return self;
}
*/

/**
// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView {
}
*/

- (void)clearCallLog:(id)sender {
	if(callMechanismInstance != nil) {
		[callMechanismInstance clearLog];
		[contactListView reloadData];
	}
}


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	self.navigationItem.title = @"Recent Calls";
	
	UIBarButtonItem* clearLogItem = [[UIBarButtonItem alloc] initWithTitle:@"Clear" style:UIBarButtonItemStyleBordered target:self action:@selector(clearCallLog:)];									 
	
	self.navigationItem.rightBarButtonItem = clearLogItem;
	
	[clearLogItem release];
	
	if(callMechanismInstance == nil)
	{
		callMechanismInstance = [CallMechanism getInstance];
	}
	
    [super viewDidLoad];
}


/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (void)viewWillAppear:(BOOL)animated; {
	
	if(contactListView == nil)
	{
		CGRect parentFrame = self.view.frame;
		
		CGRect contactListFrame = CGRectMake(parentFrame.origin.x , parentFrame.origin.y, 
											 parentFrame.size.width, parentFrame.size.height);
		
		contactListView = [[UITableView alloc] initWithFrame: contactListFrame style: UITableViewStylePlain];
		
		contactListView.backgroundColor = [UIColor clearColor];
		
		[self.view addSubview:contactListView];
		
		contactListView.dataSource = self;
		contactListView.delegate = self;
	}
	
	if(contactListView != nil)
	{
		[contactListView reloadData];
	}
	
	[super viewWillAppear:animated];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
	
	[callMechanismInstance callNumberAtCallLogIndex:indexPath.row];
	
	[contactListView reloadData];
	
	[contactListView deselectRowAtIndexPath:indexPath animated:YES];
}


// Customize the number of rows in the table view.
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	return [callMechanismInstance.callLogs count];
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
	
	NSArray* contactInfo = [callMechanismInstance.callLogs objectAtIndex:indexPath.row];
	
    UITableViewCell *cell = (UITableViewCell *)[contactListView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];		
		cell.accessoryType = UITableViewCellAccessoryNone;
		
		// Set up the cell...
		cell.backgroundColor = [UIColor clearColor];
		
		cell.selectionStyle = UITableViewCellSelectionStyleBlue; 
    }
	else
	{
		NSArray* cellContentSubviews = cell.contentView.subviews;
		
		if(cellContentSubviews && [cellContentSubviews count] > 0)
		{
			for(int i = 0; i < [cellContentSubviews count]; ++i)
			{
				[[cellContentSubviews objectAtIndex:i] removeFromSuperview];
			}
		}
	}
	
	CGRect tzvFrame = CGRectMake(0.0, 0.0, cell.contentView.bounds.size.width, cell.contentView.bounds.size.height);
	CallLogsView* contactsView = [[CallLogsView alloc] initWithFrame:tzvFrame];		
	contactsView.callLogRecord = contactInfo;
	contactsView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	contactsView.backgroundColor = [UIColor clearColor];
	[cell.contentView addSubview:contactsView];
	
	if(([indexPath row] % 2) == 1)
	{
		contactsView.backgroundColor = [UIColor colorWithRed:216.0/255 green:216.0/255 blue:216.0/255 alpha:1.0];
	}
	else
	{
		contactsView.backgroundColor = [UIColor whiteColor];
	}
	
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
	[callMechanismInstance callNumberAtCallLogIndex:indexPath.row];
	
	[contactListView reloadData];
}

- (void)dealloc {
    [super dealloc];
}


@end
