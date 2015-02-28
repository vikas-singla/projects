using System.Linq;
using System.Web.Mvc;
using Oceanus.Models;
using Oceanus.ViewModels;

namespace Oceanus.Controllers
{
    public class SignUpController : ControllerBase
    {
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Created()
        {
            return View();
        }

        public ActionResult Delete(string emailAddress)
        {
            var record = Database.Registrations.Where(r => r.Email == emailAddress).FirstOrDefault();

            if (record != null)
            {
                ViewData["Id"] = record.Id;
                return View(new SignUpViewModel()
                {
                    Id = record.Id,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    EmailAddress = record.Email
                });
            }

            return View();
        }

        public ActionResult Deleted()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add sign up error message. (email exists)
                    viewModel.SignUpEmail(viewModel.FirstName, viewModel.LastName, viewModel.EmailAddress);

                    // http://blogs.teamb.com/craigstuntz/2009/01/23/37947/
                    TempData["FirstName"] = viewModel.FirstName;
                    TempData["LastName"] = viewModel.LastName;

                    // redirect to success
                    return RedirectToAction("Complete");
                }
                catch
                {
                    ViewBag.ErrorMessage = "Email already exists!";
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(string emailAddress, FormCollection collection)
        {
            try
            {
                var record = Database.Registrations.Where(r => r.Email == emailAddress).FirstOrDefault();

                Database.Registrations.DeleteObject(record);
                Database.SaveChanges();

                TempData["FirstName"] = record.FirstName;
                TempData["LastName"] = record.LastName;
                TempData["EmailAddress"] = record.Email;

                return View("Deleted");
            }

            catch
            {
                return View();
            }
        }
    }
}