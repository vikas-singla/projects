//
//  ContactInformationWrapper.m
//  CustomTableViewCell
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "ContactInformationWrapper.h"


@implementation ContactInformationWrapper

@synthesize contactDisplayName;
@synthesize contactDisplayNameLength;
@synthesize contactFirstName;
@synthesize contactFirstnameLength;
@synthesize contactLastName;
@synthesize contactLastNameLength;
@synthesize contactPhoneNumber;
@synthesize contactPhoneNumberLength;
@synthesize baseContactPhoneNumber;
@synthesize baseContactPhoneNumberLength;
@synthesize	contactPhoneNumberType;
@synthesize contactPhoneNumberTypeLength;
@synthesize contactOrganization;
@synthesize contactOrganizationLength;
@synthesize callLogRecencyIndex;
@synthesize contactNotes;
@synthesize contactNotesLength;

- (id) initWithContactInfo:(NSString *) personFirstName : (NSString *) personLastName : (NSString *) phoneNum : (NSString *) phoneNumLabel : (NSString *) organization : (NSString*) notes : (NSString*) basicContactPhoneNumber {
	self = [super init];
    if (self) {
				
		callLogRecencyIndex = -1;
		
		if(personFirstName != nil)
		{
			contactFirstName = personFirstName;
		}
		
		if(contactFirstName != nil)
		{
			contactFirstnameLength = [contactFirstName length];
		}
		else
		{
			contactFirstnameLength = 0;
		}
		
		if(personLastName != nil)
		{
			contactLastName = personLastName;
		}
		
		if(contactLastName != nil)
		{
			contactLastNameLength = [contactLastName length];
		}
		else
		{
			contactLastNameLength = 0;
		}
		
		if(phoneNum != nil)
		{
			contactPhoneNumber = phoneNum;
		}
		
		if(contactPhoneNumber != nil)
		{
			contactPhoneNumberLength = [contactPhoneNumber length];
		}
		else
		{
			contactPhoneNumberLength = 0;
		}
		
		if(basicContactPhoneNumber != nil)
		{
			baseContactPhoneNumber = [[NSString alloc] initWithString: basicContactPhoneNumber];
		}
		
		if(baseContactPhoneNumber != nil)
		{
			baseContactPhoneNumberLength = [baseContactPhoneNumber length];
		}
		else
		{
			baseContactPhoneNumberLength = 0;
		}
		
		if(phoneNumLabel != nil)
		{
			contactPhoneNumberType = [[NSString alloc] initWithString:phoneNumLabel];	
		}
		
		if(contactPhoneNumberType != nil)
		{
			contactPhoneNumberTypeLength = [contactPhoneNumberType length];
		}
		else
		{
			contactPhoneNumberTypeLength = 0;
		}
		
		if(organization != nil)
		{
			contactOrganization = organization;
		}
		
		if(contactOrganization != nil)
		{
			contactOrganizationLength = [contactOrganization length];
		}
		else
		{
			contactOrganizationLength = 0;
		}
		
		if(notes != nil)
		{
			contactNotes = notes;
		}
		
		if(contactNotes != nil)
		{
			contactNotesLength = [contactNotes length];
		}
		else
		{
			contactNotesLength = 0;
		}
		
		if(contactFirstName != nil)
		{
			if(contactLastName != nil)
			{
				contactDisplayName = [[NSString alloc] initWithString: [[contactFirstName stringByAppendingString: @" "] stringByAppendingString: contactLastName]];
			}
			else
			{
				contactDisplayName = [[NSString alloc] initWithString: contactFirstName];
			}
		}
		else if(contactLastName != nil)
		{
			contactDisplayName = [[NSString alloc] initWithString: contactLastName];
		}
		else if(contactOrganization != nil)
		{
			contactDisplayName = [[NSString alloc] initWithString: contactOrganization];
		}
		
		if(contactDisplayName != nil)
		{
			contactDisplayNameLength = [contactDisplayName length];
		}
		else
		{
			contactDisplayNameLength = 0;
		}
	}
	
	return self;
}

- (void)dealloc {
	
	if(contactDisplayName != nil)
	{
		[contactDisplayName release];
	}
	
	if(baseContactPhoneNumber != nil)
	{
		[baseContactPhoneNumber release];
	}
	
	if(contactPhoneNumberType != nil)
	{
		[contactPhoneNumberType release];
	}
	
	[super dealloc];
}

@end
