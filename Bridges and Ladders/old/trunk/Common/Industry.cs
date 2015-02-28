using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Industry
    {
        /// <summary>
        /// Industry ID
        /// </summary>
        private int _industryId;
        /// <summary>
        /// Name of the industry
        /// </summary>
        private string _industryName;

        /// <summary>
        /// Industry ID
        /// </summary>
        public int IndustryId
        {
            get
            {
                return _industryId;
            }
        }
        /// <summary>
        /// Name of the industry
        /// </summary>
        public string IndustryName
        {
            get
            {
                return _industryName;
            }
        }

        public Industry(int industryId_, string industryName_)
        {
            _industryId = industryId_;
            _industryName = industryName_;
        }
    }
}
