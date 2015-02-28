//
//  AppSettingsViewController.m
//  Fast Dial
//
//  Created by Vikas on 12/20/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "AppSettingsViewController.h"
#import "SettingsConstants.h"
#import "ContactInformationController.h"

#define kSwitchButtonWidth		94.0
#define kSwitchButtonHeight		27.0

@implementation AppSettingsViewController

@synthesize myTableView;
@synthesize userDefaults;

enum ControlTableSections
{
	kUIDisplayDefaultNumbersOnly_Section = 0,
	kUISearch_Section,
	kUIKeyboardSearch_Section,
	kUIKeypad_Section
};

+ (void)initialize{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSArray	*displayNumberValue = [NSArray arrayWithObjects: kIDialDisplayAllNumbers, nil];
    NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
								 displayNumberValue, kIDialSearchDisplayNumbers, kIDialTrue, kIDialSearchConfigRecentFirst, kIDialTrue, kIDialSearchConfigNotes, kIDialTrue, kIDialKeypadConfigTones, nil];
	
    [defaults registerDefaults:appDefaults];
}

// The designated initializer. Override to perform setup that is required before the view is loaded.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
        // Custom initialization
    }
    return self;
}

/*
 // Implement loadView to create a view hierarchy programmatically, without using a nib.
 - (void)loadView {
 }
 */


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	userDefaults = [NSUserDefaults standardUserDefaults];
	
	// create and configure the table view
	myTableView = [[UITableView alloc] initWithFrame:CGRectMake(0, 0, 320, 411) style:UITableViewStyleGrouped];	
	myTableView.delegate = self;
	myTableView.dataSource = self;
	myTableView.autoresizesSubviews = YES;
	[self.view addSubview: myTableView];
}

/*
 // Override to allow orientations other than the default portrait orientation.
 - (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
 // Return YES for supported orientations
 return (interfaceOrientation == UIInterfaceOrientationPortrait);
 }
 */

- (void)switchActionSearchRecentFirst:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		[userDefaults setObject:kIDialTrue forKey:kIDialSearchConfigRecentFirst];
	}
	else
	{
		[userDefaults setObject:kIDialFalse forKey:kIDialSearchConfigRecentFirst];
	}
}


- (void)switchActionKeypadTones:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		[userDefaults setObject:kIDialTrue forKey:kIDialKeypadConfigTones];
	}
	else
	{
		[userDefaults setObject:kIDialFalse forKey:kIDialKeypadConfigTones];
	}
}


- (void)switchActionSearchNotes:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		[userDefaults setObject:kIDialTrue forKey:kIDialSearchConfigNotes];
	}
	else
	{
		[userDefaults setObject:kIDialFalse forKey:kIDialSearchConfigNotes];
	}
}

- (UISwitch*)create_UISwitch_SearchRecentFirst
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionSearchRecentFirst:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIDialSearchConfigRecentFirst];
	
	if([value compare:kIDialTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_KeypadTones
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionKeypadTones:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIDialKeypadConfigTones];
	
	if([value compare:kIDialTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_SearchNotes
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionSearchNotes:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIDialSearchConfigNotes];
	
	if([value compare:kIDialTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

NSArray* checkAndAddValueToDisplayArray(NSArray* array, NSString* valueToAdd) {
	NSMutableArray* result = [NSMutableArray arrayWithCapacity:1];
	
	for(int i = 0; i < [array count]; ++i)
	{
		NSString* value = (NSString*)[array objectAtIndex:i];
		
		if([value compare:kIDialDisplayAllNumbers] != NSOrderedSame)
		{
			[result addObject:value];			
		}
	}
	
	if([result count] == 0 || [result indexOfObject:valueToAdd] == NSNotFound)
	{
		[result addObject:valueToAdd];
	}
	else if([result count] > 1) 
	{
		[result removeObject:valueToAdd];
	}
	
	return result;
	
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    /*
	 To conform to the Human Interface Guidelines, selections should not be persistent --
	 deselect the row after it has been selected.
	 */
	[myTableView deselectRowAtIndexPath:indexPath animated:YES];
	
	switch (indexPath.section)
	{
		case kUIDisplayDefaultNumbersOnly_Section:			
		{
			NSArray* displayArray = [userDefaults stringArrayForKey:kIDialSearchDisplayNumbers];
			
			if ([indexPath row] == 0)
			{
				displayArray = [NSArray arrayWithObjects: kIDialDisplayAllNumbers, nil];
				[userDefaults setObject:displayArray forKey:kIDialSearchDisplayNumbers];
			}
			else if ([indexPath row] == 1)
			{
				NSArray* newDisplayArray = checkAndAddValueToDisplayArray(displayArray, kIDialDisplayMainNumber);		
				[userDefaults setObject:newDisplayArray forKey:kIDialSearchDisplayNumbers];
			}
			else if ([indexPath row] == 2)
			{
				NSArray* newDisplayArray = checkAndAddValueToDisplayArray(displayArray, kIDialDisplayWorkNumber);
				[userDefaults setObject:newDisplayArray forKey:kIDialSearchDisplayNumbers];
			}
			else if ([indexPath row] == 3)
			{
				NSArray* newDisplayArray = checkAndAddValueToDisplayArray(displayArray, kIDialDisplayMobileNumber);	
				[userDefaults setObject:newDisplayArray forKey:kIDialSearchDisplayNumbers];
			}
			else if ([indexPath row] == 4)
			{
				NSArray* newDisplayArray = checkAndAddValueToDisplayArray(displayArray, kIDialDisplayHomeNumber);	
				[userDefaults setObject:newDisplayArray forKey:kIDialSearchDisplayNumbers];
			}
			
			[ContactInformationController updateDataSettingsUpdated];
			
			break;
		}
		case kUISearch_Section:
		{			
			break;
		}
		case kUIKeyboardSearch_Section:
		{
			break;
		}
		case kUIKeypad_Section:
		{
			break;
		}			
	}
	
	[myTableView reloadData];
}


- (UITableViewCellEditingStyle)tableView:(UITableView *)tableView editingStyleForRowAtIndexPath:(NSIndexPath *)indexPath
{
	return UITableViewCellEditingStyleNone;
}

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
	return 4;
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section
{
	NSString *title;
	switch (section)
	{
		case kUIDisplayDefaultNumbersOnly_Section:
		{
			title = @"Search Display";
			break;
		}
		case kUISearch_Section:
		{
			title = @"Global Search Configuration";
			break;
		}
		case kUIKeyboardSearch_Section:
		{
			title = @"Keyboard Search Configuration";
			break;
		}
		case kUIKeypad_Section:
		{
			title = @"Keypad Configuration";
			break;
		}
	}
	return title;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSInteger result = 1;
	switch (section)
	{
		case kUIDisplayDefaultNumbersOnly_Section:
		{
			result = 5;
			break;
		}
		case kUISearch_Section:
		{
			result = 1;
			break;
		}
		case kUIKeyboardSearch_Section:
		{
			result = 1;
			break;
		}
		case kUIKeypad_Section:
		{
			result = 1;
			break;
		}
	}
	return result;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	
    UITableViewCell *cell = (UITableViewCell *)[myTableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];		
		cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
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
	
	switch (indexPath.section)
	{
		case kUIDisplayDefaultNumbersOnly_Section:			
		{
			NSArray* displayArray = [userDefaults stringArrayForKey:kIDialSearchDisplayNumbers];
			
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"All phone numbers";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				if([displayArray count] > 0 && [displayArray indexOfObject:kIDialDisplayAllNumbers] != NSNotFound)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 1)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Main phone number";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				if([displayArray count] > 0 && [displayArray indexOfObject:kIDialDisplayMainNumber] != NSNotFound)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 2)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Work phone number";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				if([displayArray count] > 0 && [displayArray indexOfObject:kIDialDisplayWorkNumber] != NSNotFound)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 3)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Mobile phone number";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				if([displayArray count] > 0 && [displayArray indexOfObject:kIDialDisplayMobileNumber] != NSNotFound)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 4)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Home phone number";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				if([displayArray count] > 0 && [displayArray indexOfObject:kIDialDisplayHomeNumber] != NSNotFound)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			
			//[displayArray release];
			
			break;
		}
		case kUISearch_Section:
		{
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Display recent first";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				cell.accessoryView = [self create_UISwitch_SearchRecentFirst];
			}
			break;
		}
		case kUIKeyboardSearch_Section:
		{
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Search Notes (slower)";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				cell.accessoryView = [self create_UISwitch_SearchNotes];
			}
			break;
		}
		case kUIKeypad_Section:
		{
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 0, 250, 40)];
				cellTextLabel.text = @"Keypress Tones";
				cellTextLabel.highlightedTextColor = [UIColor whiteColor];
				cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
				cellTextLabel.backgroundColor = [UIColor clearColor];
				
				[cell.contentView addSubview: cellTextLabel];
				
				cell.accessoryView = [self create_UISwitch_KeypadTones];
			}
			break;
		}
	}
	
	return cell;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning]; // Releases the view if it doesn't have a superview
    // Release anything that's not essential, such as cached data
}


- (void)dealloc {
	[myTableView release];
	[userDefaults release];
    [super dealloc];
}


@end
