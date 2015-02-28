using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BasePhotoVideoClipViewModel : BaseClipAndArticleViewModel
    {
        /// <summary>
        /// Id of the photo/video clip
        /// </summary>
        public long? ClipId { get; set; }
        /// <summary>
        /// Reference to the web page that is the source of the clip
        /// </summary>
        public long WebPageId { get; set; }
        /// <summary>
        /// Source of the image/video
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// Url of the image in the clip
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Embed Url of the video in the clip
        /// </summary>
        public string VideoEmbedUrl { get; set; }
        /// <summary>
        /// Id of the video in the clip
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// Provider of the video in the clip (e.g. YouTube)
        /// </summary>
        public string VideoProviderId { get; set; }
        /// <summary>
        /// Full name of the contributing user
        /// </summary>
        public string ContributingUserFullName { get; set; }
        /// <summary>
        /// Width of the image
        /// </summary>
        public short? ImageWidth { get; set; }
        /// <summary>
        /// Height of the image
        /// </summary>
        public short? ImageHeight { get; set; }

        public BasePhotoVideoClipViewModel(long? clipId_, long webPageId_, string sourceUrl_, string imageUrl_, string videoEmbedUrl_, string videoId_, string videoProviderId_,
            Guid contributingUserId_, DateTime contributionDate_, short? imageWidth_, short? imageHeight_)
        {
            ClipId = clipId_;
            WebPageId = webPageId_;
            SourceUrl = sourceUrl_;
            ImageUrl = imageUrl_;
            VideoEmbedUrl = videoEmbedUrl_;
            VideoId = videoId_;
            VideoProviderId = videoProviderId_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            ImageWidth = imageWidth_;
            ImageHeight = imageHeight_;
        }
    }
}