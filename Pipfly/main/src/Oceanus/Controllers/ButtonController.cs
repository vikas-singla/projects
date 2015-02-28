using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oceanus.Controllers
{
    public class ButtonController : Controller
    {
        //
        // GET: /Button/

        public ActionResult Index()
        {
            return View("_InstallButton");
        }

        public ActionResult Login(string pageUrl)
        {
            ViewBag.WebDomainUrl = pageUrl;

            return View("_BookmarkletLogin", "_BookmarkletLoginLayout");
        }
    }
}
