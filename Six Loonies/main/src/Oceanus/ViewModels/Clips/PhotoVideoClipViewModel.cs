using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels.Shared;

namespace Oceanus.ViewModels
{
    public class PhotoVideoClipViewModel : BasePhotoVideoClipViewModel
    {
        /// <summary>
        /// Tags associated with the photo/video clip
        /// </summary>
        public IEnumerable<BaseTagViewModel> AssociatedTags { get; set; }
        /// <summary>
        /// Descriptions (Captions) associated with the clip
        /// </summary>
        public IEnumerable<ClipDescriptionViewModel> AssociatedDescriptions { get; set; }

        public PhotoVideoClipViewModel(long? clipId_, long webPageId_, string sourceUrl_, string imageUrl_, string videoEmbedUrl_, string videoId_, string videoProviderId_,
            Guid contributingUserId_, DateTime contributionDate_, short? imageWidth_, short? imageHeight_) : base(clipId_, webPageId_, sourceUrl_, imageUrl_, videoEmbedUrl_, videoId_,
            videoProviderId_, contributingUserId_, contributionDate_, imageWidth_, imageHeight_)
        {
        }
    }
}