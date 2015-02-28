//
//  ContactTableViewCell.m
//  Fast Dial
//
//  Created by Vikas on 12/15/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "ContactTableViewCell.h"

@implementation ContactTableViewCell

@synthesize contactsView;

- (id)initWithFrame:(CGRect)frame reuseIdentifier:(NSString *)reuseIdentifier {
    if (self = [super initWithFrame:frame reuseIdentifier:reuseIdentifier]) {
        // Initialization code
		CGRect tzvFrame = CGRectMake(0.0, 0.0, self.contentView.bounds.size.width, self.contentView.bounds.size.height);
		contactsView = [[DialContactsView alloc] initWithFrame:tzvFrame];
		contactsView.backgroundColor = [UIColor whiteColor];
		contactsView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
		[self.contentView addSubview:contactsView];
		self.selectionStyle = UITableViewCellSelectionStyleBlue; 
	}
    return self;
}

- (void) reInitialize {
	if(contactsView != nil)
	{
		[contactsView removeFromSuperview];
		[contactsView release];
	}
	
	CGRect tzvFrame = CGRectMake(0.0, 0.0, self.contentView.bounds.size.width, self.contentView.bounds.size.height);
	contactsView = [[DialContactsView alloc] initWithFrame:tzvFrame];
	contactsView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	contactsView.backgroundColor = [UIColor whiteColor];
	[self.contentView addSubview:contactsView];
	self.selectionStyle = UITableViewCellSelectionStyleBlue; 
	
}

- (void) setGrayColor {
	contactsView.backgroundColor = [UIColor colorWithRed:216.0/255 green:216.0/255 blue:216.0/255 alpha:1.0];
}

- (void) setWhiteColor {
	contactsView.backgroundColor = [UIColor whiteColor];
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {

    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

- (void) setContactInformationWrapper: (ContactInformationWrapper *)contactInfoWrapper {
	contactsView.contactRecord = contactInfoWrapper;
	[contactsView setNeedsDisplay];
}

- (void)dealloc {
	[contactsView release];
    [super dealloc];
}


@end
