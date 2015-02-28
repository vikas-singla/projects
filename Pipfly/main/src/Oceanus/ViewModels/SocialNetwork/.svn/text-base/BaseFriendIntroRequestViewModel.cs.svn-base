using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BaseFriendIntroRequestViewModel
    {
        /// <summary>
        /// Id of the friend introduction request
        /// </summary>
        public int? FriendIntroRequestId;
        /// <summary>
        /// Subject of the friend introduction request
        /// </summary>
        public string Subject;
        /// <summary>
        /// Timestamp of when the friend introduction request was sent by the initiating user
        /// </summary>
        public DateTime InitialSendTimestamp;
        /// <summary>
        /// Reference to the request's initiating user ID
        /// </summary>
        public Guid InitiatingUserId;
        /// <summary>
        /// User profile of the initiating user
        /// </summary>
        public UserProfileViewModel InitiatingUserProfile;
        /// <summary>
        /// Reference to the Id the user that connects the initiating and target user
        /// </summary>
        public Guid ConnectingFriendUserId;
        /// <summary>
        /// Reference to the target user of the friend intro request
        /// </summary>
        public Guid TargetUserId;
        /// <summary>
        /// Intiating user's message to the connecting friend
        /// </summary>
        public string MessageToConnectingFriend;
        /// <summary>
        /// Flag: Indicates if the connecting friend has seen this friend intro request
        /// </summary>
        public bool ConnectingFriendHasReadMessage;
        /// <summary>
        /// Initiating user's message to the target user
        /// </summary>
        public string InitiatingUserMessageToTargetUser;
        /// <summary>
        /// Connecting user's message to the target user
        /// </summary>
        public string ConnectingUserMessageToTargetUser;
        /// <summary>
        /// Flag: Indicates if the target user has seen this friend intro request
        /// </summary>
        public bool TargetUserHasReadMessage;
        /// <summary>
        /// Timestamp of when the friend introduction request was last updated
        /// </summary>
        public DateTime LastUpdatedOn;
        /// <summary>
        /// Flag: Indicates if the introduction has been forwarded by the connecting friend to the target user
        /// </summary>
        public bool IntroductionForwardedToTargetUser;
        /// <summary>
        /// Flag: Indicates if the target user has accepted the friend introduction request
        /// </summary>
        public bool TargetUserHasAccepted;
        /// <summary>
        /// Target user's message to init user if friend introduction request has been accepted
        /// </summary>
        public string TargetUserMessageToInitUser;
        /// <summary>
        /// Flag: Indicates if the init user has read the target user's acceptance message
        /// </summary>
        public bool InitUserHasReadTargetUserAcceptance;

        public BaseFriendIntroRequestViewModel(int? friendIntroRequestId_, string subject_, DateTime initialSendTimestamp_, Guid initiatingUserId_, Guid connectingFriendUserId_,
            Guid targetUserId_, string messageToConnectingFriend_, bool connectingFriendHasReadMessage_, string initiatingUserMessageToTargetUser_, 
            string connectingUserMessageToTargetUser_, bool targetUserHasReadMessage_, DateTime lastUpdatedOn_, bool introForwardedToTargetUser_, bool targetUserHasAccepted_,
            string targetUserMessageToInitUser_, bool initUserHasReadTargetUserAcceptance_)
        {
            FriendIntroRequestId = friendIntroRequestId_;
            Subject = subject_;
            InitialSendTimestamp = initialSendTimestamp_;
            InitiatingUserId = initiatingUserId_;
            ConnectingFriendUserId = connectingFriendUserId_;
            TargetUserId = targetUserId_;
            MessageToConnectingFriend = messageToConnectingFriend_;
            ConnectingFriendHasReadMessage = connectingFriendHasReadMessage_;
            InitiatingUserMessageToTargetUser = initiatingUserMessageToTargetUser_;
            ConnectingUserMessageToTargetUser = connectingUserMessageToTargetUser_;
            TargetUserHasReadMessage = targetUserHasReadMessage_;
            LastUpdatedOn = lastUpdatedOn_;
            IntroductionForwardedToTargetUser = introForwardedToTargetUser_;
            TargetUserHasAccepted = targetUserHasAccepted_;
            TargetUserMessageToInitUser = targetUserMessageToInitUser_;
            InitUserHasReadTargetUserAcceptance = initUserHasReadTargetUserAcceptance_;
        }
    }
}