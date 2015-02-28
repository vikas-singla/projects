//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Oceanus.ViewModels.Shared;

//namespace Oceanus.ViewModels
//{
//    public class BaseListItemViewModel : ViewModelBase
//    {
//        /// <summary>
//        /// Reference to the vendor's ID
//        /// </summary>
//        public int VendorId { get; set; }

//        /// <summary>
//        /// Name of the vendor
//        /// </summary>
//        public string VendorName { get; set; }

//        /// <summary>
//        /// Short description of the vendor
//        /// </summary>
//        public string VendorShortDescription { get; set; }

//        /// <summary>
//        /// A sample image from the vendor's portfolio
//        /// </summary>
//        public string VendorPortfolioImage { get; set; }

//        /// <summary>
//        /// Reference to one of the vendor videos (Value is null if VendorPortfolioImage is not null)
//        /// </summary>
//        public VendorVideosViewModels SampleVendorVideo { get; set; }
        
//        /// <summary>
//        /// Number of times the vendor's profile has been viewed
//        /// </summary>
//        public int NumberOfProfileViews { get; set; }

//        /// <summary>
//        /// Number of likes the vendor has
//        /// </summary>
//        public int NumberOfLikes { get; set; }

//        /// <summary>
//        /// Number of likes for the vendor by the user's friends
//        /// </summary>
//        public int NumberOfLikesByFriends { get; set; }

//        /// <summary>
//        /// Number of tips the vendor has
//        /// </summary>
//        public int NumberOfTips { get; set; }

//        /// <summary>
//        /// Number of tips for the vendor by the user's friends
//        /// </summary>
//        public int NumberOfTipsByFriends { get; set; }

//        /// <summary>
//        /// Number of tips for the vendor by the user's friends of friends
//        /// </summary>
//        public int NumberOfTipsByFriendsOfFriends { get; set; }

//        /// <summary>
//        /// Flag indicating if the currently logged in user has marked this vendor as a 'like'
//        /// </summary>
//        public bool CurrentUserLikesVendor { get; set; }

//        /// <summary>
//        /// List of the vendor's services
//        /// </summary>
//        public List<string> ServicesOffered { get; set; }
//    }
//}