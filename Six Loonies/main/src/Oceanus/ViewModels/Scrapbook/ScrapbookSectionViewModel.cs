using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ScrapbookSectionViewModel
    {
        /// <summary>
        /// Id of the section
        /// </summary>
        public int? ScrapbookSectionId { get; set; }
        /// <summary>
        /// Name of the section
        /// </summary>
        public string ScrapbookSectionName { get; set; }
        /// <summary>
        /// Date when this section was created
        /// </summary>
        public DateTime AddedOn { get; set; }
        /// <summary>
        /// Reference to the user who owns this section
        /// </summary>
        public Guid SectionOwnerUserId { get; set; }

        public ScrapbookSectionViewModel(int? scrapbookSectionId_, string scrapbookSectionName_, DateTime addedOn_, Guid sectionOwnerUserId_)
        {
            ScrapbookSectionId = scrapbookSectionId_;
            ScrapbookSectionName = scrapbookSectionName_;
            AddedOn = addedOn_;
            SectionOwnerUserId = sectionOwnerUserId_;
        }
    }
}