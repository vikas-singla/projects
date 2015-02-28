//
//  ContactInformationController.m
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "ContactInformationController.h"
#import <AddressBook/ABRecord.h>
#import <AddressBook/ABMultiValue.h>
#import "ContactInformationWrapper.h"
#import "SettingsConstants.h"

#define homeFAX		@"HomeFAX"
#define workFAX		@"WorkFAX"

static ContactInformationController *instanceObj;
static NSString* syncText = @"sync";
static NSString* syncText2 = @"sync2";

@implementation ContactInformationController

@synthesize callbackIsRegistered;
@synthesize abContactList;
@synthesize filteredABContactList;
@synthesize addressBook;
@synthesize callMechanismInstance;
@synthesize displayRecentFirst;
@synthesize searchNotes;
@synthesize loadingContacts;

- (void) loadContactInformationController: (id)anObject {
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
	
	ContactInformationController *controller = [ContactInformationController getInstance];
	
	[controller loadData];
	
	[pool release];
}

+ (ContactInformationController *) getInstance {
	
	@synchronized(syncText)
	{
		if(instanceObj == nil)
		{			
			instanceObj = [[ContactInformationController alloc] init];
			
			instanceObj.searchNotes = ([[[NSUserDefaults standardUserDefaults] stringForKey:kIDialSearchConfigNotes] compare:kIDialTrue] == NSOrderedSame);
			
			instanceObj.loadingContacts = YES;
		}
	}
	
	return instanceObj;
}

- (void) checkpoint {
	
}

+ (void) updateDataSettingsUpdated {
	if(instanceObj != nil)
	{
		//[instanceObj loadData];
		[NSThread detachNewThreadSelector:@selector(loadContactInformationController:) toTarget:instanceObj withObject:nil];
		
		instanceObj.searchNotes = ([[[NSUserDefaults standardUserDefaults] stringForKey:kIDialSearchConfigNotes] compare:kIDialTrue] == NSOrderedSame);
	}
}

- (id) init {
	self = [super init];
    if (self) {		
		addressBook = ABAddressBookCreate();
		callbackIsRegistered = NO;
		
		if(callMechanismInstance == nil)
		{
			callMechanismInstance = [CallMechanism getInstance];
		}
	}
	
	loadingContacts = YES;
	
	return self;
}

- (void) registerABChangeCallback {
	if(!callbackIsRegistered)
	{
		ABAddressBookRegisterExternalChangeCallback(addressBook, MyAddressBookExternalChangeCallback, self);
		
		callbackIsRegistered = YES;
	}
}

BOOL doesStringContain(NSString* string, NSString* charcter) {
	
	for (int i=0; i<[string length]; i++) {
		NSString* chr = [string substringWithRange:NSMakeRange(i, 1)];
		if([chr isEqualToString:charcter])
			return TRUE;
	}
	return FALSE;
}

NSString* retreiveBasePhoneNumber(NSString* localizedPhoneNumber) {
	NSString* result = @"";
	
	for(int i = 0; i < [localizedPhoneNumber length]; ++i) {
		NSString* chr = [localizedPhoneNumber substringWithRange:NSMakeRange(i, 1)];
		if(doesStringContain(@"+,0123456789", chr)) {
			result = [result stringByAppendingString: chr];
		}
	}
	
	return result;
}

NSInteger ContactSort(id contact1, id contact2, void *context)
{
	ContactInformationWrapper* c1 = (ContactInformationWrapper*)contact1;
	ContactInformationWrapper* c2 = (ContactInformationWrapper*)contact2;
	
	if(c1.contactDisplayName == nil)
	{
		return NSOrderedDescending;
	}
	else if(c2.contactDisplayName == nil)
	{
		return NSOrderedAscending;
	}
	else
	{
		return [c1.contactDisplayName compare:c2.contactDisplayName options:NSCaseInsensitiveSearch];
	}
}


- (void) loadData {
	
	@synchronized(syncText2)
	{
		loadingContacts = YES;
		
		if(abContactList != nil)
		{
			[abContactList removeAllObjects];
			[abContactList release];
		}
		if(filteredABContactList != nil)
		{
			[filteredABContactList removeAllObjects];
			[filteredABContactList release];
		}
		
		//CFArrayRef people = ABAddressBookCopyArrayOfAllPeople(addressBook);
		
		NSUserDefaults* userDefaults = [NSUserDefaults standardUserDefaults];
		NSArray* searchPhoneDisplay = [userDefaults stringArrayForKey:kIDialSearchDisplayNumbers];	
		
		//for(int i = 0; i < CFArrayGetCount(people); ++i)
		int personCount = (int) ABAddressBookGetPersonCount(addressBook);
		
		abContactList = [[NSMutableArray alloc] initWithCapacity:personCount];
		
		for(int i = 1; i < (personCount + 1); ++i)
		{
			//ABRecordRef abContact = CFRetain(CFArrayGetValueAtIndex(people, i));
			ABRecordRef abContact = ABAddressBookGetPersonWithRecordID(addressBook, (ABRecordID)i);
			
			if(abContact == nil)
			{
				++personCount;
				continue;
			}
			
			NSString *contactFirstName = (NSString*)ABRecordCopyValue(abContact, kABPersonFirstNameProperty);			
			NSString *contactLastName = (NSString*)ABRecordCopyValue(abContact, kABPersonLastNameProperty);
			
			NSString *contactOrganization = (NSString*)ABRecordCopyValue(abContact, kABPersonOrganizationProperty);
			
			NSString* contactNotes = (NSString*)ABRecordCopyValue(abContact, kABPersonNoteProperty);
			
			ABMultiValueRef phoneNumbers = ABRecordCopyValue(abContact, kABPersonPhoneProperty);
			
			int phoneNumberCount = (int) ABMultiValueGetCount(phoneNumbers);
			
			for(int j = 0; j < phoneNumberCount; ++j)
			{
				NSString* phoneNum = (NSString *)ABMultiValueCopyValueAtIndex(phoneNumbers, j);
				NSString* phoneNumLabel = (NSString *)ABMultiValueCopyLabelAtIndex(phoneNumbers, j);
				
				if(phoneNumLabel != nil)
				{
					NSCharacterSet * charsToRemove = [NSCharacterSet characterSetWithCharactersInString:@"_$!<>"];
					
					NSString* tempPhoneNumLabel = [phoneNumLabel stringByTrimmingCharactersInSet:charsToRemove];
					
					CFRelease(phoneNumLabel);
					
					phoneNumLabel = tempPhoneNumLabel;
				}
				
				if(searchPhoneDisplay == nil || [searchPhoneDisplay count] == 0 || [searchPhoneDisplay indexOfObject:kIDialDisplayAllNumbers] != NSNotFound)
				{
					//do nothing
				}
				else if([searchPhoneDisplay indexOfObject:[phoneNumLabel lowercaseString]] != NSNotFound)
				{
					//do nothing
				}
				else
				{
					//skip this phone number
					continue;
				}
				
				if([phoneNumLabel compare:homeFAX] == NSOrderedSame)
				{
					phoneNumLabel = [NSString stringWithString: @"Home Fax"];
				}
				else if([phoneNumLabel compare:workFAX] == NSOrderedSame)
				{
					phoneNumLabel = [NSString stringWithString: @"Work Fax"];
				}
				
				ContactInformationWrapper *infoWrapper = [[ContactInformationWrapper alloc] initWithContactInfo : contactFirstName
																												: contactLastName
																												: phoneNum
																												: phoneNumLabel
																												: contactOrganization
																												: contactNotes
																												: retreiveBasePhoneNumber(phoneNum)];
				[abContactList addObject:infoWrapper];		
				
				[infoWrapper release];
			}
			
			CFRelease(phoneNumbers);
			//CFRelease(abContact);
		}
		
		//CFRelease(people);
		
		[abContactList sortUsingFunction:ContactSort context:nil];
		
		// create our filtered list that will be the data source of our table, start its content from the master "abContactList"
		filteredABContactList = [[NSMutableArray alloc] initWithCapacity: [abContactList count]];	
		
		loadingContacts = NO;
	}
}

void MyAddressBookExternalChangeCallback (ABAddressBookRef addressBook,
										  CFDictionaryRef info,
										  void *context) {
	ABAddressBookRevert(addressBook);
	
	//[[ContactInformationController getInstance] loadData];
	[NSThread detachNewThreadSelector:@selector(loadContactInformationController:) toTarget:instanceObj withObject:nil];
}

- (void) resetFilteredContactListToMasterList {
	[filteredABContactList removeAllObjects];
	[filteredABContactList addObjectsFromArray:abContactList];
	
	displayRecentFirst = ([[[NSUserDefaults standardUserDefaults] objectForKey:kIDialSearchConfigRecentFirst] compare: kIDialTrue] == NSOrderedSame);
	searchNotes = ([[[NSUserDefaults standardUserDefaults] stringForKey:kIDialSearchConfigNotes] compare:kIDialTrue] == NSOrderedSame);
}

- (void) searchTextBeginEditing {
}

- (void) searchTextChange: (NSString *)searchText : (BOOL) isForKeypad {
	if(!isForKeypad)
	{
		[self searchTextChangeForNonKeypad: searchText];
	}
	else
	{
		[self searchTextChangeForKeypad: searchText];
	}
}

NSString* retrieveDigitCharacters(NSString* digit) {
	NSString* result = nil;
	
	if([digit compare: @"2"] == NSOrderedSame)
	{
		result = @"ABC";
	}
	else if([digit compare: @"3"] == NSOrderedSame)
	{
		result = @"DEF";
	}
	else if([digit compare: @"4"] == NSOrderedSame)
	{
		result = @"GHI";
	}
	else if([digit compare: @"5"] == NSOrderedSame)
	{
		result = @"JKL";
	}
	else if([digit compare: @"6"] == NSOrderedSame)
	{
		result = @"MNO";
	}
	else if([digit compare: @"7"] == NSOrderedSame)
	{
		result = @"PQRS";
	}
	else if([digit compare: @"8"] == NSOrderedSame)
	{
		result = @"TUV";
	}
	else if([digit compare: @"9"] == NSOrderedSame)
	{
		result = @"WXYZ";
	}
	else
	{
		result = @"";
	}
	
	return result;
}

BOOL keypadSearchCompare(NSString* searchString, NSString* searchTargetString, BOOL extrapolate) {
	BOOL result = FALSE;
	
	int searchStringLength = [searchString length];
	int searchTargetStringLength = [searchTargetString length];
	
	if(searchStringLength > searchTargetStringLength)
	{
		return result; //we can infer no match
	}
	
	for (int i=0; i<searchStringLength; i++) 
	{
		NSString* searchChar = [searchString substringWithRange:NSMakeRange(i, 1)];	
		NSString* searchTargetChar = [searchTargetString substringWithRange:NSMakeRange(i, 1)];
		
		if([searchTargetChar compare:searchChar options:NSCaseInsensitiveSearch range:NSMakeRange(0, 1)] == NSOrderedSame)
		{
			result = TRUE;
			
			continue; //optimization
		}
		else if(extrapolate)
		{
			NSString* searchCharExtrapolation = retrieveDigitCharacters(searchChar);
			
			if(searchCharExtrapolation == nil || [searchCharExtrapolation length] == 0)
			{
				result = FALSE;
			}
			else
			{				
				for(int j = 0; j < [searchCharExtrapolation length]; ++j)
				{
					NSString* searchCharExtrapolationLetter = [searchCharExtrapolation substringWithRange:NSMakeRange(j, 1)];
					
					if([searchTargetChar compare:searchCharExtrapolationLetter options:NSCaseInsensitiveSearch range:NSMakeRange(0, 1)] == NSOrderedSame)
					{
						result = TRUE;
						break;
					}
					else if(j == ([searchCharExtrapolation length] - 1))
					{
						result = FALSE;
					}
				}
			}
		}
		else
		{
			result = FALSE;
		}
		
		if(!result)
		{
			break;
		}
	}
	
	return result;
}

- (NSInteger) callLogContainsContact: (ContactInformationWrapper*) contact
{
	NSInteger result = -1;
	NSArray* callLogs = callMechanismInstance.callLogs;
	
	if([callLogs count] > 0)
	{			
		for(int i = 0; i < [callLogs count]; ++i) 
		{
			NSArray* existingCallLogEntry = [callLogs objectAtIndex:i];
			
			if([[existingCallLogEntry objectAtIndex:kIDialCallLogNameIndex] compare: contact.contactDisplayName  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
			   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneIndex] compare: contact.contactPhoneNumber  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
			   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneTypeIndex] compare: contact.contactPhoneNumberType  options:NSCaseInsensitiveSearch] == NSOrderedSame)
			{
				result = i;
				break;
			}
		}
	}
	
	return result;
}

- (void) checkCallLogAndAddToFilteredList: (ContactInformationWrapper*) contact {
	NSInteger callLogRecencyIndex = -1;
	
	if(displayRecentFirst && (callLogRecencyIndex = [self callLogContainsContact: contact]) >= 0)
	{
		NSInteger insertIndex = 0;
		contact.callLogRecencyIndex = callLogRecencyIndex;
		
		if(insertIndex < [filteredABContactList count])
		{
			ContactInformationWrapper* indexObject = [filteredABContactList objectAtIndex:insertIndex];
			
			while(indexObject.callLogRecencyIndex != -1 && indexObject.callLogRecencyIndex <= callLogRecencyIndex)
			{
				++insertIndex;
				
				if(insertIndex < [filteredABContactList count])
				{
					indexObject = [filteredABContactList objectAtIndex:insertIndex];
				}
				else
				{
					break;
				}
			}
		}
		
		[filteredABContactList insertObject:contact atIndex:insertIndex];
	}
	else
	{
		[filteredABContactList addObject:contact];
	}
}

- (void) searchTextChangeForKeypad: (NSString*)searchText {
	
	if(searchText == nil || [searchText compare:@""] == NSOrderedSame)
	{
		//	[filteredABContactList addObjectsFromArray: abContactList];
		return;
	}
	
	NSArray* searchArray = [NSArray arrayWithArray:filteredABContactList];
	
	[filteredABContactList removeAllObjects];
	
	ContactInformationWrapper *infoWrapper;
	for(infoWrapper in searchArray)
	{
		BOOL result = FALSE;
		
		if(infoWrapper.baseContactPhoneNumber != nil)
		{
			result = keypadSearchCompare(searchText, infoWrapper.baseContactPhoneNumber, NO);
			
			if (result)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactFirstName != nil)
		{
			result = keypadSearchCompare(searchText, infoWrapper.contactFirstName, YES);
			
			if (result)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactLastName != nil)
		{
			result = keypadSearchCompare(searchText, infoWrapper.contactLastName, YES);
			
			if (result)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactOrganization != nil)
		{
			result = keypadSearchCompare(searchText, infoWrapper.contactOrganization, YES);
			
			if (result)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
	}
}

- (void) searchTextChangeForNonKeypad: (NSString*)searchText {
	
	if(searchText == nil || [searchText compare:@""] == NSOrderedSame)
	{
		//	[filteredABContactList addObjectsFromArray: abContactList];
		return;
	}
	
	int searchTextLength = [searchText length];
	
	NSRange searchRange = NSMakeRange(0, searchTextLength);
	
	NSArray* searchArray = [NSArray arrayWithArray:filteredABContactList];
	
	[filteredABContactList removeAllObjects];
	
	ContactInformationWrapper *infoWrapper;
	
	for(infoWrapper in searchArray)
	{
		if(infoWrapper.baseContactPhoneNumber != nil && infoWrapper.baseContactPhoneNumberLength >= searchTextLength)
		{			
			NSRange result = [infoWrapper.baseContactPhoneNumber rangeOfString:searchText options:NSCaseInsensitiveSearch];
			
			if (result.location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];								
				continue;
			}
		}
		
		if(infoWrapper.contactFirstName != nil && infoWrapper.contactFirstnameLength >= searchTextLength)
		{
			NSRange result = [infoWrapper.contactFirstName rangeOfString:searchText options:NSCaseInsensitiveSearch];
			
			if (result.location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactLastName != nil && infoWrapper.contactLastNameLength >= searchTextLength)
		{
			NSRange result = [infoWrapper.contactLastName rangeOfString:searchText options:NSCaseInsensitiveSearch];
			
			if (result.location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactOrganization != nil && infoWrapper.contactOrganizationLength >= searchTextLength)
		{
			NSRange result = [infoWrapper.contactOrganization rangeOfString:searchText options:NSCaseInsensitiveSearch];
			
			if (result.location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactDisplayName != nil && infoWrapper.contactDisplayNameLength >= searchTextLength)
		{
			NSRange result = [infoWrapper.contactDisplayName rangeOfString:searchText options:NSCaseInsensitiveSearch];
			
			if (result.location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}
		}
		
		if(infoWrapper.contactNotes != nil && searchNotes  && infoWrapper.contactNotesLength >= searchTextLength)
		{			
			if ([infoWrapper.contactNotes rangeOfString:searchText options:NSCaseInsensitiveSearch].location != NSNotFound)
			{
				[self checkCallLogAndAddToFilteredList:infoWrapper];
				continue;
			}			
		}
	}	
}

- (void) searchReset {
	[filteredABContactList removeAllObjects];
	[filteredABContactList addObjectsFromArray:abContactList];
}

- (void) dealloc {
	if(callbackIsRegistered)
	{
		ABAddressBookUnregisterExternalChangeCallback(addressBook, MyAddressBookExternalChangeCallback, self);
	}
	
	CFRelease(addressBook);
	[abContactList release];
	[filteredABContactList release];
	[super dealloc];
}

+ (void) deallocObj {
	[instanceObj release];
}

@end
