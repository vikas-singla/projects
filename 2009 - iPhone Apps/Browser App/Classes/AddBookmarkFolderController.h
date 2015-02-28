//
//  AddBookmarkFolderController.h
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BookmarksController.h"

@interface AddBookmarkFolderController : UIViewController {
	IBOutlet	UITextField* titleField;
	
	BookmarksController* bookmarkController;
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property (nonatomic, retain) IBOutlet UITextField* titleField;
@property (nonatomic, assign) BookmarksController* bookmarkController;

- (IBAction) saveFolder: (id) sender;
- (IBAction) cancelNewFolder: (id)sender;

@end
