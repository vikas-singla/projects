//
//  iBrowserViewController.h
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface iBrowserViewController : UIViewController <UITextFieldDelegate, UIWebViewDelegate, UIActionSheetDelegate, UITableViewDelegate, UITableViewDataSource, UIAccelerometerDelegate> {
	IBOutlet UIView*	urlView;
	IBOutlet UIView*	toolbarView;
	IBOutlet UIWebView*	webBrowser;
	IBOutlet UITextField* urlTextField;
	IBOutlet UITextField* searchTextField;
	
	IBOutlet UIButton* backButton;
	IBOutlet UIButton* forwardButton;
	IBOutlet UIButton* refreshButton;
	IBOutlet UIButton* stopLoadingButton;
	IBOutlet UIButton* homePageButton;
	IBOutlet UIButton* pageActionButton;
	IBOutlet UIButton* favoritesButton;
	IBOutlet UIButton* settingsButton;
	IBOutlet UIButton* moreButtonsButton;
	IBOutlet UIButton* lessButtonsButton;
	IBOutlet UIButton* findButton;
	IBOutlet UIButton* lockRotationButton;
	
	IBOutlet UIImageView* urlViewImage;	
	IBOutlet UIButton* cancelInputButton;
	IBOutlet UILabel* webAddressLabel;
	IBOutlet UILabel* searchLabel;
	IBOutlet UILabel* pageTitleLabel;
	IBOutlet UIImageView* toolbarViewImage;
	IBOutlet UIButton* fullScreenButton;
	IBOutlet UIButton* tabsButton;
	IBOutlet UIButton* fullScreenBackButton;
	IBOutlet UIButton* fullScreenForwButton;
	IBOutlet UIButton* fullScreenShowMenuBtn;
	IBOutlet UIButton* fullScreenShowBookmarksBtn;
	IBOutlet UIView* fullScreenBackForwBtnView;
	IBOutlet UIView* tabsSelectionView;
	IBOutlet UITableView* tabsSelectionTable;
	IBOutlet UIView* browserParentView;
	IBOutlet UIButton* tabEditButton;
	IBOutlet UIView* progressParentView;
	IBOutlet UIProgressView* progressView;
	IBOutlet UIImageView* progressImageView;
	IBOutlet UIImageView* miniTabParentImageView;
	IBOutlet UIView* autoScrollView;
	IBOutlet UIButton* autoScrollUpButton;
	IBOutlet UIButton* autoScrollDownButton;
	IBOutlet UIView* downloadImageProgressView;
	IBOutlet UIScrollView* minimizedTabView;
	IBOutlet UIView* miniTabParentView;
	IBOutlet UIButton* maximizeMiniTabBtn;
	IBOutlet UIButton* closeMiniTabBtn;
	IBOutlet UIButton* newTabMiniTabBtn;
	IBOutlet UIButton* editTabsMiniTabBtn;
	IBOutlet UIImageView* miniTabSeperator;
	IBOutlet UIButton* minMaxToolbarBtn;
	
	IBOutlet UIButton* historyButton;
	
	IBOutlet UIView* findTextView;
	IBOutlet UITextField* findTextField;
	
	UIImageView* blackBackgroundImageView;
	
	UIInterfaceOrientation currentOrientation;
	
	NSMutableDictionary* tabs;
	
	NSMutableDictionary* tabButtons;
	
	NSMutableDictionary* tabDeleteButtons;
	
	NSMutableDictionary* favicons;
	
	NSMutableArray* historyFolder;
	
	NSMutableDictionary* bookmarkFoldersDict;
	
	NSTimer					*activeTimer;
	
	NSUserDefaults* userDefaults;
	bool rotateNotificationReqd;
	
	bool needToShowFullScreen;
	
	bool rotationLocked;
	
	bool rotationHardLocked;
	
	bool popupNeedToShowStatusBar;
	
	float yVelocity, xVelocity, xx, yy;
	float yaccel, xaccel;
	
	bool showProgressBar;
	
	NSTimer* scrollTimer;
	
	bool tabViewIsVisible;
	bool miniTabViewIsVisible;
	bool showTabsAfterRotation;
	bool showMiniTabsAfterRotation;
	
	bool showMiniTabsWhenDoneEdit;
	
	bool downloadImageViewIsVisible;
	bool showDownloadImageViewAfterRotation;
	
	bool findTextViewIsVisible;
	bool showFindTextViewAfterRotation;
	
	NSURLAuthenticationChallenge* challenge;
	
	NSDictionary* tapAndHoldElement;
	
	UIActionSheet* pageActionSheet;
	UIActionSheet* tapAndHoldLinkActionSheet;
	UIActionSheet* tapAndHoldLinkAndImageActionSheet;
	UIActionSheet* tapAndHoldImageActionSheet;
	UIActionSheet* historyActionSheet;
	
	bool ignoreLinkTap;
	id origWebViewPolicyDelegate;
	id origWebViewResourceLoadDelegate;
	
	bool showingMoreButtons;
	
	UIImage* desktopTabImg;
	UIImage* desktopTabHighlightedImg;
	
	UIImage* tabDeleteImg;
	UIImage* tabDeleteHighlightedImg;
	
	bool toolbarIsMinimized;
	
	bool minimizeToolbarAfterRotation;
	
	NSTimer* progressTimer;
	
	bool showingFullScreenView;
}

@property (nonatomic, assign) bool tabViewIsVisible;
@property (nonatomic, assign) bool miniTabViewIsVisible;
@property (nonatomic, assign) bool downloadImageViewIsVisible;
@property (nonatomic, assign) bool findTextViewIsVisible;

@property (nonatomic, assign) bool ignoreLinkTap;

@property (nonatomic, assign) NSTimer* scrollTimer;

@property (nonatomic, assign) bool popupNeedToShowStatusBar;

@property (nonatomic, assign) NSMutableDictionary* bookmarkFoldersDict;
@property (nonatomic, assign) NSMutableArray* historyFolder;

@property (nonatomic, assign) UIInterfaceOrientation currentOrientation;

@property (nonatomic, assign)	NSTimer*	activeTimer;

@property (nonatomic, retain) UIButton* fullScreenShowBookmarksBtn;
@property (nonatomic, assign) UIImageView* blackBackgroundImageView;
@property (nonatomic, assign) bool rotationHardLocked;
@property (nonatomic, assign) bool rotationLocked;
@property (nonatomic, assign) bool needToShowFullScreen;
@property (nonatomic, assign) NSMutableDictionary* tabs;
@property (nonatomic, assign) NSMutableDictionary* tabDeleteButtons;
@property (nonatomic, assign) NSMutableDictionary* tabButtons;
@property (nonatomic, assign) NSMutableDictionary* favicons;
@property (nonatomic, assign) bool rotateNotificationReqd;
@property (nonatomic, assign) NSUserDefaults* userDefaults;
@property (nonatomic, retain) IBOutlet UIView* browserParentView;

@property (nonatomic, retain) IBOutlet UIScrollView* minimizedTabView;
@property (nonatomic, retain) IBOutlet UIView* findTextView;
@property (nonatomic, retain) IBOutlet UITextField* findTextField;
@property (nonatomic, retain) IBOutlet UIView* downloadImageProgressView;
@property (nonatomic, retain) IBOutlet UIButton* autoScrollUpButton;
@property (nonatomic, retain) IBOutlet UIButton* autoScrollDownButton;
@property (nonatomic, retain) IBOutlet UIView* autoScrollView;
@property (nonatomic, retain) IBOutlet UIImageView* progressImageView;
@property (nonatomic, retain) IBOutlet UIView* progressParentView;
@property (nonatomic, retain) IBOutlet UIProgressView* progressView;
@property (nonatomic, retain) IBOutlet UIButton* tabEditButton;
@property (nonatomic, retain) IBOutlet UIButton* minMaxToolbarBtn;
@property (nonatomic, retain) IBOutlet UIButton* lockRotationButton;
@property (nonatomic, retain) IBOutlet UITableView* tabsSelectionTable;
@property (nonatomic, retain) IBOutlet UIView* miniTabParentView;
@property (nonatomic, retain) IBOutlet UIView* tabsSelectionView;
@property (nonatomic, retain) IBOutlet UIView* urlView;
@property (nonatomic, retain) IBOutlet UIView* toolbarView;
@property (nonatomic, retain) IBOutlet UIWebView*	webBrowser;
@property (nonatomic, retain) IBOutlet UITextField*	urlTextField;
@property (nonatomic, retain) IBOutlet UITextField*	searchTextField;

@property (nonatomic, retain) IBOutlet UIImageView* miniTabSeperator;

@property (nonatomic, retain) IBOutlet UIButton* newTabMiniTabBtn;
@property (nonatomic, retain) IBOutlet UIButton* editTabsMiniTabBtn;
@property (nonatomic, retain) IBOutlet UIButton* maximizeMiniTabBtn;
@property (nonatomic, retain) IBOutlet UIButton* closeMiniTabBtn;
@property (nonatomic, retain) IBOutlet UIButton* backButton;
@property (nonatomic, retain) IBOutlet UIButton* forwardButton;
@property (nonatomic, retain) IBOutlet UIButton* refreshButton;
@property (nonatomic, retain) IBOutlet UIButton* pageActionButton;
@property (nonatomic, retain) IBOutlet UIButton* favoritesButton;
@property (nonatomic, retain) IBOutlet UIButton* settingsButton;
@property (nonatomic, retain) IBOutlet UIButton* stopLoadingButton;
@property (nonatomic, retain) IBOutlet UIButton* homePageButton;
@property (nonatomic, retain) IBOutlet UIButton* moreButtonsButton;
@property (nonatomic, retain) IBOutlet UIButton* lessButtonsButton;
@property (nonatomic, retain) IBOutlet UIButton* findButton;
@property (nonatomic, retain) IBOutlet UIButton* historyButton;

@property (nonatomic, retain) IBOutlet UIImageView* toolbarViewImage;
@property (nonatomic, retain) IBOutlet UIImageView* urlViewImage;
@property (nonatomic, retain) IBOutlet UIImageView* miniTabParentImageView;
@property (nonatomic, retain) IBOutlet UIButton* cancelInputButton;
@property (nonatomic, retain) IBOutlet UILabel* webAddressLabel;
@property (nonatomic, retain) IBOutlet UILabel* searchLabel;
@property (nonatomic, retain) IBOutlet UILabel* pageTitleLabel;
@property (nonatomic, retain) IBOutlet UIButton* fullScreenButton;
@property (nonatomic, retain) IBOutlet UIButton* tabsButton;
@property (nonatomic, retain) IBOutlet UIView* fullScreenBackForwBtnView;
@property (nonatomic, retain) IBOutlet UIButton* fullScreenForwButton;
@property (nonatomic, retain) IBOutlet UIButton* fullScreenBackButton;
@property (nonatomic, retain) IBOutlet UIButton* fullScreenShowMenuBtn;

- (IBAction) autoScrollUp: (id) sender;
- (IBAction) autoScrollDown: (id) sender;
- (IBAction) autoScrollCancel: (id) sender;

- (IBAction) goBack: (id) sender;
- (IBAction) goForward: (id) sender;
- (IBAction) refreshBrowser: (id) sender;
- (IBAction) stopBrowser: (id) sender;
- (IBAction) loadHomePage: (id) sender;
- (IBAction) cancelInput: (id) sender;
- (IBAction) showBrowserSettings: (id) sender;
- (IBAction) showBookmarks: (id) sender;
- (IBAction) pageActionChosen: (id) sender;
- (IBAction) exitFullScreenView: (id) sender;
- (IBAction) showCustomTabsSelectionView: (id) sender;
- (IBAction) hideCustomTabsSelectionView: (id) sender;
- (IBAction) showFullScreenView: (id) sender;
- (IBAction) addTabAction: (id) sender;
- (IBAction) tabEditAction: (id) sender;
- (IBAction) setManualRotationLock: (id) sender;
- (IBAction) showLessBrowserButtons: (id) sender;
- (IBAction) showMoreBrowserButtons: (id) sender;
- (IBAction) findButtonAction: (id) sender;
- (IBAction) hideFindTextView: (id) sender;
- (IBAction) performTextSearch: (id) sender;
- (IBAction) showFullViewFromMiniTabView: (id) sender;
- (IBAction) showMinimizedTabView: (id) sender;
- (IBAction) showMiniTabsSelectionView: (id) sender;
- (IBAction) hideMiniTabsSelectionView: (id) sender;
- (IBAction) editMiniTabView: (id) sender;
- (IBAction) historyButtonTouched: (id) sender;
- (IBAction) showHideBrowserToolbar: (id) sender;

- (UIWebView*) getCurrentBrowserView;
- (void) initWebBrowserDelegate;
- (void) removeWebBrowserDelegate;
- (void) useLoginInfo: (NSString*) name : (NSString*) password;
- (void) cancelAuthentication;
- (void) loadURL : (NSString*) urlToLoad;
- (void) forceAutoRefresh;
- (void) setAccelerometerDelegate;
- (void) removeAccelerometerDelegate;
- (NSString*) formatURLText: (NSString*) urlText;
- (void) setTheme: (id)anObject;
- (void) loadHistoryBookmarks;
- (void) formatAndSetURLTextFieldText: (NSString*)urlToSet;
- (void)progressEstimateChanged:(NSTimer *)timer;
- (void) loadWebPage: (NSString*) linkURL;
- (void) stopLoadingBrowser;
- (void) checkTapAndHoldLocation: (CGPoint) location;
- (void) scrollToTop;
- (void) loadMinimizedTabView;
- (void) refreshTabTable: (id) anObject;
- (void) deleteTab: (int) indexPath;

@end

