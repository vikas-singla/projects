//
//  ContactTableViewCell.h
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DialContactsView.h"
#import "ContactInformationWrapper.h"

@interface ContactTableViewCell : UITableViewCell {
	DialContactsView	*contactsView;
}

@property (nonatomic, retain) DialContactsView	*contactsView;

- (void) setContactInformationWrapper: (ContactInformationWrapper *)contactInfoWrapper;
- (void) reInitialize;
- (void) setGrayColor;
- (void) setWhiteColor;
@end
