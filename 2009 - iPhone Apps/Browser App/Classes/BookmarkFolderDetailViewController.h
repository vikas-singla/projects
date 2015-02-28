//
//  BookmarkFolderDetailViewController.h
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BookmarksController.h"

@interface BookmarkFolderDetailViewController : UIViewController <UITableViewDelegate, UITableViewDataSource, UIActionSheetDelegate> {
	IBOutlet	UITableView*	bookmarksTable;
	IBOutlet	UINavigationItem* topNavigationItem;
	IBOutlet	UINavigationItem* bottomNavigationItem;
	IBOutlet	UIBarButtonItem* bottomButton;
	
	NSUserDefaults			*userDefaults;
	
	BookmarksController* bookmarkController;
	
	NSMutableDictionary* bookmarkDict;
	NSMutableArray* bookmarkLinks;
	NSMutableArray* bookmarkFolders;
	
	NSString* showFolderName;
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property(nonatomic, retain) IBOutlet UIBarButtonItem* bottomButton;
@property(nonatomic, retain) IBOutlet UITableView*	bookmarksTable;
@property(nonatomic, retain) IBOutlet UINavigationItem* topNavigationItem;
@property(nonatomic, retain) IBOutlet UINavigationItem* bottomNavigationItem;
@property (nonatomic, assign) NSUserDefaults	*userDefaults;
@property (nonatomic, assign) NSMutableArray* bookmarkFolders;
@property (nonatomic, assign) NSMutableArray* bookmarkLinks;
@property (nonatomic, assign) NSMutableDictionary* bookmarkDict;
@property (nonatomic, retain) NSString* showFolderName;
@property (nonatomic, assign) BookmarksController* bookmarkController;

- (IBAction) doneBookmarks: (id) sender;
- (IBAction) editBookmarks: (id) sender;

@end
