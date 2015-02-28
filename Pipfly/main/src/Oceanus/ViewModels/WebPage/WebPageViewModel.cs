using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class WebPageViewModel : BaseWebPageViewModel
    {
        /// <summary>
        /// Number of posts on this page
        /// </summary>
        public int NumPosts { get; set; }
        /// <summary>
        /// Number of times this page has been liked
        /// </summary>
        public int NumLikes { get; set; }
        /// <summary>
        /// Number of recommendations this page has received
        /// </summary>
        public int NumRecommendations { get; set; }

        public WebPageViewModel(string pageTitle_, string webDomainUrl_, string pageUrl_, string pageDescription_, Guid contributingUserId_, DateTime addedOn_)
        {
            PageTitle = pageTitle_;
            PageDescription = pageDescription_;
            WebDomainUrl = webDomainUrl_;
            PageUrl = pageUrl_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
        }

        public WebPageViewModel(long? webPageId_, string webDomainUrl_, string pageUrl_, string pageTitle_, string pageDescription_, Guid contributingUserId_, DateTime addedOn_, bool isFlaggedAsSpam_, 
            int numPosts_, int numLikes_, int numRecommendations_)
        {
            WebPageId = webPageId_;
            WebDomainUrl = webDomainUrl_;
            PageUrl = pageUrl_;
            PageTitle = pageTitle_;
            PageDescription = pageDescription_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
            IsFlaggedAsSpam = isFlaggedAsSpam_;
            NumPosts = numPosts_;
            NumLikes = numLikes_;
            NumRecommendations = numRecommendations_;
        }
    }
}