using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oceanus.ViewModels
{
    public class VendorViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "Short Description Required")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Full Description Required")]
        [DataType(DataType.MultilineText)]  
        public string Description { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string Fax { get; set; }
        public List<ServiceViewModel> AssociatedServices { get; set; }
        public bool IsPublished { get; set; }

        public VendorViewModel()
        {
            AssociatedServices = new List<ServiceViewModel>();
        }

        public VendorViewModel(string vendorName, string vendorShortDesc, string vendorFullDesc, string website, string email, string busPhone, string fax, bool isPublished)
        {
            Name = vendorName;
            ShortDescription = vendorShortDesc;
            Description = vendorFullDesc;
            Website = website;
            Email = email;
            BusinessPhone = busPhone;
            Fax = fax;
            IsPublished = isPublished;
        }
    }
}