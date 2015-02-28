using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;

namespace Oceanus.Data
{
    //internal static class ScrapbookModelExtension
    //{
    //    internal static JsonScrapbookSection ToJsonScrapbookSection(this Data.ScrapbookSection model)
    //    {
    //        return new JsonScrapbookSection()
    //        {
    //            SectionId = model.ScrapbookSectionId,
    //            SectionName = model.ScrapbookSectionName
    //        };
    //    }

    //    internal static ScrapbookSectionViewModel ToScrapbookSectionViewModel(this Data.ScrapbookSection model)
    //    {
    //        return new ScrapbookSectionViewModel(model.ScrapbookSectionId, model.ScrapbookSectionName, model.AddedOn, model.SectionOwnerUserId);
    //    }

    //    internal static ScrapbookSection ToScrapbookSectionModel(this ScrapbookSectionViewModel viewModel)
    //    {
    //        return new ScrapbookSection()
    //        {
    //            ScrapbookSectionId = (viewModel.ScrapbookSectionId == null ? 0 : ((int) viewModel.ScrapbookSectionId)),
    //            ScrapbookSectionName = viewModel.ScrapbookSectionName,
    //            AddedOn = viewModel.AddedOn,
    //            SectionOwnerUserId = viewModel.SectionOwnerUserId
    //        };
    //    }

    //    internal static ScrapbookItemViewModel ToScrapbookItemViewModel(this Data.ScrapbookItem model)
    //    {
    //        return new ScrapbookItemViewModel(model.ScrapbookItemId, model.ScrapbookSectionId, model.ImageUrl, model.ItemDescription,
    //            model.ContributingUserId, model.AddedOn);
    //    }

    //    internal static ScrapbookItem ToScrapbookItemModel(this ScrapbookItemViewModel viewModel)
    //    {
    //        return new ScrapbookItem()
    //        {
    //            ScrapbookItemId = (viewModel.ScrapbookItemId == null ? 0 : ((long)viewModel.ScrapbookItemId)),
    //            ScrapbookSectionId = viewModel.ScrapbookSectionId,
    //            ContributingUserId = viewModel.ContributingUserId,
    //            AddedOn = viewModel.AddedOn,
    //            ItemDescription = viewModel.ItemDescription,
    //            ImageUrl = viewModel.ImageUrl
    //        };
    //    }
    //}
}