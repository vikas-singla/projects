//
//  CustomTabBarController.h
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class KeypadViewController;
@class KeyboardViewController;
@class ContactInformationController;

@interface CustomTabBarController : UITabBarController <UITabBarControllerDelegate> {
	IBOutlet	KeypadViewController	*keypadController;
	IBOutlet	KeyboardViewController	*keyboardController;
	
	ContactInformationController	*contactInfoController;
	
	
	NSUserDefaults					*userDefaults;
}


@property (nonatomic, retain)   NSUserDefaults		*userDefaults;
@property (nonatomic, retain) KeypadViewController		*keypadController;
@property (nonatomic, retain) KeyboardViewController	*keyboardController;

@property (nonatomic, assign)	ContactInformationController	*contactInfoController;

@end
