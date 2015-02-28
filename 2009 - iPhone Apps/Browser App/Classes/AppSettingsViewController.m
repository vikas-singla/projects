//
//  AppSettingsViewController.m
//  Fast Dial
//
//  Created by Vikas on 12/20/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "AppSettingsViewController.h"
#import "iBrowserViewController.h"
#import "SettingsConstants.h"
#import "SourceCell.h"
#import "DisplayCell.h"
#import "SourceHelpCell.h"

#define kSwitchButtonWidth		94.0
#define kSwitchButtonHeight		27.0

@implementation AppSettingsViewController

@synthesize expectedInterfaceOrientation;
@synthesize browserViewController;
@synthesize myTableView;
@synthesize doneButton;
@synthesize userDefaults;

enum ControlTableSections
{
	kUIAutoScroll = 0,
	kUIManualAutoScroll,
	kUITabStyle_section,
	kUIMobile_section,
	kUIHistory_section,
	kUIRememberLastSession,
	kUITranparency_section,
	kUIBrowserConfig_Section,
	kUIShowProgressBar,
	kUISearchEngine_section,
	kUIBrowserTheme_section
};

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

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	bool isPortrait = UIInterfaceOrientationIsPortrait(expectedInterfaceOrientation) && UIInterfaceOrientationIsPortrait(interfaceOrientation);
	bool isLandscape = UIInterfaceOrientationIsLandscape(expectedInterfaceOrientation) && UIInterfaceOrientationIsLandscape(interfaceOrientation);
	
	return (isPortrait || isLandscape);
}

- (void)doneButtonPressed:(id)sender {
	
	if(compressSettingChanged)
	{
		[browserViewController forceAutoRefresh];
	}
	
	[self.parentViewController dismissModalViewControllerAnimated:YES];
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	userDefaults = [NSUserDefaults standardUserDefaults];
	
	// create and configure the table view
	myTableView.delegate = self;
	myTableView.dataSource = self;
	myTableView.autoresizesSubviews = YES;
	[self.view addSubview: myTableView];
	
	doneButton.target = self;
	doneButton.action = @selector(doneButtonPressed:);
	
	myTableView.backgroundColor = [UIColor clearColor];
	
	compressSettingChanged = NO;
}

/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (void)switchActionShakeToShow:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		[userDefaults setObject:kIBrowserTrue forKey:kIbrowserShakeToShow];
	}
	else
	{
		[userDefaults setObject:kIBrowserFalse forKey:kIbrowserShakeToShow];
	}
	
	[userDefaults synchronize];
}

- (void)switchActionManualAutoScroll:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserAutoScroll];
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserManualAutoScroll];
		
		[tiltScrollSwitch setOn:NO animated:YES];
	}
	else
	{
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserManualAutoScroll];
	}
	
	[userDefaults synchronize];
}

- (void)switchActionShowProgressBar:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserShowProgressBar];
	}
	else
	{
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserShowProgressBar];
	}
}

- (void)switchActionCompressPages:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserCompressPages];
	}
	else
	{
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserCompressPages];
		
		[myTableView reloadData];
	}
	
	compressSettingChanged = YES;
}

- (void)switchActionDisplayHistory:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{		
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserDisplayHistory];
	}
	else
	{		
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserDisplayHistory];
	}
}

- (void)switchActionAutoScroll:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{			
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserManualAutoScroll];
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserAutoScroll];
		
		[manualAutoScrollSwitch setOn:NO animated:YES];
	}
	else
	{		
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserAutoScroll];
	}
	
	[userDefaults synchronize];
}

- (void)switchActionAlwaysDisplayHomePage:(id)sender
{
	// NSLog(@"switchAction: value = %d", [sender isOn]);
	if([sender isOn]) 
	{		
		[userDefaults setObject:kIBrowserTrue forKey:kIBrowserLaunchHomePage];
	}
	else
	{		
		[userDefaults setObject:kIBrowserFalse forKey:kIBrowserLaunchHomePage];
	}
}


- (UISwitch*)create_UISwitch_ShakeToShow
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionShakeToShow:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIbrowserShakeToShow];
	
	if([value compare:kIBrowserTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_ShowProgressBar
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionShowProgressBar:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIBrowserShowProgressBar];
	
	if([value compare:kIBrowserTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_ManualAutoScroll
{	
	if(manualAutoScrollSwitch == nil)
	{
		CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
		UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
		[switchCtl addTarget:self action:@selector(switchActionManualAutoScroll:) forControlEvents:UIControlEventValueChanged];
		
		// in case the parent view draws with a custom color or gradient, use a transparent color
		switchCtl.backgroundColor = [UIColor clearColor];
		
		NSString* value = [userDefaults stringForKey:kIBrowserManualAutoScroll];
		
		if([value compare:kIBrowserTrue] == NSOrderedSame)
		{
			switchCtl.on = YES;
		}
		else
		{
			switchCtl.on = NO;
		}
		
		manualAutoScrollSwitch = switchCtl;
	}
	
	return manualAutoScrollSwitch;
}

- (UISwitch*)create_UISwitch_CompressPages
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionCompressPages:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIBrowserCompressPages];
	
	if([value compare:kIBrowserTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}


- (UISwitch*)create_UISwitch_DisplayHistory
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionDisplayHistory:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIBrowserDisplayHistory];
	
	if([value compare:kIBrowserTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_AlwaysDisplayHomePage
{	
	CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
	UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
	[switchCtl addTarget:self action:@selector(switchActionAlwaysDisplayHomePage:) forControlEvents:UIControlEventValueChanged];
	
	// in case the parent view draws with a custom color or gradient, use a transparent color
	switchCtl.backgroundColor = [UIColor clearColor];
	
	NSString* value = [userDefaults stringForKey:kIBrowserLaunchHomePage];
	
	if([value compare:kIBrowserTrue] == NSOrderedSame)
	{
		switchCtl.on = YES;
	}
	else
	{
		switchCtl.on = NO;
	}
	
	return switchCtl;
}

- (UISwitch*)create_UISwitch_AutoScroll
{	
	if(tiltScrollSwitch == nil)
	{
		CGRect frame = CGRectMake(0.0, 0.0, kSwitchButtonWidth, kSwitchButtonHeight);
		UISwitch* switchCtl = [[UISwitch alloc] initWithFrame:frame];
		[switchCtl addTarget:self action:@selector(switchActionAutoScroll:) forControlEvents:UIControlEventValueChanged];
		
		// in case the parent view draws with a custom color or gradient, use a transparent color
		switchCtl.backgroundColor = [UIColor clearColor];
		
		NSString* value = [userDefaults stringForKey:kIBrowserAutoScroll];
		
		if([value compare:kIBrowserTrue] == NSOrderedSame)
		{
			switchCtl.on = YES;
		}
		else
		{
			switchCtl.on = NO;
		}
		
		tiltScrollSwitch = switchCtl;
	}
	return tiltScrollSwitch;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    /*
	 To conform to the Human Interface Guidelines, selections should not be persistent --
	 deselect the row after it has been selected.
	 */
	[myTableView deselectRowAtIndexPath:indexPath animated:YES];
	
	switch (indexPath.section)
	{
		case kUITabStyle_section:			
		{
			if ([indexPath row] == 0)
			{
				[userDefaults setObject:kIbrowserTabStyleDeskstop forKey:kIbrowserTabStyle];
			}
			else if ([indexPath row] == 1)
			{
				[userDefaults setObject:kIbrowserTabStyleTable forKey:kIbrowserTabStyle];
			}
			
			break;
		}
		case kUIBrowserTheme_section:			
		{
			if ([indexPath row] == 0)
			{
				[userDefaults setObject:kIBrowserThemeBlack forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 1)
			{
				[userDefaults setObject:kIBrowserThemeBlue forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 2)
			{
				[userDefaults setObject:kIBrowserThemeBrown forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 3)
			{
				[userDefaults setObject:kIBrowserThemeGreen forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 4)
			{
				[userDefaults setObject:kIBrowserThemeGray forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 5)
			{
				[userDefaults setObject:kIBrowserThemePink forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 6)
			{
				[userDefaults setObject:kIBrowserThemeRed forKey:kIBrowserTheme];
			}
			else if ([indexPath row] == 7)
			{
				[userDefaults setObject:kIBrowserThemeOrange forKey:kIBrowserTheme];
			}
			
			[NSThread detachNewThreadSelector:@selector(setTheme:) toTarget:browserViewController withObject:nil];
			
			//[browserViewController setTheme];
			
			break;
		}			
		case kUISearchEngine_section:			
		{
			if ([indexPath row] == 0)
			{
				[userDefaults setObject:kIBrowserAskVal forKey:kIBrowserSearchEngine];
			}
			else if ([indexPath row] == 1)
			{
				[userDefaults setObject:kIBrowserBaiduVal forKey:kIBrowserSearchEngine];
			}
			else if ([indexPath row] == 2)
			{
				[userDefaults setObject:kIBrowserGoogleVal forKey:kIBrowserSearchEngine];
			}
			else if ([indexPath row] == 3)
			{
				[userDefaults setObject:kIBrowserLiveVal forKey:kIBrowserSearchEngine];
			}
			else if ([indexPath row] == 4)
			{
				[userDefaults setObject:kIBrowserYahooVal forKey:kIBrowserSearchEngine];
			}
			
			break;
		}
		case kUIBrowserConfig_Section:
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

- (void)transparencySliderAction:(id)sender
{
	UISlider* slider = (UISlider*) sender;
	
	[userDefaults setObject:[NSString stringWithFormat:@"%f", slider.value] forKey:kIbrowserTransparency];
}

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
	return 11;
}

- (UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section {
	
	UILabel* hdr = nil;
	
	if([[UIApplication sharedApplication] statusBarOrientation] == UIInterfaceOrientationPortrait || [[UIApplication sharedApplication] statusBarOrientation] == UIInterfaceOrientationPortraitUpsideDown)
	{
		hdr = [[[UILabel alloc] initWithFrame:CGRectMake(20, 2, 280, 45)] autorelease];
	}
	else
	{
		hdr = [[[UILabel alloc] initWithFrame:CGRectMake(20, 2, 440, 45)] autorelease];
	}
	
	
	hdr.textAlignment = UITextAlignmentLeft;
	hdr.font = [UIFont boldSystemFontOfSize:16.0];
	hdr.opaque = NO ;
	hdr.lineBreakMode = UILineBreakModeWordWrap;
	hdr.shadowColor = [UIColor clearColor];
	hdr.shadowOffset = CGSizeMake(2,2);
	hdr.textColor = [UIColor blackColor];
	hdr.backgroundColor = [UIColor clearColor];
	
	
	switch (section)
	{
		case kUIAutoScroll:
		{
			hdr.text = @"  Tilt-scroll Web Pages";
			break;
		}
		case kUIManualAutoScroll:
		{
			hdr.text = @"  Auto-scroll Web Pages";
			break;
		}
		case kUIMobile_section:
		{
			hdr.text = @"  Mobile Browsing Configuration";
			break;
		}
		case kUIHistory_section:
		{
			hdr.text = @"  History Tracking";
			break;
		}
		case kUIRememberLastSession:
		{
			hdr.text = @"  When Browser is Launched";
			break;
		}
		case kUITabStyle_section:
		{
			hdr.text = @"  Tabs Style";
			break;
		}
		case kUISearchEngine_section:
		{
			hdr.text = @"  Search Engine";
			break;
		}
		case kUIShowProgressBar:
		{
			hdr.text = @"  Progress Bar";
			break;
		}
		case kUIBrowserConfig_Section:
		{
			hdr.text = @"  Full Screen View: Menu Visibility";
			break;
		}
		case kUITranparency_section:
		{
			hdr.text = @"  Full Screen View: Menu Transparency";
			break;
		}
		case kUIBrowserTheme_section:
		{
			hdr.text = @"  Theme";
			break;
		}
	}
	
	return hdr ;
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section {
	
	return 46;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSInteger result = 1;
	switch (section)
	{
		case kUIAutoScroll:
		{
			result = 2;
			break;
		}
		case kUIManualAutoScroll:
		{
			result = 2;
			break;
		}
		case kUITabStyle_section:
		{
			result = 2;
			break;
		}
		case kUIMobile_section:
		{
			result = 2;
			break;
		}
		case kUIHistory_section:
		{
			result = 2;
			break;
		}
		case kUIRememberLastSession:
		{
			result = 2;
			break;
		}
		case kUISearchEngine_section:
		{
			result = 5;
			break;
		}
		case kUIBrowserConfig_Section:
		{
			result = 2;
			break;
		}
		case kUIShowProgressBar:
		{
			result = 2;
			break;
		}
		case kUITranparency_section:
		{
			result = 2;
			break;
		}
		case kUIBrowserTheme_section:
		{
			result = 8;
			break;
		}
	}
	return result;
}

- (UITableViewCell*) obtainThemeCell {
	UITableViewCell *cell = nil;
	
	cell = [myTableView dequeueReusableCellWithIdentifier:kSourceCell_ID];
	
	if(cell == nil)
	{
		cell = [[[SourceCell alloc] initWithFrame:CGRectZero reuseIdentifier:kSourceCell_ID] autorelease];
	}
	
	return cell;
}

- (UITableViewCell*) obtainDisplayCell {
	UITableViewCell *cell = nil;
	
	cell = [myTableView dequeueReusableCellWithIdentifier:kDisplayCell_ID];
	
	if(cell == nil)
	{
		cell = [[[DisplayCell alloc] initWithFrame:CGRectZero reuseIdentifier:kDisplayCell_ID] autorelease];
	}
	
	return cell;
}

- (UITableViewCell*) obtainSourceCell {
	UITableViewCell *cell = nil;
	
	cell = [myTableView dequeueReusableCellWithIdentifier:kSourceHelpCell_ID];
	
	if(cell == nil)
	{
		cell = [[[SourceHelpCell alloc] initWithFrame:CGRectZero reuseIdentifier:kSourceHelpCell_ID] autorelease];
	}
	
	return cell;
}

- (UITableViewCell*) obtainRegularCell {
	static NSString *CellIdentifier = @"CellSlider";
	
	UITableViewCell *cell = nil;
	
	cell = [myTableView  dequeueReusableCellWithIdentifier:CellIdentifier];
	
	if(cell == nil)
	{
		cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
	}
	
	return cell;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{	
    UITableViewCell *cell = nil;
	
	cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
	
	switch (indexPath.section)
	{
		case kUIHistory_section:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Enable History";
				
				((DisplayCell *)cell).view = [self create_UISwitch_DisplayHistory];				
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned off, the browser will not log any new entries to its history log.";
				
				cell.accessoryType = UITableViewCellAccessoryNone;				
			}
			
			break;
		}
		case kUIShowProgressBar:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Show Progress Bar";
				
				((DisplayCell *)cell).view = [self create_UISwitch_ShowProgressBar];				
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, the browser will show a progress bar for each web page load.";
				
				cell.accessoryType = UITableViewCellAccessoryNone;				
			}
			
			break;
		}
		case kUIAutoScroll:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Enable Tilt-scroll";
								
				((DisplayCell *)cell).view = [self create_UISwitch_AutoScroll];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, the browser will automatically scroll web pages based on the orientation of the device. (Recommended with orientation lock ON)";
				
				cell.accessoryType = UITableViewCellAccessoryNone;
			}
			
			break;
		}
		case kUIManualAutoScroll:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Enable Auto-scroll";
				
				((DisplayCell *)cell).view = [self create_UISwitch_ManualAutoScroll];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, the browser will show auto-scroll buttons (which enable you to scroll in a manner that is similar to the mouse wheel auto-scroll function).";
				
				cell.accessoryType = UITableViewCellAccessoryNone;
			}
			
			break;
		}
		case kUIRememberLastSession:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Show Home Page";
				
				((DisplayCell *)cell).view = [self create_UISwitch_AlwaysDisplayHomePage];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, the browser will start a new session and show the home page when it is opened. Otherwise, the browser will resume your last session.";
				
				cell.accessoryType = UITableViewCellAccessoryNone;
			}
			
			break;
		}	
		case kUIMobile_section:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Compress Pages";
				
				((DisplayCell *)cell).view = [self create_UISwitch_CompressPages];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, web pages will be converted to mobile format, which will enable the browser to load pages faster and help save you bandwidth.";

				cell.accessoryType = UITableViewCellAccessoryNone;
			}
			break;
		}
		case kUITabStyle_section:
		{
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Desktop Browser Style";
				
				if([[userDefaults stringForKey:kIbrowserTabStyle] compare:kIbrowserTabStyleDeskstop] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Table List Style";
				
				if([[userDefaults stringForKey:kIbrowserTabStyle] compare:kIbrowserTabStyleTable] == NSOrderedSame)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			
			break;
		}
		case kUIBrowserTheme_section:			
		{			
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Black";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBlack] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Blue";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBlue] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Brown";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBrown] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Green";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeGreen] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Gray";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeGray] == NSOrderedSame)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 5)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Pink";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemePink] == NSOrderedSame)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 6)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Red";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeRed] == NSOrderedSame)
				{
					cell.accessoryType = UITableViewCellAccessoryCheckmark;
				}
				else
				{
					cell.accessoryType = UITableViewCellAccessoryNone;
				}
			}
			else if ([indexPath row] == 7)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Orange";
				
				if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeOrange] == NSOrderedSame)
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
		case kUISearchEngine_section:			
		{			
			if ([indexPath row] == 0)
			{
				// this cell hosts the UISwitch control
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Ask";
				
				if([[userDefaults stringForKey:kIBrowserSearchEngine] compare:kIBrowserAskVal] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Baidu";
				
				if([[userDefaults stringForKey:kIBrowserSearchEngine] compare:kIBrowserBaiduVal] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Google";
				
				if([[userDefaults stringForKey:kIBrowserSearchEngine] compare:kIBrowserGoogleVal] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Bing";
				
				if([[userDefaults stringForKey:kIBrowserSearchEngine] compare:kIBrowserLiveVal] == NSOrderedSame)
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
				cell = [self obtainThemeCell];
				
				((SourceCell *)cell).sourceLabel.text = @"Yahoo";
				
				if([[userDefaults stringForKey:kIBrowserSearchEngine] compare:kIBrowserYahooVal] == NSOrderedSame)
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
		case kUIBrowserConfig_Section:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainDisplayCell];
				
				// this cell hosts the UISwitch control
				((DisplayCell *)cell).nameLabel.text = @"Shake to show only";
				
				((DisplayCell *)cell).view = [self create_UISwitch_ShakeToShow];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"When this setting is turned on, the full screen view, which is accessed using the expand button in the main browser screen, will not show any menus except when the device is shook.";
				
				cell.accessoryType = UITableViewCellAccessoryNone;
			}
			
			break;
		}			
		case kUITranparency_section:
		{
			if ([indexPath row] == 0)
			{
				cell = [self obtainRegularCell];
				
				UISlider* slider = nil;
				
				if([[UIApplication sharedApplication] statusBarOrientation] == UIInterfaceOrientationPortrait || [[UIApplication sharedApplication] statusBarOrientation] == UIInterfaceOrientationPortraitUpsideDown)
				{
					slider = [[UISlider alloc] initWithFrame:CGRectMake(10, 0, 280, 40)];
				}
				else
				{
					slider = [[UISlider alloc] initWithFrame:CGRectMake(10, 0, 440, 40)];
				}
				
				
				slider.continuous = YES;
				slider.minimumValue = 0.15f;
				slider.value = [userDefaults floatForKey:kIbrowserTransparency];
				
				slider.maximumValueImage = [UIImage imageNamed:@"MoreBarsIcon.jpg"];
				slider.minimumValueImage = [UIImage imageNamed:@"LessBarsIcon.jpg"];
				
				[slider addTarget:self action:@selector(transparencySliderAction:) forControlEvents:UIControlEventValueChanged];
				
				[cell.contentView addSubview: slider];
				
				cell.accessoryType = UITableViewCellAccessoryNone;
				
				[slider release];
			}
			else if ([indexPath row] == 1)
			{
				cell = [self obtainSourceCell];
				
				((SourceHelpCell *)cell).sourceLabel.text = @"This setting adjusts the transparency of the menus shown in full screen view, which is accessed using the expand button in the main browser screen.";
				
				cell.accessoryType = UITableViewCellAccessoryNone;
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
	[doneButton release];
    [super dealloc];
}


@end
