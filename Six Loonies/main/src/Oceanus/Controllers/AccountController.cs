using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Facebook;
using Mvc.Mailer;
using Oceanus.Models;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using Oceanus.Utilities;
using System.Drawing;
using Oceanus.Controllers.Helpers;
using System.Web.Script.Serialization;

namespace Oceanus.Controllers
{
    [BrowserCache(PreventBrowserCaching = true)]
    public class AccountController : UploadControllerBase
    {
        #region constants

        internal const string ConfirmedImageSuffix = ".final";
        internal const int MAX_INITIAL_USER_LOAD_SET = 15;

        #endregion

        private static readonly Dictionary<string, int> ThumbSizes = new Dictionary<string, int>()
        {
            {"photo", 470}
        };

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult Search(string q, long? since_date, int page = 1)
        {
            var userQueryable = Database.UserProfiles.Where(user => user.FullName.IndexOf(q, StringComparison.OrdinalIgnoreCase) > 0).AsQueryable();
            int toSkip = (page - 1) * MAX_INITIAL_USER_LOAD_SET;

            userQueryable = userQueryable.Skip(toSkip);

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                userQueryable.Where(user => user.aspnet_Users.aspnet_Membership.CreateDate < since_datetime);
            }

            var searchUserViewModels = userQueryable.Take(MAX_INITIAL_USER_LOAD_SET).AsEnumerable().Select(tag => tag.ToSearchUserViewModel(Database));
            return PartialView("_PeopleCollage", searchUserViewModels);
        }

        [Authorize]
        public ActionResult InviteFriends()
        {
            return View("_InviteFriends");
        }

        public ActionResult Index()
        {
            return View("Index", "_BetaLayout");
        }

        #region User Profile Page Views / Updates

        public ActionResult GetUserProfilePhotoUploadForm(int userProfileId)
        {
            ViewBag.UserProfileId = userProfileId;

            return PartialView("_AddUserPhoto");
        }

        #region overrides

        [TokenizedAuthorize]
        [RequiresAuthentication]
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult CheckExisting(int userProfileId, string filename)
        {
            return base.CheckExisting(userProfileId, filename, ImageFolderDefinitions.FOLDER_USER_UPLOADS);
        }

        public ContentResult Upload(int userProfileId, HttpPostedFileBase fileData)
        {
            return base.Upload(userProfileId, fileData, ImageFolderDefinitions.FOLDER_USER_UPLOADS);
        }

        #endregion

        [JsonpFilter]
        public ActionResult IsUserLoggedIn()
        {
            bool successful = false;
            UserProfile loggedInUserProfile = null;
            string domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            string UserFullName = string.Empty;
            string UserProfileImageUrl = domain + "/Images/user_white.png";

            if (Request.IsAuthenticated && User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
            {
                successful = true;

                MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
                Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;

                loggedInUserProfile = Database.UserProfiles.Where(u => u.UserId == currUserGuid).FirstOrDefault();

                if (loggedInUserProfile != null)
                {
                    UserFullName = loggedInUserProfile.FullName;
                    UserProfileImageUrl = (loggedInUserProfile.UserProfileImageUrl != null ? loggedInUserProfile.UserProfileImageUrl : UserProfileImageUrl);
                }
            }

            return Json(new
            {
                successful = successful.ToString().ToLower(),
                fullname = UserFullName,
                profileimageurl = UserProfileImageUrl
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUserImage(int userProfileId, string imageUrl)
        {
            string dbMessage = string.Empty;
            bool success = false;
            string imageFileName = string.Empty;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            try
            {
                string tempName = Guid.NewGuid().ToString();

                UserProfile userProfile = Database.UserProfiles.Where(u => u.UserProfileId == userProfileId).FirstOrDefault();

                // get the post's ID that was just inserted
                var newName = base.Confirm(ImageFolderDefinitions.FOLDER_USER_UPLOADS, userProfileId, imageUrl, string.Concat("profile.", userProfileId), ConfirmedImageSuffix);
                newName = EnsureOriginalFileIsJpeg(userProfileId, ImageFolderDefinitions.FOLDER_USER_UPLOADS, newName);
                GenerateThumbnails(newName, ThumbSizes);

                imageFileName = Path.GetFileNameWithoutExtension(newName);

                base.Delete(ImageFolderDefinitions.FOLDER_USER_UPLOADS, userProfileId, imageFileName + ".jpg");

                imageFileName = imageFileName + ".thumb" + ".photo" + ".jpg";

                userProfile.UserProfileImageUrl = imageFileName;
                Database.SaveChanges();

                //update user information cookies
                string AbsolutePathFormat = @"{0}/{1}/{2}/{3}";

                if (userProfile.UserProfileImageUrl != null)
                {
                    UserHelper.AddUserProfileImageUrlCookie(string.Format(AbsolutePathFormat, RelativeUploadPath, ImageFolderDefinitions.FOLDER_USER_UPLOADS, userProfile.UserProfileId, userProfile.UserProfileImageUrl));
                }

                dbMessage = "User Photo Saved";
                success = true;
            }
            catch (Exception ex)
            {
                dbMessage = string.Format("Error saving user photo: {0}", ex.Message);
                success = false;
            }

            Sweep(ImageFolderDefinitions.FOLDER_USER_UPLOADS, userProfileId, ConfirmedImageSuffix);

            return Json(
                new
                {
                    result = success.ToString(),
                    message = dbMessage,
                    imageUrl = imageFileName
                }
            );
        }


        [Authorize]
        public ActionResult UserProfileExpandNetwork()
        {
            List<FacebookFriendListViewModel> facebookfriendListViewModel = new List<FacebookFriendListViewModel>();

            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            if (currUserMembership != null)
            {
                Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;

                //check if user has connected their account to facebook
                var userProfile = Database.UserProfiles.Where(u => u.UserId == currUserGuid).FirstOrDefault();

                if (userProfile != null && userProfile.FacebookToken != null)
                {
                    ViewBag.CurrUserIsLinkedToFacebook = true;

                    var userFacebookFriendList = Database.FacebookFriendLists.Where(f => f.UserId == currUserGuid).AsEnumerable();

                    if (userFacebookFriendList != null && userFacebookFriendList.Count() > 0)
                    {
                        facebookfriendListViewModel.AddRange(userFacebookFriendList.Select(f => f.ToFacebookFriendListViewModel()));

                        //check status of between the two users (if they are alreay friends or if a friend request already exists)
                        foreach (var friend in facebookfriendListViewModel)
                        {
                            friend.FriendRequestSentOnSixLoonies = false; //default value
                            friend.FacebookFriendIsConnectedOnSixLoonies = false; //default value

                            if (friend.FacebookFriendSixLooniesUID != null)
                            {
                                //check if the two users are already friends
                                var userIsFriend = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == currUserGuid && f.SecondUserId == friend.FacebookFriendSixLooniesUID) ||
                                                                                                   (f.FirstUserId == friend.FacebookFriendSixLooniesUID && f.SecondUserId == currUserGuid)).FirstOrDefault();

                                if (userIsFriend != null)
                                {
                                    friend.FacebookFriendIsConnectedOnSixLoonies = true;
                                }
                                else
                                {
                                    //check if there is a pending friend request
                                    var userHasSentFriendRequest = Database.SixLooniesPendingFriendRequests.Where(r => r.InitiatingUserId == currUserGuid && r.TargetSixLooniesUserId == friend.FacebookFriendSixLooniesUID).FirstOrDefault();

                                    if (userHasSentFriendRequest != null)
                                    {
                                        friend.FriendRequestSentOnSixLoonies = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.CurrUserIsLinkedToFacebook = false;
                }
            }

            return PartialView("_UserExpandNetwork", facebookfriendListViewModel);
        }

        public ActionResult UserProfileFriendsOnPipfly()
        {
            IEnumerable<SixLooniesFriendConnectionViewModel> friendConnectionsList = null;

            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            if (currUserMembership != null)
            {
                Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;
                var userSixLooniesFriendListModel = Database.SixLooniesFriendConnections.Where(f => f.FirstUserId == currUserGuid || f.SecondUserId == currUserGuid).AsEnumerable();
                friendConnectionsList = userSixLooniesFriendListModel.Select(f => f.ToSixLooniesFriendConnectionViewModel());

                ViewBag.UserId = currUserGuid;
            }

            return PartialView("_UserFriendsOnPipfly", friendConnectionsList);
        }

        public ActionResult UserProfileFollowers(Guid userId)
        {
            IEnumerable<UserProfileViewModel> followersViewModel = null;
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            var userModel = Database.UserProfiles.Where(u => u.UserId == userId).FirstOrDefault();
            Guid? currUserGuid = (currUserMembership != null ? (Guid?)currUserMembership.ProviderUserKey : null);
            var currLoggedInUserFollowers = Database.UserToUserFollowingMappings.Where(mapping => mapping.FollowingUserId == currUserGuid).AsEnumerable();

            ViewBag.UserId = userId;
            if (userModel != null)
            {
                ViewBag.UserFirstName = userModel.FirstName;
            }

            var followers = Database.UserToUserFollowingMappings.Where(f => f.FollowedUserId == userId).AsEnumerable();
            followersViewModel = followers.Select(f => f.aspnet_Users1.UserProfiles.First().ToUserProfileViewModel());
            followersViewModel = UserHelper.FlagCurrentUserAsFollowerIfApplicable(followersViewModel, currUserMembership, currLoggedInUserFollowers);

            return PartialView("_UserFollowers", followersViewModel);
        }

        public ActionResult Profile(int id, ControllerBase.SortContentCriteria? sortBy)
        {
            var userModel = Database.UserProfiles.Where(u => u.UserProfileId == id).FirstOrDefault();
            UserProfileViewModel userProfileViewModel = null;

            if (userModel != null)
            {
                Guid userId = userModel.UserId;
                var user = userModel.aspnet_Users;
                userProfileViewModel = userModel.ToUserProfileViewModel();

                //no one other than super-admin should be able to view profiles of other super-admin
                if (user != null && Roles.IsUserInRole(user.UserName, SixLooniesMembership.UserRoles.SuperAdmin.ToString()) && !Roles.IsUserInRole(SixLooniesMembership.UserRoles.SuperAdmin.ToString()))
                {
                    return RedirectToAction("Index", "Error", new { message = SixLooniesMembership.NoPermissionMessage });
                }

                MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
                if (currUserMembership != null)
                {
                    Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;
                    var userToUserMapping = Database.UserToUserFollowingMappings.Where(mapping => mapping.FollowedUserId == userId && mapping.FollowingUserId == currUserGuid).FirstOrDefault();

                    userProfileViewModel.CurrentUserHasPendingFriendRequest = false;

                    if (userToUserMapping != null)
                    {
                        userProfileViewModel.CurrentUserIsFollowing = true;
                    }

                    var userIsFriend = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == userId && f.SecondUserId == currUserGuid) ||
                                                                                       (f.FirstUserId == currUserGuid && f.SecondUserId == userId)).FirstOrDefault();

                    if (userIsFriend != null)
                    {
                        userProfileViewModel.CurrentUserIsFriend = true;
                    }
                    else
                    {
                        var userHasSentFriendRequest = Database.SixLooniesPendingFriendRequests.Where(r => r.InitiatingUserId == currUserGuid && r.TargetSixLooniesUserId == userId).FirstOrDefault();

                        if (userHasSentFriendRequest != null)
                        {
                            userProfileViewModel.CurrentUserHasPendingFriendRequest = true;
                        }
                    }

                    //get network connection path between this user and the logged in user
                    if (userId != currUserGuid)
                    {
                        IEnumerable<NetworkConnectionPath> networkConnectionModels = Database.GetFriendNetworkConnectionsPath(currUserGuid, userId).ToList();

                        if (networkConnectionModels != null && networkConnectionModels.Count() > 0)
                        {
                            IEnumerable<NetworkConnectionPathViewModel> networkConnectionViewModels = networkConnectionModels.Select(conn => conn.ToNetworkConnectionsPathViewModel(Database));

                            userProfileViewModel.UserConnectionNetworkPath = networkConnectionViewModels;
                        }
                    }
                }
            }

            //IMPORTANT!!!
            ViewBag.since_date = DateTime.Now.ToUniversalTime().Ticks;
            ViewBag.SortBy = sortBy;

            return View("_UserProfile", userProfileViewModel);
        }

        public ActionResult GetUserTags(Guid userId, ControllerBase.SortContentCriteria? sortBy)
        {
            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            var tags = Database.Tags.Where(tag => tag.PhotoVideoClipTagMappings.Where(map => map.PhotoVideoClip.ContributingUserId == userId).Count() > 0 ||
                tag.ArticleClipTagMappings.Where(article => article.ArticleClip.ContributingUserId == userId).Count() > 0).AsEnumerable();

            var tagViewModels = tags.Select(tag => tag.ToTagViewModel(Database, loggedInUserGuid));

            var userProfile = Database.UserProfiles.Where(u => u.UserId == userId).FirstOrDefault();

            if (userProfile != null)
            {
                ViewBag.UserFirstName = userProfile.FirstName;
            }
            else
            {
                ViewBag.UserFirstName = null;
            }

            return PartialView("_TagCollage", tagViewModels);
        }

        public ActionResult GetUserStats(Guid userId)
        {
            var userModel = Database.UserProfiles.Where(u => u.UserId == userId).FirstOrDefault();
            UserStatsViewModel userStatsViewModel = null;

            if (userModel != null)
            {
                userStatsViewModel = userModel.ToUserStatsViewModel(Database);
            }

            return PartialView("_UserStats", userStatsViewModel);
        }

        public ActionResult PendingFriendRequests(Guid userId)
        {
            var pendingRequests = Database.SixLooniesPendingFriendRequests.Where(r => r.TargetSixLooniesUserId == userId && r.IsDeclined == false).AsEnumerable();
            IEnumerable<SixLooniesPendingFriendRequestViewModel> pendingRequestsViewModelList = null;

            if (pendingRequests != null)
            {
                pendingRequestsViewModelList = pendingRequests.Select(r => r.ToSixLooniesPendingFriendRequestViewModel(Database));
            }

            return PartialView("_PendingFriendRequests", pendingRequestsViewModelList);
        }

        [Authorize]
        public ActionResult AcceptFriendRequest(Guid friendId)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                //check if the user is already a friend 
                var existingFriendConnection = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == userGuid && f.SecondUserId == friendId) ||
                                                                                               (f.SecondUserId == friendId && f.SecondUserId == userGuid)).FirstOrDefault();

                if (existingFriendConnection == null)
                {
                    //retrieve the pending friend request
                    var existingPendingFriendRequest = Database.SixLooniesPendingFriendRequests.Where(r => r.InitiatingUserId == friendId && r.TargetSixLooniesUserId == userGuid).FirstOrDefault();

                    if (existingPendingFriendRequest != null)
                    {
                        //everything checks out... establish a friend connection between the users
                        SixLooniesFriendConnectionViewModel friendConnectionViewModel = new SixLooniesFriendConnectionViewModel(null, friendId, userGuid, DateTime.Now);
                        SixLooniesFriendConnection friendConnectionModel = friendConnectionViewModel.ToSixLooniesFriendConnection();

                        Database.SixLooniesPendingFriendRequests.DeleteObject(existingPendingFriendRequest);
                        Database.SixLooniesFriendConnections.AddObject(friendConnectionModel);
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage });
        }

        [Authorize]
        public ActionResult RejectFriendRequest(Guid friendId)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                //check if the user is already a friend 
                var existingFriendConnection = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == userGuid && f.SecondUserId == friendId) ||
                                                                                               (f.SecondUserId == friendId && f.SecondUserId == userGuid)).FirstOrDefault();

                if (existingFriendConnection == null)
                {
                    //retrieve the pending friend request
                    var existingPendingFriendRequest = Database.SixLooniesPendingFriendRequests.Where(r => r.InitiatingUserId == friendId && r.TargetSixLooniesUserId == userGuid).FirstOrDefault();

                    if (existingPendingFriendRequest != null)
                    {
                        existingPendingFriendRequest.IsDeclined = true;
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage });
        }

        [Authorize]
        public ActionResult SendSixLooniesFriendRequest(Guid indicatedFriend)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                //check if the user is already a friend 
                var existingFriendConnection = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == userGuid && f.SecondUserId == indicatedFriend) ||
                                                                                               (f.SecondUserId == indicatedFriend && f.SecondUserId == userGuid)).FirstOrDefault();

                if (existingFriendConnection == null)
                {
                    //check if the user already has a pending friend request
                    var existingPendingFriendRequest = Database.SixLooniesPendingFriendRequests.Where(r => r.InitiatingUserId == userGuid && r.TargetSixLooniesUserId == indicatedFriend).FirstOrDefault();

                    if (existingPendingFriendRequest == null)
                    {
                        SixLooniesPendingFriendRequestViewModel requestViewModel = new SixLooniesPendingFriendRequestViewModel(null, userGuid, indicatedFriend, null, DateTime.Now, false);
                        SixLooniesPendingFriendRequest requestModel = requestViewModel.ToSixLooniesPendingFriendRequest();

                        Database.SixLooniesPendingFriendRequests.AddObject(requestModel);
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage });
        }

        [Authorize]
        public ActionResult FollowUser(Guid userToFollowId)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                if (userGuid != userToFollowId)
                {
                    //check if the user is already following the other user
                    var existingMapping = Database.UserToUserFollowingMappings.Where(mapping => mapping.FollowedUserId == userToFollowId && mapping.FollowingUserId == userGuid).FirstOrDefault();

                    if (existingMapping == null)
                    {
                        UserToUserFollowerMappingViewModel viewModel = new UserToUserFollowerMappingViewModel(null, userToFollowId, userGuid, DateTime.Now);
                        UserToUserFollowingMapping mappingModel = viewModel.ToUserToUserFollowingMapping();

                        Database.UserToUserFollowingMappings.AddObject(mappingModel);
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage });
        }

        [Authorize]
        public ActionResult UnfollowUser(Guid followedUserId)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                if (userGuid != followedUserId)
                {
                    //check if the user is already following the other user
                    var userToUserMapping = Database.UserToUserFollowingMappings.Where(mapping => (mapping.FollowedUserId == followedUserId && mapping.FollowingUserId == userGuid)).FirstOrDefault();

                    if (userToUserMapping != null)
                    {
                        Database.UserToUserFollowingMappings.DeleteObject(userToUserMapping);
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage });
        }

        #endregion

        #region Account Logon / Signup
        // **************************************
        // URL: /Account/LogOn
        // **************************************


        public ActionResult SignInForm()
        {
            return PartialView("_SignInForm");
        }

        public ActionResult SignInFormWithReturn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View("SignIn");
        }

        public ActionResult SignInHeaderForm()
        {
            return PartialView("_SignInHeaderForm");
        }

        [Authorize]
        public ActionResult SendEmailInvite(string recipients, string message)
        {
            bool successful = true;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                JavaScriptSerializer s = new JavaScriptSerializer();
                List<ShareLinkRecipientViewModel> emailRecipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);

                //retrieve user profile of currently logged in user
                var userProfile = Database.UserProfiles.Where(u => u.UserId == userGuid).FirstOrDefault();

                ViewBag.UserFirstName = userProfile.FirstName;
                ViewBag.UserFullName = userProfile.FullName;
                ViewBag.UserId = userProfile.UserProfileId;
                ViewBag.Message = message;

                string htmlMessage = EmailHelper.RenderViewToString("_InviteFriendEmailTemplate", "_EmailLayout", null, ControllerContext, ViewBag, ViewData, TempData);
                string textMessage = userProfile.FullName + " invites you to Pipfly (www.pipfly.com).\n\n\nMessage:\n\n" + message + "\n\n-----Pipfly (www.pipfly.com)\n(c) Six Loonies Inc. 2012";

                EmailValidationAttribute emailValidationAttr = new EmailValidationAttribute();
                int invalidRecipientCount = 0;

                foreach (ShareLinkRecipientViewModel emailRecipient in emailRecipientViewModels)
                {
                    string emailAddress = emailRecipient.name;

                    if (emailValidationAttr.IsValid(emailAddress))
                    {
                        bool emailSent = EmailHelper.sendEmail(null, emailAddress, userProfile.FirstName + " asks you to join Pipfly", htmlMessage, textMessage);

                        if (!emailSent)
                        {
                            successful = false;
                        }
                    }
                    else
                    {
                        ++invalidRecipientCount;
                    }
                }

                if (successful && invalidRecipientCount == emailRecipientViewModels.Count)
                {
                    successful = false;
                }
            }
            catch (Exception e)
            {
                successful = false;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SendInviteToFacebookUser(long friendFBUID)
        {
            bool requestSuccessful = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            //retrieve facebook friend's name
            var fbFriend = Database.FacebookFriendLists.Where(f => f.FacebookFriendUID == friendFBUID).FirstOrDefault();

            if (!fbFriend.InvitedToSixLoonies)
            {
                string friendFirstName = (fbFriend.FacebookFriendName.IndexOf(' ') > 0) ? (fbFriend.FacebookFriendName.Substring(0, fbFriend.FacebookFriendName.IndexOf(' '))) : fbFriend.FacebookFriendName;

                //retrieve facebook token
                var userProfile = Database.UserProfiles.Where(u => u.UserId == userGuid).FirstOrDefault();

                if (userProfile != null)
                {
                    string fbToken = userProfile.FacebookToken;

                    var domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    bool querySuccessful = FacebookHelper.PostToFacebookWall(friendFBUID, fbToken, null, "I'd like you to join my network on Pipfly", domain,
                        friendFirstName + " - Pipfly is a collection of photos, videos and articles on topics curated by everyone who uses it.", null, "http://www.pipfly.com/images/sitetheme/Pipfly_logo_sm.png", null);

                    if (querySuccessful)
                    {
                        //make invite sent for friend
                        var facebookFriendListRecord = Database.FacebookFriendLists.Where(f => f.UserId == userGuid && f.FacebookFriendUID == friendFBUID).FirstOrDefault();

                        if (facebookFriendListRecord != null)
                        {
                            facebookFriendListRecord.InvitedToSixLoonies = true;
                            Database.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                requestSuccessful = true;
            }

            return Json(new { result = requestSuccessful.ToString().ToLower() });
        }

        public ActionResult FacebookUserRegister(long uid, string token)
        {
            var users = Membership.FindUsersByName(uid.ToString());
            bool requestSuccessful = false;
            bool isNewUser = false;

            // create account if it doesn't exist for the facebook user
            if (users.Count == 0)
            {
                var fapi = new FacebookAPI(token);

                // get user's facebook account info
                JSONObject result = fapi.Get("/" + uid);

                if (result.Dictionary != null && result.Dictionary.Count > 0)
                {
                    string firstName = result.Dictionary["first_name"].String;
                    string lastName = result.Dictionary["last_name"].String;
                    string email = result.Dictionary["email"] != null ? result.Dictionary["email"].String : string.Empty;
                    string userPhotoUrl = "http://graph.facebook.com/" + uid + "/picture?type=large";

                    MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
                    Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

                    if (loggedInUserGuid != null && email != null && !email.Trim().Equals(string.Empty))
                    {
                        var existingUsersFound = Membership.FindUsersByEmail(email);

                        foreach (MembershipUser existingUser in existingUsersFound)
                        {
                            var existingUserProfile = Database.UserProfiles.Where(u => u.UserId == ((Guid)existingUser.ProviderUserKey)).FirstOrDefault();

                            if (existingUserProfile != null)
                            {
                                existingUserProfile.FacebookToken = token;
                                existingUserProfile.FacebookUserUID = uid;
                                existingUserProfile.UserProfileImageUrl = (existingUserProfile.UserProfileImageUrl == null ? userPhotoUrl : existingUserProfile.UserProfileImageUrl);
                                Database.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        MembershipCreateStatus createStatus = MembershipService.CreateFacebookUser(firstName, lastName, uid, "f6lbook", email, token, userPhotoUrl);

                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            isNewUser = true;
                            FacebookSignIn(uid.ToString(), token);

                            try
                            {
                                SendWelcomeEmail(firstName + " " + lastName, email);
                            }
                            catch (Exception e)
                            {
                            }

                            requestSuccessful = true;
                        }
                        else
                        {
                            ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                        }
                    }
                }
            }
            else
            {
                //update facebook token
                string uidString = uid.ToString();
                var userProfile = Database.UserProfiles.Where(u => u.aspnet_Users.UserName.Equals(uidString)).First();
                userProfile.FacebookToken = token;
                Database.SaveChanges();

                FacebookSignIn(uid.ToString(), token);
                requestSuccessful = true;
            }

            //retreive or refresh friend list
            RetrieveFacebookUserFriendList(uid.ToString(), token);

            MembershipUser user = Membership.GetUser(uid.ToString());

            performPostSignInUserActions(user, false);

            return Json(new { successful = requestSuccessful.ToString().ToLower(), isnewuser = isNewUser.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        private void RetrieveFacebookUserFriendList(string uid, string token)
        {
            //check if user is already logged in
            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? userGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            if (userGuid == null) //not logged in... get user details using UID
            {
                MembershipUser user = Membership.GetUser(uid);
                userGuid = (Guid)user.ProviderUserKey;
            }

            var fapi = new FacebookAPI(token);

            // get user's facebook friend list
            JSONObject result = fapi.Get("/" + uid + "/friends");

            JSONObject[] friends = result.Dictionary["data"].Array;

            foreach (JSONObject friend in friends)
            {
                long friendUID = friend.Dictionary["id"].Integer;
                string friendName = friend.Dictionary["name"].String;
                string friendProfilePhotoUrl = "http://graph.facebook.com/" + friendUID + "/picture";

                var priorFacebookFriendAssociation = Database.FacebookFriendLists.Where(f => f.UserId == userGuid && f.FacebookFriendUID == friendUID).FirstOrDefault();

                if (priorFacebookFriendAssociation == null)
                {
                    //create a new friend association
                    FacebookFriendListViewModel friendAssociationViewModel = new FacebookFriendListViewModel(null, (Guid)userGuid, friendUID, friendName, friendProfilePhotoUrl, null, false);
                    priorFacebookFriendAssociation = friendAssociationViewModel.ToFacebookFriendList();

                    Database.FacebookFriendLists.AddObject(priorFacebookFriendAssociation);
                }
                else
                {
                    priorFacebookFriendAssociation.FriendProfilePhotoUrl = friendProfilePhotoUrl;
                }

                //check if facebook friend is on six loonies
                var facebookFriendSixLooniesUserRef = Database.UserProfiles.Where(u => u.FacebookUserUID == friendUID).FirstOrDefault();

                if (facebookFriendSixLooniesUserRef != null)
                {//friend is on six loonies :)
                    //note that in the facebook friend list 
                    priorFacebookFriendAssociation.FacebookFriendSixLooniesUID = facebookFriendSixLooniesUserRef.UserId;

                    //create a six loonies friend connection as well for the friend if there isn't one already
                    var sixLooniesFriendConnection = Database.SixLooniesFriendConnections.Where(c => (c.FirstUserId == userGuid && c.SecondUserId == facebookFriendSixLooniesUserRef.UserId) ||
                        (c.FirstUserId == facebookFriendSixLooniesUserRef.UserId || c.SecondUserId == userGuid)).FirstOrDefault();

                    if (sixLooniesFriendConnection == null)
                    {
                        SixLooniesFriendConnectionViewModel sixLooniesFriendViewModel = new SixLooniesFriendConnectionViewModel(null, (Guid)userGuid, facebookFriendSixLooniesUserRef.UserId, DateTime.Now);
                        Database.SixLooniesFriendConnections.AddObject(sixLooniesFriendViewModel.ToSixLooniesFriendConnection());
                    }
                }
            }

            Database.SaveChanges();
        }

        private void FacebookSignIn(string uid, string token)
        {
            if (MembershipService.ValidateUser(uid, "f6lbook"))
            {
                FormsService.SignIn(uid, false /* createPersistentCookie */);

                MembershipUser user = Membership.GetUser(uid);

                if (user != null)
                {
                    //IMPORTANT: Mark the user as author for any profile he/she owns
                    performPostSignInUserActions(user, false);
                }
            }
        }

        [HttpPost]
        public ActionResult SignIn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.Email, model.Password))
                {
                    FormsService.SignIn(model.Email, model.RememberMe);

                    MembershipUser user = Membership.GetUser(model.Email);

                    if (user != null)
                    {
                        performPostSignInUserActions(user, true);
                    }
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The email or password provided is incorrect.");
                }
            }

            ViewBag.ReturnUrl = returnUrl;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        [Authorize]
        public ActionResult LogOff()
        {
            FormsService.SignOut();

            UserHelper.RemoveUserProfileFullName();
            UserHelper.RemoveUserProfileId();
            UserHelper.RemoveUserProfileImageUrlCookie();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult BookmarkletLogin()
        {
            return RedirectToAction("Ssshhhh", "Home");
        }

        [RequireHttps]
        public ActionResult Login()
        {
            return View("_LoginForm", "_BookmarkletLayout");
        }

        public ActionResult RegisterForm()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return PartialView("_RegisterForm");
        }

        public ActionResult RegisterHeaderForm()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return PartialView("_RegisterHeaderForm");
        }

        private void performPostSignInUserActions(MembershipUser user, bool sendConfirmationEmail)
        {
            //add user information cookies
            UserProfile profile = Database.UserProfiles.Where(p => p.UserId == ((Guid)user.ProviderUserKey)).FirstOrDefault();

            if (!profile.IsConfirmed && sendConfirmationEmail)
            {
                //re-send registration notice if applicable
                StartConfirmRegistration(MembershipCreateStatus.Success, user);
            }

            if (profile.UserProfileImageUrl != null)
            {
                UserHelper.AddUserProfileImageUrlCookie(profile.UserProfileImageUrl);
            }
            else
            {
                UserHelper.AddUserProfileImageUrlCookie(null);
            }

            UserHelper.AddUserProfileFullName(profile.FirstName.Length > 13 ? (profile.FirstName.Substring(0, 13) + "...") : profile.FirstName);
            UserHelper.AddUserProfileId(profile.UserProfileId);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.FirstName, model.LastName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);

                    MembershipUser user = Membership.GetUser(model.Email);

                    performPostSignInUserActions(user, true);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return Redirect("/welcome/1");
        }

        private void SendWelcomeEmail(string userFullName, string emailAddress)
        {
            try
            {
                ViewBag.FullName = userFullName;
                string htmlMessage = EmailHelper.RenderViewToString("_WelcomeEmailTemplate", "_EmailLayout", null, null, ControllerContext, ViewBag, ViewData, TempData);

                string textMessage = "Welcome to Pipfly!\n\nWe're excited you're here! Pipfly is all about sharing what you like on the web with friends and others. Share what you're interested in and in return create a collection of the content you like.\n\n\n"
                                + "How to use Pipfly\n\n"
                                + "1. Visit www.pipfly.com/button to get the Pipfly bookmarklet\n\n"
                                + "2. On any webpage, click on the bookmarklet button\n\n\n"
                                + "See what you have collected so far or what others have shared\n\n"
                                + "1. Discover popoular topics of your choice using hashtags on www.pipfly.com\n\n"
                                + "2. Log In with your Facebook credentials and start following tags and people\n\n"
                                + "3. Lastly, be a tastemaker! Help us set the tone with high-quality content and avoid spam/nudity.\n\n\n"
                                + "Again, welcome to Pipfly!"
                                + "\n\n-----\n-The Pipfly Team\n\n(c) Six Loonies Inc. 2012";


                bool emailSent = EmailHelper.sendEmail(null, emailAddress, "Welcome to Pipfly!", htmlMessage, textMessage);
                
                if (emailSent)
                {
                    // TO DO : redirect to page saying confirmation email has been sent.
                }
            }
            catch (Exception ex)
            {
                // TODO: add logging
                throw ex;
            }
        }

        private void StartConfirmRegistration(MembershipCreateStatus createStatus, MembershipUser user)
        {
            // create random key
            var key = Guid.NewGuid();

            if (createStatus == MembershipCreateStatus.Success)
            {
                try
                {
                    var existingPendingRegistration = Database.PendingRegistrations.Where(reg => reg.membershipUserId == (Guid)user.ProviderUserKey).FirstOrDefault();

                    if (existingPendingRegistration == null) //send confirmation email
                    {
                        PendingRegistration registration = new PendingRegistration()
                                {
                                    membershipUserId = (Guid)user.ProviderUserKey,
                                    confirmationKey = key,
                                    initialRegistrationDate = DateTime.Now,
                                    lastEmailNoticeDate = DateTime.Now,
                                    numberOfPastNoticeAttempts = 0
                                };

                        // add pending registration record
                        Database.PendingRegistrations.AddObject(registration);

                        Database.SaveChanges();

                        bool emailSent = EmailHelper.sendEmail(null, user.Email, "Pipfly Registration Confirmation", GetAccountConfirmationEmailHtmlBody(key.ToString()), "Thank you for creating your account with Pipfly. To finish the registration process, we just need you to copy and paste the following address in your web browser:\n\nhttp://www.pipfly.com/account/confirm?key=" +
                            key + "\n\nOh and did we mention you are awesome? You are awesome for choosing to use Pipfly!\n\nIf you have any questions or want to provide feedback, drop us a note at contact@pipfly.com.\n\nCheers,\nPipfly Team\n\n(c) Six Loonies Inc. 2012");

                        if (emailSent)
                        {
                            // TO DO : redirect to page saying confirmation email has been sent.
                        }
                        else
                        {
                            Database.PendingRegistrations.DeleteObject(registration);
                            Database.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: add logging
                    throw ex;
                }
            }
        }

        protected string GetAccountConfirmationEmailHtmlBody(string key)
        {
            string result = EmailHelper.RenderViewToString("ConfirmRegistration", "_EmailLayout", key, null, ControllerContext, ViewBag, ViewData, TempData);

            return result;
        }

        public ActionResult Confirm(string key)
        {
            Guid keyGuid = Guid.Empty;

            if (Guid.TryParse(key, out keyGuid))
            {
                var userId = Database.PendingRegistrations.Where(pr => pr.confirmationKey == keyGuid).FirstOrDefault();
                if (userId != null)
                {
                    var userProfile =
                        Database.UserProfiles.Where(up => up.UserId.Equals(userId.membershipUserId)).FirstOrDefault();

                    if (userProfile != null)
                    {
                        userProfile.IsConfirmed = true;
                        Database.PendingRegistrations.DeleteObject(userId);

                        Database.SaveChanges();

                        UserHelper.AddEmailConfirmedCookie();

                        SendWelcomeEmail(userProfile.FullName, userProfile.aspnet_Users.aspnet_Membership.Email);
                    }
                    else
                    {
                        // TODO: add logging
                        throw new Exception("User profile not found.");
                    }
                }
                else
                {
                    // TODO: add logging
                    throw new Exception("User not found");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Create query string based on source host and parameters
        /// </summary>
        /// <param name="source">Source Host URL</param>
        /// <param name="parameters">Query string parameters</param>
        /// <returns>Built query string</returns>
        private string CreateQueryString(string source, NameValueCollection parameters)
        {
            string result = source;
            List<String> items = new List<String>();
            foreach (String name in parameters)
            {
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name])));
            }

            result += "?" + String.Join("&", items.ToArray());

            return result;
        }

        /// <summary>
        /// Helper Method: Indicates if the user should be prompted to invite friends to Six Loonies when they log in
        /// </summary>
        /// <param name="userId">Reference to the user logged in</param>
        /// <returns>True if user should be prompted, false otherwise</returns>
        public static bool PromptUserForFriends(Guid userId)
        {
            bool result = false;
            OceanusEntities database = new OceanusEntities();

            var userProfile = database.UserProfiles.Where(u => u.UserId == userId).FirstOrDefault();

            if (userProfile != null)
            {
                result = userProfile.PromptUserForFBFriends;

                //reset flag so user is not prompted again when they login
                userProfile.PromptUserForFBFriends = false;
                database.SaveChanges();
            }

            return result;
        }

        #endregion
    }
}
