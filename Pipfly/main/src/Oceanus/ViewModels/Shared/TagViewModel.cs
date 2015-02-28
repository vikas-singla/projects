using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels.Shared
{
    public class TagViewModel : BaseTagViewModel
    {
        /// <summary>
        /// Number of topic followers
        /// </summary>
        public int NumFollowers { get; set; }
        /// <summary>
        /// Number of photo/video clips in this tag
        /// </summary>
        public int NumClips { get; set; }
        /// <summary>
        /// Number of articles in this tag
        /// </summary>
        public int NumArticles { get; set; }
        /// <summary>
        /// Flag indicating if the currently logged in user is a follower of the tag
        /// </summary>
        public bool CurrUserIsFollower { get; set; }
        /// <summary>
        /// Photo associated with the tag
        /// </summary>
        public BasePhotoVideoClipViewModel TagPhoto { get; set; }

        public TagViewModel()
        {
        }

        public TagViewModel(int? id_, string tagName_, string lowerCaseTagName_, Guid contributingUserId_, DateTime addedOn_, int numFollowers_, int numClips_, int numArticles_, bool currUserIsFollower_) :
            base(id_, tagName_, lowerCaseTagName_, contributingUserId_, addedOn_)
        {
            NumFollowers = numFollowers_;
            NumClips = numClips_;
            CurrUserIsFollower = currUserIsFollower_;
            NumArticles = numArticles_;
        }
    }
}