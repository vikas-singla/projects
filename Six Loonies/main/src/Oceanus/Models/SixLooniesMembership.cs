using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.Models
{
    public class SixLooniesMembership
    {
        public enum UserRoles
        {
            Moderator,
            SuperAdmin,
            User,
            Vendor
        };

        public static string NoPermissionMessage = "We're sorry but you do not have permission to view this profile.";
        public static string VendorProfileWaitingApprovalMessage = "We're sorry but this profile is currently in private mode by the author. Please check back again shortly.";
    }
}