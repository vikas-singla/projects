using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string message)
        {
            ErrorMessageViewModel errorViewModel = new ErrorMessageViewModel(message);

            return View("FriendlyError", errorViewModel);
        }
    }
}
