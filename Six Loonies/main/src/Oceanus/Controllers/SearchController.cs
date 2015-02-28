using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Data;
using Oceanus.ViewModels.Shared;
using System.Linq.Expressions;
using MVCControlsToolkit.Linq;
using Oceanus.ViewModels;
using System.Text.RegularExpressions;

namespace Oceanus.Controllers
{
    public class SearchController : ControllerBase
    {
        public ActionResult Index(string searchTerm, ArticleController.ArticleTypes articleType, ControllerBase.SortContentCriteria? sortBy)
        {
            searchTerm = searchTerm.Replace("#", "").Trim();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.FilterByFriends = false;
            ViewBag.ArticleType = articleType;
            ViewBag.SortBy = sortBy;

            return View();
        }
    }
}
