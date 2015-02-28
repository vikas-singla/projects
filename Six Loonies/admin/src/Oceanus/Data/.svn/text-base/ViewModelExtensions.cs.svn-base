using Oceanus.ViewModels;
using System.Linq;

namespace Oceanus.Data
{
    public static class EntityExtensions
    {

        public static CategoryViewModel ToCategoryViewModel(this Category model)
        {
            return new CategoryViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static Category ToCategoryModel(this CategoryViewModel model)
        {
            return new Category()
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static VirtualCitiesServedViewModel ToVirtualCitiesServedViewModel(this VirtualCitiesServed model)
        {
            return new VirtualCitiesServedViewModel()
            {
                Id = model.Id,
                VendorId = model.vendorId,
                City = model.City,
                LocationStatesId = model.LocationStatesId
            };
        }

        public static VirtualCitiesServed ToVirtualCitiesModel(this VirtualCitiesServedViewModel viewModel)
        {
            return new VirtualCitiesServed()
            {
                Id = viewModel.Id,
                vendorId = viewModel.VendorId,
                City = viewModel.City,
                LocationStatesId = viewModel.LocationStatesId
            };
        }

        public static ListingViewModel Create(this Service model)
        {
            return new ListingViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static ServiceViewModel ToServiceViewModel(this Service model)
        {
            return new ServiceViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                VendorId = model.VendorId,
                VendorName = model.Vendor.Name,
                Description = model.Description,
                ShortDescription = model.ShortDescription
            };
        }

        public static Service ToServiceModel(this ServiceViewModel viewModel)
        {
            return new Service()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                VendorId = viewModel.VendorId,
                Description = viewModel.Description,
                ShortDescription = viewModel.ShortDescription
            };
        }

        public static Vendor ToVendorModel(this VendorViewModel viewModel)
        {
            return new Vendor()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ShortDescription = viewModel.ShortDescription,
                WebsiteURL = viewModel.Website,
                Email = viewModel.Email,
                BusinessPhone = viewModel.BusinessPhone,
                Fax = viewModel.Fax,
                IsPublished = true
            };
        }

        public static VendorViewModel ToVendorViewModel(this Vendor model)
        {
            return new VendorViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Website = model.WebsiteURL,
                Email = model.Email,
                BusinessPhone = model.BusinessPhone,
                Fax = model.Fax,
                IsPublished = (bool) model.IsPublished,
                AssociatedServices = model.Services.AsEnumerable().Select(s => s.ToServiceViewModel()).ToList()
            };

        }
        /**
        public static CityStatesLocation ToCityStateLocationModel(this CityStateLocationViewModel viewModel)
        {
            return new CityStatesLocation()
            {
                Id = viewModel.Id,
                City = viewModel.City,
                Subdivision = viewModel.Subdivision,
                Country = viewModel.Country
            };
        }

        public static CityStateLocationViewModel ToCityStateLocationViewModel(this CityStatesLocation model)
        {
            return new CityStateLocationViewModel()
            {
                Id = model.Id,
                City = model.City,
                Subdivision = model.Subdivision,
                Country = model.Country
            };
        }**/

        public static PhysicalVendorLocation ToPhysicalVendorLocationModel(this PhysicalVendorLocationsViewModel viewModel)
        {
            return new PhysicalVendorLocation()
            {
                Id = viewModel.Id,
                VendorId = viewModel.VendorId,
                RefLocationStateId = viewModel.LocationStateId,
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                City = viewModel.City,
                PostalCode = viewModel.PostalCode,
                Phone = viewModel.Phone,
                Fax = viewModel.Fax
            };
        }

        public static PhysicalVendorLocationsViewModel ToPhysicalVendorLocationsViewModel(this PhysicalVendorLocation model)
        {
            return new PhysicalVendorLocationsViewModel()
            {
                Id = model.Id,
                VendorId = model.VendorId,
                LocationStateId = model.RefLocationStateId,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                PostalCode = model.PostalCode,
                Phone = model.Phone,
                Fax = model.Fax
            };
        }

        public static Reference_LocationStates ToReferenceLocationStates(this ReferenceLocationStatesViewModel viewModel)
        {
            return new Reference_LocationStates()
            {
                Id = viewModel.Id,
                State = viewModel.State,
                Country = viewModel.Country
            };
        }

        public static ReferenceLocationStatesViewModel ToReferenceLocationStatesViewModel(this Reference_LocationStates model)
        {
            return new ReferenceLocationStatesViewModel()
            {
                Id = model.Id,
                State = model.State,
                Country = model.Country
            };
        }

        public static ServicePackage ToServicePackageModel(this ServicePackagesViewModel viewModel)
        {
            return new ServicePackage()
            {
                Id = viewModel.Id,
                ServiceId = viewModel.ServiceId,
                Description = viewModel.Description,
                Price = viewModel.Price,
                Name = viewModel.Name
            };
        }

        public static ServicePackagesViewModel ToServicePackagesViewModel(this ServicePackage model)
        {
            return new ServicePackagesViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                ServiceId = model.ServiceId,
                Description = model.Description,
                Price = model.Price
            };
        }

        public static VendorImage ToVendorImageModel(this VendorImageViewModel viewModel)
        {
            return new VendorImage()
            {
                Id = viewModel.Id,
                vendorId = viewModel.VendorId,
                Title = viewModel.Title,
                Description = viewModel.Description,
                ImageUrl = viewModel.ImageUrl,
                copyrightAuthor = viewModel.CopyrightAuthor,
                sourceWebsiteName = viewModel.SourceWebsiteName,
                sourceWebsiteURL = viewModel.SourceWebsiteURL
            };
        }

        public static VendorImageViewModel ToVendorImageViewModel(this VendorImage model)
        {
            return new VendorImageViewModel()
            {
                Id = model.Id,
                VendorId = model.vendorId,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                CopyrightAuthor = model.copyrightAuthor,
                SourceWebsiteURL = model.sourceWebsiteURL,
                SourceWebsiteName = model.sourceWebsiteName
            };
        }
    }
}