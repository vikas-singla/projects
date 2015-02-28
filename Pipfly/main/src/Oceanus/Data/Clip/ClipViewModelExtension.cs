using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels.Shared;
using Oceanus.ViewModels;

namespace Oceanus.Data
{
    internal static class ClipViewModelExtension
    {
        internal static ClipDescriptionViewModel ToClipDescriptionViewModel(this PhotoVideoClipDescription model)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);

            return new ClipDescriptionViewModel(model.Id, model.DescriptionText, model.ContributingUserId, model.ContributionDate, userProfile, model.ClipId);
        }

        internal static PhotoVideoClipDescription ToClipCommentModel(this ClipDescriptionViewModel viewModel)
        {
            return new PhotoVideoClipDescription()
            {
                Id = viewModel.Id == null ? 0 : (int)viewModel.Id,
                ContributingUserId = viewModel.ContributingUserId,
                DescriptionText = viewModel.DescriptionText,
                ContributionDate = viewModel.ContributionDate,
                ClipId = viewModel.ClipId
            };
        }

        internal static ClipCommentViewModel ToClipCommentViewModel(this PhotoVideoClipComment model)
        {
            BaseUserViewModel userProfile = (model.aspnet_Users.UserProfiles.Count > 0 ? model.aspnet_Users.UserProfiles.First().ToBaseUserViewModel() : null);

            return new ClipCommentViewModel(model.Id, model.CommentText, model.ContributingUserId, model.ContributionDate, userProfile, model.ClipId);
        }

        internal static PhotoVideoClipComment ToClipCommentModel(this ClipCommentViewModel viewModel)
        {
            return new PhotoVideoClipComment()
            {
                Id = viewModel.Id == null? 0 : (int)viewModel.Id,
                ContributingUserId = viewModel.ContributingUserId,
                CommentText = viewModel.CommentText,
                ContributionDate = viewModel.ContributionDate,
                ClipId = viewModel.ClipId
            };
        }

        internal static BasePhotoVideoClipViewModel ToBasePhotoVideoClipViewModel(this Data.PhotoVideoClip model)
        {
            BasePhotoVideoClipViewModel result = new BasePhotoVideoClipViewModel(model.ClipId, model.WebPageId, model.SourceUrl, model.ImageUrl, model.VideoEmbedUrl, model.VideoId, model.VideoProviderId,
                model.ContributingUserId, model.ContributionDate, model.ImageWidth, model.ImageHeight);

            result.ContributingUserFullName = model.aspnet_Users.UserProfiles.FirstOrDefault().FullName;

            return result;
        }

        internal static PhotoVideoClipViewModel ToPhotoVideoClipViewModel(this Data.PhotoVideoClip model, OceanusEntities database)
        {
            PhotoVideoClipViewModel result = new PhotoVideoClipViewModel(model.ClipId, model.WebPageId, model.SourceUrl, model.ImageUrl, model.VideoEmbedUrl, model.VideoId, model.VideoProviderId,
                model.ContributingUserId, model.ContributionDate, model.ImageWidth, model.ImageHeight);

            result.ContributingUserFullName = model.aspnet_Users.UserProfiles.FirstOrDefault().FullName;
            result.AssociatedDescriptions = model.PhotoVideoClipDescriptions.Select(desc => desc.ToClipDescriptionViewModel());

            //retrieve tags associated with the clip
            IEnumerable<BaseTagViewModel> associatedTags = model.PhotoVideoClipTagMappings.Select(t => t.Tag.ToBaseTagViewModel());
            result.AssociatedTags = associatedTags;

            return result;
        }

        internal static DetailedPhotoVideoClipViewModel ToDetailedPhotoVideoClipViewModel(this Data.PhotoVideoClip model, OceanusEntities database)
        {
            DetailedPhotoVideoClipViewModel result = new DetailedPhotoVideoClipViewModel(model.ClipId, model.WebPageId, model.SourceUrl, model.ImageUrl, model.VideoEmbedUrl, model.VideoId, model.VideoProviderId,
                model.ContributingUserId, model.ContributionDate, model.ImageWidth, model.ImageHeight, model.WebPage.PageUrl);

            result.ContributingUserFullName = model.aspnet_Users.UserProfiles.FirstOrDefault().FullName;
            result.AssociatedDescriptions = model.PhotoVideoClipDescriptions.Select(desc => desc.ToClipDescriptionViewModel());

            //retrieve tags associated with the clip
            IEnumerable<BaseTagViewModel> associatedTags = model.PhotoVideoClipTagMappings.Select(t => t.Tag.ToBaseTagViewModel());
            //retrieve users who have clipped this clip
            IEnumerable<BaseUserViewModel> clippingUsers = model.PhotoVideoClipUserMappings.Where(clip => clip.PhotoVideoClipId == model.ClipId).AsEnumerable().Select(map => map.aspnet_Users.UserProfiles.First().ToBaseUserViewModel());

            result.AssociatedTags = associatedTags;
            result.ClippingUsers = clippingUsers;
            return result;
        }

        internal static PhotoVideoClip ToPhotoVideoClipModel(this PhotoVideoClipViewModel viewModel)
        {
            return new PhotoVideoClip()
            {
                ClipId = (viewModel.ClipId == null ? 0 : (long)viewModel.ClipId),
                WebPageId = viewModel.WebPageId,
                SourceUrl = viewModel.SourceUrl,
                ImageWidth = viewModel.ImageWidth,
                ImageHeight = viewModel.ImageHeight,
                ImageUrl = viewModel.ImageUrl,
                VideoEmbedUrl = viewModel.VideoEmbedUrl,
                VideoId = viewModel.VideoId,
                VideoProviderId = viewModel.VideoProviderId,
                ContributingUserId = viewModel.ContributingUserId,
                ContributionDate = viewModel.ContributionDate
            };
        }
    }
}