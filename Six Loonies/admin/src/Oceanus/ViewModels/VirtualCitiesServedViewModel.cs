using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class VirtualCitiesServedViewModel : ViewModelBase
    {
        public int VendorId { get; set; }
        public string City { get; set; }
        public int LocationStatesId { get; set; }

        public VirtualCitiesServedViewModel()
        {

        }

        public VirtualCitiesServedViewModel(int vendorId, string city, int locationStatesId)
        {
            VendorId = vendorId;
            City = city;
            LocationStatesId = locationStatesId;
        }
    }
}