using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class PhysicalVendorLocationsViewModel : ViewModelBase
    {
        public int VendorId { get; set; }
        public int LocationStateId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public PhysicalVendorLocationsViewModel()
        {
        }

        public PhysicalVendorLocationsViewModel(int vendorId, string addressLine1, string addressLine2, string city, string postalCode, int refStateId, string phone, string fax, string email)
        {
            VendorId = vendorId;
            AddressLine1 = addressLine1;

            if (addressLine2 != null && !addressLine2.Equals(string.Empty))
            {
                AddressLine2 = addressLine2;
            }

            City = city;
            PostalCode = postalCode;
            LocationStateId = refStateId;

            if (phone != null && !phone.Equals(string.Empty))
            {
                Phone = phone;
            }

            if (fax != null && !fax.Equals(string.Empty))
            {
                Fax = fax;
            }

            if (email != null && !email.Equals(string.Empty))
            {
                Email = email;
            }
        }
    }
}