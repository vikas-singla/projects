using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ShareLinkRecipientViewModel
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Facebook id of the user
        /// </summary>
        public long fbuid { get; set; }
        /// <summary>
        /// Full name of the user
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Email of the user
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Filename of the user profile image
        /// </summary>
        public String userprofileimageurl { get; set; }
    }
}