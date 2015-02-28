using System;
using System.Linq;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Data;
using Oceanus.ViewModels;
using System.Collections.Generic;

namespace Oceanus.Controllers
{
    public class PhysicalLocationsController : ControllerBase
    {

        [RequiresAuthentication]
        public ActionResult Index()
        {
            var locations = Database.PhysicalVendorLocations.AsEnumerable().Select(s => s.ToPhysicalVendorLocationsViewModel());

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAdmin", locations);
            }

            return View(locations);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartial()
        {
            var locations = Database.PhysicalVendorLocations.AsEnumerable().Select(s => s.ToPhysicalVendorLocationsViewModel());

            SetViewBagData();

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", locations);
            }

            return PartialView("Index", locations);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartialFiltered(int vendorId)
        {
            var locations = Database.PhysicalVendorLocations.AsEnumerable().Select(s => s.ToPhysicalVendorLocationsViewModel());

            if (locations.Count() > 0)
            {
                locations = locations.Where(s => s.VendorId == vendorId);
            }

            SetViewBagData();

            // TODO: change to admin users
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("IndexAdmin", locations);
            }

            return PartialView("Index", locations);
        }

        [RequiresAuthentication]
        public ActionResult Details(int id)
        {
            var serviceModel = Database.PhysicalVendorLocations.Where(s => s.Id == id).FirstOrDefault();

            if (serviceModel != null)
            {
                return View(serviceModel.ToPhysicalVendorLocationsViewModel());
            }

            throw new Exception("Physical Location not found, id: " + id);
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
        public ActionResult AjaxCreate(int vendorId, string addressLine1, string addressLine2, string city, string postalCode, int refStateId, string phone, string fax, string email)
        {
            string dbMessage = string.Empty;
            bool successful = false;
            int physicalLocationId = -1;

            if (Database.PhysicalVendorLocations.Any(p => p.VendorId == vendorId && p.AddressLine1 == addressLine1 && p.AddressLine2 == addressLine2 && p.City == city))
            {
                dbMessage = "Physical Location already exists!";
            }
            else
            {
                PhysicalVendorLocationsViewModel viewModel = new PhysicalVendorLocationsViewModel(vendorId, addressLine1, addressLine2, city, postalCode, refStateId, phone, fax, email);

                Database.PhysicalVendorLocations.AddObject(viewModel.ToPhysicalVendorLocationModel());
                Database.SaveChanges();

                var savedLocation = Database.PhysicalVendorLocations.Where(p => p.VendorId == vendorId && p.AddressLine1 == addressLine1 && p.AddressLine2 == addressLine2 && p.City == city).FirstOrDefault();
                physicalLocationId = savedLocation.Id;

                dbMessage = "Physical Location Saved";
                successful = true;
            }

            return Json(new { result = successful.ToString(), message = dbMessage, plid = physicalLocationId });
        }

        [RequiresAuthentication]
        public ActionResult AjaxEdit(int physicalLocId, int vendorId, string addressLine1, string addressLine2, string city, string postalCode, int refStateId, string phone, string fax, string email)
        {
            var dbMessage = string.Empty;
            bool successful = false;
            var model = Database.PhysicalVendorLocations.Where(p => p.Id == physicalLocId).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Phyical Location does not exist";
            }
            else
            {
                model.VendorId = vendorId;
                model.AddressLine1 = addressLine1;

                if (addressLine2 != null && !addressLine2.Equals(string.Empty))
                {
                    model.AddressLine2 = addressLine2;
                }

                model.City = city;
                model.PostalCode = postalCode;
                model.RefLocationStateId = refStateId;

                if (phone != null && !phone.Equals(string.Empty))
                {
                    model.Phone = phone;
                }

                if (fax != null && !fax.Equals(string.Empty))
                {
                    model.Fax = fax;
                }

                Database.SaveChanges();

                dbMessage = "Physical Location Updated";
                successful = true;
            }

            // return to view)
            return Json(new { result = successful.ToString(), message = dbMessage, plid = physicalLocId });
        }

        [RequiresAuthentication]
        public ActionResult AjaxCountryList()
        {
            var countries = Database.Reference_LocationStates.AsEnumerable().Select(c => c.Country).Distinct();
            var countriesList = new List<object>();

            foreach (var countryName in countries)
            {
                countriesList.Add(new { Country = countryName });
            }

            return Json(new SelectList(
                                countriesList,
                                "Country",
                                "Country")
                            );
        }

        [RequiresAuthentication]
        public ActionResult AjaxStateList(string country)
        {
            var states = Database.Reference_LocationStates.AsEnumerable().Where(s => s.Country.Equals(country));

            return Json(new SelectList(
                                states,
                                "Id",
                                "State")
                            );
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Create(PhysicalVendorLocationsViewModel viewModel)
        {
            if (Database.PhysicalVendorLocations.Any(p => p.VendorId == viewModel.VendorId && p.AddressLine1 == viewModel.AddressLine1 && p.AddressLine2 == viewModel.AddressLine2 && p.City == viewModel.City))
            {
                throw new Exception("Physical Location already exists!");
            }

            Database.PhysicalVendorLocations.AddObject(viewModel.ToPhysicalVendorLocationModel());

            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult CreatePartial(PhysicalVendorLocationsViewModel viewModel)
        {
            if (Database.PhysicalVendorLocations.Any(p => p.VendorId == viewModel.VendorId && p.AddressLine1 == viewModel.AddressLine1 && p.AddressLine2 == viewModel.AddressLine2 && p.City == viewModel.City))
            {
                throw new Exception("Physical Location already exists!");
            }

            Database.PhysicalVendorLocations.AddObject(viewModel.ToPhysicalVendorLocationModel());

            Database.SaveChanges();

            return RedirectToAction("IndexPartial");
        }

        [RequiresAuthentication]
        public ActionResult Edit(int id)
        {
            SetViewBagData();

            var model = Database.PhysicalVendorLocations.Where(s => s.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Physical Location not found, id: " + id);
            }

            return View(model.ToPhysicalVendorLocationsViewModel());
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = Database.PhysicalVendorLocations.Where(s => s.Id == id).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Physical Location not found, id: " + id);
            }

            TryUpdateModel(model);
            Database.SaveChanges();

            return RedirectToAction("Index");
        }

        [RequiresAuthentication]
        public ActionResult AjaxDelete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.PhysicalVendorLocations.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Can't delete non-existent Physical Location!";
            }
            else
            {
                Database.DeleteObject(model);
                Database.SaveChanges();

                dbMessage = "Physical Location deleted";
            }

            return Json(new { message = dbMessage });
        }


        #region helper methods

        private void SetViewBagData()
        {
        }

        #endregion

    }
}
