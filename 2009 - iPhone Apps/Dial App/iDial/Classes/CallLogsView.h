//
//  CallLogsView.h
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface CallLogsView : UIView {
	NSArray* callLogRecord;
	BOOL	highlighted;
	BOOL	editing;
}

@property (nonatomic, retain) NSArray*	callLogRecord;
@property (nonatomic, getter=isHighlighted) BOOL highlighted;
@property (nonatomic, getter=isEditing) BOOL editing;

@end
