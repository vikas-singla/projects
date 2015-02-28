using System;
using System.Collections.Generic;
using System.Text;

namespace JobCrawler.HTMLParser.HTMLContentFilters.Filters
{
    public class AdLinkRepository
    {
        private List<string> _adLinks;

        public AdLinkRepository()
        {
            //initialize the list
            _adLinks = new List<string>();
        }

        public void addAdLink(string adLink_)
        {
            if (adLink_ != null && !_adLinks.Contains(adLink_))
            {
                _adLinks.Add(adLink_);
            }
        }

        public List<string> retrieveAdLinks()
        {
            return new List<string>(_adLinks);
        }
    }
}
