using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class UserWallPostCommentViewModel
    {
        /// <summary>
        /// ID of the user's wall post comment
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Reference to the associated wall post
        /// </summary>
        public int WallPostId { get; set; }
        /// <summary>
        /// Reference to the post comment contributing user ID
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Reference to the post comment contribution date
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

        public UserWallPostCommentViewModel()
        {
        }

        public UserWallPostCommentViewModel(int? id_, int wallPostId_, string commentText_, Guid contributingUserId_, DateTime contributionDate_, BaseUserViewModel contributingUserProfile_)
        {
            Id = id_;
            WallPostId = wallPostId_;
            CommentText = commentText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ContributingUserProfile = contributingUserProfile_;
        }
    }
}