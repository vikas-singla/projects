//
//  CallMechanism.h
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ContactInformationWrapper.h"

@interface CallMechanism : NSObject {
	NSString*	cachedRawNumber;
	NSMutableArray*	callLogs;
	NSUserDefaults		*userDefaults;
}

@property (nonatomic, retain) NSString*	cachedRawNumber;
@property (nonatomic, assign) NSMutableArray*	callLogs;
@property (nonatomic, retain) NSUserDefaults	*userDefaults;

+ (CallMechanism *) getInstance;

- (void) callNumber: (NSString*) rawNumber;
- (void) callNumberWithInfo:(ContactInformationWrapper*) phoneInfo;
- (void) callNumberWithCallLogInfo:(NSArray*) phoneInfo;
- (void) callNumberAtCallLogIndex: (NSUInteger) index;
- (id) init;
- (void) clearLog;
- (void) addToCallLog:(NSString*) phoneNumber;

@end
