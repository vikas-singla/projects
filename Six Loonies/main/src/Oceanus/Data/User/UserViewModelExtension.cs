using Oceanus.ViewModels;
using System.Linq;
using System.Diagnostics;
using Oceanus.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace Oceanus.Data
{
    internal static class UserViewModelExtension
    {
        internal static UserToTagFollowerMappingViewModel ToTagFollowerMappingViewModel(this Data.UserToTagFollowingMapping model)
        {
            var mappingViewModel = new UserToTagFollowerMappingViewModel(model.MappingId, model.FollowingUserId, model.TagId, model.AddedOn);
            var userProfile = model.aspnet_Users.UserProfiles.First();

            if (userProfile != null)
            {
                mappingViewModel.BaseUserProfile = userProfile.ToBaseUserViewModel();
            }

            return mappingViewModel;
        }

        internal static UserToTagFollowingMapping ToUserToUserFollowingMapping(this UserToTagFollowerMappingViewModel viewModel)
        {
            return new UserToTagFollowingMapping()
            {
                MappingId = viewModel.MappingId == null ? 0 : (int)viewModel.MappingId,
                TagId = viewModel.TagId,
                FollowingUserId = viewModel.FollowingUserId,
                AddedOn = viewModel.AddedOn
            };
        }

        internal static UserStatsViewModel ToUserStatsViewModel(this Data.UserProfile model, OceanusEntities database)
        {
            return new UserStatsViewModel()
            {
                UserId = model.UserId,
                NumClips = model.aspnet_Users.PhotoVideoClips.Count + model.aspnet_Users.ArticleClips.Where(clip => clip.IsPublished).Count(),
                NumLikes = database.UserPhotoVideoClipLikes.Where(clipLike => clipLike.PhotoVideoClip.ContributingUserId == model.UserId).Count() + 
                           database.UserArticleClipLikes.Where(articleLike => articleLike.ArticleClip.ContributingUserId == model.UserId).Count(),
                NumFollowed = database.UserToUserFollowingMappings.Where(f => f.FollowingUserId == model.UserId).Count(),
                NumFollowers = database.UserToUserFollowingMappings.Where(f => f.FollowedUserId == model.UserId).Count()
            };
        }

        internal static BaseUserViewModel ToBaseUserViewModel(this Data.UserProfile model)
        {
            return new BaseUserViewModel()
            {
                Id = model.UserProfileId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FullName,
                UserId = model.UserId,
                UserProfileImageUrl = model.UserProfileImageUrl
            };
        }

        internal static ShareLinkRecipientViewModel ToShareLinkRecipientViewModel(this Data.FacebookFriendList model)
        {
            return new ShareLinkRecipientViewModel()
            {
                id = -1,
                fbuid = model.FacebookFriendUID,
                name = model.FacebookFriendName,
                email = "",
                userprofileimageurl = model.FriendProfilePhotoUrl
            };
        }

        internal static ShareLinkRecipientViewModel ToShareLinkRecipientViewModel(this Data.UserProfile model)
        {
            return new ShareLinkRecipientViewModel()
            {
                id = model.UserProfileId,
                fbuid = (model.FacebookUserUID == null ? -1 : ((long)model.FacebookUserUID)),
                name = model.FullName,
                userprofileimageurl = model.UserProfileImageUrl,
                email = model.aspnet_Users.aspnet_Membership.Email
            };
        }

        internal static SearchUserViewModel ToSearchUserViewModel(this Data.UserProfile model, OceanusEntities database)
        {
            return new SearchUserViewModel()
            {
                Id = model.UserProfileId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FullName,
                UserId = model.UserId,
                UserProfileImageUrl = model.UserProfileImageUrl,
                UserStats = model.ToUserStatsViewModel(database)
            };
        }

        internal static UserProfileViewModel ToUserProfileViewModel(this Data.UserProfile model)
        {
            return new UserProfileViewModel()
            {
                Id = model.UserProfileId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FullName,
                UserId = model.UserId,
                FacebookToken = model.FacebookToken,
                PromptUserForFBFriends = model.PromptUserForFBFriends,
                FacebookUserUID = model.FacebookUserUID,
                UserLevelId = model.UserLevelId,
                UserLevel = model.UserLevel.LevelName,
                IsConfirmed = model.IsConfirmed,
                UserProfileImageUrl = model.UserProfileImageUrl,
                ShowGetStartedScreen = model.ShowGetStartedScreen
            };
        }

        internal static UserProfile ToUserProfile(this UserProfileViewModel viewModel)
        {
            return new UserProfile()
            {
                UserProfileId = viewModel.Id == null ? 0 : (int)viewModel.Id,
                UserId = viewModel.UserId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                FacebookToken = viewModel.FacebookToken,
                FacebookUserUID = viewModel.FacebookUserUID,
                PromptUserForFBFriends = viewModel.PromptUserForFBFriends,
                FullName = viewModel.FullName,
                UserLevelId = viewModel.UserLevelId,
                IsConfirmed = viewModel.IsConfirmed,
                UserProfileImageUrl = viewModel.UserProfileImageUrl,
                ShowGetStartedScreen = viewModel.ShowGetStartedScreen
            };
        }

        internal static UserToUserFollowingMapping ToUserToUserFollowingMapping(this UserToUserFollowerMappingViewModel viewModel)
        {
            return new UserToUserFollowingMapping()
            {
                FollowerMappingId = viewModel.MappingId == null ? 0 : (int)viewModel.MappingId,
                FollowedUserId = viewModel.FollowedUserId,
                FollowingUserId = viewModel.FollowingUserId,
                AddedOn = viewModel.AddedOn
            };
        }

        internal static UserToUserFollowerMappingViewModel ToUserToUserFollowingMappingViewModel(this Data.UserToUserFollowingMapping model)
        {
            return new UserToUserFollowerMappingViewModel()
            {
                MappingId = model.FollowerMappingId,
                FollowingUserId = model.FollowingUserId,
                FollowedUserId = model.FollowedUserId,
                AddedOn = model.AddedOn
            };
        }

        internal static UserWebPageLikesViewModel ToUserLikesViewModel(this Data.UserWebPageLike model)
        {
            return new UserWebPageLikesViewModel(model.UserLikesId, model.ContributingUserId, model.AddedOn, model.WebPageId);
        }

        internal static UserWebPageLike ToUserLikeModel(this UserWebPageLikesViewModel viewModel)
        {
            return new UserWebPageLike()
            {
                UserLikesId = (viewModel.UserLikesId == null ? 0 : (int) viewModel.UserLikesId), 
                AddedOn = viewModel.AddedOn,
                ContributingUserId = viewModel.ContributingUserId,
                WebPageId = viewModel.WebPageId
            };
        }
    }
}