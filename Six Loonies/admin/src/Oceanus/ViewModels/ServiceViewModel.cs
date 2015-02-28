using System.ComponentModel.DataAnnotations;
namespace Oceanus.ViewModels
{
    public class ServiceViewModel : ViewModelBase
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        [Required(ErrorMessage = "Short Description Required")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Full Description Required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ServiceViewModel()
        {
        }

        public ServiceViewModel(int vendorId, string serviceName, string serviceShortDesc, string serviceFullDesc)
        {
            VendorId = vendorId;
            Name = serviceName;
            ShortDescription = serviceShortDesc;
            Description = serviceFullDesc;
        }
    }
}