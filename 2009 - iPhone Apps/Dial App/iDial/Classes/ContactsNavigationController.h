//
//  ContactsNavigationController.h
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AddressBookUI/ABPeoplePickerNavigationController.h>
#import <AddressBookUI/ABPersonViewController.h>
#import <AddressBookUI/ABNewPersonViewController.h>
#import <AddressBookUI/ABUnknownPersonViewController.h>

@interface ContactsNavigationController : ABPeoplePickerNavigationController <UINavigationControllerDelegate, ABPersonViewControllerDelegate, ABPeoplePickerNavigationControllerDelegate, ABNewPersonViewControllerDelegate> {
	BOOL displayAddBarButton;
}

@property(nonatomic, assign) BOOL displayAddBarButton;

@end
