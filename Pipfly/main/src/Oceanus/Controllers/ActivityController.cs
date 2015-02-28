using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Web.Security;

namespace Oceanus.Controllers
{
    public class ActivityController : ControllerBase
    {
        //
        // GET: /Activity/

        public ActionResult GetUserWebPosts(Guid userId)
        {
            IEnumerable<UserWallPost> userWallPosts = Database.UserWallPosts.Where(p => p.ContributingUserId == userId).AsEnumerable().OrderByDescending(p => p.ContributionDate);
            IEnumerable<WebPostViewModel> userWallPostsViewModel = userWallPosts.Select(p => p.ToWebPostViewModel(Database));

            ViewBag.UserId = userId;

            return PartialView("_UserWebPosts", userWallPostsViewModel);
        }

        public ActionResult GetTopicPosts(Guid userId, int topicId)
        {
            IEnumerable<UserWallPost> userWallPosts = Database.UserWallPosts.Where(post => post.WebPage.WebPageTagMappings.Select(map => map.TagId == topicId).Count() > 0).AsEnumerable();
            IEnumerable<WebPostViewModel> topicWallPostsViewModel = userWallPosts.Select(post => post.ToWebPostViewModel(Database));

            ViewBag.UserId = userId;

            return PartialView("_TopicPostsContainer", topicWallPostsViewModel);
        }

        public ActionResult GetBookmarkletUserWallPosts(long pageId)
        {
            IEnumerable<BasePostViewModel> userWallPostsViewModel = null;

            //check if this page exists
            var pageObject = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();

            if (pageObject != null)
            {
                IEnumerable<UserWallPost> userWallPosts = Database.UserWallPosts.Where(p => p.WebPageId == pageObject.Id).AsEnumerable().OrderByDescending(p => p.ContributionDate);
                userWallPostsViewModel = userWallPosts.Select(p => p.ToBasePostViewModel());
            }

            ViewBag.PageId = pageId;

            return PartialView("_BookmarkletPostsContainer", userWallPostsViewModel);
        }

        public ActionResult GetWallPostComments(int wallPostId)
        {
            IEnumerable<UserWallPostComment> commentModels = Database.UserWallPostComments.Where(c => c.UserWallPostId == wallPostId).AsEnumerable();
            IEnumerable<UserWallPostCommentViewModel> commentsViewModels = commentModels.Select(c => c.ToUserPostCommentViewModel());

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (user != null) ? ((Guid)user.ProviderUserKey) : ((Guid?)null);

            ViewBag.WallPostId = wallPostId;

            if (loggedInUserGuid != null)
            {
                UserProfile loggedInUserProfile = Database.UserProfiles.Where(u => u.UserId == ((Guid)loggedInUserGuid)).FirstOrDefault();
                if (loggedInUserProfile != null)
                {
                    ViewBag.LoggedInUserProfileViewModel = loggedInUserProfile.ToBaseUserViewModel();
                }
            }

            return PartialView("_WallPostComments", commentsViewModels);
        }

        public ActionResult GetUserWall(Guid userId)
        {
            ViewBag.UserId = userId;

            return PartialView("_UserProfileWebPostContainer");
        }

        [Authorize]
        public ActionResult AddWallTextPost(string postText, long pageId)
        {
            string dbMessage = string.Empty;
            bool success = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            WebPage pageModel = Database.WebPages.Where(page => page.Id == pageId).FirstOrDefault();

            BasePostViewModel BasePostViewModel = new BasePostViewModel(null, postText, loggedInUserGuid, DateTime.Now, null, pageModel.Id);
            UserWallPost wallPost = BasePostViewModel.ToUserPost();

            Database.UserWallPosts.AddObject(wallPost);
            Database.SaveChanges();

            return Json(
                new
                {
                    result = success.ToString(),
                    message = dbMessage
                }
            );
        }

        [Authorize]
        public ActionResult AddWallTextPostComment(int wallPostId, string commentText)
        {
            string dbMessage = string.Empty;
            bool success = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            UserWallPostCommentViewModel wallPostCommentViewModel = new UserWallPostCommentViewModel(null, wallPostId, commentText, loggedInUserGuid, DateTime.Now, null);
            UserWallPostComment wallPostComment = wallPostCommentViewModel.ToUserPostComment();

            Database.UserWallPostComments.AddObject(wallPostComment);
            Database.SaveChanges();

            return Json(
                new
                {
                    result = success.ToString(),
                    message = dbMessage
                }
            );
        }
    }
}
