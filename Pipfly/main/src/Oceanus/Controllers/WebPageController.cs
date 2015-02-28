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
using AmazonSES;
using Oceanus.Controllers.Helpers;
using System.Web.Script.Serialization;
using Oceanus.ViewModels.Shared;

namespace Oceanus.Controllers
{
    public class WebPageController : ControllerBase
    {
        [Authorize]
        [JsonpFilter]
        public ActionResult GetTokenSearchShareLinkUsers(string q)
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            IEnumerable<UserProfile> userProfiles = Database.UserProfiles.Where(p => p.FullName.Contains(q) &&
                p.aspnet_Users.SixLooniesFriendConnections.Where(conn => (conn.FirstUserId == p.UserId && conn.SecondUserId == userGuid) || 
                    (conn.FirstUserId == userGuid && conn.SecondUserId == p.UserId)).Count() > 0).Take(5);
            List<ShareLinkRecipientViewModel> baseUsersViewModel = userProfiles.Select(p => p.ToShareLinkRecipientViewModel()).AsEnumerable().ToList();

            if (baseUsersViewModel.Count() < 5)
            {
                var remaining = 5 - baseUsersViewModel.Count();
                var foundFBUIDs = baseUsersViewModel.Where(m => m.fbuid > 0).Select(m => m.fbuid).AsEnumerable();

                IEnumerable<FacebookFriendList> fbFriendMatches = Database.FacebookFriendLists.Where(fbfriend => fbfriend.FacebookFriendName.Contains(q) && fbfriend.UserId == userGuid && !foundFBUIDs.Contains(fbfriend.FacebookFriendUID)).Take(remaining);
                var fbFriendViewModels = fbFriendMatches.Select(fbfriend => fbfriend.ToShareLinkRecipientViewModel()).AsEnumerable();
                baseUsersViewModel.AddRange(fbFriendViewModels);
            }

            baseUsersViewModel.OrderBy(matches => matches.name);

            return Json(baseUsersViewModel.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [JsonpFilter]
        public ActionResult GetTokenSearchClipTopics(string q)
        {
            IEnumerable<Tag> tagModels = Database.Tags.Where(t => t.TagName.Contains(q)).Take(10);
            IEnumerable<TokenListTopicsViewModel> tokens = tagModels.Select(t => new TokenListTopicsViewModel() { id = t.TagId, name = t.TagName });
            return Json(tokens, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetShareLinkUI(long pageId)
        {
            try
            {
                //create the URI
                WebPage pageModel = Database.WebPages.Where(webPage => webPage.Id == pageId).FirstOrDefault();

                if (pageModel != null)
                {
                    ViewBag.PageUrl = pageModel.PageUrl;
                    ViewBag.PageDescription = pageModel.PageDescription;
                    ViewBag.PageTitle = pageModel.PageTitle;
                }
            }
            catch (Exception e)
            {
            }

            return View("_ShareLink", "_BookmarkletLayout");
        }

        [Authorize]
        public ActionResult ShareLink(string recipients, string pageTitle, string pageUrl, string pageDescription, string message)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    //retrieve user profile of currently logged in user
                    var userProfile = Database.UserProfiles.Where(u => u.UserId == userGuid).FirstOrDefault();

                    WebPage pageObject = WebPageHelper.ValidateWebPage(userGuid, pageUrl, pageTitle, pageDescription, Database);

                    //create the URI
                    pageUrl = pageUrl.IndexOf('#') > 0 ? (pageUrl.Substring(0, pageUrl.IndexOf('#'))) : pageUrl;
                    Uri pageURI = new Uri(pageUrl);

                    string textMessage = "Personal Message:\n" + message + "\n\n" + pageTitle + "\n" + pageUrl + "\n\nSent using Pipfly (www.pipfly.com)";
                    string htmlMessage = EmailHelper.GetMessageEchoEmailHtmlBody(true, userProfile.FullName, userProfile.FirstName + " shared a web page with you",
                        message, -1, ControllerContext, ViewBag, ViewData, TempData);

                    JavaScriptSerializer s = new JavaScriptSerializer();
                    List<ShareLinkRecipientViewModel> recipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);

                    int numShares = 0; //keep track of number of recipients this page gets shared with

                    //get the page share count for this page by the user
                    var pageShareModel = Database.WebPageShares.Where(share => share.WebPageId == pageObject.Id && share.ContributingUserId == userGuid).FirstOrDefault();

                    if (pageShareModel == null)
                    {
                        //first time the user is sharing this page with anyone
                        pageShareModel = new WebPageShare() { WebPageShareId = 0, ContributingUserId = userGuid, WebPageId = pageObject.Id, LastShareDate = DateTime.Now, NumShares = recipientViewModels.Count };
                        Database.WebPageShares.AddObject(pageShareModel);
                        Database.SaveChanges();
                    }

                    //get recipients on pipfly
                    IEnumerable<ShareLinkRecipientViewModel> pipflyRecipients = recipientViewModels.Where(r => r.id > 0).AsEnumerable();

                    foreach (ShareLinkRecipientViewModel pipflyRecipient in pipflyRecipients)
                    {
                        var recipientUserProfile = Database.UserProfiles.Where(u => u.UserProfileId == pipflyRecipient.id).FirstOrDefault();

                        if (recipientUserProfile != null)
                        {
                            ++numShares;

                            bool emailSuccessful = EmailHelper.sendEmail(userProfile.FullName, recipientUserProfile.aspnet_Users.aspnet_Membership.LoweredEmail,
                                userProfile.FirstName + " shared a web page with you", htmlMessage, textMessage);

                            if (recipientUserProfile.FacebookUserUID != null)
                            {
                                var domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                                FacebookHelper.PostToFacebookWall((long)recipientUserProfile.FacebookUserUID, userProfile.FacebookToken, message, pageTitle, pageUrl, "", "",
                                     "http://www.sixloonies.com/images/sitetheme/SixLooniesLogo.png", null);
                            }
                        }
                    }

                    //get recipients to be notified via email
                    IEnumerable<ShareLinkRecipientViewModel> emailRecipients = recipientViewModels.Where(r => r.id == -1).AsEnumerable();
                    EmailValidationAttribute emailValidationAttr = new EmailValidationAttribute();

                    foreach (ShareLinkRecipientViewModel emailRecipient in emailRecipients)
                    {
                        string emailAddress = emailRecipient.name;

                        if (emailValidationAttr.IsValid(emailAddress))
                        {
                            ++numShares;

                            EmailHelper.sendEmail(null, emailAddress, userProfile.FirstName + " shared a web page with you", htmlMessage, textMessage);
                        }
                    }

                    //check for share count consistency
                    if (numShares != recipientViewModels.Count)
                    {
                        //adjust the number of times this page was shared
                        pageShareModel.NumShares = numShares;
                        Database.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookmarkletPageStats(long pageId)
        {
            WebPage pageModel = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();
            WebPageViewModel pageViewModel = null;

            if (pageModel != null)
            {
                pageViewModel = pageModel.ToWebPageViewModel(Database);
            }

            return PartialView("_BookmarkletWebPageStats", pageViewModel);
        }

        [Authorize]
        public ActionResult BoomarkletDashboard(string pageUrl, string pageTitle, string pageDescription)
        {
            WebPageViewModel pageViewModel = null;
            MembershipUser loggedinuser = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)loggedinuser.ProviderUserKey;

            //create the URI
            pageUrl = pageUrl.IndexOf('#') > 0 ? (pageUrl.Substring(0, pageUrl.IndexOf('#'))) : pageUrl;
            Uri pageURI = new Uri(pageUrl);

            //set viewbag params
            ViewBag.LoggedInUserId = loggedInUserGuid;
            ViewBag.PageUrl = pageURI.AbsoluteUri.ToLower();
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = pageDescription;

            //check if this page has been added in the DB
            var pageObject = Database.WebPages.Where(page => page.LowerPageUrl.Equals((pageURI.AbsoluteUri.ToLower()))).FirstOrDefault();

            if (pageObject == null)
            {
                //first time activity on the page
                pageViewModel = new WebPageViewModel(pageTitle.Trim(), pageURI.Scheme + "://" + pageURI.DnsSafeHost +
                    (pageURI.IsDefaultPort ? "" : (":" + pageURI.Port)), pageURI.AbsoluteUri, pageDescription, loggedInUserGuid, DateTime.Now);
                pageObject = pageViewModel.ToWebPageModel();

                Database.WebPages.AddObject(pageObject);
                Database.SaveChanges();

                pageViewModel.WebPageId = pageObject.Id;
            }

            if (pageObject != null && pageViewModel == null)
            {
                pageViewModel = pageObject.ToWebPageViewModel(Database);
            }

            return View("_BookmarkletPage", "_BookmarkletLayout", pageViewModel);
        }

        [JsonpFilter]
        [Authorize]
        public JsonResult LikePage(long pageId)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    WebPage pageModel = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();

                    if (pageModel != null)
                    {
                        //has the user already liked this page?
                        var userLikeObject = Database.UserWebPageLikes.Where(u => u.ContributingUserId == userGuid && u.WebPageId == pageModel.Id).FirstOrDefault();

                        if (userLikeObject == null)
                        {
                            //user has not liked this page before
                            UserWebPageLikesViewModel likesViewModel = new UserWebPageLikesViewModel(null, userGuid, DateTime.Now, pageModel.Id);
                            UserWebPageLike likeModel = likesViewModel.ToUserLikeModel();

                            Database.UserWebPageLikes.AddObject(likeModel);
                            Database.SaveChanges();
                            successful = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [JsonpFilter]
        [Authorize]
        public JsonResult UnlikePage(long pageId)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    WebPage pageModel = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();

                    if (pageModel != null)
                    {
                        //has the user already liked this page?
                        var userLikeObject = Database.UserWebPageLikes.Where(u => u.ContributingUserId == userGuid && u.WebPageId == pageModel.Id).FirstOrDefault();

                        if (userLikeObject != null)
                        {
                            Database.UserWebPageLikes.DeleteObject(userLikeObject);
                            Database.SaveChanges();
                            successful = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [JsonpFilter]
        public JsonResult IsWebPageLiked(long pageId)
        {
            bool result = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    WebPage pageModel = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();

                    if (pageModel != null)
                    {
                        //has the user liked this page?
                        var userLikeObject = Database.UserWebPageLikes.Where(u => u.ContributingUserId == userGuid && u.WebPageId == pageModel.Id).FirstOrDefault();

                        if (userLikeObject != null)
                        {
                            //user has liked this page
                            result = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = result.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }
    }
}
