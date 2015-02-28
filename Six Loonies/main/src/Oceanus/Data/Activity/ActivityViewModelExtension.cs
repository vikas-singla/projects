using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;

namespace Oceanus.Data
{
    internal static class ActivityViewModelExtension
    {
        internal static BasePostViewModel ToBasePostViewModel(this UserWallPost model)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);

            return new BasePostViewModel(model.Id, model.WallText, model.ContributingUserId, model.ContributionDate, userProfile, model.WebPageId);
        }

        internal static WebPostViewModel ToWebPostViewModel(this UserWallPost model, OceanusEntities database)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);
            WebPageViewModel associatedWebPage = model.WebPage.ToWebPageViewModel(database);

            return new WebPostViewModel(model.Id, model.WallText, model.ContributingUserId, model.ContributionDate, userProfile, model.WebPageId, associatedWebPage);
        }

        internal static UserWallPost ToUserPost(this BasePostViewModel viewModel)
        {
            return new UserWallPost()
            {
                Id = (viewModel.Id == null ? 0 : ((int) viewModel.Id)),
                WallText = viewModel.WallText,
                ContributionDate = viewModel.ContributionDate,
                ContributingUserId = viewModel.ContributingUserId,
                WebPageId = viewModel.WebPageId
            };
        }

        internal static UserWallPostCommentViewModel ToUserPostCommentViewModel(this UserWallPostComment model)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);

            return new UserWallPostCommentViewModel(model.Id, model.UserWallPostId, model.CommentText, model.ContributingUserId, model.ContributionDate, userProfile);
        }

        internal static UserWallPostComment ToUserPostComment(this UserWallPostCommentViewModel viewModel)
        {
            return new UserWallPostComment()
            {
                Id = (viewModel.Id == null ? 0 : ((int)viewModel.Id)),
                UserWallPostId = viewModel.WallPostId,
                CommentText = viewModel.CommentText,
                ContributionDate = viewModel.ContributionDate,
                ContributingUserId = viewModel.ContributingUserId
            };
        }
    }
}