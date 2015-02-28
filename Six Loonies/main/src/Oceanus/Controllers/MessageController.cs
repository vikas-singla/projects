using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Oceanus.ViewModels;
using Oceanus.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace Oceanus.Controllers
{
    [BrowserCache(PreventBrowserCaching = true)]
    public class MessageController : ControllerBase
    {
        public ActionResult Index()
        {
            return View("_MessageListing");
        }

        [Authorize]
        public ActionResult ForwardFriendIntroductionMessage(int friendIntroRequestId, string messageToTargetUser)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            var friendIntroRequest = Database.FriendIntroductionRequests.Where(r => r.FriendIntroRequestId == friendIntroRequestId).FirstOrDefault();

            if (friendIntroRequest != null)
            { 
                //verify the logged in user is the connecting friend for the friend intro request
                if (friendIntroRequest.ConnectingFriendUserId == loggedInUserGuid)
                {
                    friendIntroRequest.ConnectingUserMessageToTargetUser = messageToTargetUser;
                    friendIntroRequest.IntroductionForwardedToTargetUser = true;
                    Database.SaveChanges();
                }
            }

            return Json(new { result = successful.ToString(), message = dbMessage }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AcceptFriendIntroductionMessage(int friendIntroRequestId, string messageToInitUser)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            var friendIntroRequest = Database.FriendIntroductionRequests.Where(r => r.FriendIntroRequestId == friendIntroRequestId).FirstOrDefault();

            if (friendIntroRequest != null)
            {
                //verify the logged in user is the target user for the friend intro request
                if (friendIntroRequest.TargetUserId == loggedInUserGuid)
                {
                    friendIntroRequest.TargetUserMessageToInitUser = messageToInitUser;
                    friendIntroRequest.TargetUserHasAccepted = true;
                    Database.SaveChanges();
                }
            }

            return Json(new { result = successful.ToString(), message = dbMessage }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SendFriendIntroductionMessage(Guid firstDegUID, Guid secondDegUID, string subject, string messageToFirstDegUser, string messageToSecondDegUser)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            var existingFriendIntroRequest = Database.FriendIntroductionRequests.Where(f => f.InitiatingUserId == loggedInUserGuid && f.ConnectingFriendUserId == firstDegUID &&
                f.TargetUserId == secondDegUID).FirstOrDefault();

            //verify that the user doesn't already have a friend introduction request
            if (existingFriendIntroRequest == null)
            {
                BaseFriendIntroRequestViewModel introRequestViewModel = new BaseFriendIntroRequestViewModel(null, subject, DateTime.Now, loggedInUserGuid, firstDegUID, secondDegUID,
                    messageToFirstDegUser, false, messageToSecondDegUser, string.Empty, false, DateTime.Now, false, false, string.Empty, false);
                FriendIntroductionRequest requestModel = introRequestViewModel.ToFriendIntroRequest();

                Database.FriendIntroductionRequests.AddObject(requestModel);
                Database.SaveChanges();
            }

            return Json(new { result = successful.ToString(), message = dbMessage }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetAskForIntroductionUI(Guid firstDegUID, Guid secondDegUID)
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            var fromUserProfile = Database.UserProfiles.Where(u => u.UserId == userGuid).FirstOrDefault();
            var connectingFriendUserProfile = Database.UserProfiles.Where(u => u.UserId == firstDegUID).FirstOrDefault();
            var targetUserProfile = Database.UserProfiles.Where(u => u.UserId == secondDegUID).FirstOrDefault();

            UserProfileViewModel fromUserProfileViewModel = fromUserProfile.ToUserProfileViewModel();
            UserProfileViewModel connectingFriendUserProfileViewModel = connectingFriendUserProfile.ToUserProfileViewModel();
            UserProfileViewModel targetUserProfileViewModel = targetUserProfile.ToUserProfileViewModel();

            NetworkConnectionPathViewModel networkPathViewModel = new NetworkConnectionPathViewModel(userGuid, secondDegUID, targetUserProfileViewModel, 2, userGuid.ToString() + "." +
                firstDegUID.ToString() + "." + secondDegUID.ToString());
            networkPathViewModel.FromUserProfile = fromUserProfileViewModel;
            networkPathViewModel.ConnectingFriendUserProfile = connectingFriendUserProfileViewModel;
            networkPathViewModel.ConnectingFriendName = connectingFriendUserProfileViewModel.FullName;

            return PartialView("_AskForIntroduction", networkPathViewModel);
        }

        [Authorize]
        public ActionResult GetPredefinedWriteMessageUI(Guid recipientUID)
        {
            var userProfile = Database.UserProfiles.Where(u => u.UserId == recipientUID).FirstOrDefault();

            if (userProfile != null)
            {
                ViewBag.RecipientId = recipientUID;
                ViewBag.RecipientName = userProfile.FullName;
            }

            return PartialView("_WriteMessagePredefinedRecipient");
        }

        public ActionResult GetVendorQuoteMessageForm(int vendorId)
        {
            ViewBag.VendorId = vendorId;

            return PartialView("_GetVendorQuoteMessage");
        }

        public ActionResult GetMessageParticipants(int messageId)
        {
            var messageParticipants = Database.MessageVisibilities.Where(mv => mv.MessageId == messageId).AsEnumerable();
            IEnumerable<MessageVisibilityViewModel> messageParticipantsViewModel = messageParticipants.Select(mv => mv.ToMessageVisibilityViewModel());

            return PartialView("_MessageParticipants", messageParticipantsViewModel);
        }

        [Authorize]
        public ActionResult GetFriendIntroductionListingsForUser()
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;
            var friendIntroRequests = Database.FriendIntroductionRequests.Where(r => (r.ConnectingFriendUserId == userGuid || (r.TargetUserId == userGuid && r.IntroductionForwardedToTargetUser) ||
                (r.TargetUserHasAccepted && r.InitiatingUserId == userGuid))).AsEnumerable();

            IEnumerable<BaseFriendIntroRequestViewModel> viewModels = friendIntroRequests.Select(r => r.ToBaseFriendIntroRequestViewModel(Database));

            ViewBag.UserId = userGuid;

            return PartialView("_FriendIntroductionRequestListing", viewModels);
        }

        [Authorize]
        public ActionResult GetMessageListingsForUser(Guid user)
        {
            var messageVisibilities = Database.MessageVisibilities.Where(mv => mv.UserId == user).AsEnumerable();
            List<BaseMessageViewModel> messages = new List<BaseMessageViewModel>();

            foreach (var messageVisiblity in messageVisibilities)
            {
                var messageModel = messageVisiblity.Message.ToBaseMessageViewModel();

                if (messageVisiblity.MessageIsUnread)
                {
                    messageModel.MessageIsUnreadForCurrentUser = true;
                }

                messages.Add(messageModel);
            }

            messages.OrderBy(m => m.LastUpdatedOn);

            return PartialView("_MessageListing", messages.AsEnumerable());
        }

        [Authorize]
        public ActionResult GetFriendIntroductionRequestDetails(int introRequestId)
        {
            FriendIntroductionRequest introRequest = null;
            FriendIntroRequestViewModel introRequestViewModel = null;
        
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            ViewBag.UserId = userGuid;

            introRequest = Database.FriendIntroductionRequests.Where(f => f.ConnectingFriendUserId == userGuid || (f.TargetUserId == userGuid && f.IntroductionForwardedToTargetUser) ||
                (f.TargetUserHasAccepted && f.InitiatingUserId == userGuid)).FirstOrDefault();

            if (introRequest != null)
            {
                introRequestViewModel = introRequest.ToFriendIntroRequestViewModel(Database);

                //mark request as unread
                if (introRequest.ConnectingFriendUserId == userGuid)
                {
                    introRequest.ConnectingFriendHasReadMessage = true;
                }
                else if(introRequest.TargetUserId == userGuid)
                {
                    introRequest.TargetUserHasReadMessage = true;
                }
                else if (introRequest.TargetUserHasAccepted && introRequest.InitiatingUserId == userGuid)
                {
                    introRequest.InitUserHasReadTargetUserAcceptance = true;
                }
                    
                Database.SaveChanges();

                ViewBag.NotAuthorized = false;
            }
            else
            {
                ViewBag.NotAuthorized = true;
            }

            return View("_FriendIntroductionRequestDetails", introRequestViewModel);
        }

        public ActionResult GetMessageDetails(int messageId)
        {
            Message message = null;
            MessageViewModel messageViewModel = null;

            //prevent people who are not message participants from viewing the message
            if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                ViewBag.UserId = userGuid;

                var msgVisibility = Database.MessageVisibilities.Where(mv => mv.MessageId == messageId && mv.UserId == userGuid).FirstOrDefault();

                if (msgVisibility != null)
                {
                    message = Database.Messages.Where(m => m.MessageId == messageId).FirstOrDefault();

                    //mark message as unread
                    msgVisibility.MessageIsUnread = false;
                    Database.SaveChanges();

                    ViewBag.NotAuthorized = false;
                }
                else
                {
                    ViewBag.NotAuthorized = true;
                }
            }
            else
            {
                ViewBag.NotAuthorized = true;
                return RedirectToAction("SignInFormWithReturn", "Account", new { returnUrl = ("/message/" + messageId)  });
            }

            if (message != null)
            {
                messageViewModel = message.ToMessageViewModel();
            }

            return View("_MessageDetails", messageViewModel);
        }

        [Authorize]
        public ActionResult AddMessageReply(int messageId, string messageBody)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    //create response associated with message (i.e. user response)
                    MessageResponsesViewModel responseViewModel = new MessageResponsesViewModel(null, messageId, messageBody, DateTime.Now, userGuid);
                    MessageResponses responseModel = responseViewModel.ToMessageResponsesModel();

                    Database.MessageResponses.AddObject(responseModel);

                    //mark message to unread for all users except for the one who just replied
                    var messageVisibilities = Database.MessageVisibilities.Where(mv => mv.MessageId == messageId).AsEnumerable();
                    foreach (var messageVisibility in messageVisibilities)
                    {
                        if (messageVisibility.UserId != userGuid)
                        {
                            messageVisibility.MessageIsUnread = true;
                        }
                    }

                    Database.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString(), message = dbMessage }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddMessage(string messageTo, string messageSubject, string messageBody)
        {
            string dbMessage = string.Empty;
            bool successful = false;

            MessageHelper.CreateMessage(messageTo, messageSubject, messageBody, User, out dbMessage, out successful, Database, ControllerContext, ViewBag, ViewData, TempData);

            return Json(new { result = successful.ToString(), message = dbMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsersAutoComplete(string searchText, int maxResults)
        {
            IEnumerable<UserProfile> userProfiles = Database.UserProfiles.Where(p => p.FullName.Contains(searchText)).Take(maxResults);
            IEnumerable<BaseUserViewModel> baseUsersViewModel = userProfiles.Select(p => p.ToBaseUserViewModel());
            return Json(baseUsersViewModel, JsonRequestBehavior.AllowGet);
        }

        public class MessageToJson
        {
            /// <summary>
            /// 
            /// </summary>
            public string FullName;
            /// <summary>
            /// 
            /// </summary>
            public string UserId;
        }
    }
}
