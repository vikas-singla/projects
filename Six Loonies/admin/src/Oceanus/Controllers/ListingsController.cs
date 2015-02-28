using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MVCControlsToolkit.Linq;
using Oceanus.Models;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class ListingsController : ControllerBase
    {
        //in actual application this should be put in a config file
        public const int ListingsPerPage = 5; 

        public ActionResult Index(int? categoryId)
        {
            if (categoryId == null)
            {
                return RedirectToAction("Index", "Category");
            }

            var category = Database.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            if (category == null)
            {
                throw new Exception("Invalid category!");
            }

            int totalPages;
            
            List<KeyValuePair<LambdaExpression, OrderType>> order = null;
            
            var result = new ListingsViewModel()
                {
                    CategoryView = new CategoryViewModel() { Id = category.Id, Name = category.Name },
                    ListingList = ListingsViewModel.GetPage(ListingsPerPage, out totalPages, ref order),
                    TotalPages = totalPages,
                    CurrentPage=1,
                    PreviousPage=1,
                    ListingOrder=order
                };

            return View(result);


            //var listings = category.Services.Select(
            //        s => new ListingModel()
            //        {
            //            Id = s.Id,
            //            Name = s.Name,
            //        }
            //    );

            //    var listingsModel = new ListingsModel()
            //    {
            //        Category = new CategoryModel() { Id = category.Id, Name = category.Name },
            //        Listings = listings
            //    };

            //    return View(listingsModel);
            //}

            //return View();
        }

        
    }
}
