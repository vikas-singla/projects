using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.ViewModels;
using Oceanus.Data;

namespace Oceanus.Controllers
{
    public class ServicePackagesController : ControllerBase
    {
        //
        // GET: /ServicePackages/

        public ActionResult Index()
        {
            var servicePackages = Database.ServicePackages.AsEnumerable().Select(v => v.ToServicePackagesViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", servicePackages);
            }

            return View(servicePackages);
        }

        [RequiresAuthentication]
        public ActionResult AjaxDelete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.ServicePackages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Can't delete non-existent Service Package!";
            }
            else
            {
                Database.DeleteObject(model);
                Database.SaveChanges();

                dbMessage = "Package deleted";
            }

            return Json(new { message = dbMessage });
        }

        [RequiresAuthentication]
        public ActionResult IndexPartial()
        {
            var servicePricePackages = Database.ServicePackages.AsEnumerable().Select(s => s.ToServicePackagesViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", servicePricePackages);
            }

            return PartialView("Index", servicePricePackages);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartialFiltered(int serviceId)
        {
            var servicePricePackages = Database.ServicePackages.AsEnumerable().Select(s => s.ToServicePackagesViewModel());

            if (servicePricePackages.Count() > 0)
            {
                servicePricePackages = servicePricePackages.Where(s => s.ServiceId == serviceId);
            }

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", servicePricePackages);
            }

            return PartialView("Index", servicePricePackages);
        }

        public ActionResult Details(int id)
        {
            var model = Database.ServicePackages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Package not found, id: " + id);
            }

            return View(model.ToServicePackagesViewModel());
        }

        [RequiresAuthentication]
        public ActionResult Create()
        {
            SetViewBagData();

            return View();
        }

        [RequiresAuthentication]
        public ActionResult CreatePartial(int id)
        {
            var viewModel = new ServicePackagesViewModel();
            viewModel.ServiceId = id;

            SetViewBagData();

            return PartialView("Create", viewModel);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(ServicePackagesViewModel viewModel)
        {
            if (Database.ServicePackages.Any(v => v.Name.Equals(viewModel.Name)))
            {
                throw new Exception("Service Package already exists!");
            }

            Database.ServicePackages.AddObject(viewModel.ToServicePackageModel());

            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult CreatePartial(ServicePackagesViewModel viewModel)
        {
            if (Database.ServicePackages.Any(v => v.Name.Equals(viewModel.Name)))
            {
                throw new Exception("Service Package already exists!");
            }

            Database.ServicePackages.AddObject(viewModel.ToServicePackageModel());

            Database.SaveChanges();

            return RedirectToAction("IndexPartial");
        }

        [RequiresAuthentication]
        public ActionResult Edit(int id)
        {
            SetViewBagData();

            var model = Database.ServicePackages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Package not found, id: " + id);
            }
            return View(model.ToServicePackagesViewModel());
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = Database.ServicePackages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Package not found, id: " + id);
            }

            TryUpdateModel(model);
            Database.SaveChanges();

            // return to view)
            return RedirectToAction("Index");
        }


        #region helper methods

        private void SetViewBagData()
        {
            ViewBag.Services = Database.Services.AsEnumerable().Select(s => s.ToServiceViewModel());
        }

        #endregion
    }
}
