using Oceanus.ViewModels;
using System.Linq;
using System.Diagnostics;
using Oceanus.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace Oceanus.Data
{
    internal static class ViewModelExtensions
    {
        internal static BaseTagViewModel ToBaseTagViewModel(this Data.Tag model)
        {
            return new BaseTagViewModel(model.TagId, model.TagName, model.LowerCaseTagName, model.ContributingUserId, model.AddedOn);
        }

        internal static TagViewModel ToTagViewModel(this Data.Tag model, OceanusEntities database, Guid? loggedInUserGuid)
        {
            int numFollowers = model.UserToTagFollowingMappings.Count;
            int numClips = model.PhotoVideoClipTagMappings.Count;
            int numArticles = model.ArticleClipTagMappings.Where(clip => clip.ArticleClip.IsPublished).Count();
            PhotoVideoClip photoViewModel = database.PhotoVideoClips.Where(clip => clip.PhotoVideoClipTagMappings.Where(map => map.TagId == model.TagId).Count() > 0 && clip.ImageWidth > 250 && clip.ImageHeight > 250).OrderBy(o => Guid.NewGuid()).FirstOrDefault();
            BasePhotoVideoClipViewModel basePhotoViewModel = (photoViewModel == null) ? null : photoViewModel.ToBasePhotoVideoClipViewModel();

            bool currUserIsFollower = false;

            if (loggedInUserGuid != null)
            {
                currUserIsFollower = model.UserToTagFollowingMappings.Where(map => map.FollowingUserId == loggedInUserGuid).Count() > 0;
            }

            TagViewModel viewModel = new TagViewModel(model.TagId, model.TagName, model.LowerCaseTagName, model.ContributingUserId, model.AddedOn, numFollowers, numClips, numArticles, currUserIsFollower);
            viewModel.TagPhoto = basePhotoViewModel;

            return viewModel;
        }

        internal static Tag ToTag(this BaseTagViewModel viewModel, OceanusEntities database)
        {
            return new Tag
            {
                TagId = viewModel.Id != null? (int) viewModel.Id : 0,
                TagName = viewModel.TagName.Trim(),
                LowerCaseTagName = viewModel.LowerTagName,
                ContributingUserId = viewModel.ContributingUserId,
                AddedOn = viewModel.AddedOn
            };
        }

        internal static CategoryViewModel ToCategoryViewModel(this Data.Category category)
        {
            var viewModel =
                new CategoryViewModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        ContributingUserId = category.ContributingUserId,
                        AddedOn = category.AddedOn,
                        IsVisibleOnListingPg = category.IsVisibleOnListingPg
                    };

            return viewModel;
        }

        internal static Category ToCategoryModel(this CategoryViewModel viewModel)
        {
            return new Category()
            {
                Id = viewModel.Id == null? 0 : (int) viewModel.Id,
                Name = viewModel.Name,
                ContributingUserId = viewModel.ContributingUserId,
                AddedOn = viewModel.AddedOn,
                IsVisibleOnListingPg = viewModel.IsVisibleOnListingPg
            };
        }
    }
}