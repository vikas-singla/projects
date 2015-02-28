using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels.Shared;

namespace Oceanus.ViewModels
{
    public class UserToUserFollowerMappingViewModel
    {
        /// <summary>
        /// Reference to the mapping ID
        /// </summary>
        public long? MappingId { get; set; }
        /// <summary>
        /// Reference to the user being followed
        /// </summary>
        public Guid FollowedUserId { get; set; }
        /// <summary>
        /// Reference to the user who is following
        /// </summary>
        public Guid FollowingUserId { get; set; }
        /// <summary>
        /// Timestamp for when the following user started following
        /// </summary>
        public DateTime AddedOn { get; set; }

        public UserToUserFollowerMappingViewModel()
        {
        }

        public UserToUserFollowerMappingViewModel(long? mappingId_, Guid followedUserId_, Guid followingUserId_, DateTime addedOn_)
        {
            MappingId = mappingId_;
            FollowedUserId = followedUserId_;
            FollowingUserId = followingUserId_;
            AddedOn = addedOn_;
        }
    }
}