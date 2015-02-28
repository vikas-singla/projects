using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Web.Security;
using System.IO;

namespace Oceanus.Controllers
{
    public class VendorController : ControllerBase
    {

        [RequiresAuthentication]
        public ActionResult Index()
        {
            var vendors = Database.Vendors.AsEnumerable().Select(v => v.ToVendorViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", vendors);
            }
            
            return View(vendors);
        }

        public ActionResult Details(int id)
        {
            var model = Database.Vendors.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Vendor not found, id: " + id);
            }

            return View(model.ToVendorViewModel());
        }

        [RequiresAuthentication]
        public ActionResult Create()
        {
            SetViewBagData();

            var viewModel = new VendorViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(VendorViewModel viewModel)
        {
            if (Database.Vendors.Any(v => v.Name.Equals(viewModel.Name)))
            {
                throw new Exception("Vendor already exists!");
            }

            Database.Vendors.AddObject(viewModel.ToVendorModel());

            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [RequiresAuthentication]
        public ActionResult AjaxCreate(string vendorName, string vendorShortDesc, string vendorFullDesc, string websiteURL, string email, string busPhone, string fax)
        {
            string dbMessage = string.Empty;
            bool successful = false;
            int vendorId = -1; //vendor ID

            VendorViewModel viewModel = new VendorViewModel(vendorName, vendorShortDesc, vendorFullDesc, websiteURL, email, busPhone, fax, true);

            if (Database.Vendors.Any(v => v.Name.Equals(viewModel.Name)))
            {
                dbMessage = "Vendor already exists";
            }
            else
            {
                Database.Vendors.AddObject(viewModel.ToVendorModel());
                Database.SaveChanges();

                var savedVendor = Database.Vendors.Where(v => v.Name == vendorName && v.ShortDescription == vendorShortDesc).FirstOrDefault();
                vendorId = savedVendor.Id;

                dbMessage = "Vendor Saved";
                successful = true;
            }

            return Json(new { result = successful.ToString(), message = dbMessage, vid = vendorId });
        }


        [RequiresAuthentication]
        public ActionResult AjaxEdit(int vendorId, string vendorName, string vendorShortDesc, string vendorFullDesc, string websiteURL, string email, string busPhone, string fax)
        {
            var dbMessage = string.Empty;
            bool successful = false;
            var model = Database.Vendors.Where(v => v.Id == vendorId).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Vendor does not exist";
            }
            else
            {
                model.Name = vendorName;
                model.ShortDescription = vendorShortDesc;
                model.Description = vendorFullDesc;
                model.WebsiteURL = websiteURL;
                model.Email = email;
                model.BusinessPhone = busPhone;
                model.Fax = fax;

                Database.SaveChanges();

                dbMessage = "Vendor Updated";
                successful = true;
            }

            // return to view)
            return Json(new { result = successful.ToString(), message = dbMessage, vid = vendorId });
        }

        [RequiresAuthentication]
        public ActionResult Edit(int id)
        {
            SetViewBagData();

            var model = Database.Vendors.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Vendor not found, id: " + id);
            }

            return View(model.ToVendorViewModel());
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = Database.Vendors.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Vendor not found, id: " + id);
            }

            TryUpdateModel(model);
            Database.SaveChanges();

            // return to view)
            return RedirectToAction("Index");
        }

        [RequiresAuthentication]
        public ActionResult Delete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.Vendors.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Vendor not found, id: " + id);
            }
            else
            {
                Database.DeleteObject(model);
                Database.SaveChanges();

                dbMessage = "Vendor deleted";
            }

            // return to view)
            return RedirectToAction("Index");
        }


        #region helper methods

        private void SetViewBagData()
        {
            ViewBag.Categories = Database.Categories.AsEnumerable().Select(c => c.ToCategoryViewModel());
        }

        #endregion
    }
}