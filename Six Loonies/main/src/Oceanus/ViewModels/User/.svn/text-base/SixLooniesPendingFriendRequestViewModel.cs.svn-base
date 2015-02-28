using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class SixLooniesPendingFriendRequestViewModel
    {
        /// <summary>
        /// Record Id
        /// </summary>
        public long? FriendRequestId { get; set; }

        /// <summary>
        /// ID of the user who initiated the friend request
        /// </summary>
        public Guid InitiatingUserId { get; set; }

        /// <summary>
        /// Reference to the user profile of the friend request initiating user
        /// </summary>
        public UserProfileViewModel InitiatingUser { get; set; }

        /// <summary>
        /// Target ID of the user (if user is part of Six Loonies network)
        /// </summary>
        public Guid? TargetSixLooniesUserId { get; set; }

        /// <summary>
        /// Reference to the user profile of the target user (if user is part of Six Loonies network)
        /// </summary>
        public UserProfileViewModel TargetSixLooniesUser { get; set; }

        /// <summary>
        /// Target Facebook ID of the user (if user is on Facebook but not on Six Loonies)
        /// </summary>
        public long? TargetFacebookUID { get; set; }

        /// <summary>
        /// Full name of the Target Facebook user (if user is on Facebook but not on Six Loonies)
        /// </summary>
        public string TargetFacebookUserFullName { get; set; }

        /// <summary>
        /// Date timestamp for when the friend request was initiated
        /// </summary>
        public DateTime InitiatedOn { get; set; }

        /// <summary>
        /// Flag: Indicates if the friend request was declined by the target user
        /// </summary>
        public bool IsDeclined { get; set; }

        public SixLooniesPendingFriendRequestViewModel()
        {
        }

        public SixLooniesPendingFriendRequestViewModel(long? friendRequestId_, Guid initiatingUserId_, Guid? targetSixLooniesUserId_, long? targetFacebookUID_, DateTime initiatedOn_, bool isDeclined_)
        {
            FriendRequestId = friendRequestId_;
            InitiatingUserId = initiatingUserId_;
            TargetSixLooniesUserId = targetSixLooniesUserId_;
            TargetFacebookUID = targetFacebookUID_;
            InitiatedOn = initiatedOn_;
            IsDeclined = isDeclined_;
        }
    }
}