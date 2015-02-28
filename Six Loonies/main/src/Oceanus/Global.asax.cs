using System;
using System.Web.Mvc;
using System.Web.Routing;
using Oceanus.Data;
using Oceanus.Logging;
using Oceanus.Controllers;

namespace Oceanus
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Root", // Route name
                "", // URL with parameters
                new {controller = "Home", action = "Index"} // Parameter defaults
                );

            routes.MapRoute(
                "GetMessage",
                "m/{messageId}",
                new {controller = "Message", Action = "GetMessageDetails"}
                );

            routes.MapRoute(
                "SignupDelete",
                "SignUp/Delete/{emailAddress}",
                new {controller = "SignUp", action = "Delete"}
                );

            routes.MapRoute(
                "TopicSearch", // Route name
                "t/{searchTag}", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.All, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchFilterByFriends", // Route name
                "t/{searchTag}/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.All, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchNews", // Route name
                "t/{searchTag}/news", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.News, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchNewsFilterByFriends", // Route name
                "t/{searchTag}/news/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.News, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchHowTo", // Route name
                "t/{searchTag}/howto", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.HowTo, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchHowToFilterByFriends", // Route name
                "t/{searchTag}/howto/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.HowTo, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchFacts", // Route name
                "t/{searchTag}/facts", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Facts, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchFactsFilterByFriends", // Route name
                "t/{searchTag}/facts/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Facts, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchOpinion", // Route name
                "t/{searchTag}/opinion", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Opinion, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchOpinionFilterByFriends", // Route name
                "t/{searchTag}/opinion/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Opinion, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchOther", // Route name
                "t/{searchTag}/other", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Other, filterByFriends = false } // Parameter defaults
                );

            routes.MapRoute(
                "TopicSearchOtherFilterByFriends", // Route name
                "t/{searchTag}/other/friends", // URL with parameters
                new { controller = "Topic", action = "Index", articleType = ArticleController.ArticleTypes.Other, filterByFriends = true } // Parameter defaults
                );

            routes.MapRoute(
                "TrendingTopics", // Route name
                "trending/", // URL with parameters
                new { controller = "Topic", action = "Trending" } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearch", // Route name
                "s/{searchTerm}", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.All } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearchNews", // Route name
                "s/{searchTerm}/news", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.News } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearchHowTo", // Route name
                "s/{searchTerm}/howto", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.HowTo } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearchFacts", // Route name
                "s/{searchTerm}/facts", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.Facts } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearchOpinion", // Route name
                "s/{searchTerm}/opinion", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.Opinion } // Parameter defaults
                );

            routes.MapRoute(
                "MainSearchOther", // Route name
                "s/{searchTerm}/other", // URL with parameters
                new { controller = "Search", action = "Index", articleType = ArticleController.ArticleTypes.Other } // Parameter defaults
                );

            routes.MapRoute(
                "UserProfile", // Route name
                "u/{id}", // URL with parameters
                new { controller = "Account", action = "Profile" } // Parameter defaults
                );

            routes.MapRoute(
                "ArticleView", // Route name
                "a/{id}", // URL with parameters
                new { controller = "Article", action = "Index" } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightView", // Route name
                "spotlight/", // URL with parameters
                new { controller = "Home", action = "Spotlight" } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightViewNews", // Route name
                "spotlight/News", // URL with parameters
                new { controller = "Home", action = "Spotlight", articleType = ArticleController.ArticleTypes.News } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightViewFacts", // Route name
                "spotlight/Facts", // URL with parameters
                new { controller = "Home", action = "Spotlight", articleType = ArticleController.ArticleTypes.Facts } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightViewOpinion", // Route name
                "spotlight/Opinion", // URL with parameters
                new { controller = "Home", action = "Spotlight", articleType = ArticleController.ArticleTypes.Opinion } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightViewHowTo", // Route name
                "spotlight/HowTo", // URL with parameters
                new { controller = "Home", action = "Spotlight", articleType = ArticleController.ArticleTypes.HowTo } // Parameter defaults
                );

            routes.MapRoute(
                "SpotlightViewOther", // Route name
                "spotlight/Other", // URL with parameters
                new { controller = "Home", action = "Spotlight", articleType = ArticleController.ArticleTypes.Other } // Parameter defaults
                );

            routes.MapRoute(
                "WelcomeStep1", // Route name
                "welcome/1", // URL with parameters
                new { controller = "Welcome", action = "Step1" } // Parameter defaults
                );

            routes.MapRoute(
                "WelcomeStep2", // Route name
                "welcome/2", // URL with parameters
                new { controller = "Welcome", action = "Step2" } // Parameter defaults
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                //new { controller = "SignUp", action = "Create" }
                );

            // uncomment to see route mapping
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        protected void Application_Start()
        {
            try
            {
                Logger.Instance.Info("Site started");

                AreaRegistration.RegisterAllAreas();

                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);

                ReferenceData.Instance.Initialize();
            }

            catch (Exception ex)
            {
                Logger.Instance.Error("Error starting application", ex);
            }
        }
    }
}