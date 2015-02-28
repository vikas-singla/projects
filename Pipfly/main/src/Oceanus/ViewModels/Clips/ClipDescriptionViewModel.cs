using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ClipDescriptionViewModel
    {
        /// <summary>
        /// ID of the user's wall post
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Reference to the clip to which this comment belongs
        /// </summary>
        public long ClipId { get; set; }
        /// <summary>
        /// Reference to the post contributing user ID
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Reference to the post contribution date
        /// </summary>
        public DateTime ContributionDate { get; set; }
        /// <summary>
        /// Reference to the description text
        /// </summary>
        public string DescriptionText { get; set; }
        /// <summary>
        /// Reference to the basic profile of the contributing user
        /// </summary>
        public BaseUserViewModel ContributingUserProfile { get; set; }

        public ClipDescriptionViewModel(long? id_, string descriptionText_, Guid contributingUserId_, DateTime contributionDate_, long clipId_)
        {
            Id = id_;
            DescriptionText = descriptionText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ClipId = clipId_;
        }

        public ClipDescriptionViewModel(long? id_, string descriptionText_, Guid contributingUserId_, DateTime contributionDate_, BaseUserViewModel contributingUserProfile_,
            long clipId_)
        {
            Id = id_;
            DescriptionText = descriptionText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ContributingUserProfile = contributingUserProfile_;
            ClipId = clipId_;
        }
    }
}