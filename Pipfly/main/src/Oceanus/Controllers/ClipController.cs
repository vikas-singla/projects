using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Controllers.Helpers;
using System.Web.Security;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Oceanus.Utilities;
using System.Web.Script.Serialization;
using Oceanus.ViewModels.Shared;
using System.Text.RegularExpressions;

namespace Oceanus.Controllers
{
    public class ClipController : ControllerBase
    {
        public static string RelativeScrapbookImageUploadPath
        {
            get { return "Uploads"; }
        }

        public static string AbsoluteScrapbookFilePathFormat
        {
            get { return @"{0}{1}\{2}"; }
        }

        public enum ClipType
        {
            Photo = 1,
            Video
        };

        internal const int MAX_INITIAL_PHOTO_LOAD_SET = 30;

        /// <summary>
        /// View the photo/video clip
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            bool currUserLikesClip = false;
            var clipModel = Database.PhotoVideoClips.Where(clip => clip.ClipId == id).FirstOrDefault();
            DetailedPhotoVideoClipViewModel clipViewModel = null;

            if (Request.IsAuthenticated)
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserPhotoVideoClipLikes.Where(like => like.ContributingUserId == userGuid && like.PhotoVideoClipId == id).FirstOrDefault();

                if (existingLike != null)
                {
                    currUserLikesClip = true;
                }
            }

            if (clipModel != null)
            {
                clipViewModel = clipModel.ToDetailedPhotoVideoClipViewModel(Database);
            }

            ViewBag.CurrUserLikesClip = currUserLikesClip;

            return View("ViewClip", clipViewModel);
        }

        /// <summary>
        /// View the photo/video clip
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewFancybox(int id)
        {
            bool currUserLikesClip = false;
            var clipModel = Database.PhotoVideoClips.Where(clip => clip.ClipId == id).FirstOrDefault();
            DetailedPhotoVideoClipViewModel clipViewModel = null;

            if (Request.IsAuthenticated)
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserPhotoVideoClipLikes.Where(like => like.ContributingUserId == userGuid && like.PhotoVideoClipId == id).FirstOrDefault();

                if (existingLike != null)
                {
                    currUserLikesClip = true;
                }
            }

            if (clipModel != null)
            {
                clipViewModel = clipModel.ToDetailedPhotoVideoClipViewModel(Database);
            }

            ViewBag.CurrUserLikesClip = currUserLikesClip;

            return PartialView("_ViewClipFancybox", clipViewModel);
        }

        public ActionResult GetTopicPhotos(int topic_id, int page, bool filterByFriends, long? since_date, ControllerBase.SortContentCriteria? sortBy)
        {
            Tag tag = Database.Tags.Where(t => t.TagId == topic_id).FirstOrDefault();
            List<Guid> friendConnectionsList = null;
            IEnumerable<PhotoVideoClipViewModel> clips = null;

            if (tag != null)
            {
                if (filterByFriends && Request.IsAuthenticated)
                {
                    MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
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

                int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

                var userMappingsQueryable = tag.PhotoVideoClipTagMappings.Where(map => map.PhotoVideoClip.VideoProviderId == null && (friendConnectionsList != null ? friendConnectionsList.Contains(map.PhotoVideoClip.ContributingUserId) : true));

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
                    userMappingsQueryable = userMappingsQueryable.OrderByDescending(map => (map.PhotoVideoClip.PhotoVideoClipUserMappings.Count + map.PhotoVideoClip.UserPhotoVideoClipLikes.Count + map.PhotoVideoClip.PhotoVideoClipComments.Count));
                }

                var userMappings = userMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable();

                clips = userMappings.Select(map => map.PhotoVideoClip.ToPhotoVideoClipViewModel(Database));
            }

            return PartialView("_Photos", clips);
        }

        public ActionResult GetTopicVideos(int topic_id, int page, bool filterByFriends, long? since_date, ControllerBase.SortContentCriteria? sortBy)
        {
            Tag tag = Database.Tags.Where(t => t.TagId == topic_id).FirstOrDefault();
            List<Guid> friendConnectionsList = null;
            IEnumerable<PhotoVideoClipViewModel> clips = null;

            if (tag != null)
            {
                if (filterByFriends && Request.IsAuthenticated)
                {
                    MembershipUser currUserMembership = Membership.GetUser(User.Identity.Name);
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

                int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

                var userMappingsQueryable = tag.PhotoVideoClipTagMappings.Where(map => map.PhotoVideoClip.VideoProviderId != null && (friendConnectionsList != null ? friendConnectionsList.Contains(map.PhotoVideoClip.ContributingUserId) : true));

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
                    userMappingsQueryable = userMappingsQueryable.OrderByDescending(map => (map.PhotoVideoClip.PhotoVideoClipUserMappings.Count + map.PhotoVideoClip.UserPhotoVideoClipLikes.Count + map.PhotoVideoClip.PhotoVideoClipComments.Count));
                }

                var userMappings = userMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable();

                clips = userMappings.Select(map => map.PhotoVideoClip.ToPhotoVideoClipViewModel(Database));
            }

            return PartialView("_Videos", clips);
        }

        public ActionResult GetUserPhotos(int user_id, long? since_date, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            IEnumerable<PhotoVideoClipViewModel> clips = null;
            var userProfile = Database.UserProfiles.Where(p => p.UserProfileId == user_id).FirstOrDefault();

            if (userProfile != null)
            {
                int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

                var userMappingsQueryable = Database.PhotoVideoClipUserMappings.Where(map => map.UserId == userProfile.UserId && map.PhotoVideoClip.VideoProviderId == null);

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
                    userMappingsQueryable = userMappingsQueryable.OrderByDescending(map => (map.PhotoVideoClip.PhotoVideoClipUserMappings.Count + map.PhotoVideoClip.UserPhotoVideoClipLikes.Count + map.PhotoVideoClip.PhotoVideoClipComments.Count));
                }

                var userMappings = userMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable();

                clips = userMappings.Select(map => map.PhotoVideoClip.ToPhotoVideoClipViewModel(Database));
            }

            ViewBag.user_id = user_id;

            return PartialView("_Photos", clips);
        }

        public ActionResult GetUserVideos(int user_id, long? since_date, int page, ControllerBase.SortContentCriteria? sortBy)
        {
            IEnumerable<PhotoVideoClipViewModel> clips = null;
            var userProfile = Database.UserProfiles.Where(p => p.UserProfileId == user_id).FirstOrDefault();

            if (userProfile != null)
            {
                int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

                var userMappingsQueryable = Database.PhotoVideoClipUserMappings.Where(map => map.UserId == userProfile.UserId && map.PhotoVideoClip.VideoProviderId != null);

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
                    userMappingsQueryable = userMappingsQueryable.OrderByDescending(map => (map.PhotoVideoClip.PhotoVideoClipUserMappings.Count + map.PhotoVideoClip.UserPhotoVideoClipLikes.Count + map.PhotoVideoClip.PhotoVideoClipComments.Count));
                }

                var userMappings = userMappingsQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable();

                clips = userMappings.Select(map => map.PhotoVideoClip.ToPhotoVideoClipViewModel(Database));
            }

            ViewBag.user_id = user_id;

            return PartialView("_Videos", clips);
        }

        public ActionResult Search(string q, long? since_date, int page, ControllerBase.SortContentCriteria? sortBy, ClipType type)
        {
            IEnumerable<PhotoVideoClipViewModel> clips = null;

            int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

            IQueryable<PhotoVideoClip> clipQueryable = null;

            if (type == ClipType.Photo)
            {
                clipQueryable = Database.PhotoVideoClips.Where(clip => clip.VideoProviderId == null && clip.PhotoVideoClipDescriptions.Where(map => map.DescriptionText.Contains(q)).Count() > 0);
            }
            else
            {
                clipQueryable = Database.PhotoVideoClips.Where(clip => clip.VideoProviderId != null && clip.PhotoVideoClipDescriptions.Where(map => map.DescriptionText.Contains(q)).Count() > 0);
            }

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                clipQueryable.Where(map => map.ContributionDate < since_datetime);
            }

            if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
            {
                clipQueryable = clipQueryable.OrderBy(map => map.ContributionDate);
            }
            else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
            {
                clipQueryable = clipQueryable.OrderByDescending(clip => (clip.PhotoVideoClipUserMappings.Count + clip.UserPhotoVideoClipLikes.Count + clip.PhotoVideoClipComments.Count));
            }

            clips = clipQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable().Select(clip => clip.ToPhotoVideoClipViewModel(Database));


            if (type == ClipType.Photo)
            {
                return PartialView("_Photos", clips);
            }
            else
            {
                return PartialView("_Videos", clips);
            }
        }

        [Authorize]
        public ActionResult Spotlight(long? since_date, int page, ControllerBase.SortContentCriteria? sortBy, ClipType type)
        {
            IEnumerable<PhotoVideoClipViewModel> clips = null;
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            int toSkip = (page - 1) * MAX_INITIAL_PHOTO_LOAD_SET;

            IQueryable<PhotoVideoClip> clipQueryable = null;

            if (type == ClipType.Photo)
            {
                clipQueryable = Database.PhotoVideoClips.Where(clip => clip.VideoProviderId == null && (clip.PhotoVideoClipDescriptions.Where(desc => desc.aspnet_Users.UserToUserFollowingMappings.Where(map => map.FollowingUserId == userGuid).Count() > 0).Count() > 0 ||
                    clip.PhotoVideoClipTagMappings.Where(tagMap => tagMap.Tag.UserToTagFollowingMappings.Where(userTagMap => userTagMap.FollowingUserId == userGuid).Count() > 0).Count() > 0)).AsQueryable();
            }
            else
            {
                clipQueryable = Database.PhotoVideoClips.Where(clip => clip.VideoProviderId != null && (clip.PhotoVideoClipDescriptions.Where(desc => desc.aspnet_Users.UserToUserFollowingMappings.Where(map => map.FollowingUserId == userGuid).Count() > 0).Count() > 0 ||
                                   clip.PhotoVideoClipTagMappings.Where(tagMap => tagMap.Tag.UserToTagFollowingMappings.Where(userTagMap => userTagMap.FollowingUserId == userGuid).Count() > 0).Count() > 0)).AsQueryable();
            }

            if (since_date != null)
            {
                DateTime since_datetime = new DateTime((long)since_date);
                clipQueryable.Where(map => map.ContributionDate < since_datetime);
            }

            if (sortBy != null && sortBy == SortContentCriteria.RecentlyAdded)
            {
                clipQueryable = clipQueryable.OrderBy(map => map.ContributionDate);
            }
            else if (sortBy == null || sortBy == SortContentCriteria.MostPopular)
            {
                clipQueryable = clipQueryable.OrderByDescending(clip => (clip.PhotoVideoClipUserMappings.Count + clip.UserPhotoVideoClipLikes.Count + clip.PhotoVideoClipComments.Count));
            }

            clips = clipQueryable.Skip(toSkip).Take(MAX_INITIAL_PHOTO_LOAD_SET).AsEnumerable().Select(clip => clip.ToPhotoVideoClipViewModel(Database));


            if (type == ClipType.Photo)
            {
                return PartialView("_Photos", clips);
            }
            else
            {
                return PartialView("_Videos", clips);
            }
        }

        public ActionResult GetClipLikes(long clipId)
        {
            var clipModel = Database.PhotoVideoClips.Where(clip => clip.ClipId == clipId).FirstOrDefault();
            IEnumerable<BaseUserViewModel> clipLikes = null;

            if (clipModel != null)
            {
                clipLikes = clipModel.UserPhotoVideoClipLikes.Select(like => like.aspnet_Users.UserProfiles.ElementAt(0).ToBaseUserViewModel());
            }

            return PartialView("_ClipLikedBy", clipLikes);
        }

        [Authorize]
        public ActionResult GetClipPhotoUI(string pageUrl, string pageTitle, string pageDescription, string imageUrl, int width, int height)
        {
            ViewBag.PageUrl = pageUrl;
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = pageDescription;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.ImageWidth = width;
            ViewBag.ImageHeight = height;

            return View("_BookmarkletClipPhotoForm", "_BookmarkletLayout");
        }

        [Authorize]
        public ActionResult GetClipVideoUI(string pageUrl, string pageTitle, string pageDescription, string imageUrl, int width, int height, string videoId)
        {
            ViewBag.PageUrl = pageUrl;
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageDescription = pageDescription;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.ImageWidth = width;
            ViewBag.ImageHeight = height;
            ViewBag.VideoId = videoId;

            return View("_BookmarkletClipVideoForm", "_BookmarkletLayout");
        }

        [Authorize]
        public JsonResult SharePhotoVideoClip(string recipients, long clipId, string comments)
        {
            bool successful = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    PhotoVideoClip clipModel = Database.PhotoVideoClips.Where(clip => clip.ClipId == clipId).FirstOrDefault();

                    if (clipModel != null)
                    {
                        UserProfile userProfile = Database.UserProfiles.Where(profile => profile.UserId == userGuid).FirstOrDefault();

                        if (userProfile != null)
                        {
                            JavaScriptSerializer s = new JavaScriptSerializer();
                            List<ShareLinkRecipientViewModel> recipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);

                            //share with recipients
                            successful = PostOrEmailPhotoVideo(recipientViewModels, userProfile, clipModel, comments);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                successful = false;
            }

            return Json(new { successful = successful.ToString().ToLower() }, JsonRequestBehavior.AllowGet);
        }

        [JsonpFilter]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult ClipPhotoVideo(string recipients, string pageUrl, string pageTitle, string pageDescription, string imageUrl, short imageWidth, short imageHeight, string videoId, string clipDesc)
        {
            bool successful = false;
            bool isNewClip = false;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    WebPage pageObject = WebPageHelper.ValidateWebPage(userGuid, pageUrl, pageTitle, pageDescription, Database);

                    imageUrl = (imageUrl == null ? string.Empty : imageUrl.Trim());
                    videoId = (videoId == null ? string.Empty : videoId.Trim());
                    clipDesc = (clipDesc == null ? string.Empty : clipDesc.Trim());

                    if (!((imageUrl.Equals(string.Empty) && videoId.Equals(string.Empty)) || clipDesc.Equals(string.Empty)))
                    {
                        //find tags in the description
                        string regExHashTagPattern = @"(?:(?<=\s)|^)(#|\uFF03)([0-9A-Z_]*[A-Z_]+[a-z0-9_\\u00c0-\\u00d6\\u00d8-\\u00f6\\u00f8-\\u00ff]*)";
                        var matchedTags = Regex.Matches(clipDesc, regExHashTagPattern, RegexOptions.IgnoreCase);

                        bool isYouTube = imageUrl.Contains("youtube.com/") || imageUrl.Contains("youtu.be");
                        bool isVimeo = imageUrl.Contains("vimeo/");

                        string videoProviderId = (isYouTube ? VideoHelper.VideoProvider.YouTube.ToString() : (isVimeo ? VideoHelper.VideoProvider.Vimeo.ToString() : null));

                        PhotoVideoClip clipModel = Database.PhotoVideoClips.Where(clip => clip.SourceUrl == imageUrl && clip.WebPageId == pageObject.Id).FirstOrDefault();

                        if (clipModel == null) //create a clip if it doesn't exist already
                        {
                            PhotoVideoClipViewModel clipViewModel = null;

                            isNewClip = true;

                            //store the clip
                            clipViewModel = new PhotoVideoClipViewModel(-1, pageObject.Id, imageUrl, null, null,
                                videoId, videoProviderId, userGuid, DateTime.Now, imageWidth, imageHeight);
                            clipModel = clipViewModel.ToPhotoVideoClipModel();

                            Database.PhotoVideoClips.AddObject(clipModel);

                            Database.SaveChanges();

                            //download image from web if required
                            if (!imageUrl.Equals(string.Empty))
                            {
                                //string localImageUrl = null;
                                //short? imageWidth = null;
                                //short? imageHeight = null;

                                //if (DownloadScrapbookImageFromWeb(imageUrl, userGuid, clipModel.ClipId, out localImageUrl, out imageWidth, out imageHeight))
                                //{
                                //    clipModel.ImageUrl = localImageUrl;
                                //    clipModel.ImageWidth = imageWidth;
                                //    clipModel.ImageHeight = imageHeight;
                                //    Database.SaveChanges();
                                //}
                            }
                        }

                        ClipDescriptionViewModel descriptionViewModel = new ClipDescriptionViewModel(null, clipDesc, userGuid, DateTime.Now, clipModel.ClipId);
                        Database.PhotoVideoClipDescriptions.AddObject(descriptionViewModel.ToClipCommentModel());
                        Database.SaveChanges();

                        MapUserToPhotoVideoClip(clipModel, userGuid);
                        MapTagsToPhotoVideoClip(clipModel, matchedTags, userGuid, isNewClip);

                        UserProfile userProfile = Database.UserProfiles.Where(profile => profile.UserId == userGuid).FirstOrDefault();

                        if (userProfile != null)
                        {
                            JavaScriptSerializer s = new JavaScriptSerializer();
                            List<ShareLinkRecipientViewModel> recipientViewModels = s.Deserialize<List<ShareLinkRecipientViewModel>>(recipients);

                            //share with recipients
                            successful = PostOrEmailPhotoVideo(recipientViewModels, userProfile, clipModel, clipDesc);
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

        private void MapUserToPhotoVideoClip(PhotoVideoClip clipModel, Guid userId)
        {
            //check if user is already mapped to the clip
            PhotoVideoClipUserMapping mapping = Database.PhotoVideoClipUserMappings.Where(map => map.PhotoVideoClipId == clipModel.ClipId && map.UserId == userId).FirstOrDefault();

            if (mapping == null)
            {
                Database.PhotoVideoClipUserMappings.AddObject(new PhotoVideoClipUserMapping() { MappingId = 0, UserId = userId, PhotoVideoClipId = clipModel.ClipId, ContributionDate = DateTime.Now });
                Database.SaveChanges();
            }
        }

        private void MapTagsToPhotoVideoClip(PhotoVideoClip clipModel, MatchCollection clipTags, Guid userGuid, bool isNewClip)
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
                }

                bool addNewMapping = true;

                //ensure tag isn't already mapped to the clip
                if (isNewClip)
                {
                    addNewMapping = !(Database.PhotoVideoClipTagMappings.Where(t => t.ClipId == clipModel.ClipId && t.TagId == userTagModel.TagId).Count() > 0);
                }

                if (addNewMapping)
                {
                    clipModel.PhotoVideoClipTagMappings.Add(new PhotoVideoClipTagMapping() { ClipTagMappingId = 0, TagId = userTagModel.TagId, ClipId = clipModel.ClipId, ContributionDate = DateTime.Now });
                }
            }

            Database.SaveChanges();
        }

        private bool DownloadScrapbookImageFromWeb(string imageUrl, Guid userId, long itemId, out string localImagePath, out short? imageWidth, out short? imageHeight)
        {
            bool successful = false;
            localImagePath = string.Empty;
            var folder = string.Format(AbsoluteScrapbookFilePathFormat, Request.PhysicalApplicationPath, RelativeScrapbookImageUploadPath, ImageFolderDefinitions.FOLDER_SCRAPBOOK_UPLOADS);
            string fileName = itemId + "_" + Guid.NewGuid() + ".jpg";
            string fullPath = string.Format(@"{0}\{1}", folder, fileName);

            imageWidth = null;
            imageHeight = null;

            imageUrl = imageUrl.IndexOf('#') > 0 ? (imageUrl.Substring(0, imageUrl.IndexOf('#'))) : imageUrl;
            Uri imageURI = new Uri(imageUrl);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(imageURI);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 20000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.ContentLength > 0)
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                response.Close();

                imageWidth = (short?)img.Width;
                imageHeight = (short?)img.Height;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                ImageUtilities.SaveImage(fullPath, img);

                localImagePath = fileName;

                successful = true;
            }

            return successful;
        }

        public ActionResult GetClipCommentPosts(long clipId)
        {
            IEnumerable<PhotoVideoClipComment> clipComments = Database.PhotoVideoClipComments.Where(p => p.ClipId == clipId).AsEnumerable().OrderByDescending(p => p.ContributionDate);
            IEnumerable<ClipCommentViewModel> clipCommentViewModels = clipComments.Select(p => p.ToClipCommentViewModel());

            ViewBag.ClipId = clipId;

            return PartialView("_ClipComments", clipCommentViewModels);
        }

        public ActionResult GetClipComments(long clipId)
        {
            ViewBag.ClipId = clipId;
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid? loggedInUserGuid = (user != null) ? ((Guid)user.ProviderUserKey) : ((Guid?)null);

            if (loggedInUserGuid != null)
            {
                UserProfile loggedInUserProfile = Database.UserProfiles.Where(u => u.UserId == ((Guid)loggedInUserGuid)).FirstOrDefault();
                if (loggedInUserProfile != null)
                {
                    ViewBag.LoggedInUserProfileViewModel = loggedInUserProfile.ToBaseUserViewModel();
                }
            }

            return PartialView("_ClipCommentsContainer");
        }

        [Authorize]
        public ActionResult AddClipComment(string commentText, long clipId)
        {
            string dbMessage = string.Empty;
            bool success = false;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid loggedInUserGuid = (Guid)user.ProviderUserKey;

            ClipCommentViewModel commentsViewModel = new ClipCommentViewModel(null, commentText.Trim(), loggedInUserGuid, DateTime.Now, clipId);
            PhotoVideoClipComment comment = commentsViewModel.ToClipCommentModel();

            Database.PhotoVideoClipComments.AddObject(comment);
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
        public ActionResult LikeClip(long clipId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserPhotoVideoClipLikes.Where(like => like.ContributingUserId == userGuid && like.PhotoVideoClipId == clipId).FirstOrDefault();

                if (existingLike == null)
                {
                    Database.UserPhotoVideoClipLikes.AddObject(new UserPhotoVideoClipLike() { PhotoVideoClipId = clipId, ContributingUserId = userGuid, AddedOn = DateTime.Now });
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
        public ActionResult UnikeClip(long clipId)
        {
            bool successful = false;

            try
            {
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                Guid userGuid = (Guid)user.ProviderUserKey;

                var existingLike = Database.UserPhotoVideoClipLikes.Where(like => like.ContributingUserId == userGuid && like.PhotoVideoClipId == clipId).FirstOrDefault();

                if (existingLike != null)
                {
                    Database.UserPhotoVideoClipLikes.DeleteObject(existingLike);
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
        public ActionResult ShareClipOverlay(long id)
        {
            PhotoVideoClip clipModel = Database.PhotoVideoClips.Where(clip => clip.ClipId == id).FirstOrDefault();
            DetailedPhotoVideoClipViewModel clipViewModel = (clipModel == null ? null : clipModel.ToDetailedPhotoVideoClipViewModel(Database));
            return PartialView("_ShareClip", clipViewModel);
        }

        private bool PostOrEmailPhotoVideo(List<ShareLinkRecipientViewModel> recipientViewModels, UserProfile userProfile, PhotoVideoClip photoModel, string comments)
        {
            bool successful = true;

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);

                    ViewBag.UserFirstName = userProfile.FirstName;
                    ViewBag.UserFullName = userProfile.FullName;
                    ViewBag.UserId = userProfile.UserProfileId;
                    ViewBag.ClipComments = comments;

                    string shareClipType = photoModel.VideoProviderId == null ? "photo" : "video";

                    string htmlMessage = EmailHelper.RenderViewToString("_EmailClipTemplate", "_EmailLayout", photoModel.ToBasePhotoVideoClipViewModel(), ControllerContext, ViewBag, ViewData, TempData);
                    string textMessage = userProfile.FirstName + " shared a " + shareClipType + " that you can view here: www.pipfly.com/clip/view/?id=" + photoModel.ClipId
                                + "\n\n-----\nShared via Pipfly (www.pipfly.com)\n\n(c) Six Loonies Inc. 2012";

                    //get recipients on facebook
                    try
                    {
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
                                    postedOnFB = FacebookHelper.PostToFacebookWall((long)friendConnection.FacebookFriendUID, userProfile.FacebookToken, comments, "Check out this interesting " + shareClipType, "www.pipfly.com/clip/view/?id=" + photoModel.ClipId,
                                        photoModel.WebPage.WebDomainUrl, null, photoModel.ImageUrl == null ? photoModel.SourceUrl : photoModel.ImageUrl, null);

                                    if (!postedOnFB)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    //get recipients to be notified via email
                    IEnumerable<ShareLinkRecipientViewModel> emailRecipients = recipientViewModels.Where(r => r.email != null && r.email != string.Empty).AsEnumerable();
                    EmailValidationAttribute emailValidationAttr = new EmailValidationAttribute();

                    foreach (ShareLinkRecipientViewModel emailRecipient in emailRecipients)
                    {
                        string emailAddress = emailRecipient.name;

                        if (emailValidationAttr.IsValid(emailAddress))
                        {
                            bool emailSent = EmailHelper.sendEmail(null, emailAddress, userProfile.FirstName + " shared a " + shareClipType + " with you", htmlMessage, textMessage);

                            if (!emailSent)
                            {
                                successful = false;
                            }
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
    }
}
