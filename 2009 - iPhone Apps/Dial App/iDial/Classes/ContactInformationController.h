//
//  ContactInformationController.h
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AddressBook/AddressBook.h>
#import "CallMechanism.h"

@interface ContactInformationController : NSObject {
	NSMutableArray	*abContactList;
	NSMutableArray	*filteredABContactList;
	
	CallMechanism *callMechanismInstance;
	ABAddressBookRef addressBook;
	
	BOOL displayRecentFirst;
	BOOL callbackIsRegistered;
	BOOL searchNotes;
	BOOL loadingContacts;

}

@property (nonatomic, assign) BOOL searchNotes;
@property (nonatomic, assign) BOOL	callbackIsRegistered;
@property (nonatomic, assign) BOOL	displayRecentFirst;
@property (nonatomic, assign) CallMechanism		*callMechanismInstance;
@property (nonatomic, assign) ABAddressBookRef  addressBook;
@property (nonatomic, assign) NSMutableArray	*abContactList;
@property (nonatomic, assign) NSMutableArray	*filteredABContactList;
@property (nonatomic, assign) BOOL loadingContacts;

+ (ContactInformationController *) getInstance;
- (id)init;
- (void) registerABChangeCallback;
+ (void) deallocObj;
+ (void) updateDataSettingsUpdated;

void MyAddressBookExternalChangeCallback (ABAddressBookRef addressBook,
										  CFDictionaryRef info,
										  void *context);

- (void) checkpoint;
- (void) loadData;
- (void) resetFilteredContactListToMasterList;
- (void) searchTextBeginEditing;
- (void) searchTextChange: (NSString *)searchText : (BOOL) isForKeypad;
- (void) searchTextChangeForKeypad: (NSString*)searchText;
- (void) searchTextChangeForNonKeypad: (NSString*)searchText;
- (void) searchReset;

@end
