using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Oceanus.Attributes
{
    /// <summary>
    /// Checks the User's role using FormsAuthentication
    /// and throws and UnauthorizedAccessException if not authorized
    /// http://blog.wekeroad.com/blog/aspnet-mvc-securing-your-controller-actions/
    /// Adapter for MVC 3
    /// </summary>
    public class RequiresRoleAttribute : ActionFilterAttribute
    {

        public string RoleToCheckFor { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //redirect if the user is not authenticated
            if (!String.IsNullOrEmpty(RoleToCheckFor))
            {

                if (!actionContext.HttpContext.User.Identity.IsAuthenticated)
                {

                    //use the current url for the redirect
                    string redirectOnSuccess = actionContext.HttpContext.Request.Url.AbsolutePath;

                    //send them off to the login page
                    string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                    string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                    actionContext.HttpContext.Response.Redirect(loginUrl, true);

                }
                else
                {
                    bool isAuthorized = actionContext.HttpContext.User.IsInRole(this.RoleToCheckFor);
                    if (!isAuthorized)
                        throw new UnauthorizedAccessException("You are not authorized to view this page");
                }
            }
            else
            {
                throw new InvalidOperationException("No Role Specified");
            }
        }
    }
}