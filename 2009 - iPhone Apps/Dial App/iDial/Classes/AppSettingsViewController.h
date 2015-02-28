//
//  AppSettingsViewController.h
//  Fast Dial
//
//  Created by Vikas on 12/20/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface AppSettingsViewController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	UITableView				*myTableView;
	NSUserDefaults			*userDefaults;
}

@property (nonatomic, retain) UITableView *myTableView;
@property (nonatomic, retain) NSUserDefaults	*userDefaults;

@end
