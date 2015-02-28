//
//  DialContactsView.h
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "ContactInformationWrapper.h"
#import <AddressBook/AddressBook.h>
#import <AddressBook/ABPerson.h>

@interface DialContactsView : UIView {
	ContactInformationWrapper	*contactRecord;
	BOOL			highlighted;
	BOOL			editing;
}

@property (nonatomic, assign) ContactInformationWrapper	*contactRecord;
@property (nonatomic, getter=isHighlighted) BOOL highlighted;
@property (nonatomic, getter=isEditing) BOOL editing;

@end
