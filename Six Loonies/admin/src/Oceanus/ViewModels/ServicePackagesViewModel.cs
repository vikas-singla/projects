using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Oceanus.ViewModels
{
    public class ServicePackagesViewModel : ViewModelBase
    {
        public double Price { get; set; }
        [DataType(DataType.MultilineText)]  
        public string Description { get; set; }
        public int ServiceId { get; set; }
    }
}