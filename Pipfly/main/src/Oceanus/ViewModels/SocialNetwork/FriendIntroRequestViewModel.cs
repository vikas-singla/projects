using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class FriendIntroRequestViewModel : BaseFriendIntroRequestViewModel
    {
        /// <summary>
        /// User profile of the connecting user
        /// </summary>
        public UserProfileViewModel ConnectingFriendUserProfile;
        /// <summary>
        /// User profile of the target user
        /// </summary>
        public UserProfileViewModel TargetUserProfile;

        public FriendIntroRequestViewModel(int? friendIntroRequestId_, string subject_, DateTime initialSendTimestamp_, Guid initiatingUserId_, Guid connectingFriendUserId_,
            Guid targetUserId_, string messageToConnectingFriend_, bool connectingFriendHasReadMessage_, string initiatingUserMessageToTargetUser_,
            string connectingUserMessageToTargetUser_, bool targetUserHasReadMessage_, DateTime lastUpdatedOn_, bool introForwardedToTargetUser_, bool targetUserHasAccepted_,
            string targetUserMessageToInitUser_, bool initUserHasReadTargetUserAcceptance_) :
            base(friendIntroRequestId_, subject_, initialSendTimestamp_, initiatingUserId_, connectingFriendUserId_, targetUserId_, messageToConnectingFriend_,
                connectingFriendHasReadMessage_, initiatingUserMessageToTargetUser_, connectingUserMessageToTargetUser_, targetUserHasReadMessage_,
                lastUpdatedOn_, introForwardedToTargetUser_, targetUserHasAccepted_, targetUserMessageToInitUser_, initUserHasReadTargetUserAcceptance_)
        {
        }
    }
}