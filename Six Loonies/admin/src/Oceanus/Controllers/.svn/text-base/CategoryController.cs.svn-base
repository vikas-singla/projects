using System;
using System.Linq;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class CategoryController : ControllerBase
    {
        public ActionResult Index()
        {
            var categories = Database.Categories.AsEnumerable().Select(c => c.ToCategoryViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", categories);
            }
            
            return View(categories);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(CategoryViewModel viewModel)
        {
            if (Database.Categories.Any(c => c.Name.Equals(viewModel.Name)))
            {
                throw new Exception("Category already exists!");
            }

            Database.Categories.AddObject(viewModel.ToCategoryModel());

            Database.SaveChanges();

            return RedirectToAction("Index", "Category");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Delete(int id)
        {
            var category = Database.Categories.Where(c => c.Id == id).FirstOrDefault();

            if (category == null)
            {
                throw new Exception("Can't delete non-existent category!");
            }

            Database.DeleteObject(category);
            Database.SaveChanges();

            return RedirectToAction("Index", "Category");
        }
    }
}