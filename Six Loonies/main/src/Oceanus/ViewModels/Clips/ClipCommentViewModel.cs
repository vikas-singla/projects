using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ClipCommentViewModel
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
        /// Reference to the comment text
        /// </summary>
        public string CommentText { get; set; }
        /// <summary>
        /// Reference to the basic profile of the contributing user
        /// </summary>
        public BaseUserViewModel ContributingUserProfile { get; set; }

        public ClipCommentViewModel(long? id_, string commentText_, Guid contributingUserId_, DateTime contributionDate_, long clipId_)
        {
            Id = id_;
            CommentText = commentText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ClipId = clipId_;
        }

        public ClipCommentViewModel(long? id_, string commentText_, Guid contributingUserId_, DateTime contributionDate_, BaseUserViewModel contributingUserProfile_,
            long clipId_)
        {
            Id = id_;
            CommentText = commentText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ContributingUserProfile = contributingUserProfile_;
            ClipId = clipId_;
        }
    }
}