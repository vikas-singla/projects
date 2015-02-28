using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;
using System.Web.Security;

namespace Oceanus.Data
{
    internal static class ArticleViewModelExtension
    {
        internal static ArticleCommentViewModel ToArticleCommentViewModel(this ArticleComment model)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);

            return new ArticleCommentViewModel(model.Id, model.CommentText, model.ContributingUserId, model.ContributionDate, model.ArticleClipId, userProfile);
        }

        internal static ArticleComment ToArticleComment(this ArticleCommentViewModel viewModel)
        {
            return new ArticleComment()
            {
                Id = viewModel.Id == null ? 0 : (long) viewModel.Id,
                CommentText = viewModel.CommentText,
                ArticleClipId = viewModel.ArticleClipId,
                ContributingUserId = viewModel.ContributingUserId,
                ContributionDate = viewModel.ContributionDate
            };
        }

        internal static ArticleClip ToArticleModel(this ArticleViewModel viewModel)
        {
            return new ArticleClip()
            {
                ArticleId = (viewModel.ArticleId == null ? 0 : ((long)viewModel.ArticleId)),
                ArticleMarkup = viewModel.ArticleMarkup,
                ShareArticleMarkup = viewModel.ShareArticleMarkup,
                ArticleTitle = viewModel.ArticleTitle,
                ArticleText = viewModel.ArticleText,
                ArticleType = viewModel.ArticleType.ToString(),
                WebPageId = viewModel.WebPageId,
                ContributingUserId = viewModel.ContributingUserId,
                ContributionDate = viewModel.ContributionDate,
                IsPublished = viewModel.IsPublished
            };
        }

        internal static ArticleViewModel ToArticleViewModel(this Data.ArticleClip model, OceanusEntities database, MembershipUser currUserMembership)
        {
            bool currUserLikesArticle = false;
            Guid? currUserGuid = (currUserMembership != null ? (Guid?)currUserMembership.ProviderUserKey : null);

            if (currUserGuid != null)
            {
                var existingLike = database.UserArticleClipLikes.Where(like => like.ContributingUserId == currUserGuid && like.ArticleClipId == model.ArticleId).FirstOrDefault();

                if (existingLike != null)
                {
                    currUserLikesArticle = true;
                }
            }

            ArticleViewModel result = new ArticleViewModel(model.ArticleId, model.WebPageId, model.ArticleMarkup, model.ShareArticleMarkup, model.ArticleTitle, model.ArticleText, model.ArticleType, model.IsPublished, model.ContributingUserId, model.ContributionDate, currUserLikesArticle);
            result.WebPage = model.WebPage.ToBaseWebPageViewModel();

            IEnumerable<BaseUserViewModel> userMappingsList = model.ArticleClipUserMappings.Select(map => map.aspnet_Users.UserProfiles.First().ToBaseUserViewModel());
            result.UserMappings = userMappingsList;

            return result;
        }
    }
}