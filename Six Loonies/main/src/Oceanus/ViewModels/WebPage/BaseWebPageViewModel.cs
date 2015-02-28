using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BaseWebPageViewModel
    {
        /// <summary>
        /// Id of the page
        /// </summary>
        public long? WebPageId { get; set; }
        /// <summary>
        /// Url of the page domain
        /// </summary>
        public string WebDomainUrl { get; set; }
        /// <summary>
        /// Url of the Page
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// Title of the page
        /// </summary>
        public string PageTitle { get; set; }
        /// <summary>
        /// Description of the page
        /// </summary>
        public string PageDescription { get; set; }
        /// <summary>
        /// First user who initiated activity on this page
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date when this first catalogued
        /// </summary>
        public DateTime AddedOn { get; set; }
        /// <summary>
        /// Flag: Is this page spam?
        /// </summary>
        public bool IsFlaggedAsSpam { get; set; }

        public BaseWebPageViewModel()
        {
        }

        public BaseWebPageViewModel(long? webPageId_, string webDomainUrl_, string pageUrl_, string pageTitle_, string pageDescription_, Guid contributingUserId_, DateTime addedOn_, bool isFlaggedAsSpam_)
        {
            WebPageId = webPageId_;
            WebDomainUrl = webDomainUrl_;
            PageUrl = pageUrl_;
            PageTitle = pageTitle_;
            PageDescription = pageDescription_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
            IsFlaggedAsSpam = isFlaggedAsSpam_;
        }
    }
}