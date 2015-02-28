using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BasePostViewModel
    {
        /// <summary>
        /// ID of the user's wall post
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Reference to the post contributing user ID
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Reference to the post contribution date
        /// </summary>
        public DateTime ContributionDate { get; set; }
        /// <summary>
        /// Reference to the post text
        /// </summary>
        public string WallText { get; set; }
        /// <summary>
        /// Reference to the basic profile of the contributing user
        /// </summary>
        public BaseUserViewModel ContributingUserProfile { get; set; }
        /// <summary>
        /// Reference to the web page in which this post was made
        /// </summary>
        public long WebPageId { get; set; }

        public BasePostViewModel()
        {
        }

        public BasePostViewModel(int? id_, string wallText_, Guid contributingUserId_, DateTime contributionDate_, BaseUserViewModel contributingUserProfile_,
            long webPageId_)
        {
            Id = id_;
            WallText = wallText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ContributingUserProfile = contributingUserProfile_;
            WebPageId = webPageId_;
        }
    }
}