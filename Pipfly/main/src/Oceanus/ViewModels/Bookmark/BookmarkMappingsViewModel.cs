using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BookmarkMappingsViewModel
    {
        /// <summary>
        /// Id of the bookmark
        /// </summary>
        public long? BookmarkId { get; set; }
        /// <summary>
        /// User whose bookmark it is
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Reference to the web page that is bookmarked
        /// </summary>
        public long WebPageId { get; set; }
        /// <summary>
        /// Date timestamp of when the bookmark was added
        /// </summary>
        public DateTime AddedOn { get; set; }

        public BookmarkMappingsViewModel(long? bookmarkId_, Guid userId_, long webPageId_, DateTime addedOn_)
        {
            BookmarkId = bookmarkId_;
            UserId = userId_;
            WebPageId = webPageId_;
            AddedOn = addedOn_;
        }
    }
}