//
//  AuthenticationChallengeController.h
//  iBrowser
//
//  Created by Vikas on 3/29/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class iBrowserViewController;

@interface AuthenticationChallengeController : UIViewController {
	IBOutlet	UITextField* nameField;
	IBOutlet	UITextField* passwordField;
	
	iBrowserViewController* browserController;
	
	UIInterfaceOrientation expectedInterfaceOrientation;
}

@property (nonatomic, assign) UIInterfaceOrientation expectedInterfaceOrientation;

@property (nonatomic, assign) iBrowserViewController *browserController;
@property (nonatomic, retain) IBOutlet UITextField* nameField;
@property (nonatomic, retain) IBOutlet UITextField* passwordField;

- (IBAction) logIn: (id) sender;
- (IBAction) cancelAuthentication: (id)sender;

@end
