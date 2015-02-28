//
//  iBrowserViewController.m
//  iBrowser
//
//  Created by Vikas on 3/28/09.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
//

#import "iBrowserViewController.h"
#import "AppSettingsViewController.h"
#import "BookmarksController.h"
#import "AddBookmarkController.h"
#import "SettingsConstants.h"
#import "WebView.h"
#import "UIWebDocumentView.h"
#import "AuthenticationChallengeController.h"

static NSString* syncObj = @"SyncObj";

@implementation iBrowserViewController

@synthesize minMaxToolbarBtn;
@synthesize miniTabViewIsVisible;
@synthesize miniTabParentImageView;
@synthesize tabDeleteButtons;
@synthesize historyButton;
@synthesize newTabMiniTabBtn;
@synthesize editTabsMiniTabBtn;
@synthesize maximizeMiniTabBtn;
@synthesize closeMiniTabBtn;
@synthesize miniTabSeperator;
@synthesize miniTabParentView;
@synthesize tabButtons;
@synthesize minimizedTabView;
@synthesize favicons;
@synthesize fullScreenShowBookmarksBtn;
@synthesize tabViewIsVisible;
@synthesize findTextViewIsVisible;
@synthesize downloadImageViewIsVisible;
@synthesize findTextView;
@synthesize findTextField;
@synthesize moreButtonsButton;
@synthesize lessButtonsButton;
@synthesize findButton;
@synthesize ignoreLinkTap;
@synthesize downloadImageProgressView;
@synthesize autoScrollUpButton;
@synthesize autoScrollDownButton;
@synthesize autoScrollView;
@synthesize scrollTimer;
@synthesize popupNeedToShowStatusBar;
@synthesize bookmarkFoldersDict;
@synthesize currentOrientation;
@synthesize activeTimer;
@synthesize progressImageView;
@synthesize progressView;
@synthesize progressParentView;
@synthesize blackBackgroundImageView;
@synthesize tabEditButton;
@synthesize lockRotationButton;
@synthesize rotationHardLocked;
@synthesize rotationLocked;
@synthesize needToShowFullScreen;
@synthesize tabs;
@synthesize browserParentView;
@synthesize userDefaults;
@synthesize urlView;
@synthesize toolbarView;
@synthesize webBrowser;
@synthesize urlTextField;
@synthesize searchTextField;
@synthesize backButton;
@synthesize forwardButton;
@synthesize refreshButton;
@synthesize stopLoadingButton;
@synthesize homePageButton;
@synthesize cancelInputButton;
@synthesize urlViewImage;
@synthesize webAddressLabel;
@synthesize searchLabel;
@synthesize pageActionButton;
@synthesize favoritesButton;
@synthesize pageTitleLabel;
@synthesize rotateNotificationReqd;
@synthesize toolbarViewImage;
@synthesize settingsButton;
@synthesize fullScreenButton;
@synthesize tabsButton;
@synthesize fullScreenBackForwBtnView;
@synthesize fullScreenBackButton;
@synthesize fullScreenForwButton;
@synthesize fullScreenShowMenuBtn;
@synthesize tabsSelectionView;
@synthesize tabsSelectionTable;
@synthesize historyFolder;

+ (void)initialize{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSArray	*bookmarkLinks = [NSArray arrayWithObjects:nil];
	NSArray	*historyLinks = [NSArray arrayWithObjects:nil];
	NSArray *defaultTabInfo	= [NSArray arrayWithObjects:@"Google", @"http://www.google.com", nil];
	NSDictionary *defaultTabs = [NSDictionary dictionaryWithObjectsAndKeys:defaultTabInfo, @"0", nil];
	NSArray	*bookmarksFolders = [NSDictionary dictionaryWithObjectsAndKeys: historyLinks, kIBrowserHistoryFolder, bookmarkLinks, kIBrowserParentBookmarkFolder, nil];
    NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
								 kIBrowserGoogleVal, kIBrowserSearchEngine, bookmarksFolders, kIBrowserBookmarkFolders, 
								 kIBrowserDefaultHomePage, kIBrowserHomePage, defaultTabs, kIBrowserTabsInfo, @"0", kIBrowserCurrentTabIndex, 
								 @"0.70", kIbrowserTransparency, kIBrowserFalse, kIbrowserShakeToShow, kIBrowserThemeBlack, kIBrowserTheme,
								 kIBrowserFalse, kIBrowserCompressPages, kIBrowserTrue, kIBrowserDisplayHistory, kIBrowserFalse, kIBrowserLaunchHomePage, 
								 kIBrowserFalse, kIBrowserAutoScroll, kIBrowserFalse, kIBrowserManualAutoScroll, kIBrowserTrue, kIBrowserShowProgressBar,
								 kIBrowserFalse, kIBrowserShowingMiniTabs, kIbrowserTabStyleDeskstop, kIbrowserTabStyle, @"0", kIbrowserDiagnosticVal, nil];
	
    [defaults registerDefaults:appDefaults];
}

- (void) setHomePageForLaunch {
	NSString* homePage = [userDefaults stringForKey:kIBrowserHomePage];
	
	NSArray *defaultTabInfo	= [NSArray arrayWithObjects:@"", homePage, nil];
	NSDictionary *defaultTabs = [NSDictionary dictionaryWithObjectsAndKeys:defaultTabInfo, @"0", nil];
	
	[userDefaults setObject:defaultTabs forKey:kIBrowserTabsInfo];
	[userDefaults setObject:@"0" forKey:kIBrowserCurrentTabIndex];
}

- (void) hideStatusBar {
	[[UIApplication sharedApplication] setStatusBarHidden:YES animated:YES];
}

- (void) showStatusBar {
	[[UIApplication sharedApplication] setStatusBarHidden:NO animated:YES];
}

- (void) loadHistoryBookmarks {
	NSDictionary* bookmarkFoldersDictTemp = [[NSUserDefaults standardUserDefaults] dictionaryForKey:kIBrowserBookmarkFolders];

	if(bookmarkFoldersDict != nil)
	{
		[bookmarkFoldersDict release];
		bookmarkFoldersDict = nil;
	}
	
	bookmarkFoldersDict = [[NSMutableDictionary alloc] init];
	[bookmarkFoldersDict setDictionary:bookmarkFoldersDictTemp];
	
	NSArray* history = [bookmarkFoldersDict objectForKey:kIBrowserHistoryFolder];
	
	if(historyFolder != nil)
	{
		[historyFolder release];
		historyFolder = nil;
	}
	
	historyFolder = [[NSMutableArray alloc] initWithCapacity:[history count]];
	[historyFolder addObjectsFromArray:history];
}

- (void) forceAutoRefresh {

	NSString* pageURL = urlTextField.text;
	
	if(pageURL == nil || ([pageURL compare:@""] == NSOrderedSame))
	{
		return;
	}
	
	[self loadURL:pageURL];
}

- (void) initWebBrowserDelegate {
	WebView *coreWebView = [[webBrowser _documentView] webView];

	origWebViewPolicyDelegate = [coreWebView policyDelegate];
	origWebViewResourceLoadDelegate = [coreWebView resourceLoadDelegate];	
	
	objc_msgSend(coreWebView, @selector(setPolicyDelegate:),self);
	objc_msgSend(coreWebView, @selector(setResourceLoadDelegate:), self);
	
	webBrowser.delegate = self;
}

- (void) removeWebBrowserDelegate {
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	objc_msgSend(coreWebView, @selector(setPolicyDelegate:), origWebViewPolicyDelegate);
	objc_msgSend(coreWebView, @selector(setResourceLoadDelegate:), origWebViewResourceLoadDelegate);
	
	webBrowser.delegate = self;
}

- (void) useLoginInfo: (NSString*) name : (NSString*) password {
	id sender = [challenge sender];
		
	NSURLCredential* credential = [NSURLCredential credentialWithUser:name password:password persistence:NSURLCredentialPersistenceForSession];
	
	[sender useCredential: credential forAuthenticationChallenge:challenge];
	
	challenge = nil;
}

- (void) cancelAuthentication {
	id sender = [challenge sender];
	
	[sender cancelAuthenticationChallenge: challenge];
	
	challenge = nil;
	
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	NSString* pageURL = coreWebView.mainFrameURL;
	
	[self formatAndSetURLTextFieldText:pageURL];
}

- (void)webView:(WebView *)sender resource:(id)identifier didReceiveAuthenticationChallenge:(NSURLAuthenticationChallenge *)challenge_ fromDataSource:(id)dataSource {
	challenge = challenge_;
	
	AuthenticationChallengeController* challengeController = nil;
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		challengeController = [[AuthenticationChallengeController alloc]  initWithNibName:@"AuthenticationChallengeView" bundle:nil];
	}
	else
	{
		challengeController = [[AuthenticationChallengeController alloc]  initWithNibName:@"AuthenticationChallengeViewLandscape" bundle:nil];
	}
	
	challengeController.browserController = self;
	
	challengeController.expectedInterfaceOrientation = self.interfaceOrientation;
	
	[self hideStatusBar];
	
	popupNeedToShowStatusBar = YES;
	
	[self presentModalViewController: challengeController animated:YES];
	
	[challengeController release];
}

- (void) addHistoryBookmark: (UIWebView*)webView {
	
	if(webView.loading)
	{
		return;
	}
	
	if([[userDefaults stringForKey:kIBrowserDisplayHistory] compare:kIBrowserFalse] == NSOrderedSame)
	{
		return;
	}
	
	WebView *coreWebView = [[webView _documentView] webView];
	
	NSString* pageHeader = coreWebView.mainFrameTitle;
	NSString* pageURL = coreWebView.mainFrameURL;
	
	if(pageURL != nil)
	{
		pageURL = [self formatURLText:pageURL];
	}
	
	if(pageHeader == nil || [pageHeader compare:@""] == NSOrderedSame)
	{
		return;
	}
	
	if(pageURL == nil || [pageURL compare:@""] == NSOrderedSame)
	{
		return;
	}
	
	NSDate* currTime = [NSDate date];
	NSDateFormatter* dateFormatter = [[[NSDateFormatter alloc] init] autorelease];
	[dateFormatter setDateStyle:NSDateFormatterMediumStyle];
	[dateFormatter setTimeStyle:NSDateFormatterShortStyle];
	NSString* timestampStr = [[dateFormatter stringFromDate: currTime] copy];
	
	if([historyFolder count] > 0)
	{
		for(int i = 0; i < [historyFolder count]; ++i)
		{
			NSDictionary* bookmark = [historyFolder objectAtIndex:i];
			
			NSString* name = [bookmark objectForKey:kIBrowserLinkName];
			
			if([name compare:pageHeader options: NSCaseInsensitiveSearch] == NSOrderedSame)
			{
				[historyFolder removeObject:bookmark];
				
				break;
			}
		}
	}
	
	NSMutableDictionary* newBookmark = [[NSMutableDictionary alloc] init];
	[newBookmark setObject:pageHeader forKey:kIBrowserLinkName];
	[newBookmark setObject:pageURL forKey:kIBrowserLinkURL];
	[newBookmark setObject:timestampStr forKey:kIBrowserLinkDesc];
	
	[historyFolder insertObject:newBookmark atIndex:0];
	
	[bookmarkFoldersDict setObject:historyFolder forKey:kIBrowserHistoryFolder];
	
	[userDefaults setObject:bookmarkFoldersDict forKey:kIBrowserBookmarkFolders];
	
	[userDefaults synchronize];
	
	[newBookmark release];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
	
	if(!rotationLocked && !rotationHardLocked)
	{
		/**
		if(interfaceOrientation == UIInterfaceOrientationLandscapeLeft || interfaceOrientation == UIInterfaceOrientationLandscapeRight)
		{
			if(currentOrientation == UIInterfaceOrientationPortrait || currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
			{
				self.rotateNotificationReqd = YES;
			}
			else
			{
				self.rotateNotificationReqd = NO;
			}
		}
		else if(interfaceOrientation == UIInterfaceOrientationPortrait || interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationLandscapeRight)
			{
				self.rotateNotificationReqd = YES;
			}
			else
			{
				self.rotateNotificationReqd = NO;
			}
		}
		 **/
		
		self.rotateNotificationReqd = YES;
		
		currentOrientation = interfaceOrientation;
	}
	
	return (!rotationLocked && !rotationHardLocked);
}

- (void) showProgressIndicator {	
	
	[UIApplication sharedApplication].networkActivityIndicatorVisible = YES;
	
	UIImage* stopImg = [UIImage imageNamed:@"StopLoadIcon.png"];
	UIImage* stopImgHigh = [UIImage imageNamed:@"StopLoadIconHighlighted.png"];
	
	[refreshButton setImage:stopImg forState:UIControlStateNormal];
	[refreshButton setImage:stopImgHigh forState:UIControlStateHighlighted];	
	[refreshButton setImage:stopImgHigh forState:UIControlStateSelected];
	
	//----------------
	
	if(showProgressBar)
	{		
		int offset = 0;
		
		if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 394 + StatusBarHeight + offset, progressParentView.frame.size.width, progressParentView.frame.size.height);
		}
		else
		{
			progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 234 + StatusBarHeight + offset, progressParentView.frame.size.width, progressParentView.frame.size.height);
		}
		
		[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
		[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
		[ UIView setAnimationDuration: 0.4f ]; // Set the duration to 5/10ths of a second.
		
		[progressParentView setAlpha:1.0];
		
		[UIView commitAnimations];
	}
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error {

	//[webBrowser stopLoading];
	
	//UIImage* stopImg = [UIImage imageNamed:@"RefreshIcon.png"];
	//UIImage* stopImgHigh = [UIImage imageNamed:@"RefreshIconHighlighted.png"];
	
	//[refreshButton setImage:stopImg forState:UIControlStateNormal];
	//[refreshButton setImage:stopImgHigh forState:UIControlStateHighlighted];	
	//[refreshButton setImage:stopImgHigh forState:UIControlStateSelected];
	
	if ([error code] != -999) 
	{	
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Page Load Error" message:[@"Could not load page. Error: " stringByAppendingString:error.localizedDescription]
													delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];	
	}
}

-(void) hideNetworkIndicatorIfApplicable {
	bool result = YES;
	NSArray* tabBrowsers = [tabs allValues];
	for(int i = 0; i < [tabBrowsers count]; ++i)
	{
		UIWebView* tempWeb = (UIWebView*)[tabBrowsers objectAtIndex:i];
		if(tempWeb.loading)
		{
			result = NO;
			break;
		}
	}
	
	if(result)
	{
		[UIApplication sharedApplication].networkActivityIndicatorVisible = NO;
	}
	else
	{
		[UIApplication sharedApplication].networkActivityIndicatorVisible = YES;
	}
}

- (void) hideProgressIndicator {
	
	// finished loading, hide the activity indicator in the status bar
	[self hideNetworkIndicatorIfApplicable];
	
	UIImage* stopImg = [UIImage imageNamed:@"RefreshIcon.png"];
	UIImage* stopImgHigh = [UIImage imageNamed:@"RefreshIconHighlighted.png"];
	
	[refreshButton setImage:stopImg forState:UIControlStateNormal];
	[refreshButton setImage:stopImgHigh forState:UIControlStateHighlighted];	
	[refreshButton setImage:stopImgHigh forState:UIControlStateSelected];
	
	[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	[progressParentView setAlpha: 0.0];
	
	[UIView commitAnimations];
	
	[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.9f ];
	
	if([UIApplication sharedApplication].statusBarHidden == YES)
	{
		progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 494, progressParentView.frame.size.width, progressParentView.frame.size.height);
	}
	else
	{
		progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 494, progressParentView.frame.size.width, progressParentView.frame.size.height);
	}
	
	[UIView commitAnimations];
}

- (void)progressEstimateChanged:(NSTimer *)timer {
	// You can get the progress as a float with
	// [[theNotification object] estimatedProgress], and then you
	// can set that to a UIProgressView if you'd like.
	// theProgressView is just an example of what you could do.
	
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	float progress = [coreWebView estimatedProgress];
	
	[progressView setProgress:progress];
	if (progress == 0.0) {
		[timer invalidate];
		progressTimer = nil;
		
		[self hideProgressIndicator];
	}
	else
	{
		[self showProgressIndicator];
	}
}

- (void) hideFullScreenButtons {
	[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.4f ]; // Set the duration to 5/10ths of a second.
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{						
		fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 481, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
	}
	else
	{			
		fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 321, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
	}
	
	[UIView commitAnimations];
}

- (IBAction) exitFullScreenView: (id) sender {
	
	if(activeTimer.isValid)
	{
		[activeTimer invalidate];
	}
	
	[[UIApplication sharedApplication] setStatusBarHidden:NO animated:YES];
	
	[self hideFullScreenButtons];
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.3f ]; // Set the duration to 5/10ths of a second.
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{				
		urlView.frame = CGRectMake(urlView.frame.origin.x, StatusBarHeight, 320, 61);
		
		toolbarView.frame = CGRectMake(toolbarView.frame.origin.x, 424 + StatusBarHeight, 320, 37);
		
		browserParentView.frame = CGRectMake(0, 61 + StatusBarHeight, 320, 364);
		
		webBrowser.frame = CGRectMake(0, 0, 320, 364);		
	}
	else
	{			
		urlView.frame = CGRectMake(0, StatusBarHeight, 480, 61);
		
		toolbarView.frame = CGRectMake(0, 264 + StatusBarHeight, 480, 37);
		
		browserParentView.frame = CGRectMake(0, 61 + StatusBarHeight, 480, 204);
		
		webBrowser.frame = CGRectMake(0, 0, 480, 204);		
	}
	
	[UIView commitAnimations];		
	
	showingFullScreenView = NO;
}

- (IBAction) showMinimizedTabView: (id) sender {
	if(tabViewIsVisible)
	{
		[self hideCustomTabsSelectionView:sender];
	}
	
	[self showMiniTabsSelectionView:sender];
}

- (IBAction) showFullViewFromMiniTabView: (id) sender {	
	[self hideMiniTabsSelectionView:sender];
	
	[self showCustomTabsSelectionView:sender];
}

- (IBAction) showCustomTabsSelectionView: (id) sender {
	
	if([[userDefaults objectForKey:kIbrowserTabStyle] compare:kIbrowserTabStyleDeskstop] == NSOrderedSame)
	{
		[self showMinimizedTabView:sender];
		return;
	}
	
	tabViewIsVisible = YES;
			
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		tabsSelectionView.frame = CGRectMake(10, tabsSelectionView.frame.origin.y, tabsSelectionView.frame.size.width, tabsSelectionView.frame.size.height);
	}
	else
	{
		tabsSelectionView.frame = CGRectMake(95, tabsSelectionView.frame.origin.y, tabsSelectionView.frame.size.width, tabsSelectionView.frame.size.height);
	}
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseInOut ];
	[ UIView setAnimationDuration: 0.4f ]; // Set the duration to 5/10ths of a second.

	[tabsSelectionView setAlpha:0.71];
	
	[UIView commitAnimations];
}

- (IBAction) showMiniTabsSelectionView: (id) sender {	
	
	miniTabViewIsVisible = YES;
	
	int yOffset = 0;
	
	if(toolbarIsMinimized)
	{
		yOffset = -38;
	}
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		miniTabParentView.frame = CGRectMake(0, 80 + yOffset, miniTabParentView.frame.size.width, miniTabParentView.frame.size.height);
	}
	else
	{
		miniTabParentView.frame = CGRectMake(0, 80 + yOffset, miniTabParentView.frame.size.width, miniTabParentView.frame.size.height);
	}
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseInOut ];
	[ UIView setAnimationDuration: 0.4f ]; // Set the duration to 5/10ths of a second.

	[miniTabParentView setAlpha:0.95];
	
	[UIView commitAnimations];
}

- (IBAction) hideMiniTabsSelectionView: (id) sender {
	
	miniTabViewIsVisible = NO;
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseInOut ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	[miniTabParentView setAlpha:0];
	
	[UIView commitAnimations];
	
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseInOut ];
	[ UIView setAnimationDuration: 0.8f ];
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		miniTabParentView.frame = CGRectMake(0, -300, miniTabParentView.frame.size.width, miniTabParentView.frame.size.height);
	}
	else
	{
		miniTabParentView.frame = CGRectMake(0, -300, miniTabParentView.frame.size.width, miniTabParentView.frame.size.height);
	}	
	
	[UIView commitAnimations];
}

- (IBAction) showDownloadImageProgressView: (id) sender {
	downloadImageViewIsVisible = YES;
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		downloadImageProgressView.frame = CGRectMake(10, downloadImageProgressView.frame.origin.y, downloadImageProgressView.frame.size.width, downloadImageProgressView.frame.size.height);
	}
	else
	{
		downloadImageProgressView.frame = CGRectMake(95, downloadImageProgressView.frame.origin.y, downloadImageProgressView.frame.size.width, downloadImageProgressView.frame.size.height);
	}
}

- (IBAction) hideCustomTabsSelectionView: (id) sender {	
	tabViewIsVisible = NO;
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	[tabsSelectionView setAlpha:0];
	
	[UIView commitAnimations];
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.9f ];
	
	tabsSelectionView.frame = CGRectMake(-500, tabsSelectionView.frame.origin.y, tabsSelectionView.frame.size.width, tabsSelectionView.frame.size.height);
	
	[UIView commitAnimations];
}

- (IBAction) hideDownloadImageProgressView: (id) sender {
	downloadImageViewIsVisible = NO;
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	downloadImageProgressView.frame = CGRectMake(-500, downloadImageProgressView.frame.origin.y, downloadImageProgressView.frame.size.width, downloadImageProgressView.frame.size.height);
	
	[UIView commitAnimations];
}

- (void) showFullScreenButtons {
	if([[userDefaults stringForKey:kIbrowserShakeToShow] compare:kIBrowserTrue] == NSOrderedSame)
	{
		fullScreenBackForwBtnView.alpha = 0.74f;
	}
	else
	{
		fullScreenBackForwBtnView.alpha = [userDefaults floatForKey:kIbrowserTransparency];
	}
	
	[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.6f ]; // Set the duration to 5/10ths of a second.
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{		
		fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 480 - fullScreenBackForwBtnView.frame.size.height, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
	}
	else
	{		
		fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 320 - fullScreenBackForwBtnView.frame.size.height, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
	}
	
	[UIView commitAnimations];
}

- (IBAction) editMiniTabView: (id) sender {
	[self hideMiniTabsSelectionView:0];
	
	[userDefaults setObject:kIBrowserFalse forKey:kIBrowserShowingMiniTabs];
	
	[self showCustomTabsSelectionView:0];
	
	[self tabEditAction:0];
	showMiniTabsWhenDoneEdit = YES;
}

- (IBAction) showFullScreenView: (id) sender {
	
	if(toolbarIsMinimized)
	{
		[self showHideBrowserToolbar:0];
	}	
	
	[[UIApplication sharedApplication] setStatusBarHidden:YES animated:YES];
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		urlView.frame = CGRectMake(0, -85, 320, 61);
		
		toolbarView.frame = CGRectMake(0, 550, 320, 37);
		
		browserParentView.frame = CGRectMake(0, 0, 320, 500);
		
		webBrowser.frame = CGRectMake(0, 0, 320, 480);
	}
	else
	{
		urlView.frame = CGRectMake(urlView.frame.origin.x, -85, 480, 61);
		
		toolbarView.frame = CGRectMake(toolbarView.frame.origin.x, 400, 480, 37);
		
		browserParentView.frame = CGRectMake(0, 0, 480, 340);
		
		webBrowser.frame = CGRectMake(0, 0, 480, 320);
	}
	
	[UIView commitAnimations];	
	
	if([[userDefaults stringForKey:kIbrowserShakeToShow] compare:kIBrowserTrue] != NSOrderedSame)
	{
		[self showFullScreenButtons];
	}
	
	showingFullScreenView = YES;
}

- (void) setManualOrientationPortrait {
	
	urlView.frame = CGRectMake(urlView.frame.origin.x, StatusBarHeight, 320, 61);
	
	toolbarView.frame = CGRectMake(toolbarView.frame.origin.x, 424 + StatusBarHeight, 320, 37);
	
	progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 514, 320, progressParentView.frame.size.height);
	progressImageView.frame = CGRectMake(progressImageView.frame.origin.x, progressImageView.frame.origin.y, 320, progressImageView.frame.size.height);
	progressView.frame = CGRectMake(progressView.frame.origin.x, progressView.frame.origin.y, 200, progressView.frame.size.height);
	
	fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 481, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
		
	browserParentView.frame = CGRectMake(0, 81, 320, 364);
	
	webBrowser.frame = CGRectMake(0, 0, 320, 364);
	
	autoScrollView.frame = CGRectMake(280, 315, 30, 110);
	
	miniTabParentView.frame = CGRectMake(miniTabParentView.frame.origin.x, -300, 320, miniTabParentView.frame.size.height);
	minimizedTabView.frame = CGRectMake(minimizedTabView.frame.origin.x, minimizedTabView.frame.origin.y, 320, minimizedTabView.frame.size.height);
		
	urlViewImage.frame = CGRectMake(urlViewImage.frame.origin.x, urlViewImage.frame.origin.y, 350, 61);
	
	tabsSelectionView.frame = CGRectMake(tabsSelectionView.frame.origin.x, 70, 300, 275);
	
	toolbarViewImage.frame = CGRectMake(toolbarViewImage.frame.origin.x, toolbarViewImage.frame.origin.y, 350, 37);
	
	if(showingMoreButtons)
	{
		backButton.frame = CGRectMake(-500, backButton.frame.origin.y, backButton.frame.size.width, backButton.frame.size.height);
		forwardButton.frame = CGRectMake(-500, forwardButton.frame.origin.y, forwardButton.frame.size.width, forwardButton.frame.size.height);
		pageActionButton.frame = CGRectMake(-500, pageActionButton.frame.origin.y, pageActionButton.frame.size.width, pageActionButton.frame.size.height);
		favoritesButton.frame = CGRectMake(-500, favoritesButton.frame.origin.y, favoritesButton.frame.size.width, favoritesButton.frame.size.height);
		fullScreenButton.frame = CGRectMake(-500, fullScreenButton.frame.origin.y, fullScreenButton.frame.size.width, fullScreenButton.frame.size.height);
		tabsButton.frame = CGRectMake(-500, tabsButton.frame.origin.y, tabsButton.frame.size.width, tabsButton.frame.size.height);
		moreButtonsButton.frame = CGRectMake(-500, moreButtonsButton.frame.origin.y, moreButtonsButton.frame.size.width, moreButtonsButton.frame.size.height);
				
		lessButtonsButton.frame = CGRectMake(10, lessButtonsButton.frame.origin.y, lessButtonsButton.frame.size.width, lessButtonsButton.frame.size.height);
		findButton.frame = CGRectMake(55, findButton.frame.origin.y, findButton.frame.size.width, findButton.frame.size.height);
		lockRotationButton.frame = CGRectMake(100, lockRotationButton.frame.origin.y, lockRotationButton.frame.size.width, lockRotationButton.frame.size.height);
		settingsButton.frame = CGRectMake(145, settingsButton.frame.origin.y, settingsButton.frame.size.width, settingsButton.frame.size.height);		
	}
	else
	{
		lessButtonsButton.frame = CGRectMake(500, lessButtonsButton.frame.origin.y, lessButtonsButton.frame.size.width, lessButtonsButton.frame.size.height);
		findButton.frame = CGRectMake(500, findButton.frame.origin.y, findButton.frame.size.width, findButton.frame.size.height);
		lockRotationButton.frame = CGRectMake(500, lockRotationButton.frame.origin.y, lockRotationButton.frame.size.width, lockRotationButton.frame.size.height);
		settingsButton.frame = CGRectMake(500, settingsButton.frame.origin.y, settingsButton.frame.size.width, settingsButton.frame.size.height);
				
		backButton.frame = CGRectMake(10, backButton.frame.origin.y, backButton.frame.size.width, backButton.frame.size.height);
		forwardButton.frame = CGRectMake(50, forwardButton.frame.origin.y, forwardButton.frame.size.width, forwardButton.frame.size.height);
		pageActionButton.frame = CGRectMake(95, pageActionButton.frame.origin.y, pageActionButton.frame.size.width, pageActionButton.frame.size.height);
		favoritesButton.frame = CGRectMake(140, favoritesButton.frame.origin.y, favoritesButton.frame.size.width, favoritesButton.frame.size.height);
		fullScreenButton.frame = CGRectMake(185, fullScreenButton.frame.origin.y, fullScreenButton.frame.size.width, fullScreenButton.frame.size.height);
		tabsButton.frame = CGRectMake(230, tabsButton.frame.origin.y, tabsButton.frame.size.width, tabsButton.frame.size.height);
		moreButtonsButton.frame = CGRectMake(280, moreButtonsButton.frame.origin.y, moreButtonsButton.frame.size.width, moreButtonsButton.frame.size.height);
	}
	
	urlTextField.frame = CGRectMake(urlTextField.frame.origin.x, urlTextField.frame.origin.y, 150, urlTextField.frame.size.height);
	minMaxToolbarBtn.frame = CGRectMake(290, minMaxToolbarBtn.frame.origin.y, minMaxToolbarBtn.frame.size.width, minMaxToolbarBtn.frame.size.height);
	pageTitleLabel.frame = CGRectMake(pageTitleLabel.frame.origin.x, pageTitleLabel.frame.origin.y, 240, pageTitleLabel.frame.size.height);
	searchTextField.frame = CGRectMake(216, searchTextField.frame.origin.y, 97, searchTextField.frame.size.height);
	refreshButton.frame = CGRectMake(185, refreshButton.frame.origin.y, refreshButton.frame.size.width, refreshButton.frame.size.height);
}

- (void) setManualOrientationLandscape {
	
	urlView.frame = CGRectMake(0, StatusBarHeight, 480, 61);
	
	toolbarView.frame = CGRectMake(0, 264 + StatusBarHeight, 480, 37);
	
	progressParentView.frame = CGRectMake(progressParentView.frame.origin.x, 350 + StatusBarHeight, 320 + 160, progressParentView.frame.size.height);
	progressImageView.frame = CGRectMake(progressImageView.frame.origin.x, progressImageView.frame.origin.y, 320 + 160, progressImageView.frame.size.height);
	progressView.frame = CGRectMake(progressView.frame.origin.x, progressView.frame.origin.y, 200 + 160, progressView.frame.size.height);
	
	fullScreenBackForwBtnView.frame = CGRectMake(fullScreenBackForwBtnView.frame.origin.x, 301 + StatusBarHeight, fullScreenBackForwBtnView.frame.size.width, fullScreenBackForwBtnView.frame.size.height);
		
	browserParentView.frame = CGRectMake(0, 61 + StatusBarHeight, 480, 204);
	
	webBrowser.frame = CGRectMake(0, 0, 480, 204);
	
	autoScrollView.frame = CGRectMake(440, 160, 30, 110);
	
	urlViewImage.frame = CGRectMake(urlViewImage.frame.origin.x, urlViewImage.frame.origin.y, 510, 61);
	
	tabsSelectionView.frame = CGRectMake(tabsSelectionView.frame.origin.x, 20, 300, 275);
	
	toolbarViewImage.frame = CGRectMake(toolbarViewImage.frame.origin.x, toolbarViewImage.frame.origin.y, 510, 37);
	
	miniTabParentView.frame = CGRectMake(0, -300, 480, miniTabParentView.frame.size.height);
	minimizedTabView.frame = CGRectMake(minimizedTabView.frame.origin.x, minimizedTabView.frame.origin.y, 480, minimizedTabView.frame.size.height);
	
	backButton.frame = CGRectMake(13, backButton.frame.origin.y, backButton.frame.size.width, backButton.frame.size.height);	
	forwardButton.frame = CGRectMake(48 + 7, forwardButton.frame.origin.y, forwardButton.frame.size.width, forwardButton.frame.size.height);	
	pageActionButton.frame = CGRectMake(88 + 14, pageActionButton.frame.origin.y, pageActionButton.frame.size.width, pageActionButton.frame.size.height);	
	favoritesButton.frame = CGRectMake(128 + 21, favoritesButton.frame.origin.y, favoritesButton.frame.size.width, favoritesButton.frame.size.height);	
	fullScreenButton.frame = CGRectMake(168 + 28, fullScreenButton.frame.origin.y, fullScreenButton.frame.size.width, fullScreenButton.frame.size.height);
	tabsButton.frame = CGRectMake(208 + 35, tabsButton.frame.origin.y, tabsButton.frame.size.width, tabsButton.frame.size.height);
	findButton.frame = CGRectMake(248 + 42, findButton.frame.origin.y, findButton.frame.size.width, findButton.frame.size.height);
	lockRotationButton.frame = CGRectMake(288 + 49, lockRotationButton.frame.origin.y, lockRotationButton.frame.size.width, lockRotationButton.frame.size.height);
	historyButton.frame = CGRectMake(328 + 56, historyButton.frame.origin.y, historyButton.frame.size.width, historyButton.frame.size.height);		
	settingsButton.frame = CGRectMake(368 + 63, settingsButton.frame.origin.y, settingsButton.frame.size.width, settingsButton.frame.size.height);
	
	moreButtonsButton.frame = CGRectMake(800, moreButtonsButton.frame.origin.y, moreButtonsButton.frame.size.width, moreButtonsButton.frame.size.height);
	lessButtonsButton.frame = CGRectMake(800, lessButtonsButton.frame.origin.y, lessButtonsButton.frame.size.width, lessButtonsButton.frame.size.height);
	
	urlTextField.frame = CGRectMake(urlTextField.frame.origin.x, urlTextField.frame.origin.y, 150 + 100, urlTextField.frame.size.height);	
	minMaxToolbarBtn.frame = CGRectMake(450, minMaxToolbarBtn.frame.origin.y, minMaxToolbarBtn.frame.size.width, minMaxToolbarBtn.frame.size.height);
	pageTitleLabel.frame = CGRectMake(pageTitleLabel.frame.origin.x, pageTitleLabel.frame.origin.y, 400, pageTitleLabel.frame.size.height);
	searchTextField.frame = CGRectMake(216 + 100, searchTextField.frame.origin.y, 97 + 60, searchTextField.frame.size.height);
	refreshButton.frame = CGRectMake(185 + 100, refreshButton.frame.origin.y, refreshButton.frame.size.width, refreshButton.frame.size.height);
}

- (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration {
	
	if([[userDefaults stringForKey:kIBrowserManualAutoScroll] compare:kIBrowserTrue] == NSOrderedSame)
	{
		[self autoScrollCancel:0];
	}
	
	if(rotateNotificationReqd)
	{
		if(tabViewIsVisible)
		{
			[self hideCustomTabsSelectionView:0];
			showTabsAfterRotation = YES;
		}
		else
		{
			showTabsAfterRotation = NO;
		}
		
		if(toolbarIsMinimized)
		{
			[self showHideBrowserToolbar:0];
			minimizeToolbarAfterRotation = YES;
		}	
		else
		{
			minimizeToolbarAfterRotation = NO;
		}
		
		if(miniTabViewIsVisible)
		{
			[self hideMiniTabsSelectionView:0];
			showMiniTabsAfterRotation = YES;
		}
		else
		{
			showMiniTabsAfterRotation = NO;
		}
		
		if(findTextViewIsVisible)
		{
			[self hideFindTextView:0];
			showFindTextViewAfterRotation = YES;
		}
		else
		{
			showFindTextViewAfterRotation = NO;
		}
		
		if([UIApplication sharedApplication].statusBarHidden)
		{
			needToShowFullScreen = YES;
			
			[self exitFullScreenView:0];
		}			
		
		if(toInterfaceOrientation == UIInterfaceOrientationPortrait || toInterfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			[self setManualOrientationPortrait];
		}
		else
		{
			[self setManualOrientationLandscape];
		}
	}
	
	[[UIApplication sharedApplication] setStatusBarHidden:YES animated:YES];
}

- (void)didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation {
	
	//self.view.bounds = CGRectMake(0, 20, 480, 300);
	
	yaccel = 5.0f;
	xaccel = 5.0f;
	yVelocity = 0.0f;
	xVelocity = 0.0f;
	
	if(showTabsAfterRotation)
	{
		[self showCustomTabsSelectionView:0]; 
		showTabsAfterRotation = NO;
	}
	
	if(showMiniTabsAfterRotation)
	{
		[self showMiniTabsSelectionView:0];
		showMiniTabsAfterRotation = NO;
	}
	
	if(showFindTextViewAfterRotation)
	{
		[self findButtonAction:0];
		showFindTextViewAfterRotation = NO;
	}
	
	if(minimizeToolbarAfterRotation)
	{
		[self showHideBrowserToolbar:0];
	}
	
	if(needToShowFullScreen)
	{		
		needToShowFullScreen = NO;
		
		[self showFullScreenView:0];
	}
	else
	{
		[[UIApplication sharedApplication] setStatusBarHidden:NO animated:YES];
	}
}

/*
// The designated initializer. Override to perform setup that is required before the view is loaded.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
        // Custom initialization
    }
    return self;
}
*/

/*
// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView {
}
*/

- (void) loadWebPage: (NSString*) linkURL {
	if(linkURL != nil)
	{
		linkURL = [linkURL stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
		
		if(linkURL.length > 0 && [linkURL rangeOfString:@"http://"].location == NSNotFound && [linkURL rangeOfString:@"https://"].location == NSNotFound)
		{
			linkURL = [@"http://" stringByAppendingString:linkURL];
		}
		
		if(linkURL != nil && [linkURL compare: @""] != NSNotFound)
		{	
			[self loadURL:linkURL];
		}
	}
}

- (void) didBeginEditingURL {
	
	rotationLocked = YES;
	
	[ UIView beginAnimations: @"URLAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.15f ]; // Set the duration to 5/10ths of a second.
	
	CGRect urlViewSize = [urlView frame];
	urlViewSize.size.height = urlViewSize.size.height + 20;
	urlView.frame = urlViewSize;
	
	CGRect miniTabSize = [miniTabParentView frame];
	miniTabSize.origin.y = miniTabSize.origin.y + 20;
	miniTabParentView.frame = miniTabSize;
	
	CGRect urlViewImageRect = [urlViewImage frame];
	urlViewImageRect.size.height = urlViewImageRect.size.height + 20;
	urlViewImage.frame = urlViewImageRect;
	
	CGRect pageTitleLabelRect = [pageTitleLabel frame];
	pageTitleLabelRect.origin.y = pageTitleLabelRect.origin.y - 30;
	pageTitleLabel.frame = pageTitleLabelRect;
	
	CGRect urlLabelRect = [webAddressLabel frame];
	urlLabelRect.origin.y = urlLabelRect.origin.y + 30;
	webAddressLabel.frame = urlLabelRect;
	
	CGRect homePageButtonRect = [homePageButton frame];
	homePageButtonRect.origin.x = homePageButtonRect.origin.x - 180;
	homePageButton.frame = homePageButtonRect;

	refreshButton.hidden = YES;
	CGRect refreshButtonRect = [refreshButton frame];
	refreshButtonRect.origin.x = refreshButtonRect.origin.x + 300;
	refreshButton.frame = refreshButtonRect;	
	
	minMaxToolbarBtn.hidden = YES;
	
	if([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait || [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y + 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x + 300;
		searchTextField.frame = searchTextFieldRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x - 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y + 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width + 154;
		urlTextField.frame = urlTextFieldRect;
	}
	else
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.x = cancelInputButtonRect.origin.x + 160;
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y + 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y + 20;
		searchTextField.frame = searchTextFieldRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x - 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y + 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width + 42;
		urlTextField.frame = urlTextFieldRect;
	}
	
	CGRect browserRect = [browserParentView frame];
	browserRect.size.height = browserRect.size.height - 20;
	browserRect.origin.y = browserRect.origin.y + 20;
	browserParentView.frame = browserRect;
	
	[UIView commitAnimations];
}

- (void) didEndEditingURL {
	
	rotationLocked = NO;
	
	[ UIView beginAnimations: @"URLAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.15f ]; // Set the duration to 5/10ths of a second.
	
	CGRect urlViewSize = [urlView frame];
	urlViewSize.size.height = urlViewSize.size.height - 20;
	urlView.frame = urlViewSize;
	
	CGRect miniTabSize = [miniTabParentView frame];
	miniTabSize.origin.y = miniTabSize.origin.y - 20;
	miniTabParentView.frame = miniTabSize;
	
	CGRect urlViewImageRect = [urlViewImage frame];
	urlViewImageRect.size.height = urlViewImageRect.size.height - 20;
	urlViewImage.frame = urlViewImageRect;
	
	CGRect urlLabelRect = [webAddressLabel frame];
	urlLabelRect.origin.y = urlLabelRect.origin.y - 30;
	webAddressLabel.frame = urlLabelRect;
	
	CGRect pageTitleLabelRect = [pageTitleLabel frame];
	pageTitleLabelRect.origin.y = pageTitleLabelRect.origin.y + 30;
	pageTitleLabel.frame = pageTitleLabelRect;
	
	CGRect homePageButtonRect = [homePageButton frame];
	homePageButtonRect.origin.x = homePageButtonRect.origin.x + 180;
	homePageButton.frame = homePageButtonRect;
		
	CGRect refreshButtonRect = [refreshButton frame];
	refreshButtonRect.origin.x = refreshButtonRect.origin.x - 300;
	refreshButton.frame = refreshButtonRect;
	refreshButton.hidden = NO;
	
	
	minMaxToolbarBtn.hidden = NO;
	
	if([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait || [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y - 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x - 300;
		searchTextField.frame = searchTextFieldRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x + 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y - 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width - 154;
		urlTextField.frame = urlTextFieldRect;
	}
	else
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.x = cancelInputButtonRect.origin.x - 160;
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y - 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y - 20;
		searchTextField.frame = searchTextFieldRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x + 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y - 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width - 42;
		urlTextField.frame = urlTextFieldRect;
	}
	
	CGRect browserRect = [browserParentView frame];
	browserRect.size.height = browserRect.size.height + 20;
	browserRect.origin.y = browserRect.origin.y - 20;
	browserParentView.frame = browserRect;
	
	[UIView commitAnimations];
}


- (IBAction) historyButtonTouched: (id) sender {
	
	if(historyActionSheet != nil)
	{
		[historyActionSheet release];
		historyActionSheet = nil;
	}	
	
	if(historyActionSheet == nil)
	{
		if([[userDefaults stringForKey:kIBrowserDisplayHistory] compare:kIBrowserFalse] == NSOrderedSame)
		{		
			historyActionSheet = [[UIActionSheet alloc] initWithTitle:@"Browser History"
													 delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
														otherButtonTitles:@"Clear History", @"Enable History", nil];
		}
		else
		{
			historyActionSheet = [[UIActionSheet alloc] initWithTitle:@"Browser History"
															 delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
													otherButtonTitles:@"Clear History", @"Disable History", nil];
		}
		
		historyActionSheet.actionSheetStyle = UIActionSheetStyleDefault;
	}
	
	[historyActionSheet showInView:self.view];
}

- (void) didBeginEditingSearch {
	
	rotationLocked = YES;
	
	[ UIView beginAnimations: @"URLAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.15f ]; // Set the duration to 5/10ths of a second.
	
	CGRect urlViewSize = [urlView frame];
	urlViewSize.size.height = urlViewSize.size.height + 20;
	urlView.frame = urlViewSize;
	
	CGRect miniTabSize = [miniTabParentView frame];
	miniTabSize.origin.y = miniTabSize.origin.y + 20;
	miniTabParentView.frame = miniTabSize;
	
	CGRect urlViewImageRect = [urlViewImage frame];
	urlViewImageRect.size.height = urlViewImageRect.size.height + 20;
	urlViewImage.frame = urlViewImageRect;
	
	CGRect pageTitleLabelRect = [pageTitleLabel frame];
	pageTitleLabelRect.origin.y = pageTitleLabelRect.origin.y - 30;
	pageTitleLabel.frame = pageTitleLabelRect;
	
	CGRect searchLabelRect = [searchLabel frame];
	searchLabelRect.origin.y = searchLabelRect.origin.y + 30;
	searchLabel.frame = searchLabelRect;
	
	CGRect homePageButtonRect = [homePageButton frame];
	homePageButtonRect.origin.x = homePageButtonRect.origin.x - 180;
	homePageButton.frame = homePageButtonRect;
	
	refreshButton.hidden = YES;
	CGRect refreshButtonRect = [refreshButton frame];
	refreshButtonRect.origin.x = refreshButtonRect.origin.x - 500;
	refreshButton.frame = refreshButtonRect;
		
	minMaxToolbarBtn.hidden = YES;
	
	if([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait || [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y + 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x - 500;
		urlTextField.frame = urlTextFieldRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x - 211;
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y + 20;
		searchTextFieldRect.size.width = searchTextFieldRect.size.width + 212;
		searchTextField.frame = searchTextFieldRect;
	}
	else
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.x = cancelInputButtonRect.origin.x + 160;
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y + 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x - 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y + 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width - 110;
		urlTextField.frame = urlTextFieldRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x - 150;
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y + 20;
		searchTextFieldRect.size.width = searchTextFieldRect.size.width + 150;
		searchTextField.frame = searchTextFieldRect;		
	}
		
	CGRect browserRect = [browserParentView frame];
	browserRect.size.height = browserRect.size.height - 20;
	browserRect.origin.y = browserRect.origin.y + 20;
	browserParentView.frame = browserRect;
	
	[UIView commitAnimations];
}

- (void) didEndEditingSearch {
	
	rotationLocked = NO;
	
	[ UIView beginAnimations: @"URLAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.15f ]; // Set the duration to 5/10ths of a second.
	
	CGRect urlViewSize = [urlView frame];
	urlViewSize.size.height = urlViewSize.size.height - 20;
	urlView.frame = urlViewSize;
	
	CGRect miniTabSize = [miniTabParentView frame];
	miniTabSize.origin.y = miniTabSize.origin.y - 20;
	miniTabParentView.frame = miniTabSize;
	
	CGRect urlViewImageRect = [urlViewImage frame];
	urlViewImageRect.size.height = urlViewImageRect.size.height - 20;
	urlViewImage.frame = urlViewImageRect;
			
	CGRect searchLabelRect = [searchLabel frame];
	searchLabelRect.origin.y = searchLabelRect.origin.y - 30;
	searchLabel.frame = searchLabelRect;
	
	CGRect pageTitleLabelRect = [pageTitleLabel frame];
	pageTitleLabelRect.origin.y = pageTitleLabelRect.origin.y + 30;
	pageTitleLabel.frame = pageTitleLabelRect;
	
	CGRect homePageButtonRect = [homePageButton frame];
	homePageButtonRect.origin.x = homePageButtonRect.origin.x + 180;
	homePageButton.frame = homePageButtonRect;
		
	CGRect refreshButtonRect = [refreshButton frame];
	refreshButtonRect.origin.x = refreshButtonRect.origin.x + 500;
	refreshButton.frame = refreshButtonRect;
	refreshButton.hidden = NO;
		
	minMaxToolbarBtn.hidden = NO;
	
	if([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait || [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y - 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x + 500;
		urlTextField.frame = urlTextFieldRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x + 211;
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y - 20;
		searchTextFieldRect.size.width = searchTextFieldRect.size.width - 212;
		searchTextField.frame = searchTextFieldRect;
	}
	else
	{
		CGRect cancelInputButtonRect = [cancelInputButton frame];
		cancelInputButtonRect.origin.x = cancelInputButtonRect.origin.x - 160;
		cancelInputButtonRect.origin.y = cancelInputButtonRect.origin.y - 65;
		cancelInputButton.frame = cancelInputButtonRect;
		
		CGRect urlTextFieldRect = [urlTextField frame];
		urlTextFieldRect.origin.x = urlTextFieldRect.origin.x + 25;
		urlTextFieldRect.origin.y = urlTextFieldRect.origin.y - 20;
		urlTextFieldRect.size.width = urlTextFieldRect.size.width + 110;
		urlTextField.frame = urlTextFieldRect;
		
		CGRect searchTextFieldRect = [searchTextField frame];
		searchTextFieldRect.origin.x = searchTextFieldRect.origin.x + 150;
		searchTextFieldRect.origin.y = searchTextFieldRect.origin.y - 20;
		searchTextFieldRect.size.width = searchTextFieldRect.size.width - 150;
		searchTextField.frame = searchTextFieldRect;		
	}
	
	CGRect browserRect = [browserParentView frame];
	browserRect.size.height = browserRect.size.height + 20;
	browserRect.origin.y = browserRect.origin.y - 20;
	browserParentView.frame = browserRect;
	
	[UIView commitAnimations];
}

- (void)textFieldDidBeginEditing:(UITextField *)textField {
	
	if(blackBackgroundImageView == nil)
	{
		UIImage* image = [UIImage imageNamed:@"BlackBackground.png"];
		blackBackgroundImageView = [[UIImageView alloc] initWithImage:image];
		blackBackgroundImageView.frame = CGRectMake(-10, -2, 500, 500);
		blackBackgroundImageView.alpha = 0.75;
	}
		
	[browserParentView addSubview:blackBackgroundImageView];
	
		
	refreshButton.hidden = YES;
	cancelInputButton.hidden = NO;
	pageTitleLabel.hidden = YES;
	
	if(textField == urlTextField)
	{
		[self didBeginEditingURL];
	}
	else if(textField == searchTextField)
	{
		[self didBeginEditingSearch];
	}
}

- (NSDictionary*) copyDictionary: (NSDictionary*) original {
	NSMutableDictionary* newDict = [[NSMutableDictionary alloc] init];
	
	id imageURL = [original objectForKey:@"WebElementImageURL"];
	id linkURL = [original objectForKey:@"WebElementLinkURL"];

	if(imageURL != nil)
	{
		[newDict setObject:[NSString stringWithString:(NSString*) [((NSURL*)imageURL) absoluteString]] forKey:@"WebElementImageURL"];
	}
	
	if(linkURL != nil)
	{
		[newDict setObject:[NSString stringWithString:(NSString*) [((NSURL*)linkURL) absoluteString]] forKey:@"WebElementLinkURL"];
	}
	
	return newDict;
}

- (void) checkTapAndHoldLocation: (CGPoint) location {
	
	int xScrollPosition = [[webBrowser stringByEvaluatingJavaScriptFromString:@"scrollX"] intValue];	
	int yScrollPosition = [[webBrowser stringByEvaluatingJavaScriptFromString:@"scrollY"] intValue];
	
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	location = CGPointMake(location.x + xScrollPosition, location.y + yScrollPosition);

	if(tapAndHoldElement != nil)
	{
		[tapAndHoldElement release];
	}
	tapAndHoldElement = nil;
	
	tapAndHoldElement = [self copyDictionary: [coreWebView elementAtPoint:location]];

	if(tapAndHoldElement != nil && [tapAndHoldElement count] > 0)
	{
		id imageURL = [tapAndHoldElement objectForKey:@"WebElementImageURL"];
		id linkURL = [tapAndHoldElement objectForKey:@"WebElementLinkURL"];
		
		if(imageURL != nil && linkURL == nil)
		{		
			if(tapAndHoldImageActionSheet == nil)
			{
				tapAndHoldImageActionSheet = [[UIActionSheet alloc] initWithTitle:@""
																		 delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
																otherButtonTitles:@"Save Image", nil];
				
				tapAndHoldImageActionSheet.actionSheetStyle = UIActionSheetStyleDefault;
			}
			
			[tapAndHoldImageActionSheet showInView:self.view]; // show from our table view (pops up in the middle of the table)
		}
		else if(imageURL != nil && linkURL != nil)
		{
			[self removeWebBrowserDelegate];
			self.ignoreLinkTap = YES;
			
			if(tapAndHoldLinkAndImageActionSheet == nil)
			{
				tapAndHoldLinkAndImageActionSheet = [[UIActionSheet alloc] initWithTitle:@""
																				delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
																	   otherButtonTitles:@"Open", @"Open in new tab", @"Save Image", nil];
				
				tapAndHoldLinkAndImageActionSheet.actionSheetStyle = UIActionSheetStyleDefault;
			}
			
			[tapAndHoldLinkAndImageActionSheet showInView:self.view]; // show from our table view (pops up in the middle of the table)
		}
		else if(imageURL == nil && linkURL != nil)
		{
			[self removeWebBrowserDelegate];
			self.ignoreLinkTap = YES;
			
			if(tapAndHoldLinkActionSheet == nil)
			{
				tapAndHoldLinkActionSheet = [[UIActionSheet alloc] initWithTitle:@""
																		delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
															   otherButtonTitles:@"Open", @"Open in new tab", nil];
				
				tapAndHoldLinkActionSheet.actionSheetStyle = UIActionSheetStyleDefault;
			}
			
			[tapAndHoldLinkActionSheet showInView:self.view]; // show from our table view (pops up in the middle of the table)
		}
	}
}

- (void)textFieldDidEndEditing:(UITextField *)textField {
	
	[blackBackgroundImageView removeFromSuperview];
	
	refreshButton.hidden = NO;
	pageTitleLabel.hidden = NO;
	cancelInputButton.hidden = YES;
	
	if(textField == urlTextField)
	{
		[self didEndEditingURL];
	}
	else if(textField == searchTextField)
	{
		[self didEndEditingSearch];
	}
}

- (void) updateBrowserButtons {
	if(webBrowser.canGoBack)
	{
		backButton.enabled = YES;
		fullScreenBackButton.enabled = YES;
	}
	else
	{
		backButton.enabled = NO;
		fullScreenBackButton.enabled = NO;
	}
	
	if(webBrowser.canGoForward)
	{
		forwardButton.enabled = YES;
		fullScreenForwButton.enabled = YES;
	}
	else
	{
		forwardButton.enabled = NO;
		fullScreenForwButton.enabled = NO;
	}
}

- (void) timerFiredEvent:(NSTimer *)timer {

	[timer invalidate];
	
	[self hideFullScreenButtons];
}

- (void) setTheme: (id)anObject {
	NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
	
	if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBlack] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBlue] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Blue.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;		
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeBrown] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Brown.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeGreen] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Green.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeGray] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Gray.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemePink] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Pink.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeRed] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Red.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	else if([[userDefaults stringForKey:kIBrowserTheme] compare:kIBrowserThemeOrange] == NSOrderedSame)
	{
		UIImage* themeImage = [UIImage imageNamed:@"MainBackground_Orange.gif"];
		self.urlViewImage.image = themeImage; 
		self.toolbarViewImage.image = themeImage;
		self.progressImageView.image = themeImage;
		self.miniTabParentImageView.image = themeImage;
	}
	
	[pool release];
}

- (void) scrollTick:(NSTimer *)timer {
	int xScrollPosition = [[webBrowser stringByEvaluatingJavaScriptFromString:@"scrollX"] intValue];
	
	int yScrollPosition = [[webBrowser stringByEvaluatingJavaScriptFromString:@"scrollY"] intValue];

	if(currentOrientation == UIInterfaceOrientationPortrait)
	{
		yScrollPosition += yVelocity;
	}
	else if(currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		yScrollPosition -= yVelocity;
	}
	else if(currentOrientation == UIInterfaceOrientationLandscapeLeft)
	{
		yScrollPosition -= xVelocity;
	}
	else if(currentOrientation == UIInterfaceOrientationLandscapeRight)
	{
		yScrollPosition += xVelocity;
	}
	
	[webBrowser stringByEvaluatingJavaScriptFromString:[NSString stringWithFormat:@"window.scrollTo(%d, %d);", xScrollPosition, yScrollPosition]];
}

- (void) scrollToTop {
	[webBrowser stringByEvaluatingJavaScriptFromString:[NSString stringWithFormat:@"window.scrollTo(%d, %d);", 0, 0]];
}

- (UIWebView*) getCurrentBrowserView {
	return self.webBrowser;
}

- (IBAction) autoScrollUp: (id) sender {
	UIImage* btnImage = [UIImage imageNamed:@"UpArrowHighlighted.png"];
	[autoScrollUpButton setImage:btnImage forState:UIControlStateNormal];
	
	btnImage = [UIImage imageNamed:@"DownArrow.png"];
	[autoScrollDownButton setImage:btnImage forState:UIControlStateNormal];
	
	if(scrollTimer == nil || !scrollTimer.isValid)
	{
		if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			xVelocity = 1;
			yVelocity = 1;
		}
		else
		{			
			yVelocity = -1;
			xVelocity = -1;
		}
		
		scrollTimer = nil;
		scrollTimer = [NSTimer scheduledTimerWithTimeInterval:0.03f target:self selector:@selector(scrollTick:) userInfo:nil repeats:YES];
	}
	else if(scrollTimer.isValid && yVelocity > -5)
	{
		if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			if(xVelocity < 0 || yVelocity < 0)
			{
				xVelocity = 0;
				yVelocity = 0;
			}
			
			yVelocity += 1;
			xVelocity += 1;
		}
		else
		{
			if(xVelocity > 0 || yVelocity > 0)
			{
				xVelocity = 0;
				yVelocity = 0;
			}
			
			yVelocity -= 1;
			xVelocity -= 1;
		}
	}
}

- (IBAction) autoScrollDown: (id) sender {
	UIImage* btnImage = [UIImage imageNamed:@"DownArrowHighlighted.png"];
	[autoScrollDownButton setImage:btnImage forState:UIControlStateNormal];
	
	btnImage = [UIImage imageNamed:@"UpArrow.png"];
	[autoScrollUpButton setImage:btnImage forState:UIControlStateNormal];
	
	if(scrollTimer == nil || !scrollTimer.isValid)
	{
		if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			xVelocity = -1;
			yVelocity = -1;
		}
		else
		{
			yVelocity = 1;
			xVelocity = 1;		
		}
		
		scrollTimer = nil;
		scrollTimer = [NSTimer scheduledTimerWithTimeInterval:0.03f target:self selector:@selector(scrollTick:) userInfo:nil repeats:YES];
	}
	else if(scrollTimer.isValid && yVelocity < 5)
	{
		if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationPortraitUpsideDown)
		{
			if(xVelocity > 0 || yVelocity > 0)
			{
				xVelocity = 0;
				yVelocity = 0;
			}
			
			yVelocity -= 1;
			xVelocity -= 1;
		}
		else
		{
			if(xVelocity < 0 || yVelocity < 0)
			{
				xVelocity = 0;
				yVelocity = 0;
			}
			
			yVelocity += 1;
			xVelocity += 1;
		}
	}
}

- (IBAction) autoScrollCancel: (id) sender {
	yVelocity = 0;
	xVelocity = 0;
	
	UIImage* btnImage = [UIImage imageNamed:@"DownArrow.png"];
	[autoScrollDownButton setImage:btnImage forState:UIControlStateNormal];

	btnImage = [UIImage imageNamed:@"UpArrow.png"];
	[autoScrollUpButton setImage:btnImage forState:UIControlStateNormal];
	
	if(scrollTimer != nil && scrollTimer.isValid)
	{
		[scrollTimer invalidate];
		scrollTimer = nil;
	}
}

- (void)viewWillAppear:(BOOL)animated  {
	[super viewWillAppear:animated];
	
	[self setAccelerometerDelegate];
	
	if([[userDefaults stringForKey:kIBrowserShowProgressBar] compare:kIBrowserTrue] == NSOrderedSame)
	{
		showProgressBar = YES;
	}
	else
	{
		showProgressBar = NO;
	}
	
	if([[userDefaults stringForKey:kIBrowserAutoScroll] compare:kIBrowserTrue] == NSOrderedSame)
	{
		[UIApplication sharedApplication].idleTimerDisabled = YES;
		scrollTimer = [NSTimer scheduledTimerWithTimeInterval:0.03f target:self selector:@selector(scrollTick:) userInfo:nil repeats:YES];
	}
	else if([[userDefaults stringForKey:kIBrowserAutoScroll] compare:kIBrowserFalse] == NSOrderedSame)
	{
		[UIApplication sharedApplication].idleTimerDisabled = NO;
	}
	
	if([[userDefaults stringForKey:kIBrowserManualAutoScroll] compare:kIBrowserTrue] == NSOrderedSame)
	{
		autoScrollView.hidden = NO;
		
		if(currentOrientation == UIInterfaceOrientationLandscapeLeft || currentOrientation == UIInterfaceOrientationLandscapeRight)
		{
			autoScrollView.frame = CGRectMake(440, 160, 30, 110);
		}
		else
		{
			autoScrollView.frame = CGRectMake(280, 315, 30, 110);
		}
	}
	else if([[userDefaults stringForKey:kIBrowserManualAutoScroll] compare:kIBrowserFalse] == NSOrderedSame)
	{
		autoScrollView.hidden = YES;
		
		autoScrollView.frame = CGRectMake(-200, 700, 30, 110);
	}
	
	if(popupNeedToShowStatusBar)
	{
		[self showStatusBar];
		
		popupNeedToShowStatusBar = NO;
	}
	
	[self loadHistoryBookmarks];
	
	fullScreenBackForwBtnView.alpha = [userDefaults floatForKey:kIbrowserTransparency];
	
	if([[userDefaults stringForKey:kIbrowserShakeToShow] compare:kIBrowserTrue] == NSOrderedSame)
	{		
		activeTimer = [[NSTimer scheduledTimerWithTimeInterval:5.0 target:self selector:@selector(timerFiredEvent:) userInfo:nil repeats:YES] retain];
	}
	
	searchTextField.placeholder = [[NSUserDefaults standardUserDefaults] stringForKey:kIBrowserSearchEngine];
}

- (void)viewWillDisappear:(BOOL)animated
{	
	[UIApplication sharedApplication].idleTimerDisabled = NO;
	
	if(scrollTimer != nil && scrollTimer.isValid)
	{
		[scrollTimer invalidate];
		scrollTimer = nil;
	}
	
	if(activeTimer != nil && activeTimer.isValid)
	{
		[activeTimer invalidate];
	}
	
	[self removeAccelerometerDelegate];
}

- (void) loadURL : (NSString*) urlToLoad {
	
	if([urlToLoad compare:kIBrowserBlank] != NSOrderedSame)
	{
		if(urlToLoad.length > 0 && [urlToLoad rangeOfString:@"."].location == NSNotFound)
		{
			urlToLoad = [urlToLoad stringByAppendingString:@".com"];
		}
		
		if ([urlToLoad rangeOfString:@"javascript:"].location != NSNotFound)
		{
			
			urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@"'" withString:@"\\'"];
			urlToLoad = [[@"var t = document.createElement('a'); t.setAttribute('href', '" stringByAppendingString:urlToLoad] stringByAppendingString:
						 @"'); var e = document.createEvent('MouseEvent'); e.initMouseEvent('click'); t.dispatchEvent(e);"];
			
			[webBrowser stringByEvaluatingJavaScriptFromString:urlToLoad];
			
			return;
		}
		
		if(urlToLoad.length > 0 && [urlToLoad rangeOfString:@"http://"].location == NSNotFound && [urlToLoad rangeOfString:@"https://"].location == NSNotFound)
		{
			urlToLoad = [@"http://" stringByAppendingString:urlToLoad];
		}
		
		if(urlToLoad != nil && [urlToLoad compare: @""] != NSNotFound)
		{			
			if([[userDefaults stringForKey:kIBrowserCompressPages] compare:kIBrowserTrue] == NSOrderedSame)
			{
				urlToLoad = [urlToLoad stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
				urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@"&" withString:@"%26"];
				urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@"/" withString:@"%2F"];
				urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@":" withString:@"%3A"];				
				urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@"?" withString:@"%3F"];				
				urlToLoad = [urlToLoad stringByReplacingOccurrencesOfString:@"=" withString:@"%3D"];
				
				urlToLoad = [kIBrowserCompressionLink stringByAppendingString:urlToLoad];					
			}
			
			NSURL* url = [NSURL URLWithString:urlToLoad];
			
			NSURLRequest* urlRequest = [NSURLRequest requestWithURL:url cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 180];
			
			[NSURLRequest setAllowsAnyHTTPSCertificate: YES forHost:urlRequest.URL.host];
			
			[webBrowser loadRequest:urlRequest];
			
			[self formatAndSetURLTextFieldText:url.absoluteString];
			
			pageTitleLabel.text = @"";
		}
	}
	else
	{
		urlTextField.text = @"";
		pageTitleLabel.text = @"";
	}
}

- (NSString*) formatURLText: (NSString*) urlText {
	urlText = [urlText stringByReplacingOccurrencesOfString:kIBrowserCompressionLink withString:@""];
	urlText = [urlText stringByReplacingOccurrencesOfString:kIBrowserCompressionNoImg withString:@""];
	
	urlText = [urlText stringByReplacingOccurrencesOfString:@"%26" withString:@"&"];
	urlText = [urlText stringByReplacingOccurrencesOfString:@"%2F" withString:@"/"];
	urlText = [urlText stringByReplacingOccurrencesOfString:@"%3A" withString:@":"];	
	urlText = [urlText stringByReplacingOccurrencesOfString:@"%3F" withString:@"?"];
	urlText = [urlText stringByReplacingOccurrencesOfString:@"%3D" withString:@"="];
	
	urlText = [urlText stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	return urlText;
}

- (void) formatAndSetURLTextFieldText: (NSString*)urlToSet {
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:kIBrowserCompressionLink withString:@""];
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:kIBrowserCompressionNoImg withString:@""];
	
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:@"%26" withString:@"&"];
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:@"%2F" withString:@"/"];
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:@"%3A" withString:@":"];	
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:@"%3F" withString:@"?"];
	urlToSet = [urlToSet stringByReplacingOccurrencesOfString:@"%3D" withString:@"="];
	
	urlToSet = [urlToSet stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	urlTextField.text = urlToSet;
}

- (void) loadHomePageURL {
	NSString* homePageURL = [[NSUserDefaults standardUserDefaults] stringForKey:kIBrowserHomePage];
	
	if(homePageURL.length > 0)
	{	
		homePageURL = [homePageURL stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
		
		[self loadURL:homePageURL];
	}
}

- (void) switchViewVisibility : (UIWebView*) viewToHide : (UIWebView*) viewToShow {	
	
	[self stopLoadingBrowser];
	
	[ UIView beginAnimations: @"WebViewAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseInOut ];
	[ UIView setAnimationDuration: 0.75f ]; // Set the duration to 5/10ths of a second.

	viewToHide.frame = CGRectMake(0 - viewToHide.frame.size.width - 350, viewToHide.frame.origin.y, viewToHide.frame.size.width, viewToHide.frame.size.height);
	viewToShow.frame = 	CGRectMake(0, viewToHide.frame.origin.y, viewToHide.frame.size.width, viewToHide.frame.size.height);
	
	//viewToHide.delegate = nil;
	
	self.webBrowser = viewToShow;
	self.webBrowser.delegate = self;
	
	[self updateBrowserButtons];
	
	[self initWebBrowserDelegate];
	
	[UIView commitAnimations];
}

- (void) switchViewVisibilityAndLoadURL : (UIWebView*) viewToHide : (UIWebView*) viewToShow : (NSString*) newURL {
	[self switchViewVisibility: viewToHide : viewToShow];
	
	[self loadURL:newURL];
}


- (IBAction) tabEditAction: (id) sender {

	if(tabsSelectionTable.editing)
	{
		[tabsSelectionTable setEditing:NO animated:YES];
		//tabsSelectionTable.editing = NO;
		
		UIImage* editTabImg = [UIImage imageNamed:@"TabEdit.png"];
		UIImage* editTabHighImg = [UIImage imageNamed:@"TabEditHighlighted.png"];
		
		[tabEditButton setImage:editTabImg forState:UIControlStateNormal];
		[tabEditButton setImage:editTabHighImg forState:UIControlStateHighlighted];
		[tabEditButton setImage:editTabHighImg forState:UIControlStateSelected];
		
		[self refreshTabTable:0];
		
		if(showMiniTabsWhenDoneEdit)
		{
			[self hideCustomTabsSelectionView:0];
			
			[userDefaults setObject:kIBrowserTrue forKey:kIBrowserShowingMiniTabs];
			
			[self showMiniTabsSelectionView:0];
			
			showMiniTabsWhenDoneEdit = NO;
		}
	}
	else
	{
		[tabsSelectionTable setEditing:YES animated:YES];
		//tabsSelectionTable.editing = YES;
		
		UIImage* editTabImg = [UIImage imageNamed:@"TabEditDone.png"];
		UIImage* editTabHighImg = [UIImage imageNamed:@"TabEditDoneHighlighted.png"];
		
		[tabEditButton setImage:editTabImg forState:UIControlStateNormal];
		[tabEditButton setImage:editTabHighImg forState:UIControlStateHighlighted];
		[tabEditButton setImage:editTabHighImg forState:UIControlStateSelected];
	}
}

- (void) loadTab: (int) index : (BOOL) loadInCurrentWebView {

	NSString* strIndex = [NSString stringWithFormat:@"%d", index];
	
	if(!loadInCurrentWebView && [strIndex compare:[userDefaults stringForKey:kIBrowserCurrentTabIndex]] == NSOrderedSame)
	{
		return;
	}
	
	NSArray* tabInfo = [[userDefaults objectForKey:kIBrowserTabsInfo] objectForKey:strIndex];
	
	if([self.tabs objectForKey:strIndex] != nil)
	{
		[self switchViewVisibility:self.webBrowser :[self.tabs objectForKey:strIndex]];
		
		NSString* tabURL = [tabInfo objectAtIndex:kIBrowserTabURLIndex];
		
		if([tabURL compare:kIBrowserBlank] == NSOrderedSame)
		{
			tabURL = @"";
		}
		
		[self formatAndSetURLTextFieldText: tabURL];
		
		NSString* pageHeader = [tabInfo objectAtIndex:kIBrowserTabNameIndex];
		
		self.pageTitleLabel.text = pageHeader;
		
		if(pageHeader != nil && [pageHeader compare:@""] != NSOrderedSame)
		{
			self.pageTitleLabel.enabled = YES;
		}
		else
		{
			self.pageTitleLabel.enabled = NO;
		}
	}
	else
	{	
		if(tabInfo != nil)
		{
			NSString* tabURL = [tabInfo objectAtIndex:kIBrowserTabURLIndex];
			
			tabURL = [tabURL stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];			
		
			if(loadInCurrentWebView)
			{
				[self.tabs setObject: self.webBrowser forKey: strIndex];	
				[self loadURL:tabURL];
			}
			else
			{
				UIWebView* newWebView = [[UIWebView alloc] initWithFrame:CGRectMake(self.webBrowser.frame.origin.x - self.webBrowser.frame.size.width - 350, self.webBrowser.frame.origin.y, self.webBrowser.frame.size.width, self.webBrowser.frame.size.height)];
				newWebView.scalesPageToFit = YES;
				newWebView.multipleTouchEnabled = YES;				
				newWebView.backgroundColor = [UIColor whiteColor];
				newWebView.autoresizingMask = (UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight);				
				
				[self.browserParentView addSubview:newWebView];
				
				[self.tabs setObject: newWebView forKey: strIndex];	
				
				[self switchViewVisibilityAndLoadURL: self.webBrowser : newWebView: tabURL];
				
				[newWebView release];
			}
		}
	}
	
	[userDefaults setObject:strIndex forKey:kIBrowserCurrentTabIndex];
}

- (IBAction) addTabAction: (id) sender {
	NSArray* newTabInfo = [NSArray arrayWithObjects:kIBrowserBlank, kIBrowserBlank,  nil];
	
	NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
	
	NSMutableDictionary* newTabsInfoDict = [[NSMutableDictionary alloc] init];
	[newTabsInfoDict addEntriesFromDictionary:tabsInfoDict];
	
	NSString* strIndex = [NSString stringWithFormat:@"%d", [[tabsInfoDict allKeys] count]];
	
	[newTabsInfoDict setObject:newTabInfo forKey:strIndex];
	
	[userDefaults setObject:newTabsInfoDict forKey:kIBrowserTabsInfo];
	
	[newTabsInfoDict release];
	
	[tabsSelectionTable reloadData];
}

- (void) setAccelerometerDelegate {
	UIAccelerometer *accel = [UIAccelerometer sharedAccelerometer];
	accel.delegate = self;
	accel.updateInterval = kUpdateInterval;		
}

- (void) removeAccelerometerDelegate {
	UIAccelerometer *accel = [UIAccelerometer sharedAccelerometer];
	accel.delegate = nil;
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	
	currentOrientation = UIInterfaceOrientationPortrait;
	
	[[UIApplication sharedApplication] setStatusBarHidden:NO animated: NO];

    [super viewDidLoad];
	
	if(self.userDefaults == nil)
	{
		self.userDefaults = [NSUserDefaults standardUserDefaults];
	}
	
	[NSThread detachNewThreadSelector:@selector(setTheme:) toTarget:self withObject:nil];
	
	self.tabs = [[NSMutableDictionary alloc] init];
	self.tabButtons = [[NSMutableDictionary alloc] init];
	self.tabDeleteButtons = [[NSMutableDictionary alloc] init];
	self.favicons = [[NSMutableDictionary alloc] init];
	
	int diagnosticVal = [[userDefaults stringForKey:kIbrowserDiagnosticVal] intValue];
	
	if([[userDefaults stringForKey:kIBrowserLaunchHomePage] compare:kIBrowserTrue] == NSOrderedSame || diagnosticVal > 1)
	{
		[self setHomePageForLaunch];
	}
	
	self.urlTextField.delegate = self;
	self.searchTextField.delegate = self;
	self.webBrowser.delegate = self;
	self.webBrowser.backgroundColor = [UIColor whiteColor];
	self.webBrowser.scalesPageToFit = YES;
	self.webBrowser.multipleTouchEnabled = YES;
	
	self.urlTextField.clearsOnBeginEditing = NO;
	self.urlTextField.clearButtonMode = UITextFieldViewModeWhileEditing;
	
	self.searchTextField.clearsOnBeginEditing = NO;
	self.searchTextField.clearButtonMode = UITextFieldViewModeWhileEditing;
	
	[urlView addSubview:cancelInputButton];
	
	[self updateBrowserButtons];
	
	self.urlTextField.font = [UIFont systemFontOfSize:15.5];
	self.searchTextField.font = [UIFont systemFontOfSize:15.5];
	self.findTextField.font = [UIFont systemFontOfSize:15.5];
	self.pageTitleLabel.font = [UIFont boldSystemFontOfSize:12];
	
	self.tabsSelectionTable.delegate = self;
	self.tabsSelectionTable.dataSource = self;
	
	[self loadTab:[userDefaults integerForKey:kIBrowserCurrentTabIndex] : YES];
	
	yaccel = 5.0f;
	xaccel = 5.0f;
	yVelocity = 0.0f;
	xVelocity = 0.0f;
	xx = 0.0f;
	yy = 0.0f;
	
	desktopTabImg = [UIImage imageNamed:@"DesktopTab.png"];
	desktopTabHighlightedImg = [UIImage imageNamed:@"DesktopTabHighlighted.png"];
	
	tabDeleteImg = [UIImage imageNamed:@"StopIconSmall.png"];
	tabDeleteHighlightedImg = [UIImage imageNamed:@"StopIconHighlightedImg.png"];
	
	[self initWebBrowserDelegate];
	
	[userDefaults setObject:@"0" forKey:kIbrowserDiagnosticVal];
	[userDefaults synchronize];
}

#define kFilteringFactor 0.05

- (float) adjustAcceleration: (float)value {
	float newValue = value;
	
	if(newValue < 0.0f)
	{
		newValue = -newValue;
	}
	
	if(newValue > 0.0f)
	{
		if(newValue >= 0.4f && newValue <= 0.7f)
		{
			newValue = 0.0f;
		}
		else if(newValue <= 0.06f && newValue >= 0.0f)
		{
			newValue = 0.0f;
		}
		else if(newValue < 0.4f)
		{
			newValue = 0.5f - newValue;
			newValue = -newValue;
		}
		else
		{
			newValue = newValue - 0.5f;
		}
	}
	
	return newValue*1.7f;
}

#define SIGN(x)	((x < 0.0f) ? -1.0f : 1.0f)

- (void)accelerometer:(UIAccelerometer *)accelerometer didAccelerate:(UIAcceleration *)acceleration {
	
	if ([UIApplication sharedApplication].statusBarHidden && [[userDefaults stringForKey:kIbrowserShakeToShow] compare:kIBrowserTrue] == NSOrderedSame ) {
		if (fabsf(acceleration.x) > kAccelerationThreshold || fabsf(acceleration.y) > kAccelerationThreshold || fabsf(acceleration.z) > kAccelerationThreshold) {

			fullScreenBackForwBtnView.alpha = 0.74f;
			[self showFullScreenButtons];
			
			if(activeTimer.isValid)
			{
				[activeTimer invalidate];
			}
			
			activeTimer = [[NSTimer scheduledTimerWithTimeInterval:5.0 target:self selector:@selector(timerFiredEvent:) userInfo:nil repeats:YES] retain];
		}
	}
	
	if([[userDefaults stringForKey:kIBrowserAutoScroll] compare:kIBrowserTrue] == NSOrderedSame)
	{
		xx = acceleration.x;
		yy = acceleration.y;
		
		xx = SIGN(xx) * [self adjustAcceleration:xx];
		yy = SIGN(yy) * [self adjustAcceleration:yy];
		
		float accelDirX = SIGN(xVelocity) * -1.0f;
		float newDirX = SIGN(xx);
		
		float accelDirY = SIGN(yVelocity) * -1.0f;
		float newDirY = SIGN(yy);
		
		if(accelDirX == newDirX)
		{
			xaccel = (abs(xaccel) + 0.85f) * SIGN(xaccel);
		}
		if(accelDirY == newDirY)
		{
			yaccel = (abs(yaccel) + 0.85f) * SIGN(yaccel);
		}
		
		xVelocity = -xaccel * xx;
		yVelocity = -yaccel * yy;
	}
}

/*
// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}
*/

- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
	
	if(ignoreLinkTap)
	{
		if(navigationType == UIWebViewNavigationTypeLinkClicked)
		{
			ignoreLinkTap = NO;
			[self initWebBrowserDelegate];
			
			return false;
		}
	}
	
	if([[[request URL] absoluteString] rangeOfString:@"itunes.apple.com/WebObjects/MZStore.woa"].location != NSNotFound)
	{
		NSURL* newURL = [NSURL URLWithString:[NSString stringWithFormat:@"http://phobos.apple.com%@?%@", [request URL].path, [request URL].query]];
		
		[[UIApplication sharedApplication] openURL:newURL];
		
		return NO;
	}
	
	NSArray* appLinks = [NSArray arrayWithObjects:@"tel", @"sms", @"mailto", @"http://maps.google.com", nil];
	
	if([appLinks containsObject:[[request URL] scheme]]) 
	{
		[[UIApplication sharedApplication] openURL:[request URL]];
		
		return NO;
	}
	
	[NSURLRequest setAllowsAnyHTTPSCertificate: YES forHost:request.URL.host];
	
	return YES;
}

- (void) loadPageTitle {
	
	/**NSString* pageHeader = [webBrowser stringByEvaluatingJavaScriptFromString:@"document.getElementsByTagName('title')[0].innerHTML;"];
	
	 **/
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	NSString* pageHeader = coreWebView.mainFrameTitle;
	
	
	if(pageHeader == nil)
	{
		pageHeader = @"";
	}
	else
	{
		pageHeader = [pageHeader stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
	}
	
	pageTitleLabel.text = pageHeader;
	
	if([pageHeader compare:@""] == NSOrderedSame)
	{
		pageTitleLabel.enabled = NO;
	}
	else
	{
		pageTitleLabel.enabled = YES;
	}
}

- (void) checkURLForRedirectAndCertificates: (UIWebView*) webView {
	NSURLRequest* request = [webView request];
	
	if([[[request URL] absoluteString] rangeOfString:@"itunes.apple.com/WebObjects/MZStore.woa"].location != NSNotFound)
	{
		NSURL* newURL = [NSURL URLWithString:[NSString stringWithFormat:@"http://phobos.apple.com%@?%@", [request URL].path, [request URL].query]];
		
		[[UIApplication sharedApplication] openURL:newURL];
	}
	
	NSArray* appLinks = [NSArray arrayWithObjects:@"tel", @"sms", @"mailto", @"http://maps.google.com", nil];
	
	if([appLinks containsObject:[[request URL] scheme]]) 
	{
		[[UIApplication sharedApplication] openURL:[request URL]];
	}
	
	[NSURLRequest setAllowsAnyHTTPSCertificate: YES forHost:request.URL.host];	
}

- (void)webViewDidStartLoad: (UIWebView *)webView
{
	[self checkURLForRedirectAndCertificates:webView];
	
	if(webView == self.webBrowser)
	{		
		[UIApplication sharedApplication].networkActivityIndicatorVisible = YES;
		
		if(progressTimer == nil)
		{		
			[self showProgressIndicator];
			
			progressTimer =	[NSTimer scheduledTimerWithTimeInterval:0.25 target:self selector:@selector(progressEstimateChanged:) userInfo:nil repeats:YES];
				
			[self loadPageTitle];
		
			// starting the load, show the activity indicator in the status bar
			[self updateBrowserButtons];
		}
	}
}

- (void) refreshTabTable: (id) anObject {
	[tabsSelectionTable reloadData];
}

- (void)webView:(NSObject*)sender decidePolicyForNewWindowAction:(NSDictionary*)info request:(NSURLRequest*)request newFrameName:(NSObject*) frame decisionListener:(id)listener { 
	objc_msgSend(listener, @selector(ignore)); 
	
	if([[[request URL] absoluteString] rangeOfString:@"itunes.apple.com/WebObjects/MZStore.woa"].location != NSNotFound)
	{
		NSURL* newURL = [NSURL URLWithString:[NSString stringWithFormat:@"http://phobos.apple.com%@?%@", [request URL].path, [request URL].query]];
		
		[[UIApplication sharedApplication] openURL:newURL];
		
		return;
	}
	
	NSArray* appLinks = [NSArray arrayWithObjects:@"tel", @"sms", @"mailto", @"http://maps.google.com", nil];
	
	if([appLinks containsObject:[[request URL] scheme]]) 
	{
		[[UIApplication sharedApplication] openURL:[request URL]];
		
		return;
	}
	
	[NSURLRequest setAllowsAnyHTTPSCertificate: YES forHost:request.URL.host];
	
	[webBrowser loadRequest:request]; 
}

- (void)webViewDidFinishLoad:(UIWebView *)webView {
	
	[webView stringByEvaluatingJavaScriptFromString:@"window.open = function(url) { var t = document.createElement('a'); t.setAttribute('href', url); var e = document.createEvent('MouseEvent'); e.initMouseEvent('click'); t.dispatchEvent(e); };"];
	
	if(webView.loading)
	{
		return;
	}
	
	[NSThread detachNewThreadSelector:@selector(loadFavicons:) toTarget:self withObject: webView];
	
	[self hideNetworkIndicatorIfApplicable];
	
	if(webView == self.webBrowser)
	{
		[self updateBrowserButtons];
		
		[self formatAndSetURLTextFieldText: [webBrowser.request URL].absoluteString];
		

//		[webBrowser stringByEvaluatingJavaScriptFromString:@"var tags = document.getElementsByTagName('a'); for(var i=0; i < tags.length; i++) { var tag = tags[i]; if(tag.getAttribute('target').toLowerCase() === '_blank' && tag.getAttribute('href') !== '' && tag.getAttribute('href') !== '#') { tag.setAttribute('target', '_top'); } }"];
		
//		[webBrowser stringByEvaluatingJavaScriptFromString:@"var tags = document.getElementsByTagName('form'); for(var i=0; i < tags.length; i++) { var tag = tags[i]; var submit = tag.submit; tag.submit = function() { var t = tag.target; var a = tag.action; tag.action = a; return submit.apply(this, arguments); }; }"];
		
		[webBrowser stringByEvaluatingJavaScriptFromString:@"document.documentElement.style.webkitTouchCallout = \"none\";"];
		
		[self loadPageTitle];
		
		NSString* currTabIndex = [userDefaults stringForKey:kIBrowserCurrentTabIndex];
		NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
		
		NSMutableDictionary* newTabsInfoDict = [[NSMutableDictionary alloc] init];
		[newTabsInfoDict addEntriesFromDictionary:tabsInfoDict];
		
		NSString* url = urlTextField.text;
		
		if(url == nil)
		{
			url = kIBrowserBlank;
		}
		
		NSString* pageHeader = pageTitleLabel.text;
		
		if([pageHeader compare:@""] == NSOrderedSame)
		{
			pageHeader = kIBrowserBlank;
		}
		
		NSArray* selectTabInfo = [NSArray arrayWithObjects:pageHeader, url, nil];
		
		[newTabsInfoDict setObject:selectTabInfo forKey:currTabIndex];
		
		[userDefaults setObject:newTabsInfoDict forKey:kIBrowserTabsInfo];
		
		[newTabsInfoDict release];
		
		[self performSelectorOnMainThread:@selector(refreshTabTable:) withObject:nil waitUntilDone:NO];
	}
	else
	{
		NSString* currTabIndex = [NSString stringWithFormat:@"%d", [[tabs allValues] indexOfObject:webView]];
		NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
		
		NSMutableDictionary* newTabsInfoDict = [[NSMutableDictionary alloc] init];
		[newTabsInfoDict addEntriesFromDictionary:tabsInfoDict];
		
		WebView *coreWebView = [[webView _documentView] webView];
		
		NSString* pageHeader = coreWebView.mainFrameTitle;
		NSString* url = coreWebView.mainFrameURL;		
		
		if(url == nil)
		{
			url = kIBrowserBlank;
		}
		
		if([pageHeader compare:@""] == NSOrderedSame)
		{
			pageHeader = kIBrowserBlank;
		}
		
		NSArray* selectTabInfo = [NSArray arrayWithObjects:pageHeader, url, nil];
		
		[newTabsInfoDict setObject:selectTabInfo forKey:currTabIndex];
		
		[userDefaults setObject:newTabsInfoDict forKey:kIBrowserTabsInfo];
		
		[newTabsInfoDict release];
		
		[self performSelectorOnMainThread:@selector(refreshTabTable:) withObject:nil waitUntilDone:NO];
	}
	
	[self addHistoryBookmark: webView];
}

- (IBAction) setManualRotationLock: (id) sender {
	
	if(rotationHardLocked)
	{
		lockRotationButton.highlighted = NO;
		rotationHardLocked = NO;
		
		UIImage* lockedImage = [UIImage imageNamed:@"LockIcon.png"];
		[lockRotationButton setImage:lockedImage forState:UIControlStateNormal];
	}
	else
	{
		lockRotationButton.highlighted = YES;
		rotationHardLocked = YES;
		
		UIImage* lockedImage = [UIImage imageNamed:@"LockIconHighlighted.png"];
		[lockRotationButton setImage:lockedImage forState:UIControlStateNormal];
	}
}

- (IBAction) goBack: (id) sender {
	[webBrowser goBack];
	
	UIButton* btn = (UIButton*) sender;
	
	if(fullScreenBackButton == btn && activeTimer.isValid)
	{
		[activeTimer invalidate];
		activeTimer = [[NSTimer scheduledTimerWithTimeInterval:5.0 target:self selector:@selector(timerFiredEvent:) userInfo:nil repeats:YES] retain];
	}
}

- (IBAction) goForward: (id) sender {
	[webBrowser goForward];
	
	UIButton* btn = (UIButton*) sender;
	
	if(fullScreenForwButton == btn && activeTimer.isValid)
	{
		[activeTimer invalidate];
		activeTimer = [[NSTimer scheduledTimerWithTimeInterval:5.0 target:self selector:@selector(timerFiredEvent:) userInfo:nil repeats:YES] retain];
	}
}

- (void) stopLoadingBrowser {
	if(webBrowser.loading)
	{
		[self hideProgressIndicator];
		
		//[webBrowser stopLoading];		
	}	
	
	[self updateBrowserButtons];
	
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	NSString* pageURL = coreWebView.mainFrameURL;
	
	[self formatAndSetURLTextFieldText:pageURL];
}

- (IBAction) refreshBrowser: (id) sender {
	
	if(webBrowser.loading)
	{
		[self stopBrowser:sender];
		[self updateBrowserButtons];
	}
	else
	{	
		[webBrowser reload];
		[self updateBrowserButtons];
	}
	
	[self updateBrowserButtons];
}

- (IBAction) stopBrowser: (id) sender {
	[webBrowser stopLoading];
	
	[webBrowser stringByEvaluatingJavaScriptFromString:@"document.documentElement.style.webkitTouchCallout = \"none\";"];
	
	WebView *coreWebView = [[webBrowser _documentView] webView];
	
	NSString* pageURL = coreWebView.mainFrameURL;
	
	[self formatAndSetURLTextFieldText:pageURL];
}

- (IBAction) loadHomePage: (id) sender {
	[self loadHomePageURL];
}

- (IBAction) pageActionChosen: (id) sender {
	
	if(pageActionSheet == nil)
	{
		pageActionSheet = [[UIActionSheet alloc] initWithTitle:@""
						    delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil
							otherButtonTitles:@"Add Bookmark", @"Set as Home Page", @"Mail Link to this Page", @"Open in Safari", nil];
		
		pageActionSheet.actionSheetStyle = UIActionSheetStyleDefault;
	}
	
	[pageActionSheet showInView:self.view]; // show from our table view (pops up in the middle of the table)
}

- (IBAction) showMoreBrowserButtons: (id) sender {
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		showingMoreButtons = YES;
		
		[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
		[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
		[ UIView setAnimationDuration: 0.3f ]; // Set the duration to 5/10ths of a second.	
						
		backButton.frame = CGRectMake(-500, backButton.frame.origin.y, backButton.frame.size.width, backButton.frame.size.height);
		forwardButton.frame = CGRectMake(-500, forwardButton.frame.origin.y, forwardButton.frame.size.width, forwardButton.frame.size.height);
		pageActionButton.frame = CGRectMake(-500, pageActionButton.frame.origin.y, pageActionButton.frame.size.width, pageActionButton.frame.size.height);
		favoritesButton.frame = CGRectMake(-500, favoritesButton.frame.origin.y, favoritesButton.frame.size.width, favoritesButton.frame.size.height);
		fullScreenButton.frame = CGRectMake(-500, fullScreenButton.frame.origin.y, fullScreenButton.frame.size.width, fullScreenButton.frame.size.height);
		tabsButton.frame = CGRectMake(-500, tabsButton.frame.origin.y, tabsButton.frame.size.width, tabsButton.frame.size.height);
		moreButtonsButton.frame = CGRectMake(-500, moreButtonsButton.frame.origin.y, moreButtonsButton.frame.size.width, moreButtonsButton.frame.size.height);
		
		[UIView commitAnimations];
		
		[ UIView beginAnimations: @"FullScreenAnimations3" context: nil ]; // Tell UIView we're ready to start animations.
		[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
		[ UIView setAnimationDuration: 0.3f ]; // Set the duration to 5/10ths of a second.	
		
		lessButtonsButton.frame = CGRectMake(10, lessButtonsButton.frame.origin.y, lessButtonsButton.frame.size.width, lessButtonsButton.frame.size.height);
		findButton.frame = CGRectMake(55, findButton.frame.origin.y, findButton.frame.size.width, findButton.frame.size.height);
		lockRotationButton.frame = CGRectMake(100, lockRotationButton.frame.origin.y, lockRotationButton.frame.size.width, lockRotationButton.frame.size.height);
		historyButton.frame = CGRectMake(145, historyButton.frame.origin.y, historyButton.frame.size.width, historyButton.frame.size.height);		
		settingsButton.frame = CGRectMake(190, settingsButton.frame.origin.y, settingsButton.frame.size.width, settingsButton.frame.size.height);
		
		[UIView commitAnimations];
	}
}

- (IBAction) showLessBrowserButtons: (id) sender {
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{			
		showingMoreButtons = NO;
		
		[ UIView beginAnimations: @"FullScreenAnimations3" context: nil ]; // Tell UIView we're ready to start animations.
		[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
		[ UIView setAnimationDuration: 0.3f ]; // Set the duration to 5/10ths of a second.	
		
		lessButtonsButton.frame = CGRectMake(500, lessButtonsButton.frame.origin.y, lessButtonsButton.frame.size.width, lessButtonsButton.frame.size.height);
		findButton.frame = CGRectMake(500, findButton.frame.origin.y, findButton.frame.size.width, findButton.frame.size.height);
		lockRotationButton.frame = CGRectMake(500, lockRotationButton.frame.origin.y, lockRotationButton.frame.size.width, lockRotationButton.frame.size.height);
		historyButton.frame = CGRectMake(500, historyButton.frame.origin.y, historyButton.frame.size.width, historyButton.frame.size.height);		
		settingsButton.frame = CGRectMake(500, settingsButton.frame.origin.y, settingsButton.frame.size.width, settingsButton.frame.size.height);
		
		[UIView commitAnimations];
		
		[ UIView beginAnimations: @"FullScreenAnimations2" context: nil ]; // Tell UIView we're ready to start animations.
		[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
		[ UIView setAnimationDuration: 0.3f ]; // Set the duration to 5/10ths of a second.	
		
		backButton.frame = CGRectMake(10, backButton.frame.origin.y, backButton.frame.size.width, backButton.frame.size.height);
		forwardButton.frame = CGRectMake(50, forwardButton.frame.origin.y, forwardButton.frame.size.width, forwardButton.frame.size.height);
		pageActionButton.frame = CGRectMake(95, pageActionButton.frame.origin.y, pageActionButton.frame.size.width, pageActionButton.frame.size.height);
		favoritesButton.frame = CGRectMake(140, favoritesButton.frame.origin.y, favoritesButton.frame.size.width, favoritesButton.frame.size.height);
		fullScreenButton.frame = CGRectMake(185, fullScreenButton.frame.origin.y, fullScreenButton.frame.size.width, fullScreenButton.frame.size.height);
		tabsButton.frame = CGRectMake(230, tabsButton.frame.origin.y, tabsButton.frame.size.width, tabsButton.frame.size.height);
		moreButtonsButton.frame = CGRectMake(280, moreButtonsButton.frame.origin.y, moreButtonsButton.frame.size.width, moreButtonsButton.frame.size.height);
		
		[UIView commitAnimations];
	}
}

- (IBAction) findButtonAction: (id) sender {
	findTextViewIsVisible = YES;
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseOut ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		findTextView.frame = CGRectMake(10, 110, findTextView.frame.size.width, findTextView.frame.size.height);
	}
	else
	{
		findTextView.frame = CGRectMake(95, 22, findTextView.frame.size.width, findTextView.frame.size.height);
	}
	
	[UIView commitAnimations];
	
	[findTextField becomeFirstResponder]; 
}

- (IBAction) hideFindTextView: (id) sender {
	findTextViewIsVisible = NO;
	
	[ UIView beginAnimations: @"FullScreenAnimations" context: nil ]; // Tell UIView we're ready to start animations.
	[ UIView setAnimationCurve: UIViewAnimationCurveEaseIn ];
	[ UIView setAnimationDuration: 0.2f ]; // Set the duration to 5/10ths of a second.
	
	findTextView.frame = CGRectMake(-500, findTextView.frame.origin.y, findTextView.frame.size.width, findTextView.frame.size.height);
	
	[UIView commitAnimations];
	
	[findTextField resignFirstResponder];
}

- (IBAction) performTextSearch: (id) sender {
//	NSString* searchQueryPart1 = @"javascript:function findAndReplace(searchText, replacement, searchNode) { if (!searchText || typeof replacement === 'undefined') { return; } var regex = typeof searchText === 'string' ? new RegExp(searchText, 'g') : searchText, childNodes = (searchNode || document.body).childNodes, cnLength = childNodes.length, excludes = 'html,head,style,title,link,meta,script,object,iframe'; while (cnLength--) { var currentNode = childNodes[cnLength]; if (currentNode.nodeType === 1 && (excludes + ',').indexOf(currentNode.nodeName.toLowerCase() + ',') === -1) { arguments.callee(searchText, replacement, currentNode); } if (currentNode.nodeType !== 3 || !regex.test(currentNode.data) ) { continue; } var parent = currentNode.parentNode, frag = (function(){ var html = currentNode.data.replace(regex, replacement), wrap = document.createElement('div'), frag = document.createDocumentFragment(); wrap.innerHTML = html; while (wrap.firstChild) { frag.appendChild(wrap.firstChild); } return frag; })(); parent.insertBefore(frag, currentNode); parent.removeChild(currentNode); } } findAndReplace('";
//	NSString* searchQueryPart2 = @"', function(text){ return '<span style=\"background-color:#ff0000\">' + text + '</span>'; });";
	
	int randNum = rand() * 98 * 3;
	
	NSString* iden = [@"ocean" stringByAppendingString:[NSString stringWithFormat:@"%d", randNum]];
	
	NSString* searchQueryPart1 = @"javascript: rn=Math.floor(Math.random()*100); x=new RegExp('";
	NSString* searchQueryPart2 = @"', 'gi'); b = document.body.innerHTML.replace(x, '<span name=\"rid$\" style=\"background-color:yellow; font-weight:bold; \">";
	NSString* searchQueryPart3 = @"</span>'); void(document.body.innerHTML=b); window.scrollTo(0, document.getElementsByName(\"rid$\")[0].offsetTop);";
		
	NSString* searchText = findTextField.text;
	
	NSString* finalQuery = nil;
	
	if(searchText != nil && searchText.length > 0)
	{
		finalQuery = [[searchQueryPart1 stringByAppendingString:searchText] stringByAppendingString:[searchQueryPart2 stringByReplacingOccurrencesOfString:@"rid$" withString:iden]];		
		finalQuery = [[finalQuery stringByAppendingString:searchText] stringByAppendingString:[searchQueryPart3 stringByReplacingOccurrencesOfString:@"rid$" withString:iden]];
		
		finalQuery = [finalQuery stringByReplacingOccurrencesOfString:@"'" withString:@"\\'"];
		finalQuery = [[@"var t = document.createElement('a'); t.setAttribute('href', '" stringByAppendingString:finalQuery] stringByAppendingString:
					 @"'); var e = document.createEvent('MouseEvent'); e.initMouseEvent('click'); t.dispatchEvent(e);"];
				
		[webBrowser stringByEvaluatingJavaScriptFromString:finalQuery];
		
		[self hideFindTextView:0];
	}
	else
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Please input a valid search query."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
		
		return;
	}
}

- (IBAction) cancelInput: (id) sender {
	[urlTextField resignFirstResponder];
	[searchTextField resignFirstResponder];
}

- (IBAction) showHideBrowserToolbar: (id) sender {
	if(toolbarIsMinimized)
	{
		CGRect urlViewSize = [urlView frame];
		urlViewSize.size.height = urlViewSize.size.height + 38;
		urlView.frame = urlViewSize;
		
		CGRect miniTabSize = [miniTabParentView frame];
		miniTabSize.origin.y = miniTabSize.origin.y + 38;
		miniTabParentView.frame = miniTabSize;
		
		CGRect browserRect = [browserParentView frame];
		browserRect.size.height = browserRect.size.height - 38;
		browserRect.origin.y = browserRect.origin.y + 38;
		browserParentView.frame = browserRect;
		
		browserRect = [webBrowser frame];
		browserRect.size.height = browserParentView.frame.size.height;
		webBrowser.frame = browserRect;
		
		toolbarIsMinimized = NO;
		
		UIImage* img = [UIImage imageNamed:@"MinimizeToolbarIcon.png"];
		
		[minMaxToolbarBtn setImage:img forState:UIControlStateNormal];
		[minMaxToolbarBtn setImage:img forState:UIControlStateHighlighted];
		[minMaxToolbarBtn setImage:img forState:UIControlStateSelected];
	}
	else
	{
		CGRect urlViewSize = [urlView frame];
		urlViewSize.size.height = urlViewSize.size.height - 38;
		urlView.frame = urlViewSize;
		
		CGRect miniTabSize = [miniTabParentView frame];
		miniTabSize.origin.y = miniTabSize.origin.y - 38;
		miniTabParentView.frame = miniTabSize;
		
		CGRect browserRect = [browserParentView frame];
		browserRect.size.height = browserRect.size.height + 38;
		browserRect.origin.y = browserRect.origin.y - 38;
		browserParentView.frame = browserRect;
		
		browserRect = [webBrowser frame];
		browserRect.size.height = browserParentView.frame.size.height;
		webBrowser.frame = browserRect;
		
		toolbarIsMinimized = YES;
		
		UIImage* img = [UIImage imageNamed:@"MaximizeToolbarIcon.png"];
		
		[minMaxToolbarBtn setImage:img forState:UIControlStateNormal];
		[minMaxToolbarBtn setImage:img forState:UIControlStateHighlighted];
		[minMaxToolbarBtn setImage:img forState:UIControlStateSelected];
	}
}

- (IBAction) showBrowserSettings: (id) sender {
	AppSettingsViewController* settingsController = nil;
	
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		settingsController = [[AppSettingsViewController alloc] initWithNibName:@"AppSettings" bundle:nil];
	}
	else
	{
		settingsController = [[AppSettingsViewController alloc] initWithNibName:@"AppSettingsLandscape" bundle:nil];
	}
	
	settingsController.browserViewController = self;
	
	settingsController.expectedInterfaceOrientation = self.interfaceOrientation;
	
	[self hideStatusBar];
	
	popupNeedToShowStatusBar = YES;
	
	[self presentModalViewController:settingsController animated:YES];
	
	[settingsController release];
}

- (IBAction) showBookmarks: (id) sender {
	BookmarksController* bookmarkController = nil;
		
	if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		bookmarkController = [[BookmarksController alloc] initWithNibName:@"BookmarksView" bundle:nil];
	}
	else
	{
		bookmarkController = [[BookmarksController alloc] initWithNibName:@"BookmarksViewLandscape" bundle:nil];
	}
	
	bookmarkController.browserController = self;
	
	bookmarkController.expectedInterfaceOrientation = self.interfaceOrientation;

	[self hideStatusBar];
	
	if(!showingFullScreenView)
	{
		popupNeedToShowStatusBar = YES;
	}
	
	[self presentModalViewController:bookmarkController animated:YES];
	
	[bookmarkController release];
}

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{	
	if(actionSheet == tapAndHoldImageActionSheet)
	{
		NSString* imageURL = (NSString*) [tapAndHoldElement objectForKey:@"WebElementImageURL"];

		if(buttonIndex == 0)
		{
			[self showDownloadImageProgressView:0];
			
			NSData *imageData = [NSData dataWithContentsOfURL:[NSURL URLWithString:imageURL]];
			UIImage *theImage = [UIImage imageWithData:imageData];
			UIImageWriteToSavedPhotosAlbum(theImage, self, nil, nil);
			
			[self hideDownloadImageProgressView:0];
		}
	}
	else if(actionSheet == tapAndHoldLinkActionSheet)
	{
		NSString* linkURL = (NSString*) [tapAndHoldElement objectForKey:@"WebElementLinkURL"];

		if(buttonIndex == 0)
		{
			[self loadURL:linkURL];
		}
		else if(buttonIndex == 1)
		{
			[self addTabAction:0];
			
			NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
			[self loadTab:([[tabsInfoDict allKeys] count] - 1) :NO];
			
			[self loadURL:linkURL];
		}
	}
	else if(actionSheet == historyActionSheet)
	{
		if(buttonIndex == 0)
		{
			NSDictionary* bookmarkFoldersDict2 = [userDefaults dictionaryForKey:kIBrowserBookmarkFolders];
			NSMutableDictionary* bookmarkDict = [[NSMutableDictionary alloc] init];
			[bookmarkDict setDictionary:bookmarkFoldersDict2];	
						
			NSMutableArray* bookmarkLinks = [[NSMutableArray alloc] initWithCapacity:1];
			
			[bookmarkDict setObject:bookmarkLinks forKey:kIBrowserHistoryFolder];
			
			[userDefaults setObject:bookmarkDict forKey:kIBrowserBookmarkFolders];
			
			[userDefaults synchronize];
			
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Browser History" message:@"Browser history has been cleared" 
														   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
			[alert show];	
			[alert release];
		}
		else if(buttonIndex == 1)
		{
			if([[userDefaults stringForKey:kIBrowserDisplayHistory] compare:kIBrowserFalse] == NSOrderedSame)
			{
				[userDefaults setObject:kIBrowserTrue forKey:kIBrowserDisplayHistory];
				
				UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Browser History" message:@"Browser history logging has been enabled" 
															   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
				[alert show];	
				[alert release];
			}
			else
			{
				[userDefaults setObject:kIBrowserFalse forKey:kIBrowserDisplayHistory];
				
				UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Browser History" message:@"Browser history logging has been disabled" 
															   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
				[alert show];	
				[alert release];
			}
		}
	}
	else if(actionSheet == tapAndHoldLinkAndImageActionSheet)
	{
		NSString* imageURL = (NSString*) [tapAndHoldElement objectForKey:@"WebElementImageURL"];
		NSString* linkURL = (NSString*) [tapAndHoldElement objectForKey:@"WebElementLinkURL"];
		
		if(buttonIndex == 0)
		{
			[self loadURL:linkURL];
		}
		else if(buttonIndex == 1)
		{
			[self addTabAction:0];
			
			NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
			[self loadTab:([[tabsInfoDict allKeys] count] - 1) :NO];
			
			[self loadURL:linkURL];
		}
		else if(buttonIndex == 2)
		{
			[self showDownloadImageProgressView:0];
			
			NSData *imageData = [NSData dataWithContentsOfURL:[NSURL URLWithString:imageURL]];
			UIImage *theImage = [UIImage imageWithData:imageData];
			UIImageWriteToSavedPhotosAlbum(theImage, self, nil, nil);
			
			[self hideDownloadImageProgressView:0];
		}
	}
	else if(actionSheet == pageActionSheet)
	{
		// the user clicked one of the OK/Cancel buttons
		if (buttonIndex == 0)
		{
			AddBookmarkController* addBookmarkController = nil;
			
			if(self.interfaceOrientation == UIInterfaceOrientationPortrait || self.interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
			{
				addBookmarkController = [[AddBookmarkController alloc] initWithNibName:@"AddBookmarkView" bundle:nil];
				addBookmarkController.isInLandscapeMode = NO;
			}
			else
			{
				addBookmarkController = [[AddBookmarkController alloc] initWithNibName:@"AddBookmarkViewLandscape" bundle:nil];
				addBookmarkController.isInLandscapeMode = YES;
			}
			
			if(pageTitleLabel.text == nil)
			{
				pageTitleLabel.text = @"";
			}
			
			addBookmarkController.bookmarkLink = urlTextField.text;
			addBookmarkController.bookmarkName = pageTitleLabel.text;
			
			addBookmarkController.expectedInterfaceOrientation = self.interfaceOrientation;
			
			[self hideStatusBar];
			
			popupNeedToShowStatusBar = YES;
			
			[self presentModalViewController:addBookmarkController animated:YES];
			
			[addBookmarkController release];
		}
		else if(buttonIndex == 1)
		{
			NSString* newHomepage = @"";
			
			if(urlTextField.text != nil && urlTextField.text.length > 0)
			{
				newHomepage = urlTextField.text;
			}
			
			[[NSUserDefaults standardUserDefaults] setObject:newHomepage forKey:kIBrowserHomePage];
			
			if([newHomepage compare:@""] == NSOrderedSame)
			{
				newHomepage = @"<blank>";
			}
			
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Home Page" message:[@"Your homepage has been set to " stringByAppendingString:newHomepage]
														   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
			[alert show];	
			[alert release];
		}
		else if(buttonIndex == 2)
		{
			NSString* emailSubject = nil;
			
			if(pageTitleLabel.text == nil || pageTitleLabel.text.length <= 0)
			{
				emailSubject = @"Web URL";
			}
			else
			{
				emailSubject = [pageTitleLabel.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
			}
			
			
			NSString* mailToStr = @"mailto:?";
			
			if(emailSubject != nil)
			{
				emailSubject = [emailSubject stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
				
				mailToStr = [mailToStr stringByAppendingString:@"subject="];
				mailToStr = [mailToStr stringByAppendingString:emailSubject];
				mailToStr = [mailToStr stringByAppendingString:@"&"];
			}
			
			if(urlTextField.text != nil && urlTextField.text.length > 0)
			{
				NSString* urlStr = [urlTextField.text stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
				urlStr = [urlStr stringByReplacingOccurrencesOfString:@"/" withString:@"%2F"];
				urlStr = [urlStr stringByReplacingOccurrencesOfString:@":" withString:@"%3A"];
				urlStr = [urlStr stringByReplacingOccurrencesOfString:@"?" withString:@"%3F"];
				urlStr = [urlStr stringByReplacingOccurrencesOfString:@";" withString:@"%3B"];
				urlStr = [urlStr stringByReplacingOccurrencesOfString:@"#" withString:@"%23"];
				
				mailToStr = [mailToStr stringByAppendingString:@"body="];
				mailToStr = [mailToStr stringByAppendingString:urlStr];
			}
			else
			{
				mailToStr = [mailToStr stringByAppendingString:@"body="];
			}
			
			[[UIApplication sharedApplication] openURL:[NSURL URLWithString:mailToStr]];
		}
		else if(buttonIndex == 3)
		{
			NSString* address = @"";
			
			if(urlTextField.text != nil)
			{
				address = urlTextField.text;
			}
			
			[[UIApplication sharedApplication] openURL:[NSURL URLWithString: address]];
		}
	}
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField {
	if(textField == urlTextField)
	{
		urlTextField.text = [urlTextField.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]];
		
		if(urlTextField.text.length > 0 && [urlTextField.text rangeOfString:@"http://"].location == NSNotFound && [urlTextField.text rangeOfString:@"https://"].location == NSNotFound)
		{
			urlTextField.text = [@"http://" stringByAppendingString:urlTextField.text];
		}
			
		if(urlTextField.text != nil && [urlTextField.text compare: @""] != NSNotFound)
		{			
			[self loadURL:urlTextField.text];
		}
	}
	else if(textField == searchTextField)
	{
		NSString* currentSearchEngine = [[NSUserDefaults standardUserDefaults] stringForKey:kIBrowserSearchEngine];
		
		if(searchTextField.text.length > 0)
		{		
			NSString* searchStr = [searchTextField.text stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
			searchStr = [searchStr stringByReplacingOccurrencesOfString:@"/" withString:@"%2F"];
			searchStr = [searchStr stringByReplacingOccurrencesOfString:@":" withString:@"%3A"];
			searchStr = [searchStr stringByReplacingOccurrencesOfString:@"?" withString:@"%3F"];
			searchStr = [searchStr stringByReplacingOccurrencesOfString:@";" withString:@"%3B"];
			searchStr = [searchStr stringByReplacingOccurrencesOfString:@"#" withString:@"%23"];
			
			if([currentSearchEngine compare:kIBrowserAskVal] == NSOrderedSame)
			{
				searchStr = [@"http://www.ask.com/web?q=" stringByAppendingString:searchStr];
			}
			else if([currentSearchEngine compare:kIBrowserBaiduVal] == NSOrderedSame)
			{
				searchStr = [@"http://www.baidu.com/s?wd=" stringByAppendingString:searchStr];
			}
			else if([currentSearchEngine compare:kIBrowserGoogleVal] == NSOrderedSame)
			{
				searchStr = [@"http://www.google.com/m/search?q=" stringByAppendingString:searchStr];
			}
			else if([currentSearchEngine compare:kIBrowserLiveVal] == NSOrderedSame)
			{
				searchStr = [@"http://bing.com/search?q=" stringByAppendingString:searchStr];
			}
			else if([currentSearchEngine compare:kIBrowserYahooVal] == NSOrderedSame)
			{
				searchStr = [@"http://search.m.yahoo.com/search?p=" stringByAppendingString:searchStr];
			}
			
			if(searchStr.length > 0)
			{
				[self loadURL:searchStr];			
			}
		}
	}
	
	[textField resignFirstResponder];
	
	return YES;
}

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
	return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSDictionary* tabsInfo = [userDefaults objectForKey:kIBrowserTabsInfo];
	
	int width = kMinimizedTabXOffset + [[tabsInfo allKeys] count]*kMinimizedTabXSpacing + 74;

	minimizedTabView.contentSize = CGSizeMake(width, minimizedTabView.frame.size.height);
	
	[self loadMinimizedTabView];
	
	return [[tabsInfo allKeys] count];
}

- (void) loadFavicons: (id) anObject {
	
	@synchronized(syncObj)
	{			
		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
				
		UIWebView* webView = (UIWebView*)anObject;
		
		NSArray* keys = [tabs allKeysForObject:webView];
		
		if(keys != nil && [keys count] > 0)
		{
			NSString* key = (NSString*)[keys objectAtIndex:0];
			
			WebView *coreWebView = [[webView _documentView] webView];
			
			NSString* tabURL = coreWebView.mainFrameURL;
			
			if(tabURL != nil)
			{
				NSArray* split = [tabURL componentsSeparatedByString:@"/"];
				
				if([split count] >= 3)
				{
					tabURL = [NSString stringWithFormat:@"%@",[split objectAtIndex:2]];
					tabURL = [@"http://" stringByAppendingString:tabURL];	
				}
			}
			
			tabURL = [tabURL stringByAppendingString:@"/favicon.ico"];
			
			NSData* imageData = [NSData dataWithContentsOfURL:[NSURL URLWithString:tabURL]];
			UIImage* iconImage = nil;
			bool releaseImage = NO;
			
			if([imageData bytes] != nil)
			{
				iconImage = [[UIImage alloc] initWithData:imageData];
				releaseImage = YES;
			}
			else
			{
				iconImage = [UIImage imageNamed:@"PageIcon.png"];
			}
			
			UIImageView* imageView = [[UIImageView alloc] initWithImage:iconImage];
			imageView.frame = CGRectMake(2, 7, 25, 25);
			
			[favicons setObject:imageView forKey:key];
			
			if(releaseImage)
			{
				[iconImage release];
			}
			
			[imageView release];
		}
		
		[self performSelectorOnMainThread:@selector(refreshTabTable:) withObject:nil waitUntilDone:NO];
		
		[pool release];
	}
}

- (void) miniTabBtnDeleted: (id) obj {
	NSArray* keys = [tabDeleteButtons allKeysForObject:obj];
	
	if(keys != nil && [keys count] > 0)
	{
		NSString* key = (NSString*)[keys objectAtIndex:0];
		
		[self deleteTab:[key intValue]];
	}
}

- (void) miniTabBtnSelected: (id) obj {
	NSArray* keys = [tabButtons allKeysForObject:obj];
	
	if(keys != nil && [keys count] > 0)
	{
		NSString* key = (NSString*)[keys objectAtIndex:0];
		
		[self loadTab:[key intValue] :NO];
	}
	
	[self loadMinimizedTabView];
}

-(UIImage*)resizedImage:(UIImage*)inImage  inRect:(CGRect)thumbRect {
	// Creates a bitmap-based graphics context and makes it the current context.
	UIGraphicsBeginImageContext(thumbRect.size);
	[inImage drawInRect:thumbRect];
	
	return UIGraphicsGetImageFromCurrentImageContext();
}

- (void) loadMinimizedTabView {
	
	NSDictionary* tabsInfo = [userDefaults objectForKey:kIBrowserTabsInfo];
		
	int count = [[tabsInfo allKeys] count];
	
	for(int i = 0; i < count; ++i)
	{		
		NSString* index = [NSString stringWithFormat:@"%d", i];

		NSArray* selectTabInfo = [tabsInfo objectForKey:index];
	
		UIImageView* faviconImageView = [favicons objectForKey:index];
		UIImage* img = nil;
		
		if(faviconImageView != nil)
		{
			img = [faviconImageView image];
		}
		else
		{
			img = [UIImage imageNamed:@"PageIcon.png"];		
		}
		
		UIButton* btn = [tabButtons objectForKey:index];
		UIButton* deleteBtn = [tabDeleteButtons objectForKey:index];
		
		if(btn == nil)
		{
			int x = kMinimizedTabXOffset;
			if([index intValue] > 0)
			{
				x += [index intValue]*kMinimizedTabXSpacing;
			}
			
			btn = [UIButton buttonWithType:UIButtonTypeCustom];
			btn.frame = CGRectMake(x, 0, 110, 30);
			[minimizedTabView addSubview:btn];	
			
			deleteBtn = [UIButton buttonWithType:UIButtonTypeCustom];
			deleteBtn.frame = CGRectMake(x + 120 - 36, 3, 15, 15);
			[minimizedTabView addSubview:deleteBtn];
			
			[tabButtons setObject:btn forKey:index];
			[tabDeleteButtons setObject:deleteBtn forKey:index];
			
			[btn addTarget:self action:@selector(miniTabBtnSelected:) forControlEvents:UIControlEventTouchUpInside];
			[deleteBtn addTarget:self action:@selector(miniTabBtnDeleted:) forControlEvents:UIControlEventTouchUpInside];
			
			[btn setContentEdgeInsets:UIEdgeInsetsMake(0, 20, 0, 15)];
			[btn setImageEdgeInsets:UIEdgeInsetsMake(0, -15, 0, 15)];
			[btn setTitleEdgeInsets:UIEdgeInsetsMake(2, -10, -2, 3)];
			
			btn.font = [UIFont systemFontOfSize: 11];
			btn.lineBreakMode = UILineBreakModeTailTruncation;
			
			[btn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];			
			[btn setTitleColor:[UIColor blackColor] forState:UIControlStateHighlighted];			
			[btn setTitleColor:[UIColor blackColor] forState:UIControlStateDisabled];
			
			[deleteBtn setBackgroundImage:tabDeleteImg forState:UIControlStateNormal];
			[deleteBtn setBackgroundImage:tabDeleteHighlightedImg forState:UIControlStateHighlighted];
			[deleteBtn setBackgroundImage:tabDeleteHighlightedImg forState:UIControlStateDisabled];			
		}
		
		UIImage* resizedImg = [self resizedImage:img inRect:CGRectMake(2, 2, 20, 20)];
		
		if([[userDefaults stringForKey:kIBrowserCurrentTabIndex] intValue] == i)
		{
			[btn setBackgroundImage:desktopTabHighlightedImg forState:UIControlStateNormal];
			[btn setBackgroundImage:desktopTabHighlightedImg forState:UIControlStateHighlighted];
			[btn setBackgroundImage:desktopTabHighlightedImg forState:UIControlStateDisabled];
		}
		else
		{		
			[btn setBackgroundImage:desktopTabImg forState:UIControlStateNormal];
			[btn setBackgroundImage:desktopTabImg forState:UIControlStateHighlighted];
			[btn setBackgroundImage:desktopTabImg forState:UIControlStateDisabled];
		}
		
		[btn setTitle:[selectTabInfo objectAtIndex:kIBrowserTabNameIndex] forState:UIControlStateNormal];
		
		[btn setImage:resizedImg forState:UIControlStateNormal];
		[btn setImage:resizedImg forState:UIControlStateHighlighted];
		[btn setImage:resizedImg forState:UIControlStateDisabled];
	}
	
	if([tabButtons count] > count)
	{
		for (int j = count; j < [tabButtons count]; ++j)
		{
			NSString* tabIndex = [NSString stringWithFormat:@"%d", j];
			UIButton* btn = [tabButtons objectForKey:tabIndex];
			UIButton* deleteBtn = [tabDeleteButtons objectForKey:tabIndex];
			
			[deleteBtn removeFromSuperview];
			[btn removeFromSuperview];
			
			[tabButtons removeObjectForKey:tabIndex];
			[tabDeleteButtons removeObjectForKey:tabIndex];
		}
	}
	
	int xCoord = 10;
	xCoord += count * kMinimizedTabXSpacing;
	
	newTabMiniTabBtn.frame = CGRectMake(xCoord, newTabMiniTabBtn.frame.origin.y, newTabMiniTabBtn.frame.size.width, newTabMiniTabBtn.frame.size.height);
	
	xCoord += 36;
	
	closeMiniTabBtn.frame = CGRectMake(xCoord, closeMiniTabBtn.frame.origin.y, closeMiniTabBtn.frame.size.width, closeMiniTabBtn.frame.size.height);
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	
	NSString* strIndex = [NSString stringWithFormat:@"%d", [indexPath row]];
	
	NSDictionary* tabsInfo = [userDefaults objectForKey:kIBrowserTabsInfo];
	
	NSArray* selectTabInfo = [tabsInfo objectForKey:strIndex];
	
    UITableViewCell *cell = nil;//(UITableViewCell *)[tabsSelectionTable dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];		
    }
	else
	{
		//clear prior content views
		NSArray* cellContentSubviews = cell.contentView.subviews;
		
		if(cellContentSubviews && [cellContentSubviews count] > 0)
		{
			for(int i = 0; i < [cellContentSubviews count]; ++i)
			{
				[[cellContentSubviews objectAtIndex:i] removeFromSuperview];
			}
		}
	}
	
	cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
	
	UILabel* cellTextLabel = [[UILabel alloc] initWithFrame:CGRectMake(35, 0, 235, 40)];
	cellTextLabel.text = [selectTabInfo objectAtIndex:kIBrowserTabNameIndex];
	cellTextLabel.textColor = [UIColor whiteColor];
	cellTextLabel.font = [UIFont boldSystemFontOfSize:17];
	cellTextLabel.backgroundColor = [UIColor clearColor];
	
	[cell.contentView addSubview: cellTextLabel];	
	
	UIImageView* faviconImageView = [favicons objectForKey:strIndex];
	
	if(faviconImageView != nil)
	{
		[cell.contentView addSubview:faviconImageView];
	}
	else
	{
		UIImage* iconImage = [UIImage imageNamed:@"PageIcon.png"];
		faviconImageView = [[UIImageView alloc] initWithImage:iconImage];
		faviconImageView.frame = CGRectMake(2, 7, 25, 25);
		
		[cell.contentView addSubview:faviconImageView];
	}
		
	return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    /*
	 To conform to the Human Interface Guidelines, selections should not be persistent --
	 deselect the row after it has been selected.
	 */
	[tabsSelectionTable deselectRowAtIndexPath:indexPath animated:YES];
	
	[self hideCustomTabsSelectionView:0];

	[self loadTab:[indexPath row] :NO];
}

- (void) deleteTab: (int) indexPath {
	NSDictionary* tabsInfoDict = [userDefaults objectForKey:kIBrowserTabsInfo];
	
	if([[tabsInfoDict allKeys] count] <= 1)
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"You must have atleast one tab in the browser."
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];	
		[alert release];
		
		[self tabEditAction:0];
	}
	else
	{		
		NSMutableDictionary* newTabsInfoDict = [[NSMutableDictionary alloc] init];
		[newTabsInfoDict addEntriesFromDictionary:tabsInfoDict];
		
		NSString* strIndex = [NSString stringWithFormat:@"%d", indexPath];
		
		if(indexPath >= 1)
		{
			NSString* prevStrIndex = [NSString stringWithFormat:@"%d", (indexPath - 1)];
			
			[self loadTab:(indexPath - 1) :NO];
			
			[userDefaults setObject:prevStrIndex forKey:kIBrowserCurrentTabIndex];
		}
		else
		{
			[self loadTab:(indexPath + 1) :NO];
			
			[userDefaults setObject:strIndex forKey:kIBrowserCurrentTabIndex];
		}
		
		
		int index = indexPath + 1;
		
		if(index == [[tabsInfoDict allKeys] count])
		{
			strIndex = [NSString stringWithFormat:@"%d", index - 1];
			
			UIWebView* oldTab = [self.tabs objectForKey:strIndex];
			
			if(oldTab != nil)
			{
				//IMPORTANT!
				[oldTab removeFromSuperview];
				
				[self.tabs removeObjectForKey:strIndex];
			}
			
			[newTabsInfoDict removeObjectForKey:strIndex];
			
			id faviconImg = [favicons objectForKey:strIndex];
			
			if(faviconImg != nil)
			{
				[favicons removeObjectForKey:strIndex];
			}
		}	
		else
		{				
			while(index < [[tabsInfoDict allKeys] count])
			{				
				strIndex = [NSString stringWithFormat:@"%d", index];
				
				UIWebView* tabBrowser = [self.tabs objectForKey:strIndex];
				id faviconImg = [favicons objectForKey:strIndex];
				
				[tabBrowser retain];
				
				if(faviconImg != nil)
				{
					[faviconImg retain];
				}
				
				[self.tabs removeObjectForKey:strIndex];
				
				if(faviconImg != nil)
				{
					[favicons removeObjectForKey:strIndex];
				}
				
				id obj = [newTabsInfoDict objectForKey:strIndex];
				
				[obj retain];
				
				[newTabsInfoDict removeObjectForKey:strIndex];
				
				strIndex = [NSString stringWithFormat:@"%d", (index - 1)];
				
				[newTabsInfoDict removeObjectForKey:strIndex];
				
				id favImg = [favicons objectForKey:strIndex];
				
				if(favImg != nil)
				{					
					[favicons removeObjectForKey:strIndex];
				}
				
				[newTabsInfoDict setObject:obj forKey:strIndex];
				
				if(faviconImg != nil)
				{
					[favicons setObject:faviconImg forKey:strIndex];
				}
				
				[obj release];
				
				if(faviconImg != nil)
				{
					[faviconImg release];
				}
				
				UIWebView* oldTab = [self.tabs objectForKey:strIndex];
				
				if(oldTab != nil)
				{
					//IMPORTANT!
					[oldTab removeFromSuperview];
					
					[self.tabs removeObjectForKey:strIndex];
				}				
				
				if(tabBrowser != nil)
				{									
					[self.tabs setObject:tabBrowser forKey:strIndex];
					[tabBrowser release];
				}
				/**
				 else
				 {
				 [self.tabs removeObjectForKey:strIndex];
				 }		
				 **/
				
				++index;
			}
		}
		
		--index;			
		
		[userDefaults setObject:newTabsInfoDict forKey:kIBrowserTabsInfo];
		
		[userDefaults synchronize];
		
		[newTabsInfoDict release];
		
		[tabsSelectionTable reloadData];
	}	
}

// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath {
    
    if (editingStyle == UITableViewCellEditingStyleDelete) {
		[self deleteTab:[indexPath row]];
	}
}

- (void)didReceiveMemoryWarning {
	if(tabs != nil)
	{
		NSString* currentTabIndex = [userDefaults objectForKey:kIBrowserCurrentTabIndex];
		
		NSArray* keys = [tabs allKeys];
		
		for(int i = 0; i < [keys count]; ++i)
		{
			NSString* keyStr = [keys objectAtIndex:i];
			
			if([keyStr compare:currentTabIndex] != NSOrderedSame)
			{
				UIWebView* webView = [tabs objectForKey:keyStr];
				
				if(webView != nil)
				{
					[tabs removeObjectForKey:keyStr];
					
					if([webView retainCount] > 0)
					{
						[webView removeFromSuperview];
					}
				}
			}
		}
	}
	
	[super didReceiveMemoryWarning];
}

- (void)dealloc {
		
	[tabs release];
	[ findTextView release];
	[ findTextField release];
	[ userDefaults release];
	[ pageActionSheet release];
	[ downloadImageProgressView release];
	[ browserParentView release];
	[ blackBackgroundImageView release];
	[ tabEditButton release];
	[ moreButtonsButton release];
	[ lessButtonsButton release];
	[ findButton release];
	[ lockRotationButton release];
	[ tabsSelectionTable release];
	[ tabsSelectionView release];
	[ urlView release];
	[ toolbarView release];
	[ webBrowser release];
	[ historyFolder release];
	[ bookmarkFoldersDict release];
	[ progressImageView release];
	[ progressParentView release];
	[ progressView release];
	[ urlTextField release];
	[ searchTextField release];
	[ backButton release];
	[ forwardButton release];
	[ refreshButton release];
	[ pageActionButton release];
	[ favoritesButton release];
	[ settingsButton release];
	[ stopLoadingButton release];
	[ homePageButton release];
	[ toolbarViewImage release];
	[ urlViewImage release];
	[ cancelInputButton release];
	[ webAddressLabel release];
	[ searchLabel release];
	[ pageTitleLabel release];
	[ fullScreenButton release];
	[ tabsButton release];
	[ favicons release];
	[ miniTabSeperator release];
	[ newTabMiniTabBtn release];
	[ editTabsMiniTabBtn release];
	[ minMaxToolbarBtn release];
	[ maximizeMiniTabBtn release];
	[ closeMiniTabBtn release];
	[ miniTabParentView release];
	[ miniTabParentImageView release];
	[ tabButtons release];
	[ tabDeleteButtons release];
	[ historyButton release];
	[ minimizedTabView release];
	[ fullScreenBackForwBtnView release];
	[ fullScreenForwButton release];
	[ fullScreenBackButton release];
	[ fullScreenShowMenuBtn release];
	[ fullScreenShowBookmarksBtn release];
	
    [super dealloc];
}

@end
