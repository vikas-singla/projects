//
//  CallLogsView.m
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "CallLogsView.h"
#import "SettingsConstants.h"

@implementation CallLogsView

@synthesize callLogRecord;
@synthesize highlighted;
@synthesize editing;

- (id)initWithFrame:(CGRect)frame {
    if (self = [super initWithFrame:frame]) {
        // Initialization code
		self.opaque = YES;
		self.backgroundColor = [UIColor whiteColor];
		
		UIImage* image = [UIImage imageNamed:@"ListCallImage.png"];
		
		UIImageView* imageView = [[UIImageView alloc] initWithImage:image];
		imageView.frame = CGRectMake(285, 6, 30, 30);
		
		[self addSubview:imageView];
		
		[imageView release];
    }
    return self;
}

- (void)setHighlighted:(BOOL)lit {
	// If highlighted state changes, need to redisplay.
	if (highlighted != lit) {
		highlighted = lit;	
		[self setNeedsDisplay];
	}
}

- (void)drawRect:(CGRect)rect {
	
#define LEFT_COLUMN_OFFSET 10
#define LEFT_COLUMN_WIDTH 155
	
#define MIDDLE_COLUMN_OFFSET 165
#define MIDDLE_COLUMN_WIDTH 110
	
#define UPPER_ROW_TOP 4
#define LOWER_ROW_TOP 25
	
#define MAIN_FONT_SIZE 18
#define MIN_MAIN_FONT_SIZE 16
#define SECONDARY_FONT_SIZE 13
#define MIN_SECONDARY_FONT_SIZE 11
	
	// Color and font for the main text items (time zone name, time)
	UIColor *mainTextColor = nil;
	UIFont *mainFont = [UIFont systemFontOfSize:MAIN_FONT_SIZE];
	
	// Color and font for the secondary text items (GMT offset, day)
	UIColor *secondaryTextColor = nil;
	UIFont *secondaryFont = [UIFont systemFontOfSize:SECONDARY_FONT_SIZE];
	
	// Choose font color based on highlighted state.
	if (self.highlighted) {
		mainTextColor = [UIColor whiteColor];
		secondaryTextColor = [UIColor whiteColor];
	}
	else {
		mainTextColor = [UIColor blackColor];
		secondaryTextColor = [UIColor blueColor];
		self.backgroundColor = [UIColor whiteColor];
	}
	
	CGRect contentRect = self.bounds;
	
	// In this example we will never be editing, but this illustrates the appropriate pattern.
    if (!self.editing) {
		
		CGFloat boundsX = contentRect.origin.x;
		CGPoint point;
		
		CGFloat actualFontSize;
		CGSize size;
		
		// Set the color for the main text items
		[mainTextColor set];
		
		/*
		 Draw the contact name top left; use the NSString UIKit method to scale the font size down if the text does not fit in the given area
		 */
		point = CGPointMake(boundsX + LEFT_COLUMN_OFFSET, UPPER_ROW_TOP);
		NSString *contactName = (NSString*)[callLogRecord objectAtIndex:kIDialCallLogNameIndex];
		
		if(contactName != nil && [contactName compare: kIDialNoValue] != NSOrderedSame)
		{
			[contactName drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:mainFont minFontSize:MIN_MAIN_FONT_SIZE actualFontSize:NULL lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
			
			if(self.highlighted)
			{
				[[UIColor whiteColor] set];
			}
			else
			{
				[[UIColor darkGrayColor] set];
			}
			
			/*
			 Draw the contact organization bottom left; use the NSString UIKit method to scale the font size down if the text does not fit in the given area
			 */
			point = CGPointMake(boundsX + LEFT_COLUMN_OFFSET, LOWER_ROW_TOP);
			
			NSString* phone = [callLogRecord objectAtIndex:kIDialCallLogPhoneIndex];
			if(phone != nil)
			{
				phone = [[[callLogRecord objectAtIndex:kIDialCallLogPhoneTypeIndex] stringByAppendingString:@": "] stringByAppendingString:phone];
								
				[phone drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:secondaryFont minFontSize:MIN_SECONDARY_FONT_SIZE actualFontSize:NULL lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
			}		
		}
		else
		{
			NSString* phone = [callLogRecord objectAtIndex:kIDialCallLogPhoneIndex];
			if(phone != nil)
			{
				point = CGPointMake(boundsX + LEFT_COLUMN_OFFSET, UPPER_ROW_TOP + 5);
				[phone drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:mainFont minFontSize:MIN_MAIN_FONT_SIZE actualFontSize:NULL lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
			}
		}
		
		// Set the color for the secondary text items
		[secondaryTextColor set];
		
		/*
		 Draw the phone number type label, right-aligned in the middle column.
		 To ensure it is right-aligned, first find its width with the given font and minimum allowed font size. Then draw the string at the appropriate offset.
		 */
		NSDate* timestamp = [callLogRecord objectAtIndex:kIDialCallLogTimestampIndex];
		if(timestamp != nil)
		{
			NSDateFormatter* dateFormatter = [[[NSDateFormatter alloc] init] autorelease];
			[dateFormatter setDateStyle:NSDateFormatterMediumStyle];
			[dateFormatter setTimeStyle:NSDateFormatterNoStyle];
			NSString* timestampStr = [dateFormatter stringFromDate: timestamp];
			
			size = [timestampStr sizeWithFont:secondaryFont minFontSize:SECONDARY_FONT_SIZE actualFontSize:&actualFontSize forWidth:MIDDLE_COLUMN_WIDTH lineBreakMode:UILineBreakModeTailTruncation];
			
			point = CGPointMake(boundsX + MIDDLE_COLUMN_OFFSET + MIDDLE_COLUMN_WIDTH - size.width, UPPER_ROW_TOP + ((self.frame.size.height - size.height) / 2) - 3);
			[timestampStr drawAtPoint:point forWidth:MIDDLE_COLUMN_WIDTH withFont:secondaryFont minFontSize:actualFontSize actualFontSize:&actualFontSize lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
		}
		
	}
}

- (void)dealloc {
	[callLogRecord release];
    [super dealloc];
}

@end
