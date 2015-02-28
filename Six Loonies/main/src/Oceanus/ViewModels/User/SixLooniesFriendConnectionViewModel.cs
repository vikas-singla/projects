using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class SixLooniesFriendConnectionViewModel
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public long? FriendConnectionId { get; set; }

        /// <summary>
        /// Reference to a user
        /// </summary>
        public Guid FirstUserId { get; set; }

        /// <summary>
        /// Reference to the profile of the first user
        /// </summary>
        public UserProfileViewModel FirstUserProfile { get; set; }

        /// <summary>
        /// Reference to a second user who is a friend of the first user
        /// </summary>
        public Guid SecondUserId { get; set; }

        /// <summary>
        /// Reference to the profile of the second user
        /// </summary>
        public UserProfileViewModel SecondUserProfile { get; set; }

        /// <summary>
        /// Timestamp for when the two users were marked as friends
        /// </summary>
        public DateTime AddedOn { get; set; }

        public SixLooniesFriendConnectionViewModel()
        {
        }

        public SixLooniesFriendConnectionViewModel(long? friendConnectionId_, Guid firstUserId_, Guid secondUserId_, DateTime addedOn_)
        {
            FriendConnectionId = friendConnectionId_;
            FirstUserId = firstUserId_;
            SecondUserId = secondUserId_;
            AddedOn = addedOn_;
        }
    }
}