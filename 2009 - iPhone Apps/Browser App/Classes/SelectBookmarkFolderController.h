//
//  SelectBookmarkFolderController.h
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "AddBookmarkController.h"
#import "EditBookmarkController.h"

@interface SelectBookmarkFolderController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	IBOutlet	UITableView*	folderTable;
	NSUserDefaults			*userDefaults;
	
	AddBookmarkController*	saveBookmarkController;
	
	EditBookmarkController* editBookmarkController;
	
	NSArray* bookmarkLinks;
	NSMutableArray* bookmarkFolders;
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property(nonatomic, retain) IBOutlet UITableView* folderTable;
@property (nonatomic, assign) NSUserDefaults	*userDefaults;
@property (nonatomic, assign) EditBookmarkController* editBookmarkController;
@property (nonatomic, assign) AddBookmarkController* saveBookmarkController;
@property (nonatomic, assign) NSMutableArray* bookmarkFolders;
@property (nonatomic, assign) NSArray* bookmarkLinks;

- (IBAction) cancelAction: (id) sender;

@end
