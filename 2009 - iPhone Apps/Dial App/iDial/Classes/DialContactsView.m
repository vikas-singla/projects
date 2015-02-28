//
//  DialContactsView.m
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "DialContactsView.h"

@implementation DialContactsView

@synthesize contactRecord;
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
#define LEFT_COLUMN_WIDTH 160
	
#define MIDDLE_COLUMN_OFFSET 170
#define MIDDLE_COLUMN_WIDTH 110
	
#define RIGHT_COLUMN_OFFSET 280
	
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
		secondaryTextColor = [UIColor darkGrayColor];
		self.backgroundColor = [UIColor clearColor];
	}
	
	CGRect contentRect = self.bounds;
	
	// In this example we will never be editing, but this illustrates the appropriate pattern.
    if (!self.editing) {
		
		CGFloat boundsX = contentRect.origin.x;
		CGPoint point;
		
		CGFloat actualFontSize;
		CGSize size;
		
		bool showOrganization = YES;
		
		// Set the color for the main text items
		[mainTextColor set];
		
		/*
		 Draw the contact name top left; use the NSString UIKit method to scale the font size down if the text does not fit in the given area
		 */
		point = CGPointMake(boundsX + LEFT_COLUMN_OFFSET, UPPER_ROW_TOP);
		
		if(self.contactRecord.contactDisplayName != nil)
		{
			[self.contactRecord.contactDisplayName drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:mainFont minFontSize:MIN_MAIN_FONT_SIZE actualFontSize:NULL lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
		}
		
		[[UIColor blueColor] set];
		
		/*
		 Draw the phone number type label, right-aligned in the middle column.
		 To ensure it is right-aligned, first find its width with the given font and minimum allowed font size. Then draw the string at the appropriate offset.
		 */
		if(self.contactRecord.contactPhoneNumberType != nil)
		{
			size = [self.contactRecord.contactPhoneNumberType sizeWithFont:secondaryFont minFontSize:MIN_SECONDARY_FONT_SIZE actualFontSize:&actualFontSize forWidth:MIDDLE_COLUMN_WIDTH lineBreakMode:UILineBreakModeTailTruncation];
			
			point = CGPointMake(boundsX + MIDDLE_COLUMN_OFFSET + MIDDLE_COLUMN_WIDTH - size.width, UPPER_ROW_TOP);
			[self.contactRecord.contactPhoneNumberType drawAtPoint:point forWidth:MIDDLE_COLUMN_WIDTH withFont:secondaryFont minFontSize:actualFontSize actualFontSize:&actualFontSize lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
		}
		
		// Set the color for the secondary text items
		[secondaryTextColor set];
		
		/*
		 Draw the contact organization bottom left; use the NSString UIKit method to scale the font size down if the text does not fit in the given area
		 */
		point = CGPointMake(boundsX + LEFT_COLUMN_OFFSET, LOWER_ROW_TOP);
		
		if(self.contactRecord.contactOrganization != nil && showOrganization == YES)
		{
			[self.contactRecord.contactOrganization drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:secondaryFont minFontSize:MIN_SECONDARY_FONT_SIZE actualFontSize:NULL lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
		}
		
		/*
		 Draw the phone number string, right-aligned in the middle column.
		 To ensure it is right-aligned, first find its width with the given font and minimum allowed font size. Then draw the string at the appropriate offset.
		 */
		if(self.contactRecord.contactPhoneNumber != nil)
		{
			size = [self.contactRecord.contactPhoneNumber sizeWithFont:secondaryFont minFontSize:MIN_SECONDARY_FONT_SIZE actualFontSize:&actualFontSize forWidth:MIDDLE_COLUMN_WIDTH lineBreakMode:UILineBreakModeTailTruncation];
			
			point = CGPointMake(boundsX + MIDDLE_COLUMN_OFFSET + MIDDLE_COLUMN_WIDTH - size.width, LOWER_ROW_TOP);
			[self.contactRecord.contactPhoneNumber drawAtPoint:point forWidth:LEFT_COLUMN_WIDTH withFont:secondaryFont minFontSize:actualFontSize actualFontSize:&actualFontSize lineBreakMode:UILineBreakModeTailTruncation baselineAdjustment:UIBaselineAdjustmentAlignBaselines];
		}
	}
}


- (void)dealloc {
    [super dealloc];
}


@end
