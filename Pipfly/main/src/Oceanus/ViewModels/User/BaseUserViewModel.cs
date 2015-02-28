using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels.Shared;

namespace Oceanus.ViewModels
{
    public class BaseUserViewModel : ViewModelBase
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId;
        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName;
        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName;
        /// <summary>
        /// Filename of the user profile image
        /// </summary>
        public String UserProfileImageUrl;
    }
}