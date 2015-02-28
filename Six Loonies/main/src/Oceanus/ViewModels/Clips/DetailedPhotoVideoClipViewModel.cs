using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class DetailedPhotoVideoClipViewModel : PhotoVideoClipViewModel
    {
        public IEnumerable<BaseUserViewModel> ClippingUsers;
        /// <summary>
        /// Reference to the web page from which this media was clipped
        /// </summary>
        public string ClipWebPageSourceUrl { get; set; }

        public DetailedPhotoVideoClipViewModel(long? clipId_, long webPageId_, string sourceUrl_, string imageUrl_, string videoEmbedUrl_, string videoId_, string videoProviderId_,
            Guid contributingUserId_, DateTime contributionDate_, short? imageWidth_, short? imageHeight_, string clipWebPageSourceUrl_)
            : base(clipId_, webPageId_, sourceUrl_, imageUrl_, videoEmbedUrl_, videoId_,
                videoProviderId_, contributingUserId_, contributionDate_, imageWidth_, imageHeight_)
        {
            ClipWebPageSourceUrl = clipWebPageSourceUrl_;
        }
    }
}