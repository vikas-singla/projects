using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BaseClipAndArticleViewModel
    {
        /// <summary>
        /// Reference to the user who contributed the clip
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date when this clip was added
        /// </summary>
        public DateTime ContributionDate { get; set; }
    }
}