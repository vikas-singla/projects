using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.ViewModels.Shared;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Web.Security;
using System.Linq.Expressions;
using MVCControlsToolkit.Linq;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace Oceanus.Controllers
{
    public class ArticleController : ControllerBase
    {
        public enum ArticleTypes
        {
            All = 1,
            News,
            Facts,
            Opinion,
            HowTo,
            Other
        };

        internal const int MAX_INITIAL_ARTICLE_LOAD_SET = 10;

        public ActionResult Index(long id)
        {
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            Guid? currUserGuid = (currUserMembership != null ? (Guid?)currUserMembership.ProviderUserKey : null);

            var article = Database.ArticleClips.Where(clip => clip.ArticleId == id).FirstOrDefault();
            bool currUserLikes = (currUserGuid == null ? false : Database.UserArticleClipLikes.Where(like => like.ArticleClipId == id && like.ContributingUserId == currUserGuid).Count() > 0);

            ViewBag.CurrUserLikes = currUserLikes;

            return View("Index", article == null ? null : article.ToArticleViewModel(Database, currUserMembership));
        }

        [Authorize]
        public ActionResult ShareArticleOverlay(long id)
        {
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            var article = Database.ArticleClips.Where(clip => clip.ArticleId == id).FirstOrDefault();
            return PartialView("_ShareArticle", article.ToArticleViewModel(Database, currUserMembership));
        }

        public ActionResult GetTopicArticles(int topic_id, ArticleController.ArticleTypes articleType, bool filterByFriends, long? since_date, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            Tag tag = Database.Tags.Where(t => t.TagId == topic_id).FirstOrDefault();
            IEnumerable<ArticleViewModel> articleViewModels = null;
            List<Guid> friendConnectionsList = null;
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);

            if (tag != null)
            {
                //compile a list of friends for currently logged in user
                if (filterByFriends && Request.IsAuthenticated)
                {
                    Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;

                    friendConnectionsList = new List<Guid>();

                    var userSixLooniesFriendListModel = Database.SixLooniesFriendConnections.Where(f => (f.FirstUserId == currUserGuid || f.SecondUserId == currUserGuid)).AsEnumerable();

                    foreach (var friend in userSixLooniesFriendListModel)
                    {
                        Guid friendGuid = (friend.FirstUserId == currUserGuid) ? friend.SecondUserId : friend.FirstUserId;
                        if (!friendConnectionsList.Contains(friendGuid))
                        {
                            friendConnectionsList.Add(friendGuid);
                        }
                    }
                }

                int toSkip = (page - 1) * MAX_INITIAL_ARTICLE_LOAD_SET;
                IQueryable<ArticleClipTagMapping> tagMappingsQueryable = tag.ArticleClipTagMappings.Where(map => map.ArticleClip.IsPublished).AsQueryable();

                if (friendConnectionsList != null)
                {
                    tagMappingsQueryable = tagMappingsQueryable.Where(map => friendConnectionsList.Contains(map.ArticleClip.ContributingUserId)).AsQueryable();
                }

                if (articleType != ArticleTypes.All)
                {
                    string articleTypeFilter = articleType.ToString();
                    tagMappingsQueryable = tagMappingsQueryable.Where(map => map.ArticleClip.ArticleType == articleTypeFilter);
                }

                if (since_date != null)
                {
                    DateTime since_datetime = new DateTime((long)since_date);
                    tagMappingsQueryable.Where(map => map.ContributionDate < since_datetime);
                }

                if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
                {
                    tagMappingsQueryable = tagMappingsQueryable.OrderBy(map => map.ContributionDate);
                }
                else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
                {
                    tagMappingsQueryable = tagMappingsQueryable.OrderByDescending(map => (map.ArticleClip.ArticleClipUserMappings.Count + map.ArticleClip.UserArticleClipLikes.Count + map.ArticleClip.ArticleComments.Count));
                }

                var tagMappings = tagMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_ARTICLE_LOAD_SET).AsEnumerable();
                articleViewModels = tagMappings.Select(map => map.ArticleClip.ToArticleViewModel(Database, Membership.GetUser(User.Identity.Name)));
            }

            ViewBag.TopicId = topic_id;
            return PartialView("_ArticlesCollage", articleViewModels);
        }

        public ActionResult GetUserArticles(int user_id, long? since_date, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            IEnumerable<ArticleViewModel> articles = null;
            var userProfile = Database.UserProfiles.Where(p => p.UserProfileId == user_id).FirstOrDefault();

            if (userProfile != null)
            {
                int toSkip = (page - 1) * MAX_INITIAL_ARTICLE_LOAD_SET;

                var userMappingsQueryable = Database.ArticleClipUserMappings.Where(map => map.ArticleClip.IsPublished && map.UserId == userProfile.UserId);

                if (since_date != null)
                {
                    DateTime since_datetime = new DateTime((long)since_date);
                    userMappingsQueryable.Where(map => map.ContributionDate < since_datetime);
                }

                if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
                {
                    userMappingsQueryable = userMappingsQueryable.OrderBy(map => map.ContributionDate);
                }
                else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
                {
                    userMappingsQueryable = userMappingsQueryable.OrderByDescending(map => (map.ArticleClip.ArticleClipUserMappings.Count + map.ArticleClip.UserArticleClipLikes.Count + map.ArticleClip.ArticleComments.Count));
                }

                var userMappings = userMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_ARTICLE_LOAD_SET).AsEnumerable();

                articles = userMappings.Select(map => map.ArticleClip.ToArticleViewModel(Database, Membership.GetUser(User.Identity.Name)));
            }

            return PartialView("_ArticlesCollage", articles);
        }

        public ActionResult Search(string q, long? since_date, ArticleController.ArticleTypes articleType, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            IEnumerable<ArticleViewModel> articles = null;
            q = q.ToLower().Trim();

            int toSkip = (page - 1) * MAX_INITIAL_ARTICLE_LOAD_SET;

            var articlesQueryable = Database.ArticleClips.Where(clip => clip.IsPublished && clip.ArticleText.Contains(q));

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                articlesQueryable.Where(clip => clip.ContributionDate < since_datetime);
            }

            if (articleType != ArticleTypes.All)
            {
                string articleTypeFilter = articleType.ToString();
                articlesQueryable = articlesQueryable.Where(clip => clip.ArticleType == articleTypeFilter);
            }

            if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
            {
                articlesQueryable = articlesQueryable.OrderBy(clip => clip.ContributionDate);
            }
            else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
            {
                articlesQueryable = articlesQueryable.OrderByDescending(clip => (clip.ArticleClipUserMappings.Count + clip.UserArticleClipLikes.Count + clip.ArticleComments.Count));
            }

            articles = articlesQueryable.Skip(toSkip).Take(MAX_INITIAL_ARTICLE_LOAD_SET).AsEnumerable().Select(clip => clip.ToArticleViewModel(Database, Membership.GetUser(User.Identity.Name)));

            return PartialView("_ArticlesCollage", articles);
        }

        [Authorize]
        public ActionResult Spotlight(long? since_date, ArticleController.ArticleTypes articleType, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;
            IEnumerable<ArticleViewModel> articles = null;

            int toSkip = (page - 1) * MAX_INITIAL_ARTICLE_LOAD_SET;

            var articlesQueryable = Database.ArticleClips.Where(clip => clip.ArticleClipUserMappings.Where(userMap => userMap.aspnet_Users.UserToUserFollowingMappings.Where(map => map.FollowingUserId == userGuid).Count() > 0).Count() > 0 ||
                    clip.ArticleClipTagMappings.Where(tagMap => tagMap.Tag.UserToTagFollowingMappings.Where(userTagMap => userTagMap.FollowingUserId == userGuid).Count() > 0).Count() > 0).AsQueryable();

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                articlesQueryable.Where(clip => clip.ContributionDate < since_datetime);
            }

            if (articleType != ArticleTypes.All)
            {
                string articleTypeFilter = articleType.ToString();
                articlesQueryable = articlesQueryable.Where(clip => clip.ArticleType == articleTypeFilter);
            }

            if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
            {
                articlesQueryable = articlesQueryable.OrderBy(clip => clip.ContributionDate);
            }
            else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
            {
                articlesQueryable = articlesQueryable.OrderByDescending(clip => (clip.ArticleClipUserMappings.Count + clip.UserArticleClipLikes.Count + clip.ArticleComments.Count));
            }

            articles = articlesQueryable.Skip(toSkip).Take(MAX_INITIAL_ARTICLE_LOAD_SET).AsEnumerable().Select(clip => clip.ToArticleViewModel(Database, Membership.GetUser(User.Identity.Name)));

            return PartialView("_ArticlesCollage", articles);
        }

        public ActionResult GetArticleLikes(long articleId)
        {
            var userLikes = Database.UserArticleClipLikes.Where(like => like.ArticleClipId == articleId).AsEnumerable();
            var userLikesViewModel = userLikes.Select(like => like.aspnet_Users.UserProfiles.First().ToBaseUserViewModel());

            return PartialView("_ArticleLikes", userLikesViewModel);
        }

        public ActionResult GetArticleComments(long articleId)
        {
            ViewBag.ArticleId = articleId;
            return PartialView("_ArticleCommentsContainer");
        }

        public ActionResult GetArticleCommentPosts(long articleId)
        {
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            Guid? currUserGuid = (currUserMembership != null ? (Guid?)currUserMembership.ProviderUserKey : null);

            IEnumerable<ArticleComment> userWallPosts = Database.ArticleComments.Where(comment => comment.ArticleClipId == articleId).AsEnumerable().OrderByDescending(p => p.ContributionDate);
            IEnumerable<ArticleCommentViewModel> userWallPostsViewModel = userWallPosts.Select(p => p.ToArticleCommentViewModel());

            ViewBag.UserId = currUserGuid;

            return PartialView("_ArticleCommentPosts", userWallPostsViewModel);
        }

        [Authorize]
        public ActionResult LikeArticle(long articleId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserArticleClipLikes.Where(like => like.ContributingUserId == userGuid && like.ArticleClipId == articleId).FirstOrDefault();

                if (existingLike == null)
                {
                    Database.UserArticleClipLikes.AddObject(new UserArticleClipLike() { ArticleClipId = articleId, ContributingUserId = userGuid, AddedOn = DateTime.Now });
                    Database.SaveChanges();
                    successful = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString() });
        }

        [Authorize]
        public ActionResult UnikeArticle(long articleId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserArticleClipLikes.Where(like => like.ContributingUserId == userGuid && like.ArticleClipId == articleId).FirstOrDefault();

                if (existingLike != null)
                {
                    Database.UserArticleClipLikes.DeleteObject(existingLike);
                    Database.SaveChanges();
                    successful = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { result = successful.ToString() });
        }

        [Authorize]
        public ActionResult AddArticleComment(string commentText, long articleId)
        {
            string dbMessage = string.Empty;
            bool success = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            ArticleCommentViewModel commentsViewModel = new ArticleCommentViewModel(null, commentText.Trim(), loggedInUserGuid, DateTime.Now, articleId);
            ArticleComment comment = commentsViewModel.ToArticleComment();

            Database.ArticleComments.AddObject(comment);
            Database.SaveChanges();

            return Json(
                new
                {
                    result = success.ToString(),
                    message = dbMessage
                }
            );
        }

        public ActionResult PublishArticleClip(long articleId, string articleType, string articleDesc, string recipients)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    //find tags in the description
                    string regExHashTagPattern = "(^|[^0-9A-Z&/]+)(#|\uFF03)([0-9A-Z_]*[A-Z_]+[a-z0-9_\\u00c0-\\u00d6\\u00d8-\\u00f6\\u00f8-\\u00ff]*)";
                    var matchedTags = Regex.Matches(articleDesc, regExHashTagPattern, RegexOptions.IgnoreCase);

                    ArticleClip articleModel = Database.ArticleClips.Where(clip => clip.ArticleId == articleId).FirstOrDefault();

                    if (articleModel != null)
                    {
                        //update the clip type
                        if ((articleModel.ArticleType == null || articleModel.ArticleType.Trim().Equals(string.Empty)) && !articleModel.IsPublished)
                        {
                            articleType = articleType.Trim().ToLower();

                            if (articleType.Equals(ArticleController.ArticleTypes.Facts.ToString().ToLower()))
                            {
                                articleType = ArticleController.ArticleTypes.Facts.ToString();
                            }
                            else if (articleType.Equals(ArticleController.ArticleTypes.News.ToString().ToLower()))
                            {
                                articleType = ArticleController.ArticleTypes.News.ToString();
                            }
                            else if (articleType.Equals(ArticleController.ArticleTypes.HowTo.ToString().ToLower()))
                            {
                                articleType = ArticleController.ArticleTypes.HowTo.ToString();
                            }
                            else if (articleType.Equals(ArticleController.ArticleTypes.Opinion.ToString().ToLower()))
                            {
                                articleType = ArticleController.ArticleTypes.Opinion.ToString();
                            }
                            else if (articleType.Equals(ArticleController.ArticleTypes.Other.ToString().ToLower()))
                            {
                                articleType = ArticleController.ArticleTypes.Other.ToString();
                            }
                            else
                            {
                                articleType = null;
                            }

                            ArticleViewModel.SetArticleType(articleModel, articleType);
                            articleModel.IsPublished = true;

                        }

                        //add the comments
                        ArticleCommentViewModel commentViewModel = new ArticleCommentViewModel(null, articleDesc, userGuid, DateTime.Now, articleModel.ArticleId);
                        Database.ArticleComments.AddObject(commentViewModel.ToArticleComment());

                        Database.SaveChanges();
                    }

                    MapUserToArticleClip(articleModel, userGuid);
                    MapTagsToArticleClip(articleModel, matchedTags, userGuid);
                    successful = true;

                    //de-serialize recipients
                    JavaScriptSerializer s = new JavaScriptSerializer();
                    List<ShareLinkRecipientViewModel> recipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);

                    //are there any recipients for this article
                    if (recipientViewModels.Count > 0)
                    {
                        //retrieve user profile of currently logged in user
                        var userProfile = Database.UserProfiles.Where(u => u.UserId == userGuid).FirstOrDefault();

                        PostOrEmailArticle(recipientViewModels, userProfile, articleModel, articleDesc);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        private bool PostOrEmailArticle(List<ShareLinkRecipientViewModel> recipientViewModels, UserProfile userProfile, ArticleClip articleModel, string comments)
        {
            bool successful = true;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);

                ViewBag.UserFullName = userProfile.FullName;
                ViewBag.UserId = userProfile.UserProfileId;

                string htmlMessage = EmailHelper.RenderViewToString("_EmailArticleTemplate", "_EmailLayout", articleModel.ToArticleViewModel(Database, user), ControllerContext, ViewBag, ViewData, TempData);
                string textMessage = userProfile.FirstName + " shared an article that you can view here: www.pipfly.com/a/" + articleModel.ArticleId
                                            + "\n\n-----\nShared via Pipfly (www.pipfly.com)\n\n(c) Six Loonies Inc. 2012";

                //get recipients on facebook
                IEnumerable<ShareLinkRecipientViewModel> fbRecipients = recipientViewModels.Where(r => r.fbuid > 0).AsEnumerable();
                foreach (ShareLinkRecipientViewModel facebookRecipient in fbRecipients)
                {
                    var friendConnection = userProfile.aspnet_Users.FacebookFriendLists.Where(friend => friend.FacebookFriendUID == facebookRecipient.fbuid).FirstOrDefault();

                    if (friendConnection != null)
                    {
                        if (friendConnection.FacebookFriendUID > 0)
                        {
                            bool postedOnFB = true;

                            var domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                            postedOnFB = FacebookHelper.PostToFacebookWall((long)friendConnection.FacebookFriendUID, userProfile.FacebookToken, comments, "" +
                                ((articleModel.ArticleTitle != null && articleModel.ArticleTitle.Trim().Length > 0) ? (articleModel.ArticleTitle.Trim()) : ""), "www.pipfly.com/a/" + articleModel.ArticleId,
                                articleModel.WebPage.WebDomainUrl, null, "http://www.pipfly.com/images/sitetheme/Pipfly_logo_sm.png.png", null);

                            if (postedOnFB)
                            {
                            }
                        }
                    }
                }

                //get recipients to be notified via email
                IEnumerable<ShareLinkRecipientViewModel> emailRecipients = recipientViewModels.Where(r => r.email != null && r.email != string.Empty).AsEnumerable();
                EmailValidationAttribute emailValidationAttr = new EmailValidationAttribute();

                foreach (ShareLinkRecipientViewModel emailRecipient in emailRecipients)
                {
                    string emailAddress = emailRecipient.name;

                    if (emailValidationAttr.IsValid(emailAddress))
                    {
                        bool emailSent = EmailHelper.sendEmail(null, emailAddress, userProfile.FirstName + " shared a web page with you", htmlMessage, textMessage);

                        if (!emailSent)
                        {
                            successful = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                successful = false;
            }

            return successful;
        }

        [ValidateInput(false)]
        public ActionResult ClipArticleForm(string pageUrl, string pageTitle, string pageDescription, string articleMarkup, string shortArticleMarkup, string articleText, string articleTitle)
        {
            bool successful = false;
            ArticleViewModel clipArticleViewModel = null;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    WebPage pageObject = WebPageHelper.ValidateWebPage(userGuid, pageUrl, pageTitle, pageDescription, Database);

                    articleMarkup = articleMarkup.Trim();
                    articleMarkup = articleMarkup.Replace('\n', ' ');
                    articleMarkup = articleMarkup.Replace('\t', ' ');
                    articleMarkup = articleMarkup.Replace('\r', ' ');

                    //remove double spaces
                    articleMarkup = articleMarkup.Replace("  ", " ");

                    if (shortArticleMarkup != null)
                    {
                        shortArticleMarkup = shortArticleMarkup.Trim();
                        shortArticleMarkup = shortArticleMarkup.Replace("  ", " ");
                        shortArticleMarkup = shortArticleMarkup.Replace("\n", "");
                        shortArticleMarkup = shortArticleMarkup.Replace('\t', ' ');
                        shortArticleMarkup = shortArticleMarkup.Replace('\r', ' ');
                        shortArticleMarkup = (shortArticleMarkup.Equals(string.Empty) ? null : shortArticleMarkup);
                    }

                    if (pageObject != null)
                    {
                        //check if article is already clipped
                        ArticleClip existingArticle = Database.ArticleClips.Where(a => a.WebPageId == pageObject.Id && a.ArticleMarkup == articleMarkup).FirstOrDefault();

                        if (existingArticle == null)
                        {
                            //create a new article
                            clipArticleViewModel = new ArticleViewModel(null, pageObject.Id, articleMarkup, shortArticleMarkup, articleTitle, articleText, string.Empty, false, userGuid, DateTime.Now, false);
                            ArticleClip clipArticleModel = clipArticleViewModel.ToArticleModel();

                            Database.ArticleClips.AddObject(clipArticleModel);
                            Database.SaveChanges();

                            clipArticleViewModel = clipArticleModel.ToArticleViewModel(Database, user);

                            successful = true;
                        }
                        else
                        {
                            clipArticleViewModel = existingArticle.ToArticleViewModel(Database, user);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            ViewBag.Successful = successful;
            ViewBag.PageUrl = pageUrl;
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = pageDescription;

            return View("_BookmarkletClipArticleForm", "_BookmarkletLayout", clipArticleViewModel);
        }

        private void MapUserToArticleClip(ArticleClip clipModel, Guid userId)
        {
            //check if user is already mapped to the clip
            ArticleClipUserMapping mapping = Database.ArticleClipUserMappings.Where(map => map.ArticleClipId == clipModel.ArticleId && map.UserId == userId).FirstOrDefault();

            if (mapping == null)
            {
                Database.ArticleClipUserMappings.AddObject(new ArticleClipUserMapping() { MappingId = 0, UserId = userId, ArticleClipId = clipModel.ArticleId, ContributionDate = DateTime.Now });
                Database.SaveChanges();
            }
        }

        private void MapTagsToArticleClip(ArticleClip clipModel, MatchCollection clipTags, Guid userGuid)
        {
            foreach (Match tag in clipTags)
            {
                string tagName = tag.Value.Replace("#", "").Trim();

                //ensure the tag doesn't already exist in DB
                Tag userTagModel = Database.Tags.Where(t => t.LowerCaseTagName == tagName.ToLower().Trim()).FirstOrDefault();

                if (userTagModel == null)
                {
                    BaseTagViewModel userTagViewModel = new BaseTagViewModel(null, tagName, tagName.ToLower(), userGuid, DateTime.Now);

                    userTagModel = userTagViewModel.ToTag(Database);
                    Database.Tags.AddObject(userTagModel);
                    Database.SaveChanges();
                }

                bool addNewMapping = true;

                //ensure tag isn't already mapped to the clip                
                addNewMapping = !(Database.ArticleClipTagMappings.Where(t => t.ArticleClipId == clipModel.ArticleId && t.TagId == userTagModel.TagId).Count() > 0);

                if (addNewMapping)
                {
                    clipModel.ArticleClipTagMappings.Add(new ArticleClipTagMapping() { ClipTagMappingId = 0, TagId = userTagModel.TagId, ArticleClipId = clipModel.ArticleId, ContributionDate = DateTime.Now });
                }
            }

            Database.SaveChanges();
        }

        [ValidateInput(false)]
        public JsonResult ShareArticleClip(string recipients, long articleId, string comments, string articleMarkup)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    var userProfile = Database.UserProfiles.Where(profile => profile.UserId == userGuid).FirstOrDefault();

                    if (userProfile != null)
                    {
                        var articleModel = Database.ArticleClips.Where(article => article.ArticleId == articleId).FirstOrDefault();

                        if (articleModel.ShareArticleMarkup == null && articleMarkup != null && !articleMarkup.Trim().Equals(string.Empty))
                        {
                            articleModel.ShareArticleMarkup = articleMarkup.Trim();
                            Database.SaveChanges();
                        }

                        var articleViewModel = articleModel.ToArticleViewModel(Database, user);

                        if (articleModel != null)
                        {
                            JavaScriptSerializer s = new JavaScriptSerializer();
                            List<ShareLinkRecipientViewModel> recipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);
                            successful = PostOrEmailArticle(recipientViewModels, userProfile, articleModel, comments);
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
    }
}
