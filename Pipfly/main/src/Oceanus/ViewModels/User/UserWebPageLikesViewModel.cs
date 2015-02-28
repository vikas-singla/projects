using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class UserWebPageLikesViewModel
    {
        /// <summary>
        /// Id of the like rating
        /// </summary>
        public int? UserLikesId { get; set; }

        /// <summary>
        /// Reference to the user who gave the like rating
        /// </summary>
        public Guid ContributingUserId { get; set; }

        /// <summary>
        /// Date timestamp for when the like rating was given
        /// </summary>
        public DateTime AddedOn { get; set; }
        
        /// <summary>
        /// Reference to the webpage that the like rating applies to
        /// </summary>
        public long WebPageId { get; set; }

        public UserWebPageLikesViewModel()
        {
        }

        public UserWebPageLikesViewModel(int? userLikesId_, Guid contributingUserId_, DateTime addedOn_, long webPageId_)
        {
            UserLikesId = userLikesId_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
            WebPageId = webPageId_;
        }
    }
}