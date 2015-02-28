//
//  EditBookmarkController.h
//  iBrowser
//
//  Created by Vikas on 5/11/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface EditBookmarkController : UIViewController <UITableViewDelegate, UITableViewDataSource, UIAlertViewDelegate, UITextFieldDelegate> {
	IBOutlet UITableView* inputTable;
	
	UITextField* bookmarkNameTextField;
	UITextField* bookmarkURLTextField;
	UITextField* bookmarkDescTextField;
	
	NSDictionary* oldBookmarkSettings;
	NSString* oldBookmarkFolder;
	
	NSString* bookmarkName;
	NSString* bookmarkLink;
	NSString* bookmarkDesc;
	
	NSString* bookmarkFolder;
	
	bool isInLandscapeMode;
	
	NSMutableDictionary* bookmarkFoldersDict;
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property (nonatomic, assign) NSString* oldBookmarkFolder;
@property (nonatomic, assign) NSDictionary* oldBookmarkSettings;

@property (nonatomic, assign) bool isInLandscapeMode;
@property (nonatomic, assign) IBOutlet UITableView* inputTable;
@property (nonatomic, assign) NSString* bookmarkLink;
@property (nonatomic, assign) NSString* bookmarkName;
@property (nonatomic, assign) NSString* bookmarkDesc;
@property (nonatomic, assign) NSString* bookmarkFolder;
@property (nonatomic, assign) UITextField* bookmarkNameTextField;
@property (nonatomic, assign) UITextField* bookmarkURLTextField;
@property (nonatomic, assign) UITextField* bookmarkDescTextField;
@property (nonatomic, assign) NSMutableDictionary* bookmarkFoldersDict;

- (IBAction) cancelAction: (id) sender;
- (IBAction) saveAction: (id) sender;

- (void) setBookmarkSaveFolder: (NSString*) folder;

@end
