using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class WebPageTagMappingViewModel
    {
        /// <summary>
        /// Id of the topic-webpage mapping
        /// </summary>
        public long? TagMappingId { get; set; }
        /// <summary>
        /// Reference to the topic object
        /// </summary>
        public int TagId { get; set; }
        /// <summary>
        /// Reference to the web page associated with the topic
        /// </summary>
        public long WebPageId { get; set; }
        /// <summary>
        /// Reference to the user who associated the topic to the page
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date of contribution for this topic association to the page
        /// </summary>
        public DateTime ContributionDate { get; set; }

        public WebPageTagMappingViewModel(long? tagMappingId_, int tagId_, long webPageId_, Guid contributingUserId_, DateTime contributionDate_)
        {
            TagMappingId = tagMappingId_;
            TagId = tagId_;
            WebPageId = webPageId_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
        }
    }
}