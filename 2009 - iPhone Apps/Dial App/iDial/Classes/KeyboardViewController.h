//
//  KeyboardViewController.h
//  Fast Dial
//
//  Created by Vikas on 12/17/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "CallMechanism.h"
#import "SettingsConstants.h"

@class ContactInformationController;

@interface KeyboardViewController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	IBOutlet UISearchBar		*mySearchBar;
	
	IBOutlet UITableView	*contactListView;
	
	ContactInformationController	*contactInfoController;
	CallMechanism					*callMechanismInstance;
	
	NSUserDefaults					*userDefaults;
	
	NSString	*searchBarText;
	
	NSArray	*contactListViewData;
	
	UIActivityIndicatorView* activityIndicator;
	UIImageView* activityImageView;
}

@property (nonatomic, retain)  UIActivityIndicatorView* activityIndicator;
@property (nonatomic, retain)  UIImageView* activityImageView;

@property (nonatomic, assign)	NSArray	*contactListViewData;
@property (nonatomic, retain)	NSString*	searchBarText;
@property (nonatomic, retain)   NSUserDefaults		*userDefaults;
@property (nonatomic, retain)	UISearchBar *mySearchBar;
@property (nonatomic, retain)	IBOutlet UITableView	*contactListView;
@property (nonatomic, assign)	CallMechanism					*callMechanismInstance;
@property (nonatomic, assign)	ContactInformationController	*contactInfoController;

- (void) refreshContactListView: (id) anObject;

@end
