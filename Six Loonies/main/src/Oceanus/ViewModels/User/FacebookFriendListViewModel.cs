using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class FacebookFriendListViewModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int? FacebookFriendListId;

        /// <summary>
        /// Reference to the user on Six Loonies whose friend we're looking at
        /// </summary>
        public Guid UserId;

        /// <summary>
        /// Reference to the unique id of the friend 
        /// </summary>
        public long FacebookFriendUID;

        /// <summary>
        /// Name of the friend on facebook
        /// </summary>
        public string FacebookFriendName;

        /// <summary>
        /// Six Loonies User ID of the facebook friend (if they happen to have account on Six Loonies)
        /// </summary>
        public Guid? FacebookFriendSixLooniesUID;

        /// <summary>
        /// Flag: Indicates that facebook friend is a six loonies user and is connected (ie. a friend) of the user
        /// </summary>
        public bool FacebookFriendIsConnectedOnSixLoonies;

        /// <summary>
        /// Flag: Indicates if the currently user has already sent a friend request to the friend (if he/she is on Six Loonies)
        /// </summary>
        public bool FriendRequestSentOnSixLoonies;

        /// <summary>
        /// Profile photo image URL
        /// </summary>
        public string FriendProfilePhotoUrl;

        /// <summary>
        /// Flag: Indicates if the facebook friend has already been invited to Six Loonies (via Wall Post)
        /// </summary>
        public bool InvitedToSixLoonies;

        public FacebookFriendListViewModel()
        {
        }

        public FacebookFriendListViewModel(int? facebookFriendListId_, Guid userId_, long facebookFriendUID_, string facebookFriendName_, string profilePhotoImageUrl_, Guid? facebookFriendSixLooniesUID_, bool invitedToSixLoonies_)
        {
            FacebookFriendListId = facebookFriendListId_;
            UserId = userId_;
            FacebookFriendUID = facebookFriendUID_;
            FacebookFriendName = facebookFriendName_;
            FriendProfilePhotoUrl = profilePhotoImageUrl_;
            FacebookFriendSixLooniesUID = facebookFriendSixLooniesUID_;
            InvitedToSixLoonies = invitedToSixLoonies_;
        }
    }
}