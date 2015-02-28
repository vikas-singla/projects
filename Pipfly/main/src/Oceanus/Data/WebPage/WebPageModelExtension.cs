using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;

namespace Oceanus.Data
{
    internal static class WebPageModelExtension
    {
        internal static BaseWebPageViewModel ToBaseWebPageViewModel(this WebPage model)
        {
            return new BaseWebPageViewModel(model.Id, model.WebDomainUrl, model.PageUrl, model.PageTitle, model.PageDescription, model.ContributingUserId, model.AddedOn,
                model.IsFlaggedAsSpam);
        }

        internal static WebPageViewModel ToWebPageViewModel(this Data.WebPage model, OceanusEntities database)
        {
            WebPageViewModel result = null;

            int userLikesCount = database.UserWebPageLikes.Where(like => like.WebPageId == model.Id).Count();
            int pagePostCount = database.UserWallPosts.Where(posts => posts.WebPageId == model.Id).Count();

            result = new WebPageViewModel(model.Id, model.WebDomainUrl, model.PageUrl, model.PageTitle, model.PageDescription, model.ContributingUserId, model.AddedOn,
                model.IsFlaggedAsSpam, pagePostCount, userLikesCount, model.WebPageShares.Count);

            return result;
        }

        internal static WebPage ToWebPageModel(this WebPageViewModel viewModel)
        {
            return new WebPage()
            {
                Id = (viewModel.WebPageId == null ? 0 : (int) viewModel.WebPageId),
                PageTitle = viewModel.PageTitle,
                WebDomainUrl = viewModel.WebDomainUrl,
                PageUrl = viewModel.PageUrl,
                LowerPageUrl = viewModel.PageUrl.ToLower(),
                PageDescription = viewModel.PageDescription,
                ContributingUserId = viewModel.ContributingUserId,
                AddedOn = viewModel.AddedOn,
                IsFlaggedAsSpam = viewModel.IsFlaggedAsSpam
            };
        }

        internal static WebPageTagMappingViewModel ToWebPageTagMappingViewModel(this Data.WebPageTagMapping model)
        {
            return new WebPageTagMappingViewModel(model.TagMappingId, model.TagId, model.WebPageId, model.ContributingUserId, model.ContributionDate);
        }

        internal static WebPageTagMapping ToWebPageTagMappingModel(this WebPageTagMappingViewModel viewModel)
        {
            return new WebPageTagMapping()
            {
                TagMappingId = (viewModel.TagMappingId == null ? 0 : (long) viewModel.TagMappingId),
                TagId = viewModel.TagId,
                WebPageId = viewModel.WebPageId,
                ContributingUserId = viewModel.ContributingUserId,
                ContributionDate = viewModel.ContributionDate
            };
        }
    }
}