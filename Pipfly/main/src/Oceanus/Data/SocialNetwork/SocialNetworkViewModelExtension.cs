using Oceanus.ViewModels;
using System.Linq;
using System.Diagnostics;
using Oceanus.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace Oceanus.Data
{
    internal static class SocialNetworkViewModelExtension
    {
        internal static FacebookFriendList ToFacebookFriendList(this FacebookFriendListViewModel viewModel)
        {
            return new FacebookFriendList()
            {
                FacebookFriendListId = (viewModel.FacebookFriendListId == null) ? 0 : (int)viewModel.FacebookFriendListId,
                UserId = viewModel.UserId,
                FacebookFriendUID = viewModel.FacebookFriendUID,
                FacebookFriendName = viewModel.FacebookFriendName,
                FriendProfilePhotoUrl = viewModel.FriendProfilePhotoUrl,
                FacebookFriendSixLooniesUID = viewModel.FacebookFriendSixLooniesUID,
                InvitedToSixLoonies = viewModel.InvitedToSixLoonies
            };
        }

        internal static FacebookFriendListViewModel ToFacebookFriendListViewModel(this Data.FacebookFriendList model)
        {
            return new FacebookFriendListViewModel()
            {
                FacebookFriendListId = model.FacebookFriendListId,
                FacebookFriendName = model.FacebookFriendName,
                FacebookFriendUID = model.FacebookFriendUID,
                FriendProfilePhotoUrl = model.FriendProfilePhotoUrl,
                UserId = model.UserId,
                FacebookFriendSixLooniesUID = model.FacebookFriendSixLooniesUID,
                InvitedToSixLoonies = model.InvitedToSixLoonies
            };
        }

        internal static SixLooniesPendingFriendRequest ToSixLooniesPendingFriendRequest(this SixLooniesPendingFriendRequestViewModel viewModel)
        {
            return new SixLooniesPendingFriendRequest()
            {
                SixLooniesPendingFriendRequestId = (viewModel.FriendRequestId == null) ? 0 : (long)viewModel.FriendRequestId,
                InitiatingUserId = viewModel.InitiatingUserId,
                TargetSixLooniesUserId = viewModel.TargetSixLooniesUserId,
                TargetFacebookUserUID = viewModel.TargetFacebookUID,
                InitiatedOn = viewModel.InitiatedOn,
                IsDeclined = viewModel.IsDeclined
            };
        }

        internal static SixLooniesPendingFriendRequestViewModel ToSixLooniesPendingFriendRequestViewModel(this Data.SixLooniesPendingFriendRequest model, OceanusEntities database)
        {
            string facebookUserFullName = null;

            if (model.TargetFacebookUserUID != null)
            {
                var facebookFriendListRecord = database.FacebookFriendLists.Where(f => f.FacebookFriendUID == model.TargetFacebookUserUID).FirstOrDefault();

                if (facebookFriendListRecord != null)
                {
                    facebookUserFullName = facebookFriendListRecord.FacebookFriendName;
                }
            }

            return new SixLooniesPendingFriendRequestViewModel()
            {
                FriendRequestId = model.SixLooniesPendingFriendRequestId,
                InitiatedOn = model.InitiatedOn,
                InitiatingUserId = model.InitiatingUserId,
                InitiatingUser = model.aspnet_Users.UserProfiles.First().ToUserProfileViewModel(),
                TargetFacebookUID = model.TargetFacebookUserUID,
                TargetFacebookUserFullName = facebookUserFullName,
                TargetSixLooniesUserId = model.TargetSixLooniesUserId,
                TargetSixLooniesUser = model.aspnet_Users1.UserProfiles.First().ToUserProfileViewModel(),
                IsDeclined = model.IsDeclined
            };
        }

        internal static AllFriendConnectionsViewModel ToAllFriendsConnectionsViewModel(this UserProfile model)
        {
            return new AllFriendConnectionsViewModel()
            {
                FriendFullName = model.FullName,
                SixLooniesUserId = model.UserId,
                FacebookUID = model.FacebookUserUID
            };
        }

        internal static AllFriendConnectionsViewModel ToAllFriendsConnectionsViewModel(this FacebookFriendList model)
        {
            return new AllFriendConnectionsViewModel()
            {
                FriendFullName = model.FacebookFriendName,
                SixLooniesUserId = model.FacebookFriendSixLooniesUID,
                FacebookUID = model.FacebookFriendUID
            };
        }

        internal static SixLooniesFriendConnection ToSixLooniesFriendConnection(this SixLooniesFriendConnectionViewModel viewModel)
        {
            return new SixLooniesFriendConnection()
            {
                SixLooniesFriendConnectionId = (viewModel.FriendConnectionId == null) ? 0 : (long)viewModel.FriendConnectionId,
                FirstUserId = viewModel.FirstUserId,
                SecondUserId = viewModel.SecondUserId,
                AddedOn = viewModel.AddedOn
            };
        }

        internal static SixLooniesFriendConnectionViewModel ToSixLooniesFriendConnectionViewModel(this Data.SixLooniesFriendConnection model)
        {
            return new SixLooniesFriendConnectionViewModel()
            {
                FriendConnectionId = model.SixLooniesFriendConnectionId,
                FirstUserId = model.FirstUserId,
                FirstUserProfile = model.aspnet_Users.UserProfiles.First().ToUserProfileViewModel(),
                SecondUserId = model.SecondUserId,
                SecondUserProfile = model.aspnet_Users1.UserProfiles.First().ToUserProfileViewModel(),
                AddedOn = model.AddedOn
            };
        }

        internal static NetworkConnectionPathViewModel ToNetworkConnectionsPathViewModel(this NetworkConnectionPath model, OceanusEntities database)
        {
            UserProfile targetUserProfile = database.UserProfiles.Where(u => u.UserId == model.ToUID).FirstOrDefault();

            NetworkConnectionPathViewModel path = new NetworkConnectionPathViewModel(model.FromUID, model.ToUID, targetUserProfile.ToUserProfileViewModel(), model.Distance, model.NetworkPath);

            if (path.ConnectingFriendID != null)
            {
                //retrieve connecting friend's user profile
                UserProfile connectingFriendProfile = database.UserProfiles.Where(u => u.UserId == path.ConnectingFriendID).FirstOrDefault();

                if (connectingFriendProfile != null)
                {
                    path.ConnectingFriendUserProfile = connectingFriendProfile.ToUserProfileViewModel();
                    path.ConnectingFriendName = connectingFriendProfile.FullName;
                }
            }

            return path;
        }
    }
}