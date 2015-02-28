using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Security;
using MVCControlsToolkit.Linq;
using Oceanus.Data;
using Oceanus.ViewModels;
using Oceanus.ViewModels.Shared;
using System.Web;

namespace Oceanus.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return View("Index", "_HomePageLayout");
            }

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            int tagFollowCount = Database.UserToTagFollowingMappings.Where(map => map.FollowingUserId == userGuid).Count();
            int userFollowCount = Database.UserToUserFollowingMappings.Where(map => map.FollowingUserId == userGuid).Count();

            int followingCount = tagFollowCount + userFollowCount;

            ViewBag.FollowCount = followingCount;

            if (followingCount == 0)
            {
                ViewBag.DisplaySpotlightPrompt = true;
            }
            else
            {
                ViewBag.DisplaySpotlightPrompt = false;
            }

            ViewBag.TagFollowCount = tagFollowCount;
            ViewBag.UserFollowCount = userFollowCount;
            ViewBag.ArticleType = ArticleController.ArticleTypes.All;
            ViewBag.SortBy = ControllerBase.SortContentCriteria.RecentlyAdded;

            return View("Spotlight", "_Layout");
        }

        [Authorize]
        public ActionResult Spotlight(ArticleController.ArticleTypes? articleType, ControllerBase.SortContentCriteria? sortBy)
        {
            if (articleType != null)
            {
                ViewBag.ArticleType = articleType;
            }
            else
            {
                ViewBag.ArticleType = ArticleController.ArticleTypes.All;
            }

            ViewBag.SortBy = sortBy;

            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userGuid = (Guid)user.ProviderUserKey;

            return View("Spotlight", "_Layout");
        }

        public ActionResult Ssshhhh()
        {
            return View("_Blank", "_BetaLayout");
        }
    }
}