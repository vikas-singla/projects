using System;
using System.Linq;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class ServiceController : ControllerBase
    {
        [RequiresAuthentication]
        public ActionResult Index()
        {
            var services = Database.Services.AsEnumerable().Select(s => s.ToServiceViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", services);
            }

            return View(services);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartial()
        {
            var services = Database.Services.AsEnumerable().Select(s => s.ToServiceViewModel());

            SetViewBagData();

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", services);
            }

            return PartialView("Index", services);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartialFiltered(int vendorId)
        {
            var services = Database.Services.AsEnumerable().Select(s => s.ToServiceViewModel());

            if (services.Count() > 0)
            {
                services = services.Where(s => s.VendorId == vendorId);
            }

            SetViewBagData();

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", services);
            }

            return PartialView("Index", services);
        }

        [RequiresAuthentication]
        public ActionResult Details(int id)
        {
            var serviceModel = Database.Services.Where(s => s.Id == id).FirstOrDefault();

            if (serviceModel != null)
            {
                return View(serviceModel.ToServiceViewModel());    
            }

            throw new Exception("Service not found, id: " + id);
        }

        [RequiresAuthentication]
        public ActionResult Create()
        {
            SetViewBagData();

            return View();
        }

        [RequiresAuthentication]
        public ActionResult CreatePartial()
        {
            SetViewBagData();

            return PartialView("Create");
        }

        [RequiresAuthentication]
        public ActionResult AjaxCreate(int vendorId, string serviceName, string serviceShortDesc, string serviceFullDesc)
        {
            string dbMessage = string.Empty;
            bool successful = false;
            int serviceId = -1; //service ID

            if (Database.Services.Any(s => s.Name.Equals(serviceName) && s.VendorId == vendorId))
            {
                dbMessage = "Service already exists";
            }
            else
            {
                ServiceViewModel viewModel = new ServiceViewModel(vendorId, serviceName, serviceShortDesc, serviceFullDesc);

                Database.Services.AddObject(viewModel.ToServiceModel());
                Database.SaveChanges();

                var savedService = Database.Services.Where(s => s.Name == serviceName && s.VendorId == vendorId).FirstOrDefault();
                serviceId = savedService.Id;

                dbMessage = "Service Saved";
                successful = true;
            }

            return Json(new { result = successful.ToString(), message = dbMessage, sid = serviceId });
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(ServiceViewModel viewModel)
        {
            if (Database.Services.Any(s => s.Name.Equals(viewModel.Name) && s.VendorId == viewModel.VendorId))
            {
                throw new Exception("Service already exists!");
            }

            Database.Services.AddObject(viewModel.ToServiceModel());

            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult CreatePartial(ServiceViewModel viewModel)
        {
            if (Database.Services.Any(s => s.Name.Equals(viewModel.Name)))
            {
                throw new Exception("Service already exists!");
            }

            Database.Services.AddObject(viewModel.ToServiceModel());

            Database.SaveChanges();

            return RedirectToAction("IndexPartial");
        }

        [RequiresAuthentication]
        public ActionResult EditPartial(int id)
        {
            SetViewBagData();

            var model = Database.Services.Where(s => s.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service not found, id: " + id);
            }

            return PartialView("Edit", model.ToServiceViewModel());
        }
        
        [RequiresAuthentication]
        public ActionResult Edit(int id)
        {
            SetViewBagData();

            var model = Database.Services.Where(s => s.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service not found, id: " + id);
            }

            return View(model.ToServiceViewModel());    
        }

        [RequiresAuthentication]
        public ActionResult AjaxEdit(int serviceId, string serviceName, string serviceShortDesc, string serviceFullDesc)
        {
            var dbMessage = string.Empty;
            bool successful = false;
            var model = Database.Services.Where(s => s.Id == serviceId).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Service does not exist";
            }
            else
            {
                model.Name = serviceName;
                model.ShortDescription = serviceShortDesc;
                model.Description = serviceFullDesc;

                Database.SaveChanges();

                dbMessage = "Service Updated";
                successful = true;
            }

            // return to view)
            return Json(new { result = successful.ToString(), message = dbMessage, sid = serviceId });
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = Database.Services.Where(s => s.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service not found, id: " + id);
            }
            
            TryUpdateModel(model);
            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [RequiresAuthentication]
        public ActionResult AjaxDelete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.Services.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Can't delete non-existent Service!";
            }
            else
            {
                Database.DeleteObject(model);
                Database.SaveChanges();

                dbMessage = "Service deleted";
            }

            return Json(new { message = dbMessage });
        }


        #region helper methods

        private void SetViewBagData()
        {
            ViewBag.Vendors = Database.Vendors.AsEnumerable().Select(v => v.ToVendorViewModel());
            ViewBag.Categories = Database.Categories.AsEnumerable().Select(c => c.ToCategoryViewModel());
        }

        #endregion

    }
}