using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ScrapbookItemViewModel
    {
        /// <summary>
        /// Id of the scrapbook item
        /// </summary>
        public long? ScrapbookItemId { get; set; }
        /// <summary>
        /// Reference to the scrapbook item section
        /// </summary>
        public int ScrapbookSectionId { get; set; }
        /// <summary>
        /// Reference to the image in the item
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Description of the scrapbook item
        /// </summary>
        public string ItemDescription { get; set; }
        /// <summary>
        /// Reference to the user who created this item
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date when this item was created
        /// </summary>
        public DateTime AddedOn { get; set; }

        public ScrapbookItemViewModel(long? scrapbookItemId_, int scrapbookSectionId_, string imageUrl_, string itemDescription_, Guid contributingUserId_,
            DateTime addedOn_)
        {
            ScrapbookItemId = scrapbookItemId_;
            ScrapbookSectionId = scrapbookSectionId_;
            ImageUrl = imageUrl_;
            ItemDescription = itemDescription_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
        }
    }
}