//using Oceanus.ViewModels;
//using System.Linq;
//using System.Diagnostics;
//using Oceanus.ViewModels.Shared;
//using System;
//using System.Collections.Generic;

//namespace Oceanus.Data
//{
//    public static class ListingViewModelExtension
//    {
//        internal static ListItemViewModel ToListingViewModel(this Data.Vendor vendor, OceanusEntities database, Guid? loggedInUserGuid, IEnumerable<Guid> userFriendIDs, IEnumerable<Guid?> userFriendOfFriendsIDs)
//        {
//            // TODO: determine criteria for top services and pull them out
//            // Here's how to do it: http://stackoverflow.com/questions/1387110/linq-to-sql-get-top-10-most-ordered-products
//            // for now we just grab all services

//            var servicesOffered = vendor.Services.Select(s => s.Name).ToList();
            
//            if(loggedInUserGuid != null && userFriendIDs == null)
//            {
//                userFriendIDs = database.SixLooniesFriendConnections.Where(con => con.FirstUserId == loggedInUserGuid || con.SecondUserId == loggedInUserGuid).Select(con => (con.FirstUserId == loggedInUserGuid ? con.SecondUserId : con.FirstUserId)).AsEnumerable();                
//            }
//            if (loggedInUserGuid != null && userFriendOfFriendsIDs == null)
//            {
//                userFriendOfFriendsIDs = database.GetFriendsOfFriendIDs((Guid)loggedInUserGuid).AsEnumerable();
//            }

//            int numVendorImages = vendor.VendorImages.Count;
//            string vendorShortDesc = (vendor.Description != null ? (vendor.Description.Length > 200 ? vendor.Description.Substring(0, 200) : vendor.Description) : null);
//            vendorShortDesc = (vendorShortDesc != null && vendorShortDesc.LastIndexOf(' ') > 0) ? (vendorShortDesc.Substring(0, vendorShortDesc.LastIndexOf(' ') + 1) + "...") : vendorShortDesc;

//            var viewModel =
//                new ListItemViewModel()
//                {
//                    AssociatedCategories = vendor.VendorCategoryMappings.Select(map => map.Category.ToCategoryViewModel()),
//                    VendorId = vendor.Id,
//                    VendorName = vendor.Name,
//                    VendorShortDescription = vendorShortDesc,                    
//                    VendorPortfolioImage = (numVendorImages > 0 ? vendor.VendorImages.OrderBy(i => Guid.NewGuid()).First().ImageUrl : null),
//                    SampleVendorVideo = ((numVendorImages <= 0 && vendor.VendorVideos.Count > 0) ? vendor.VendorVideos.OrderBy(i => Guid.NewGuid()).First().ToVendorVideosViewModel() : null),
//                    ServicesOffered = servicesOffered,
//                    NumberOfProfileViews = 0,
//                    NumberOfLikes = vendor.UserLikes.Count,
//                    CurrentUserLikesVendor = (loggedInUserGuid == null ? false : (vendor.UserLikes.Where(l => l.ContributingUserId == loggedInUserGuid).FirstOrDefault() != null)),
//                    NumberOfLikesByFriends = (userFriendIDs == null ? 0 : vendor.UserLikes.Where(l => userFriendIDs.Contains(l.ContributingUserId)).Count()),
//                    NumberOfTips = vendor.Tips.Count,
//                    NumberOfTipsByFriends = (userFriendIDs == null ? 0 : vendor.Tips.Where(r => userFriendIDs.Contains(r.ContributingUserId)).Count()),
//                    NumberOfTipsByFriendsOfFriends = (userFriendOfFriendsIDs == null ? 0 : vendor.Tips.Where(r => userFriendOfFriendsIDs.Contains(r.ContributingUserId)).Count())
//                };

//            return viewModel;
//        }

//        internal static BaseListItemViewModel ToBaseListingViewModel(this Data.Vendor vendor, OceanusEntities database, Guid? loggedInUserGuid, IEnumerable<Guid> userFriendIDs, IEnumerable<Guid?> userFriendOfFriendsIDs)
//        {
//            // TODO: determine criteria for top services and pull them out
//            // Here's how to do it: http://stackoverflow.com/questions/1387110/linq-to-sql-get-top-10-most-ordered-products
//            // for now we just grab all services

//            var servicesOffered = vendor.Services.Select(s => s.Name).ToList();

//            if (loggedInUserGuid != null && userFriendIDs == null)
//            {
//                userFriendIDs = database.SixLooniesFriendConnections.Where(con => con.FirstUserId == loggedInUserGuid || con.SecondUserId == loggedInUserGuid).Select(con => (con.FirstUserId == loggedInUserGuid ? con.SecondUserId : con.FirstUserId)).AsEnumerable();
//            }
//            if (loggedInUserGuid != null && userFriendOfFriendsIDs == null)
//            {
//                userFriendOfFriendsIDs = database.GetFriendsOfFriendIDs((Guid)loggedInUserGuid).AsEnumerable();
//            }

//            int numVendorImages = vendor.VendorImages.Count;
//            string vendorShortDesc = (vendor.Description != null ? (vendor.Description.Length > 200 ? vendor.Description.Substring(0, 200) : vendor.Description) : null);
//            vendorShortDesc = (vendorShortDesc != null && vendorShortDesc.LastIndexOf(' ') > 0) ? (vendorShortDesc.Substring(0, vendorShortDesc.LastIndexOf(' ') + 1) + "...") : vendorShortDesc;

//            var viewModel =
//                new BaseListItemViewModel()
//                {
//                    VendorId = vendor.Id,
//                    VendorName = vendor.Name,
//                    ServicesOffered = servicesOffered,
//                    VendorPortfolioImage = (numVendorImages > 0 ? vendor.VendorImages.OrderBy(i => Guid.NewGuid()).First().ImageUrl : null),
//                    SampleVendorVideo = ((numVendorImages <= 0 && vendor.VendorVideos.Count > 0) ? vendor.VendorVideos.OrderBy(i => Guid.NewGuid()).First().ToVendorVideosViewModel() : null),
//                    NumberOfProfileViews = 0,
//                    VendorShortDescription = vendorShortDesc,
//                    NumberOfLikes = vendor.UserLikes.Count,
//                    CurrentUserLikesVendor = (loggedInUserGuid == null ? false : (vendor.UserLikes.Where(l => l.ContributingUserId == loggedInUserGuid).FirstOrDefault() != null)),
//                    NumberOfLikesByFriends = (userFriendIDs == null ? 0 : vendor.UserLikes.Where(l => userFriendIDs.Contains(l.ContributingUserId)).Count()),
//                    NumberOfTips = vendor.Tips.Count,
//                    NumberOfTipsByFriends = (userFriendIDs == null ? 0 : vendor.Tips.Where(r => userFriendIDs.Contains(r.ContributingUserId)).Count()),
//                    NumberOfTipsByFriendsOfFriends = (userFriendOfFriendsIDs == null ? 0 : vendor.Tips.Where(r => userFriendOfFriendsIDs.Contains(r.ContributingUserId)).Count())
//                };

//            return viewModel;
//        }

//    }
//}