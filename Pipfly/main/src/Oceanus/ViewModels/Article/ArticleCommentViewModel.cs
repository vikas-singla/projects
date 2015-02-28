using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ArticleCommentViewModel
    {
        /// <summary>
        /// ID of the user's wall post
        /// </summary>
        public long? Id { get; set; }
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
        public string CommentText { get; set; }
        /// <summary>
        /// Reference to the basic profile of the contributing user
        /// </summary>
        public BaseUserViewModel ContributingUserProfile { get; set; }
        /// <summary>
        /// Reference to the article
        /// </summary>
        public long ArticleClipId { get; set; }

        public ArticleCommentViewModel(long? id_, string commentText_, Guid contributingUserId_, DateTime contributionDate_, long articleClipId_)
        {
            Id = id_;
            CommentText = commentText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ArticleClipId = articleClipId_;
        }

        public ArticleCommentViewModel(long? id_, string commentText_, Guid contributingUserId_, DateTime contributionDate_, long articleClipId_, BaseUserViewModel contributingUserProfile_)
        {
            Id = id_;
            CommentText = commentText_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ArticleClipId = articleClipId_;
            ContributingUserProfile = contributingUserProfile_;
        }
    }
}