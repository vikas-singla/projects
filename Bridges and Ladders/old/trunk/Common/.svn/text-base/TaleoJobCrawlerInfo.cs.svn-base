using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TaleoJobCrawlerInfo
    {
        /// <summary>
        /// Information ID
        /// </summary>
        private int _infoId;
        /// <summary>
        /// URL that should be used by the crawler to identify jobs
        /// </summary>
        private string _urlToIdentifyJobs;
        /// <summary>
        /// URL that should be used by the crawler to obtain job details
        /// </summary>
        private string _urlToExtractJobDetails;
        /// <summary>
        /// Reference to the company that the information is associated with
        /// </summary>
        private Company _company;
        /// <summary>
        /// List of location tags (to be used to filter jobs)
        /// </summary>
        private List<string> _locationTags;

        /// <summary>
        /// Information ID
        /// </summary>
        public int InfoId
        {
            get
            {
                return _infoId;
            }
        }
        /// <summary>
        /// URL that should be used by the crawler to identify jobs
        /// </summary>
        public string URLToIdentifyJobs
        {
            get
            {
                return _urlToIdentifyJobs;
            }
        }
        /// <summary>
        /// URL that should be used by the crawler to obtain job details
        /// </summary>
        public string URLToExtractJobDetails
        {
            get
            {
                return _urlToExtractJobDetails;
            }
        }
        /// <summary>
        /// Reference to the company that the information is associated with
        /// </summary>
        public Company Company
        {
            get
            {
                return _company;
            }
        }
        /// <summary>
        /// List of location tags (to be used to filter jobs)
        /// </summary>
        public List<string> LocationTags
        {
            get
            {
                return _locationTags;
            }
        }

        public TaleoJobCrawlerInfo(int infoId_, string urlToIdentifyJobs_, string urlToExtractJobDetails_, Company company_, List<string> locationTags_)
        {
            _infoId = infoId_;
            _urlToIdentifyJobs = urlToIdentifyJobs_;
            _urlToExtractJobDetails = urlToExtractJobDetails_;
            _company = company_;
            _locationTags = locationTags_;
        }
    }
}
