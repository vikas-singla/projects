using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;

namespace Oceanus.Data
{
    internal static class BookmarkViewModelExtension
    {
        internal static BookmarkMappingsViewModel ToBookmarkViewModel(this Data.BookmarkMapping model)
        {
            return new BookmarkMappingsViewModel(model.BookmarkId, model.UserId, model.WebPageId, model.AddedOn);
        }

        internal static BookmarkMapping ToBookmarkModel(this BookmarkMappingsViewModel viewModel)
        {
            return new BookmarkMapping()
            {
                BookmarkId = (viewModel.BookmarkId == null ? 0 : (long)viewModel.BookmarkId),
                WebPageId = viewModel.WebPageId,
                UserId = viewModel.UserId,
                AddedOn = viewModel.AddedOn
            };
        }
    }
}