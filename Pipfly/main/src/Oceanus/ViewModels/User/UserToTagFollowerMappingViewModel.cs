using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class UserToTagFollowerMappingViewModel
    {
        /// <summary>
        /// Reference to the mapping ID
        /// </summary>
        public long? MappingId { get; set; }
        /// <summary>
        /// Reference to the user who is following the topic
        /// </summary>
        public Guid FollowingUserId { get; set; }
        /// <summary>
        /// Reference to the topic being followed
        /// </summary>
        public int TagId { get; set; }
        /// <summary>
        /// Date timestamp of when the user was added as a follower of the topic
        /// </summary>
        public DateTime AddedOn { get; set; }
        /// <summary>
        /// Basic user profile
        /// </summary>
        public BaseUserViewModel BaseUserProfile { get; set; }

        public UserToTagFollowerMappingViewModel(long? mappingId_, Guid followingUserId_, int tagId_, DateTime addedOn_)
        {
            MappingId = mappingId_;
            FollowingUserId = followingUserId_;
            TagId = tagId_;
            AddedOn = addedOn_;
        }
    }
}