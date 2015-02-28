using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class VendorImageShowcaseViewModel
    {
        /// <summary>
        /// Reference to the vendor's ID
        /// </summary>
        public int VendorId { get; set; }
        /// <summary>
        /// Name of the vendor
        /// </summary>
        public string VendorName { get; set; }
        /// <summary>
        /// Portfolio image title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Reference to the image's URL on the server
        /// </summary>
        public string ImageUrl { get; set; }

        public VendorImageShowcaseViewModel(int vendorId_, string vendorName_, string title_, string imageURL_)
        {
            VendorId = vendorId_;
            VendorName = vendorName_;
            Title = title_;
            ImageUrl = imageURL_;
        }
    }
}