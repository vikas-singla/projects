//
//  KeypadViewController.h
//  Fast Dial
//
//  Created by Vikas on 12/14/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <CoreFoundation/CFArray.h>
#import <AddressBook/AddressBook.h>
#import "CallMechanism.h"
#import "SettingsConstants.h"
#import "SoundEffect.h"

@class ContactInformationController;

@interface KeypadViewController : UIViewController <UITableViewDelegate, UITableViewDataSource> {
	IBOutlet UIButton	*keypadDigitOne;
	IBOutlet UIButton	*keypadDigitTwo;
	IBOutlet UIButton	*keypadDigitThree;
	IBOutlet UIButton	*keypadDigitFour;
	IBOutlet UIButton	*keypadDigitFive;
	IBOutlet UIButton	*keypadDigitSix;
	IBOutlet UIButton	*keypadDigitSeven;
	IBOutlet UIButton	*keypadDigitEight;
	IBOutlet UIButton	*keypadDigitNine;
	IBOutlet UIButton	*keypadDigitZero;
	IBOutlet UIButton	*keypadDigitDelete;
	IBOutlet UIButton	*keypadDigitHideShow;
	IBOutlet UIButton	*keypadDigitCall;
	IBOutlet UIButton	*keypadDigitStar;
	IBOutlet UIButton	*keypadDigitPound;
	IBOutlet UILabel	*keypadDigitLabel;
	IBOutlet UIView		*keypadView;
	
	UIActivityIndicatorView* activityIndicator;
	UIImageView* activityImageView;
	UITableView	*contactListView;
	
	SoundEffect	*keypadDigitOneSound;
	SoundEffect	*keypadDigitTwoSound;
	SoundEffect	*keypadDigitThreeSound;
	SoundEffect	*keypadDigitFourSound;
	SoundEffect *keypadDigitFiveSound;
	SoundEffect	*keypadDigitSixSound;
	SoundEffect	*keypadDigitSevenSound;
	SoundEffect	*keypadDigitEightSound;
	SoundEffect	*keypadDigitNineSound;
	SoundEffect	*keypadDigitZeroSound;
	
	BOOL keypadIsHidden;
	
	ContactInformationController	*contactInfoController;
	CallMechanism					*callMechanismInstance;
	NSUserDefaults					*userDefaults;
	NSTimer*						deleteDigitsTimer;
	
	NSArray*	keypadContactListViewData;
}

@property (nonatomic, assign)	NSArray*	keypadContactListViewData;

@property (nonatomic, retain)  UIActivityIndicatorView* activityIndicator;
@property (nonatomic, retain)  UIImageView* activityImageView;

@property (nonatomic, assign)	SoundEffect			*keypadDigitOneSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitTwoSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitThreeSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitFourSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitFiveSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitSixSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitSevenSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitEightSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitNineSound;
@property (nonatomic, assign)	SoundEffect			*keypadDigitZeroSound;

@property (nonatomic, assign)	NSTimer				*deleteDigitsTimer;
@property (nonatomic, retain)   NSUserDefaults		*userDefaults;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitOne;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitTwo;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitThree;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitFour;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitFive;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitSix;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitSeven;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitEight;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitNine;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitZero;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitDelete;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitHideShow;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitCall;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitStar;
@property (nonatomic, retain)	IBOutlet UIButton	*keypadDigitPound;
@property (nonatomic, retain)	IBOutlet UILabel	*keypadDigitLabel;
@property (nonatomic, retain)	IBOutlet UIView		*keypadView;
@property (nonatomic, retain)	UITableView	*contactListView;
@property (nonatomic, assign)	BOOL	keypadIsHidden;
@property (nonatomic, assign)	CallMechanism					*callMechanismInstance;
@property (nonatomic, assign)	ContactInformationController	*contactInfoController;

- (IBAction) digitPressed: (id) sender;
- (IBAction) deleteDigitPressedDown: (id) sender;

- (void) refreshKeypadContactListView: (id) anObject;

@end
