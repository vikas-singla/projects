using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.ViewModels;
using System.Web.Security;
using Oceanus.Data;

namespace Oceanus.Controllers
{
    public static class UserHelper
    {
        internal static void AddEmailConfirmedCookie()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_EMAIL_CONFIRMED_KEY] == null)
            {
                HttpCookie cookie = new HttpCookie(CookieDefinitions.COOKIE_USER_EMAIL_CONFIRMED_KEY);
                cookie.Value = "Thank you for your email address confirmation.";
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_EMAIL_CONFIRMED_KEY];
                cookie.Value = "Thank you for your email address confirmation.";
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        internal static void AddUserProfileImageUrlCookie(string userProfileImageUrl)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_PROFILE_IMG_KEY] == null)
            {
                HttpCookie cookie = new HttpCookie(CookieDefinitions.COOKIE_USER_PROFILE_IMG_KEY);
                if (userProfileImageUrl != null)
                {
                    cookie.Value = userProfileImageUrl;
                }
                else
                {
                    cookie.Value = userProfileImageUrl;
                }

                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_PROFILE_IMG_KEY];
                if (userProfileImageUrl != null)
                {
                    cookie.Value = userProfileImageUrl;
                }
                else
                {
                    cookie.Value = userProfileImageUrl;
                } 
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        internal static void AddUserProfileId(int id)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_ID] == null)
            {
                HttpCookie cookie = new HttpCookie(CookieDefinitions.COOKIE_USER_ID);
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_ID];
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        internal static void AddUserProfileFullName(string fullName)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_PROFILE_FULLNAME_KEY] == null)
            {
                HttpCookie cookie = new HttpCookie(CookieDefinitions.COOKIE_USER_PROFILE_FULLNAME_KEY);
                cookie.Value = fullName;
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[CookieDefinitions.COOKIE_USER_PROFILE_FULLNAME_KEY];
                cookie.Value = fullName;
                cookie.Expires = DateTime.Now.AddDays(1.0);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        internal static IEnumerable<UserProfileViewModel> FlagCurrentUserAsFollowerIfApplicable(IEnumerable<UserProfileViewModel> vendorFollowingUsers, MembershipUser currUserMembership, IEnumerable<UserToUserFollowingMapping> currLoggedInUserFollowers)
        {
            List<UserProfileViewModel> result = new List<UserProfileViewModel>(vendorFollowingUsers);

            //add flag indicating if the currenlty logged in user is following any of the specified users in the list
            if (currUserMembership != null)
            {
                Guid currUserGuid = (Guid)currUserMembership.ProviderUserKey;

                foreach (UserProfileViewModel user in result)
                {
                    if (user.UserId == currUserGuid)
                    {
                        user.CurrentUserIsFollowing = true;
                    }

                    if (currLoggedInUserFollowers != null)
                    {
                        var currLoggedInUserIsFollowing = currLoggedInUserFollowers.Where(f => f.FollowedUserId == user.UserId).FirstOrDefault();

                        if (currLoggedInUserIsFollowing != null)
                        {
                            user.CurrentUserIsFollowing = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}