using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oceanus.Data;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class VirtualLocationController : ControllerBase
    {
        //
        // GET: /VirtualLocation/

        public ActionResult Index()
        {
            return View();
        }

        [RequiresAuthentication]
        public ActionResult IndexPartial()
        {
            var locations = Database.VirtualCitiesServed.AsEnumerable().Select(s => s.ToVirtualCitiesServedViewModel());

            return PartialView("Index", locations);
        }

        [RequiresAuthentication]
        public ActionResult IndexPartialFiltered(int vendorId)
        {
            var locations = Database.VirtualCitiesServed.AsEnumerable().Select(s => s.ToVirtualCitiesServedViewModel());

            if (locations.Count() > 0)
            {
                locations = locations.Where(l => l.VendorId == vendorId);
            }

            return PartialView("Index", locations);
        }

        [RequiresAuthentication]
        public ActionResult Create()
        {
            return View();
        }

        [RequiresAuthentication]
        public ActionResult AjaxDelete(int id)
        {
            var dbMessage = string.Empty;
            var model = Database.VirtualCitiesServed.Where(v => v.Id == id).FirstOrDefault();

            if (model == null)
            {
                dbMessage = "Can't delete non-existent virtual location!";
            }
            else
            {
                Database.DeleteObject(model);
                Database.SaveChanges();

                dbMessage = "Virtual location deleted";
            }

            return Json(new { message = dbMessage });
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
        [RequiresAuthentication]
        public ActionResult AjaxCreate(int vendorId, string city, int refStateId)
        {
            string dbMessage = string.Empty;
            bool successful = false;
            int virtualLocationId = -1;

            if (Database.VirtualCitiesServed.Any(p => p.vendorId == vendorId && p.City == city && p.LocationStatesId == refStateId))
            {
                dbMessage = "Virtual Location already exists!";
            }
            else
            {
                VirtualCitiesServedViewModel viewModel = new VirtualCitiesServedViewModel(vendorId, city, refStateId);

                Database.VirtualCitiesServed.AddObject(viewModel.ToVirtualCitiesModel());
                Database.SaveChanges();

                var savedLocation = Database.VirtualCitiesServed.Where(p => p.vendorId == vendorId && p.City == city && p.LocationStatesId == refStateId).FirstOrDefault();
                virtualLocationId = savedLocation.Id;

                dbMessage = "Virtual Location Saved";
                successful = true;
            }

            return Json(new { result = successful.ToString(), message = dbMessage, vlid = virtualLocationId });
        }

        [RequiresAuthentication]
        public ActionResult CreatePartial()
        {
            return PartialView("Create");
        }
    }
}
