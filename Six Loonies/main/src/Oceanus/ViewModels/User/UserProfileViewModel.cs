using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class UserProfileViewModel : BaseUserViewModel
    {
        /// <summary>
        /// Determines whether or not the user has been email confirmed in the system.
        /// </summary>
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Reference to the user's level
        /// </summary>
        public int UserLevelId;
        /// <summary>
        /// Description of the User Level (e.g. "Newbie")
        /// </summary>
        public string UserLevel;
        /// <summary>
        /// Flag: Indicates if the currently logged in user is following this user
        /// </summary>
        public bool CurrentUserIsFollowing;
        /// <summary>
        /// Flag: Indicates if the currently logged in user is a friend
        /// </summary>
        public bool CurrentUserIsFriend;
        /// <summary>
        /// Flag: Indicates if the currently logged in user has a pending request 
        /// </summary>
        public bool CurrentUserHasPendingFriendRequest;
        /// <summary>
        /// Facebook authentication token
        /// </summary>
        public string FacebookToken;
        /// <summary>
        /// Facebook UID of the user
        /// </summary>
        public long? FacebookUserUID;
        /// <summary>
        /// Flag: Indicates if the user should be prompted to invite friends to Six Loonies when they log in
        /// </summary>
        public bool PromptUserForFBFriends;
        /// <summary>
        /// Flag: Indicates if the user should be shown the 'Get Started' screen in their user profile screen
        /// </summary>
        public bool? ShowGetStartedScreen;
        /// <summary>
        /// Network connection paths between this user and the currently logged in user
        /// </summary>
        public IEnumerable<NetworkConnectionPathViewModel> UserConnectionNetworkPath;

        public UserProfileViewModel()
        {
            UserConnectionNetworkPath = (new List<NetworkConnectionPathViewModel>()).AsEnumerable(); //blank list - important!
        }

        public UserProfileViewModel(int? id_, Guid userId_, string firstName_, string lastName_, int userLevelId_, string facebookToken_, long? facebookUserUID_, bool promptUserForFBFriends_, String userProfileImageUrl_, 
            bool isConfirmed_, bool? showGetStartedScreen_)
        {
            Id = id_;
            UserId = userId_;
            FirstName = firstName_;
            LastName = lastName_;
            FullName = FirstName + " " + LastName;
            UserLevelId = userLevelId_;
            FacebookToken = facebookToken_;
            FacebookUserUID = facebookUserUID_;
            PromptUserForFBFriends = promptUserForFBFriends_;
            UserConnectionNetworkPath = (new List<NetworkConnectionPathViewModel>()).AsEnumerable(); //blank list - important!
            UserProfileImageUrl = userProfileImageUrl_;
            IsConfirmed = isConfirmed_;
            ShowGetStartedScreen = showGetStartedScreen_;
        }
    }
}