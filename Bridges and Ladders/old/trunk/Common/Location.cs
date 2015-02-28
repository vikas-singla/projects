using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Location
    {
        /// <summary>
        /// The Location ID
        /// </summary>
        private int _locationId;
        /// <summary>
        /// Name of the city
        /// </summary>
        private string _city;
        /// <summary>
        /// Name of the country
        /// </summary>
        private string _country;
        /// <summary>
        /// Address Line 1
        /// </summary>
        private string _addressLine1;
        /// <summary>
        /// Address Line 2
        /// </summary>
        private string _addressLine2;
        /// <summary>
        /// Address Line 3
        /// </summary>
        private string _addressLine3;
        /// <summary>
        /// Postal Code value
        /// </summary>
        private string _postalCode;

        /// <summary>
        /// The Location ID
        /// </summary>
        public int LocationId
        {
            get
            {
                return _locationId;
            }
        }
        /// <summary>
        /// Name of the city
        /// </summary>
        public string City
        {
            get
            {
                return _city;
            }
        }
        /// <summary>
        /// Name of the country
        /// </summary>
        public string Country
        {
            get
            {
                return _country;
            }
        }
        /// <summary>
        /// Address Line 1
        /// </summary>
        public string AddressLine1
        {
            get
            {
                return _addressLine1;
            }
        }
        /// <summary>
        /// Address Line 2
        /// </summary>
        public string AddressLine2
        {
            get
            {
                return _addressLine2;
            }
        }
        /// <summary>
        /// Address Line 3
        /// </summary>
        public string AddressLine3
        {
            get
            {
                return _addressLine3;
            }
        }
        /// <summary>
        /// Postal Code value
        /// </summary>
        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
        }

        public Location(int locationId_, string city_, string country_, string addressLine1_, string addressLine2_, string addressLine3_,
            string postalCode_)
        {
            _locationId = locationId_;
            _city = city_;
            _country = country_;
            _addressLine1 = addressLine1_;
            _addressLine2 = addressLine2_;
            _addressLine3 = addressLine3_;
        }
    }
}
