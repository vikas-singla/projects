using Oceanus.ViewModels;
using System.Linq;
using System.Diagnostics;
using Oceanus.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace Oceanus.Data
{
    internal static class MessageViewModelExtension
    {
        internal static MessageViewModel ToMessageViewModel(this Data.Message model)
        {
            var msgResponses = model.MessageResponses.Select(resp => resp.ToMessageResponsesViewModel());

            if (msgResponses != null)
            {
                msgResponses.OrderBy(i => i.ContributionDate);
            }

            return new MessageViewModel()
            {
                MessageId = model.MessageId,
                Subject = model.Subject,
                InitialSendTimestamp = model.InitialSendTimestamp,
                InitiatingUser = model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel(),
                IsSpam = model.IsSpam,
                InitiatingUserId = model.InitiatingUserId,
                LastUpdatedOn = model.LastUpdatedOn,
                MessageResponses = msgResponses
            };
        }

        internal static BaseMessageViewModel ToBaseMessageViewModel(this Data.Message model)
        {
            return new BaseMessageViewModel()
            {
                MessageId = model.MessageId,
                Subject = model.Subject,
                InitialSendTimestamp = model.InitialSendTimestamp,
                InitiatingUser = model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel(),
                IsSpam = model.IsSpam,
                InitiatingUserId = model.InitiatingUserId,
                LastUpdatedOn = model.LastUpdatedOn
            };
        }

        internal static Message ToMessageModel(this BaseMessageViewModel viewModel)
        {
            return new Message()
            {
                MessageId = viewModel.MessageId == null ? 0 : (int)viewModel.MessageId,
                Subject = viewModel.Subject,
                InitialSendTimestamp = viewModel.InitialSendTimestamp,
                InitiatingUserId = viewModel.InitiatingUserId,
                IsSpam = viewModel.IsSpam,
                LastUpdatedOn = viewModel.LastUpdatedOn
            };
        }

        internal static MessageResponsesViewModel ToMessageResponsesViewModel(this Data.MessageResponses model)
        {
            return new MessageResponsesViewModel()
            {
                MessageResponseId = model.MessageResponseId,
                MessageId = model.MessageId,
                Text = model.Text,
                ContributionDate = model.ContributionDate,
                ContributingUserId = model.ContributingUserId,
                ContributingUser = model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel()
            };
        }

        internal static MessageVisibility ToMessageVisibilityModel(this MessageVisibilityViewModel viewModel)
        {
            return new MessageVisibility()
            {
                MessageVisibilityId = viewModel.MessageVisibilityId == null ? 0 : (int)viewModel.MessageVisibilityId,
                MessageId = viewModel.MessageId,
                UserId = viewModel.UserId,
                AddedOn = viewModel.AddedOn,
                MessageIsUnread = viewModel.MessageIsUnread
            };
        }

        internal static MessageVisibilityViewModel ToMessageVisibilityViewModel(this MessageVisibility model)
        {
            var userProfile = model.aspnet_Users.UserProfiles.FirstOrDefault();

            return new MessageVisibilityViewModel()
            {
                MessageVisibilityId = model.MessageVisibilityId,
                MessageId = model.MessageId,
                UserId = model.UserId,
                AddedOn = model.AddedOn,
                MessageIsUnread = model.MessageIsUnread,
                User = userProfile == null ? null : userProfile.ToBaseUserViewModel()
            };
        }

        internal static MessageResponses ToMessageResponsesModel(this MessageResponsesViewModel viewModel)
        {
            return new MessageResponses()
            {
                MessageResponseId = viewModel.MessageResponseId == null ? 0 : (int)viewModel.MessageResponseId,
                MessageId = viewModel.MessageId,
                Text = viewModel.Text,
                ContributingUserId = viewModel.ContributingUserId,
                ContributionDate = viewModel.ContributionDate
            };
        }

        internal static FriendIntroductionRequest ToFriendIntroRequest(this BaseFriendIntroRequestViewModel viewModel)
        {
            return new FriendIntroductionRequest()
            {
                FriendIntroRequestId = (viewModel.FriendIntroRequestId == null ? 0 : (int)viewModel.FriendIntroRequestId),
                Subject = viewModel.Subject,
                InitialSendTimestamp = viewModel.InitialSendTimestamp,
                InitiatingUserId = viewModel.InitiatingUserId,
                ConnectingFriendUserId = viewModel.ConnectingFriendUserId,
                TargetUserId = viewModel.TargetUserId,
                MessageToConnectingFriend = viewModel.MessageToConnectingFriend,
                ConnectingFriendHasReadMessage = viewModel.ConnectingFriendHasReadMessage,
                InitiatingUserMessageToTargetUser = viewModel.InitiatingUserMessageToTargetUser,
                ConnectingUserMessageToTargetUser = viewModel.ConnectingUserMessageToTargetUser,
                TargetUserHasReadMessage = viewModel.TargetUserHasReadMessage,
                LastUpdatedOn = viewModel.LastUpdatedOn,
                IntroductionForwardedToTargetUser = viewModel.IntroductionForwardedToTargetUser,
                TargetUserHasAccepted = viewModel.TargetUserHasAccepted,
                TargetUserMessageToInitUser = viewModel.TargetUserMessageToInitUser,
                InitUserHasReadTargetUserAcceptance = viewModel.InitUserHasReadTargetUserAcceptance
            };
        }

        internal static FriendIntroRequestViewModel ToFriendIntroRequestViewModel(this Data.FriendIntroductionRequest model, OceanusEntities database)
        {
            var initiatingUserProfile = database.UserProfiles.Where(u => u.UserId == model.InitiatingUserId).FirstOrDefault();
            var connectingFriendUserProfile = database.UserProfiles.Where(u => u.UserId == model.ConnectingFriendUserId).FirstOrDefault();
            var targetUserProfile = database.UserProfiles.Where(u => u.UserId == model.TargetUserId).FirstOrDefault();

            FriendIntroRequestViewModel requestViewModel = new FriendIntroRequestViewModel(model.FriendIntroRequestId, model.Subject, model.InitialSendTimestamp, model.InitiatingUserId,
                model.ConnectingFriendUserId, model.TargetUserId, model.MessageToConnectingFriend, model.ConnectingFriendHasReadMessage,
                model.InitiatingUserMessageToTargetUser, model.ConnectingUserMessageToTargetUser, model.TargetUserHasReadMessage, model.LastUpdatedOn, model.IntroductionForwardedToTargetUser,
                model.TargetUserHasAccepted, model.TargetUserMessageToInitUser, model.InitUserHasReadTargetUserAcceptance);

            if (initiatingUserProfile != null)
            {
                requestViewModel.InitiatingUserProfile = initiatingUserProfile.ToUserProfileViewModel();
            }
            if (connectingFriendUserProfile != null)
            {
                requestViewModel.ConnectingFriendUserProfile = connectingFriendUserProfile.ToUserProfileViewModel();
            }
            if (targetUserProfile != null)
            {
                requestViewModel.TargetUserProfile = targetUserProfile.ToUserProfileViewModel();
            }

            return requestViewModel;
        }

        internal static BaseFriendIntroRequestViewModel ToBaseFriendIntroRequestViewModel(this Data.FriendIntroductionRequest model, OceanusEntities database)
        {
            var initiatingUserProfile = database.UserProfiles.Where(u => u.UserId == model.InitiatingUserId).FirstOrDefault();

            BaseFriendIntroRequestViewModel requestViewModel = new BaseFriendIntroRequestViewModel(model.FriendIntroRequestId, model.Subject, model.InitialSendTimestamp, model.InitiatingUserId,
                model.ConnectingFriendUserId, model.TargetUserId, model.MessageToConnectingFriend, model.ConnectingFriendHasReadMessage,
                model.InitiatingUserMessageToTargetUser, model.ConnectingUserMessageToTargetUser, model.TargetUserHasReadMessage, model.LastUpdatedOn, model.IntroductionForwardedToTargetUser,
                model.TargetUserHasAccepted, model.TargetUserMessageToInitUser, model.InitUserHasReadTargetUserAcceptance);

            if (initiatingUserProfile != null)
            {
                requestViewModel.InitiatingUserProfile = initiatingUserProfile.ToUserProfileViewModel();
            }

            return requestViewModel;
        }
    }
}