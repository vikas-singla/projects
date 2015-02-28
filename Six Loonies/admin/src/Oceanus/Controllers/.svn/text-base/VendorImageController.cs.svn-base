using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class VendorImageController : ControllerBase
    {

        #region partial actions
        
        [RequiresAuthentication]
        public ActionResult IndexPartial(int? vendorId)
        {
            var vendorImages = Database.VendorImages.AsEnumerable().Select(v => v.ToVendorImageViewModel());

            if (vendorId != null)
            {
                vendorImages = vendorImages.Where(v => v.VendorId == vendorId);
            }

            // TODO: change to admin users)
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", vendorImages);
            }

            return PartialView("Index", vendorImages);
        }

        [RequiresAuthentication]
        public ActionResult CreatePartial(int id)
        {
            var viewModel = new VendorImageViewModel();
            viewModel.VendorId = id;

            SetViewBagData();

            return PartialView("Create", viewModel);
        }

        #endregion


        #region ajax manipulation

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult AjaxCreatePartial(VendorImageViewModel viewModel)
        {
            string dbMessage = string.Empty;
            bool successful = false;
            int serviceImageId = -1; //serviceImage ID

            var newName = Confirm(viewModel.VendorId, viewModel.ImageUrl);
            if (newName != null)
            {
                viewModel.ImageUrl = newName;
            }

            if (Database.Services.Any(v => v.Id.Equals(viewModel.Id)))
            {
                dbMessage = "Service Image already exists!";
            }
            else
            {
                Database.VendorImages.AddObject(viewModel.ToVendorImageModel());
                Database.SaveChanges();

                var savedServiceImage = Database.VendorImages.Where(s => s.vendorId == viewModel.VendorId && s.ImageUrl == viewModel.ImageUrl).FirstOrDefault();
                serviceImageId = savedServiceImage.Id;

                dbMessage = "Service Image Saved";
                successful = true;
            }

            Sweep(viewModel.VendorId);

            return Json(new { result = successful.ToString(), message = dbMessage, siid = serviceImageId });
        }

        [RequiresAuthentication]
        public ActionResult AjaxDelete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.VendorImages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Can't delete non-existent Vendor Image!";
            }
            else
            {
                string fullPath;
                if (CheckFileExists(model.vendorId, model.ImageUrl, out fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    Database.DeleteObject(model);
                    Database.SaveChanges();
                }

                dbMessage = "Image deleted";
            }

            return Json(new { message = dbMessage });
        }

        #endregion


        #region standard actions

        public ActionResult Index()
        {
            var serviceImages = Database.VendorImages.AsEnumerable().Select(v => v.ToVendorImageViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", serviceImages);
            }

            return View(serviceImages);
        }

        public ActionResult Details(int id)
        {
            var model = Database.VendorImages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Package not found, id: " + id);
            }

            return View(model.ToVendorImageViewModel());
        }

        [RequiresAuthentication]
        public ActionResult Create()
        {
            SetViewBagData();

            return View();
        }

        
        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(VendorImageViewModel viewModel)
        {
            if (Database.VendorImages.Any(v => v.Id.Equals(viewModel.Id)))
            {
                throw new Exception("Service Image already exists!");
            }

            Database.VendorImages.AddObject(viewModel.ToVendorImageModel());

            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [RequiresAuthentication]
        public ActionResult Edit(int id)
        {
            SetViewBagData();

            var model = Database.VendorImages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Image not found, id: " + id);
            }

            return View(model.ToVendorImageViewModel());
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = Database.VendorImages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Service Image not found, id: " + id);
            }

            TryUpdateModel(model);
            Database.SaveChanges();

            // return to view)
            return RedirectToAction("Index");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Delete(int id)
        {
            var model = Database.VendorImages.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Can't delete non-existent Service Image!");
            }

            Database.DeleteObject(model);
            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion


        #region helper methods

        private void SetViewBagData()
        {
            ViewBag.Services = Database.Services.AsEnumerable().Select(c => c.ToServiceViewModel());
        }

        #endregion


        #region flash uploader

        private const string RelativeVendorPath = "Uploads";
        private const string AbsolutePathFormat = @"{0}\{1}\{2}";

        private bool CheckFileExists(int vendorId, string fileName, out string fullPath)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeVendorPath, vendorId);
            fullPath = string.Format(@"{0}\{1}", folder, fileName);
            return System.IO.File.Exists(fullPath);
        }

        /// <summary>
        /// Confirms a file for use in the vendor portfolio - removes the pending suffix.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="imageUrl"></param>
        private string Confirm(int vendorId, string imageUrl)
        {
            string path = null;
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeVendorPath, vendorId);

            string newName = null;

            if (CheckFileExists(vendorId, imageUrl, out path))
            {
                FileInfo info = new FileInfo(path);
                newName = string.Concat(info.Name, "__confirmed", info.Extension);
                System.IO.File.Move(path, Path.Combine(folder, newName));
            }

            return newName;
        }

        [HttpPost]
        [RequiresAuthentication]
        public void Sweep(int vendorId)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeVendorPath, vendorId);
            var files = Directory.GetFiles(folder);
            
            foreach (var file in files)
            {
                if (!file.Contains("__confirmed"))
                {
                    System.IO.File.Delete(Path.Combine(folder, file));
                }
            }
        }

        [TokenizedAuthorize]
        [RequiresAuthentication]
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult CheckExisting(int Id, string filename)
        {
            string result = "0";

            // TODO: ensure ID is valid
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeVendorPath, Id);

            string fullPath;
            if (CheckFileExists(Id, filename, out fullPath))
            {
                result = "1";
            }

            return new ContentResult() { Content = result };
        }

        [TokenizedAuthorize]
        [RequiresAuthentication]
        public ContentResult Upload(int Id, HttpPostedFileBase fileData)
        {
            // save file and return 1 on success
            string result = "1";
            try
            {
                var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeVendorPath, Id);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                fileData.SaveAs(string.Format(@"{0}\{1}", folder, fileData.FileName));
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return new ContentResult() { Content = result };
        }

        #endregion

    }
}
