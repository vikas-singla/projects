//
//  ContactInformationWrapper.h
//  CustomTableViewCell
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ContactInformationWrapper : NSObject {
	NSString	*contactFirstName;
	NSInteger	contactFirstnameLength;
	NSString	*contactLastName;
	NSInteger	contactLastNameLength;
	NSString	*contactPhoneNumber;
	NSInteger	contactPhoneNumberLength;
	NSString	*baseContactPhoneNumber;
	NSInteger	baseContactPhoneNumberLength;
	NSString	*contactPhoneNumberType;
	NSInteger	contactPhoneNumberTypeLength;
	NSString	*contactOrganization;
	NSInteger	contactOrganizationLength;
	NSString	*contactNotes;
	NSInteger	contactNotesLength;
	NSString	*contactDisplayName;
	NSInteger	contactDisplayNameLength;
	NSInteger	callLogRecencyIndex;
}

@property (nonatomic, assign)	NSString	*contactDisplayName;
@property (nonatomic, assign)	NSInteger	contactDisplayNameLength;
@property (nonatomic, assign)	NSString	*contactNotes;
@property (nonatomic, assign)	NSInteger	contactNotesLength;
@property (nonatomic, assign)   NSInteger	callLogRecencyIndex;
@property (nonatomic, assign)	NSString	*contactFirstName;
@property (nonatomic, assign)	NSInteger	contactFirstnameLength;
@property (nonatomic, assign)	NSString	*contactLastName;
@property (nonatomic, assign)	NSInteger	contactLastNameLength;
@property (nonatomic, assign)	NSString	*contactPhoneNumber;
@property (nonatomic, assign)	NSInteger	contactPhoneNumberLength;
@property (nonatomic, assign)	NSString	*baseContactPhoneNumber;
@property (nonatomic, assign)	NSInteger	baseContactPhoneNumberLength;
@property (nonatomic, assign)	NSString	*contactPhoneNumberType;
@property (nonatomic, assign)	NSInteger	contactPhoneNumberTypeLength;
@property (nonatomic, assign)	NSString	*contactOrganization;
@property (nonatomic, assign)	NSInteger	contactOrganizationLength;

- (id) initWithContactInfo:(NSString *) personFirstName : (NSString *) personLastName : (NSString *) phoneNum : (NSString *) phoneNumLabel : (NSString *) organization : (NSString*) notes : (NSString*) basicContactPhoneNumber;

- (void)dealloc;

@end
