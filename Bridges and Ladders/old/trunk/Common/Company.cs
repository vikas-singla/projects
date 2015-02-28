using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Company
    {
        /// <summary>
        /// Company ID
        /// </summary>
        private int _companyId;
        /// <summary>
        /// Name of the company
        /// </summary>
        private string _name;
        /// <summary>
        /// Reference to the industry that this company belongs to
        /// </summary>
        private Industry _industry;

        /// <summary>
        /// Company ID
        /// </summary>
        public int CompanyId
        {
            get
            {
                return _companyId;
            }
        }
        /// <summary>
        /// Name of the company
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// Reference to the industry that this company belongs to
        /// </summary>
        public Industry Industry
        {
            get
            {
                return _industry;
            }
        }

        public Company(int companyId_, string name_, Industry industry_)
        {
            _companyId = companyId_;
            _name = name_;
            _industry = industry_;
        }
    }
}
