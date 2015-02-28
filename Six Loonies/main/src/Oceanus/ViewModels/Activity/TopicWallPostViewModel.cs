using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class WebPostViewModel : BasePostViewModel
    {
        public WebPageViewModel AssociatedWebPage { get; set; }

        public WebPostViewModel(int? id_, string wallText_, Guid contributingUserId_, DateTime contributionDate_, BaseUserViewModel contributingUserProfile_,
            long webPageId_, WebPageViewModel associatedWebPage_) : base(id_, wallText_, contributingUserId_, contributionDate_, contributingUserProfile_,
            webPageId_)
        {
            AssociatedWebPage = associatedWebPage_;
        }
    }
}