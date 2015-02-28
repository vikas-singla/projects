using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;

namespace Oceanus.Controllers
{
    public class FacebookHelper
    {
        /// <summary>
        /// Publish a post on a facebook user's wall
        /// </summary>
        internal static bool PostToFacebookWall(long fbUID, string fbToken, string message, string linkName, string linkURL, string caption, string description, string picture, string actions)
        {
            bool querySuccessful = true;
            FacebookAPI fbAPI = new FacebookAPI(fbToken);

            //create query parameters
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("access_token", fbToken);

            if (linkURL != null && !linkURL.Equals(string.Empty))
            {
                queryParams.Add("link", linkURL);
            }
            if (linkName != null && !linkName.Equals(string.Empty))
            {
                queryParams.Add("name", linkName);
            }
            if (caption != null && !caption.Equals(string.Empty))
            {
                queryParams.Add("caption", caption);
            }
            if (description != null && !description.Equals(string.Empty))
            {
                queryParams.Add("description", description);
            }
            if (picture != null && !picture.Equals(string.Empty))
            {
                queryParams.Add("picture", picture);
            }
            if (actions != null && !actions.Equals(string.Empty))
            {
                queryParams.Add("actions", actions);
            }
            if (message != null && !message.Equals(string.Empty))
            {
                queryParams.Add("message", message);
            }

            // Send the request to Facebook, and query the result to get the confirmation code
            try
            {
                fbAPI.Post("/" + fbUID + "/feed", queryParams);
            }
            catch (Exception e)
            {
                querySuccessful = false;
            }

            return querySuccessful;
        }
    }
}