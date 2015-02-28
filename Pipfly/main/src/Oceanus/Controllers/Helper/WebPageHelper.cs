using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.Data;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class WebPageHelper
    {
        internal static WebPage ValidateWebPage(Guid userGuid, string pageUrl, string pageTitle, string pageDescription, OceanusEntities database)
        {
            WebPage result = null;

            //create the URI
            pageUrl = pageUrl.IndexOf('#') > 0 ? (pageUrl.Substring(0, pageUrl.IndexOf('#'))) : pageUrl;
            Uri pageURI = new Uri(pageUrl);

            //check if this page has been added in the DB
            result = database.WebPages.Where(page => page.LowerPageUrl.Equals((pageURI.AbsoluteUri.ToLower()))).FirstOrDefault();

            if (result == null)
            {
                //first time activity on the page
                WebPageViewModel pageViewModel = new WebPageViewModel(pageTitle.Trim(), pageURI.Scheme + "://" + pageURI.DnsSafeHost +
                    (pageURI.IsDefaultPort ? "" : (":" + pageURI.Port)), pageURI.AbsoluteUri, pageDescription.Trim(), userGuid, DateTime.Now);
                result = pageViewModel.ToWebPageModel();

                database.WebPages.AddObject(result);
                database.SaveChanges();
            }

            return result;
        }
    }
}