//
//  ContactsViewController.h
//  Fast Dial
//
//  Created by Vikas on 12/19/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "CustomTabBarController.h"

@interface ContactsViewController : UIViewController {
	IBOutlet CustomTabBarController	*tabBarController;
}

@property (nonatomic, retain) IBOutlet CustomTabBarController	*tabBarController;

@end
