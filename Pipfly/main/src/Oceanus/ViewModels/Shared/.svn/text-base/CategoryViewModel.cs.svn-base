using System;

namespace Oceanus.ViewModels.Shared
{
    public class CategoryViewModel : ViewModelBase
    {
        /// <summary>
        /// Reference to the user who added the tag
        /// </summary>
        public Guid ContributingUserId { get; set; }
        /// <summary>
        /// Date Timestamp for when the tag was added
        /// </summary>
        public DateTime AddedOn { get; set; }
        /// <summary>
        /// Flag indicating if this category is visible on the listing page
        /// </summary>
        public bool IsVisibleOnListingPg { get; set; }

        public CategoryViewModel()
        {
        }

        public CategoryViewModel(int? id_, string name_, Guid contributingUserId_, DateTime addedOn_, bool isVisibleOnListingPg_)
        {
            Id = id_;
            Name = name_;
            ContributingUserId = contributingUserId_;
            AddedOn = addedOn_;
            IsVisibleOnListingPg = isVisibleOnListingPg_;
        }
    }
}