//
//  CallMechanism.m
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "CallMechanism.h"
#import "SettingsConstants.h"

static CallMechanism *instanceObj;

@implementation CallMechanism

@synthesize callLogs;
@synthesize cachedRawNumber;
@synthesize userDefaults;

+ (CallMechanism *) getInstance {
	if(instanceObj == nil)
	{
		instanceObj = [[CallMechanism alloc] init];
	}
	
	return instanceObj;		
}

- (id) init {
	self = [super init];
	
	if(self)
	{		
		userDefaults = [NSUserDefaults standardUserDefaults];
		NSArray	*callLogsDefault = [NSArray arrayWithObjects: nil];
		NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
									 callLogsDefault, kIDialRecentCallLog, nil];
		
		[userDefaults registerDefaults:appDefaults];
		
		NSArray* callLogsArray = [userDefaults objectForKey:kIDialRecentCallLog];
		callLogs = [[NSMutableArray alloc] initWithCapacity: 1];
		[callLogs addObjectsFromArray: callLogsArray];
	}
	
	return self;
}

- (void) clearLog {
	if(callLogs != nil) {
		[callLogs removeAllObjects];
		[userDefaults setObject:callLogs forKey:kIDialRecentCallLog];
	}
}

- (void) callNumberAtCallLogIndex: (NSUInteger) index {
	
	if(index < [callLogs count])
	{
		NSArray* callLogSelected = [callLogs objectAtIndex:index];
		
		if(callLogSelected != nil)
		{
			[callLogSelected retain];
			[callLogs removeObjectAtIndex:index];
			
			//the following call with release 'callLogSelected'
			[self callNumberWithCallLogInfo: callLogSelected];
		}	
	}	
}

BOOL doesStringContainDigits(NSString* string, NSString* charcter){
	for (int i=0; i<[string length]; i++) {
		NSString* chr = [string substringWithRange:NSMakeRange(i, 1)];
		if([chr isEqualToString:charcter])
			return TRUE;
	}
	return FALSE;
}

void showError(id self)
{
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Invalid Phone Number" message:@"Due to iPhone 3rd party software limitations, the number that you have chosen to dial contains characters not accepted by the phone system. Click 'Fix it' button below to automatically remove invalid characters and dial."
												   delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Fix it", nil];
	[alert show];
	[alert release];
}

void showEmptyNumberError(id self)
{
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Invalid Phone Number" message:@"The 'Fix it' action resulted in a phone number with zero digits. Unable to complete dial request."
												   delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
	[alert show];
	[alert release];
}

- (void) reinsertIntoCallLogInfo:(NSArray*) info {
	NSDate* currTime = [NSDate date];
	NSMutableArray* newCallLogEntry = [NSMutableArray arrayWithCapacity:kIDialCallLogRecordFields];
	[newCallLogEntry addObjectsFromArray:info];
	[newCallLogEntry replaceObjectAtIndex:kIDialCallLogTimestampIndex withObject:currTime];
	
	[callLogs insertObject:newCallLogEntry atIndex:0];
	
	[userDefaults setObject:callLogs forKey:kIDialRecentCallLog];
	
	[info release];
}

- (void) addToCallLog:(NSString*) phoneNumber {
	NSDate* currTime = [NSDate date];
	
	for(int i = 0; i < [callLogs count]; ++i)
	{
		NSArray* existingCallLogEntry = [callLogs objectAtIndex:i];
		
		if([[existingCallLogEntry objectAtIndex:kIDialCallLogNameIndex] compare: kIDialNoValue  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
		   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneIndex] compare: phoneNumber  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
		   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneTypeIndex] compare: kIDialNoValue  options:NSCaseInsensitiveSearch] == NSOrderedSame)
		{
			[existingCallLogEntry retain];
			[callLogs removeObjectAtIndex:i];
			
			//refresh with current time and re-insert
			[self reinsertIntoCallLogInfo:existingCallLogEntry];
			
			[existingCallLogEntry release];
			
			return;
		}
	}		
	
	NSMutableArray* callLogEntry = [NSMutableArray arrayWithCapacity:kIDialCallLogRecordFields];
	[callLogEntry insertObject:kIDialNoValue atIndex: kIDialCallLogNameIndex];
	[callLogEntry insertObject:phoneNumber atIndex: kIDialCallLogPhoneIndex];
	[callLogEntry insertObject:kIDialNoValue atIndex: kIDialCallLogPhoneTypeIndex];
	[callLogEntry insertObject:currTime atIndex: kIDialCallLogTimestampIndex];

	[callLogs insertObject:callLogEntry atIndex:0];
	
	[userDefaults setObject:callLogs forKey:kIDialRecentCallLog];
}

- (void) addToCallLogFullInfo:(ContactInformationWrapper*) info {
	NSDate* currTime = [NSDate date];
	
	if(info.contactDisplayName == nil || [info.contactDisplayName compare:@""] == NSOrderedSame)
	{
		[self addToCallLog: info.contactPhoneNumber];
		return;
	}
	
	for(int i = 0; i < [callLogs count]; ++i)
	{
		NSArray* existingCallLogEntry = [callLogs objectAtIndex:i];
		
		if([[existingCallLogEntry objectAtIndex:kIDialCallLogNameIndex] compare: info.contactDisplayName  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
		   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneIndex] compare: info.contactPhoneNumber  options:NSCaseInsensitiveSearch] == NSOrderedSame &&
		   [[existingCallLogEntry objectAtIndex:kIDialCallLogPhoneTypeIndex] compare: info.contactPhoneNumberType  options:NSCaseInsensitiveSearch] == NSOrderedSame)
		{
			[existingCallLogEntry retain];
			[callLogs removeObjectAtIndex:i];
			
			//refresh with current time and re-insert
			[self reinsertIntoCallLogInfo:existingCallLogEntry];
			
			[existingCallLogEntry release];
			
			return;
		}
	}	
	
	NSMutableArray* callLogEntry = [NSMutableArray arrayWithCapacity:kIDialCallLogRecordFields];
	
	[callLogEntry insertObject:info.contactDisplayName atIndex: kIDialCallLogNameIndex];
	[callLogEntry insertObject:info.contactPhoneNumber atIndex: kIDialCallLogPhoneIndex];
	[callLogEntry insertObject:info.contactPhoneNumberType atIndex: kIDialCallLogPhoneTypeIndex];
	[callLogEntry insertObject:currTime atIndex: kIDialCallLogTimestampIndex];
	
	[callLogs insertObject:callLogEntry atIndex:0];
	
	[userDefaults setObject:callLogs forKey:kIDialRecentCallLog];
}

- (void)alertView:(UIAlertView *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
	// the user clicked one of the OK/Cancel buttons
	if (buttonIndex == 1)
	{
		NSLog(@"ok");
		if(cachedRawNumber != nil)
		{
			NSString* cleanedUpNumber = [cachedRawNumber stringByTrimmingCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@"*#"]];
			
			[cachedRawNumber release];
			cachedRawNumber = nil;
			
			if([cleanedUpNumber length] == 0)
			{
				showEmptyNumberError(self);
			}
			else
			{

				[self addToCallLog: cleanedUpNumber];
				
				cleanedUpNumber = [cleanedUpNumber stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
				
				NSString* telNumber = @"";
				
				for (int i=0; i<[cleanedUpNumber length]; i++) 
				{
					NSString* chr = [cleanedUpNumber substringWithRange:NSMakeRange(i, 1)];
					if(doesStringContainDigits(@"+,0123456789", chr)) 
					{
						telNumber = [telNumber stringByAppendingFormat:@"%@", chr];
					}
				}
				
				telNumber = [NSString stringWithFormat:@"tel:%@", telNumber]; 
				
				[[UIApplication sharedApplication] openURL:[[NSURL alloc] initWithString:telNumber]];
			}
		}
	}
	else
	{
		if(cachedRawNumber != nil)
		{
			[cachedRawNumber release];
			cachedRawNumber = nil;
		}
	}
}

- (void) callNumber: (NSString*) rawNumber {
	
	if(rawNumber && ![rawNumber isEqualToString:@""]) 
	{		
		if([rawNumber rangeOfCharacterFromSet:[NSCharacterSet characterSetWithCharactersInString:@"*#"]].location == NSNotFound)
		{
			[self addToCallLog:rawNumber];
			
			NSString* telNumber = @"";
			
			for (int i=0; i<[rawNumber length]; i++) 
			{
				NSString* chr = [rawNumber substringWithRange:NSMakeRange(i, 1)];
				if(doesStringContainDigits(@"+,0123456789", chr)) 
				{
					telNumber = [telNumber stringByAppendingFormat:@"%@", chr];
				}
			}
			
			telNumber = [NSString stringWithFormat:@"tel:%@", telNumber]; 
			
			[[UIApplication sharedApplication] openURL:[[NSURL alloc] initWithString:telNumber]];
		}
		else
		{
			cachedRawNumber = [[NSString alloc] initWithString: rawNumber];
			showError(self);
		}
	}	
}

- (void) callNumberWithInfo:(ContactInformationWrapper*) phoneInfo {
	
	if(phoneInfo && phoneInfo.contactPhoneNumber && ![phoneInfo.contactPhoneNumber isEqualToString:@""]) 
	{		
		if([phoneInfo.contactPhoneNumber rangeOfCharacterFromSet:[NSCharacterSet characterSetWithCharactersInString:@"*#"]].location == NSNotFound)
		{
			[self addToCallLogFullInfo: phoneInfo];
			
			NSString* telNumber = @"";
			
			for (int i=0; i<[phoneInfo.contactPhoneNumber length]; i++) 
			{
				NSString* chr = [phoneInfo.contactPhoneNumber substringWithRange:NSMakeRange(i, 1)];
				if(doesStringContainDigits(@"+,0123456789", chr)) 
				{
					telNumber = [telNumber stringByAppendingFormat:@"%@", chr];
				}
			}
			
			telNumber = [NSString stringWithFormat:@"tel:%@", telNumber]; 
			
			[[UIApplication sharedApplication] openURL:[[NSURL alloc] initWithString:telNumber]];
		}
		else
		{
			cachedRawNumber = [[NSString alloc] initWithString: phoneInfo.contactPhoneNumber];
			showError(self);
		}
	}	
}

- (void) callNumberWithCallLogInfo:(NSArray*) phoneInfo {
	
	if(phoneInfo) 
	{		
		NSString* rawPhone = [phoneInfo objectAtIndex:kIDialCallLogPhoneIndex];
		
		if(rawPhone && ![rawPhone isEqualToString:@""])
		{
			if([rawPhone rangeOfCharacterFromSet:[NSCharacterSet characterSetWithCharactersInString:@"*#"]].location == NSNotFound)
			{
				[self reinsertIntoCallLogInfo: phoneInfo];
				
				NSString* telNumber = @"";
				
				for (int i=0; i<[rawPhone length]; i++) 
				{
					NSString* chr = [rawPhone substringWithRange:NSMakeRange(i, 1)];
					if(doesStringContainDigits(@"+,0123456789", chr)) 
					{
						telNumber = [telNumber stringByAppendingFormat:@"%@", chr];
					}
				}
				
				telNumber = [NSString stringWithFormat:@"tel:%@", telNumber]; 
				
				[[UIApplication sharedApplication] openURL:[[NSURL alloc] initWithString:telNumber]];
			}
			else
			{
				cachedRawNumber = [[NSString alloc] initWithString: rawPhone];
				showError(self);
			}
		}
	}	
}


@end
