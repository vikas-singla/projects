using System;
using System.Collections.Generic;
using System.Text;
using JobCrawler.HTMLParser.HTMLElements;
using DatabaseHandler;

namespace JobCrawler.HTMLParser.HTMLContentFilters.Filters
{
    /// <summary>
    /// This class will filter out "head" and "script" elements from the HTML source
    /// </summary>
    internal class ScriptFilter : AbstractFilter
    {
        private string[] _elementsToFilter = { "head", "script" };

        public override void filterHTML(HtmlNodeCollection htmlTree_, DatabaseHandler.DatabaseHandler dataHandler_)
        {
            foreach (string element in _elementsToFilter)
            {
                base.filterTag(htmlTree_, element, false);
            }
        }
    }
}