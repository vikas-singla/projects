using System.Web.Mvc;
using System.Web.Security;

namespace Oceanus.Attributes
{
    /// <summary>
    /// Checks the User's authentication using FormsAuthentication
    /// and redirects to the Login Url for the application on fail
    /// http://blog.wekeroad.com/blog/aspnet-mvc-securing-your-controller-actions/
    /// Adapted for MVC 3
    /// </summary>
    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //redirect if not authenticated
            if (!actionContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //use the current url for the redirect
                string redirectOnSuccess = actionContext.HttpContext.Request.Url.AbsolutePath;

                //send them off to the login page
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                actionContext.HttpContext.Response.Redirect(loginUrl, true);

            }
        }
    }
}