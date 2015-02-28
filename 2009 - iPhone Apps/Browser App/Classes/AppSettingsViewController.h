//
//  AppSettingsViewController.h
//  Fast Dial
//
//  Created by Vikas on 12/20/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class iBrowserViewController;

@interface AppSettingsViewController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	IBOutlet UITableView				*myTableView;
	NSUserDefaults			*userDefaults;
	IBOutlet UIBarButtonItem			*doneButton;
	
	UISwitch* tiltScrollSwitch;
	UISwitch* manualAutoScrollSwitch;
	
	bool compressSettingChanged;
	
	iBrowserViewController* browserViewController;
	
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property (nonatomic, assign) iBrowserViewController* browserViewController;

@property (nonatomic, retain) IBOutlet UITableView *myTableView;
@property (nonatomic, assign) NSUserDefaults	*userDefaults;
@property (nonatomic, retain) IBOutlet UIBarButtonItem	*doneButton;

@end
