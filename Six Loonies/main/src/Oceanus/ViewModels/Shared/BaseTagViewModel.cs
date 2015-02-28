using System;

namespace Oceanus.ViewModels.Shared
{
    public class BaseTagViewModel
    {
        public int? Id { get; set; }
        /// <summary>
        /// Tag value
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// Lower case tag value
        /// </summary>
        public string LowerTagName { get; set; }
        /// <summary>
        /// Search (indexed) tag value
        /// </summary>
        public string SearchTag { get; set; }
        /// <summary>
        /// Reference to the user who added the tag
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date Timestamp for when the tag was added
        /// </summary>
        public DateTime AddedOn { get; set; }

        public BaseTagViewModel()
        {
        }

        public BaseTagViewModel(int? id_, string tagName_, string lowerCaseTagName_, Guid contributingUserId_, DateTime addedOn_)
        {
            Id = id_;
            TagName = tagName_;
            LowerTagName = lowerCaseTagName_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
        }
    }
}