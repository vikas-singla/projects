//
//  BookmarksController.h
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "iBrowserViewController.h"

@interface BookmarksController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	IBOutlet	UITableView*	bookmarksTable;
	IBOutlet	UINavigationItem* topNavigationItem;
	IBOutlet	UINavigationItem* bottomNavigationItem;
	
	iBrowserViewController* browserController;
	
	NSUserDefaults			*userDefaults;
	
	NSMutableDictionary* bookmarkDict;
	NSMutableArray* bookmarkLinks;
	NSMutableArray* bookmarkFolders;
	
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property(nonatomic, retain) IBOutlet UITableView*	bookmarksTable;
@property(nonatomic, retain) IBOutlet UINavigationItem* topNavigationItem;
@property(nonatomic, retain) IBOutlet UINavigationItem* bottomNavigationItem;
@property (nonatomic, assign) NSUserDefaults	*userDefaults;
@property (nonatomic, assign) NSMutableArray* bookmarkFolders;
@property (nonatomic, assign) NSMutableArray* bookmarkLinks;
@property (nonatomic, assign) NSMutableDictionary* bookmarkDict;
@property (nonatomic, assign) iBrowserViewController* browserController;

- (IBAction) doneBookmarks: (id) sender;
- (IBAction) editBookmarks: (id) sender;

- (BOOL) createFolder: (NSString*) name;
- (void) loadWebPage: (NSString*) linkURL;
@end
