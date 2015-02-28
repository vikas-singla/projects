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
using System.Data.Objects;

namespace Oceanus.Controllers
{
    public class TopicController : ControllerBase
    {
        internal const int MAX_INITIAL_TAG_LOAD_SET = 30;

        public ActionResult Index(string searchTag, ArticleController.ArticleTypes articleType, bool filterByFriends, ControllerBase.SortContentCriteria? sortBy)
        {
            Tag topicModel = Database.Tags.Where(t => t.LowerCaseTagName.Equals(searchTag.ToLower().Trim())).FirstOrDefault();
            BaseTagViewModel topicViewModel = topicModel.ToBaseTagViewModel();

            //is the currently logged in user a follower of this topic?
            bool currLoggedInUserIsFollower = false;
            MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
            Guid? currUserGuid = (currUserMembership != null ? (Guid?)currUserMembership.ProviderUserKey : null);

            if (currUserGuid != null)
            {
                currLoggedInUserIsFollower = Database.UserToTagFollowingMappings.Where(map => map.TagId == topicModel.TagId && map.FollowingUserId == currUserGuid).FirstOrDefault() != null;
            }

            ViewBag.ArticleType = articleType;
            ViewBag.FilterByFriends = filterByFriends;

            ViewBag.NumPhotos = Database.PhotoVideoClipTagMappings.Where(map => map.TagId == topicModel.TagId && map.PhotoVideoClip.VideoProviderId == null).Count();
            ViewBag.NumVideos = Database.PhotoVideoClipTagMappings.Where(map => map.TagId == topicModel.TagId && map.PhotoVideoClip.VideoProviderId != null).Count();
            ViewBag.NumArticles = Database.ArticleClipTagMappings.Where(map => map.TagId == topicModel.TagId).Count();

            //IMPORTANT!!!
            ViewBag.CurrLoggedInUserIsFollower = currLoggedInUserIsFollower;
            ViewBag.SortBy = sortBy;

            return View(topicViewModel);
        }

        /// <summary>
        /// Get the top topics
        /// </summary>
        /// <returns></returns>
        public ActionResult Trending()
        {
            ViewBag.since_date = DateTime.Now.ToUniversalTime().Ticks;
            return View("TrendingTags");
        }

        /// <summary>
        /// Get the top topics
        /// </summary>
        /// <returns></returns>
        public ActionResult TrendingCollage(int page, int perpage, long? since_date)
        {
            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            IEnumerable<TagViewModel> tagViewModels = null;

            int toSkip = (page - 1) * MAX_INITIAL_TAG_LOAD_SET;

            var tagsQueryable = Database.Tags.AsQueryable();

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                tagsQueryable = tagsQueryable.Where(map => map.AddedOn < since_datetime);
                tagsQueryable = tagsQueryable.OrderByDescending(tag => (tag.PhotoVideoClipTagMappings.Where(clipMap => clipMap.ContributionDate < since_datetime).Count() +
                    tag.ArticleClipTagMappings.Where(articleMap => articleMap.ContributionDate < since_datetime).Count()));
            }
            else
            {
                tagsQueryable = tagsQueryable.OrderByDescending(tag => (tag.PhotoVideoClipTagMappings.Count +
                    tag.ArticleClipTagMappings.Count));
            }

            tagsQueryable = tagsQueryable.Skip(toSkip).Take(MAX_INITIAL_TAG_LOAD_SET);

            string sql = ((ObjectQuery)tagsQueryable).ToTraceString();

            tagViewModels = tagsQueryable.AsEnumerable().Select(tag => tag.ToTagViewModel(Database, loggedInUserGuid));

            return PartialView("_TrendingTagCollage", tagViewModels);
        }

        /// <summary>
        /// Get the top topics
        /// </summary>
        /// <returns></returns>
        public ActionResult HomePageTop(int numTopics)
        {
            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            IEnumerable<BaseTagViewModel> tagViewModels = null;

            var tagsQueryable = Database.Tags.AsQueryable();

            tagsQueryable = tagsQueryable.OrderByDescending(tag => (tag.PhotoVideoClipTagMappings.Count +
                tag.ArticleClipTagMappings.Count));

            tagsQueryable = tagsQueryable.Take(numTopics);

            string sql = ((ObjectQuery)tagsQueryable).ToTraceString();

            tagViewModels = tagsQueryable.AsEnumerable().Select(tag => tag.ToBaseTagViewModel());

            return PartialView("_TopTopicsForHomePage", tagViewModels);
        }

        public ActionResult GetTopicRecommends(int id)
        {
            UserProfile profile = Database.UserProfiles.Where(p => p.UserProfileId == id).FirstOrDefault();
            IEnumerable<TagViewModel> tagViewModels = null;

            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            if (profile != null)
            {
                IEnumerable<int> userTagMappings = Database.UserToTagFollowingMappings.Where(t => t.FollowingUserId == profile.UserId).AsEnumerable().Select(t => t.TagId);

                IEnumerable<Tag> topics = Database.Tags.Where(t => !userTagMappings.Contains(t.TagId)).Take(5);
                tagViewModels = topics.Select(t => t.ToTagViewModel(Database, loggedInUserGuid));
            }
            return PartialView("_UserProfileTopicRecommendations", tagViewModels);
        }

        public ActionResult GetTopicOverview(int topicId)
        {
            ViewBag.TopicId = topicId;
            return PartialView("_TopicOverview");
        }

        public ActionResult GetTopicFollowers(int topicId)
        {
            IEnumerable<UserToTagFollowingMapping> mappingModelList = Database.UserToTagFollowingMappings.Where(map => map.TagId == topicId).AsEnumerable();
            IEnumerable<UserToTagFollowerMappingViewModel> mappingViewModelList = mappingModelList.Select(map => map.ToTagFollowerMappingViewModel());

            return PartialView("_TopicFollowers", mappingViewModelList);
        }

        [Authorize]
        public ActionResult UnfollowTopic(int topicId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                //check if the user is already following the topic
                var mapping = Database.UserToTagFollowingMappings.Where(map => map.TagId == topicId && map.FollowingUserId == userGuid).FirstOrDefault();

                if (mapping != null)
                {
                    Database.UserToTagFollowingMappings.DeleteObject(mapping);
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
        public ActionResult FollowTopic(int topicId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                //check if the user is already following the topic
                var mapping = Database.UserToTagFollowingMappings.Where(map => map.TagId == topicId && map.FollowingUserId == userGuid).FirstOrDefault();

                if (mapping == null)
                {
                    //user is not a follower and so add him
                    Database.UserToTagFollowingMappings.AddObject(new UserToTagFollowingMapping() { MappingId = 0, FollowingUserId = userGuid, TagId = topicId, AddedOn = DateTime.Now });
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

        public ActionResult Search(string q, long? since_date, int page = 1)
        {
            MembershipUser loggedInUser = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (loggedInUser != null) ? ((Guid)loggedInUser.ProviderUserKey) : ((Guid?)null);

            var tags = Database.Tags.Where(tag => tag.LowerCaseTagName.Contains(q)).AsQueryable();
            int toSkip = (page - 1) * MAX_INITIAL_TAG_LOAD_SET;

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                tags.Where(map => map.AddedOn < since_datetime);
                tags = tags.OrderBy(tag => tag.PhotoVideoClipTagMappings.Where(map => map.ContributionDate < since_datetime).Count() +
                    tag.ArticleClipTagMappings.Where(map => map.ContributionDate < since_datetime).Count());
            }
            else
            {
                tags = tags.OrderBy(tag => tag.PhotoVideoClipTagMappings.Count + tag.ArticleClipTagMappings.Count);
            }

            tags = tags.Skip(toSkip);

            var tagViewModels = tags.Take(MAX_INITIAL_TAG_LOAD_SET).AsEnumerable().Select(tag => tag.ToTagViewModel(Database, loggedInUserGuid));
            return PartialView("_TagCollage", tagViewModels);
        }
    }
}
