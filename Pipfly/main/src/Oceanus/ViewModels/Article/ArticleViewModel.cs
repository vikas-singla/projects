using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.Controllers;
using Oceanus.Data;

namespace Oceanus.ViewModels
{
    public class ArticleViewModel : BaseClipAndArticleViewModel
    {
        /// <summary>
        /// ID of the article
        /// </summary>
        public long? ArticleId { get; set; }
        /// <summary>
        /// Reference to the web page that is the source of the article
        /// </summary>
        public long WebPageId { get; set; }
        /// <summary>
        /// Reference to the associated web page
        /// </summary>
        public BaseWebPageViewModel WebPage { get; set; }
        /// <summary>
        /// Markup of the content contained in the article
        /// </summary>
        public string ArticleMarkup { get; set; }
        /// <summary>
        /// Trimmed article markup (used for sharing)
        /// </summary>
        public string ShareArticleMarkup { get; set; }
        /// <summary>
        /// Title of the article
        /// </summary>
        public string ArticleTitle { get; set; }
        /// <summary>
        /// Text contained in the article (simple text)
        /// </summary>
        public string ArticleText { get; set; }
        /// <summary>
        /// Flag indicating if the article has been published or not (note: unpublished articles will not be visible to other users)
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// Type of article
        /// </summary>
        public ClipHelper.ArticleTypes? ArticleType { get; set; }
        /// <summary>
        /// Reference to the list of users that are contributors of this article
        /// </summary>
        public IEnumerable<BaseUserViewModel> UserMappings { get; set; }
        /// <summary>
        /// Indicates if the currently logged in user likes the article
        /// </summary>
        public bool UserLikesArticle { get; set; }

        public ArticleViewModel(long? articleId_, long webPageId_, string articleMarkup_, string shareArticleMarkup_, string articleTitle_, string articleText_, string articleType_, bool isPublished_, Guid contributingUserId_, DateTime contributionDate_,
            bool userLikesArticle_)
        {
            ArticleId = articleId_;
            WebPageId = webPageId_;
            ArticleMarkup = articleMarkup_;
            ShareArticleMarkup = shareArticleMarkup_;
            ArticleTitle = articleTitle_;
            ArticleText = articleText_;
            IsPublished = isPublished_;
            ContributingUserId = contributingUserId_;
            ContributionDate = contributionDate_;
            UserLikesArticle = userLikesArticle_;

            SetArticleType(articleType_);
        }

        internal void SetArticleType(string articleType_)
        {
            if (articleType_.Equals(ClipHelper.ArticleTypes.Fact.ToString()))
            {
                ArticleType = ClipHelper.ArticleTypes.Fact;
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.News.ToString()))
            {
                ArticleType = ClipHelper.ArticleTypes.News;
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.HowTo.ToString()))
            {
                ArticleType = ClipHelper.ArticleTypes.HowTo;
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.Opinion.ToString()))
            {
                ArticleType = ClipHelper.ArticleTypes.Opinion;
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.Other.ToString()))
            {
                ArticleType = ClipHelper.ArticleTypes.Other;
            }
        }

        internal static void SetArticleType(ArticleClip article_, string articleType_)
        {
            if (articleType_.Equals(ClipHelper.ArticleTypes.Fact.ToString()))
            {
                article_.ArticleType = ClipHelper.ArticleTypes.Fact.ToString();
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.News.ToString()))
            {
                article_.ArticleType = ClipHelper.ArticleTypes.News.ToString();
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.HowTo.ToString()))
            {
                article_.ArticleType = ClipHelper.ArticleTypes.HowTo.ToString();
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.Opinion.ToString()))
            {
                article_.ArticleType = ClipHelper.ArticleTypes.Opinion.ToString();
            }
            else if (articleType_.Equals(ClipHelper.ArticleTypes.Other.ToString()))
            {
                article_.ArticleType = ClipHelper.ArticleTypes.Other.ToString();
            }
        }
    }
}