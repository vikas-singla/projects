using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Web;  
using System.Web.Mvc;  
  
namespace Oceanus.Controllers
{
    public class BrowserCacheAttribute : ActionFilterAttribute  
    {  
        /// <summary>  
        /// Gets or sets the cache duration in seconds.   
        /// The default is 10 seconds.  
        /// </summary>  
        /// <value>The cache duration in seconds.</value>  
        public int Duration  
        {  
            get;  
            set;  
        }  
  
        public bool PreventBrowserCaching   
        {   
            get;   
            set;   
        }  
  
        public BrowserCacheAttribute()  
        {  
            Duration = 10;  
        }  
  
        public override void OnActionExecuted(  
          ActionExecutedContext filterContext)  
        {  
            if (Duration < 0) return;  
  
            HttpCachePolicyBase cache = filterContext.HttpContext  
              .Response.Cache;  
  
            if (PreventBrowserCaching)  
            {  
                cache.SetCacheability(HttpCacheability.NoCache);  
                Duration = 0;  
            }  
            else  
            {  
                cache.SetCacheability(HttpCacheability.Public);  
            }  
  
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);  
            cache.SetExpires(DateTime.Now.Add(cacheDuration));  
            cache.SetMaxAge(cacheDuration);  
            cache.AppendCacheExtension("must-revalidate,"  
              + "proxy-revalidate");  
        }  
    }  
}  