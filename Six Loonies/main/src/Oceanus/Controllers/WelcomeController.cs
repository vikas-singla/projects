using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Web.Security;

namespace Oceanus.Controllers
{
    public class WelcomeController : ControllerBase
    {
        //
        // GET: /Welcome/
        [Authorize]
        public ActionResult Step1()
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            UserProfile userProfileModel = Database.UserProfiles.Where(profile => profile.UserId == userGuid).FirstOrDefault();
            BaseUserViewModel userProfileViewModel = null;

            if (userProfileModel != null)
            {
                userProfileViewModel = userProfileModel.ToBaseUserViewModel();
            }

            return View("_GetStarted", "_WelcomeLayout", userProfileViewModel);
        }

        [Authorize]
        public ActionResult Step2()
        {
            return View("_WelcomeInviteFriends", "_WelcomeLayout");
        }
    }
}
