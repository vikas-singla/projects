//
//  RecentCallController.h
//  Fast Dial
//
//  Created by Vikas on 12/22/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "CallMechanism.h"

@interface RecentCallController : UIViewController <UITableViewDelegate, UITableViewDataSource> {

	UITableView	*contactListView;

	CallMechanism* callMechanismInstance;
}

@property (nonatomic, retain)	UITableView	*contactListView;
@property (nonatomic, assign) CallMechanism* callMechanismInstance;

@end
