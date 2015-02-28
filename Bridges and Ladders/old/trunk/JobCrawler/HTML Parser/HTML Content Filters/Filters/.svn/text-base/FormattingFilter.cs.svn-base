using System;
using System.Collections.Generic;
using System.Text;
using JobCrawler.HTMLParser.HTMLElements;
using DatabaseHandler;

namespace JobCrawler.HTMLParser.HTMLContentFilters.Filters
{
    /// <summary>
    /// This class will filter out formatting elements from the HTML source
    /// </summary>
    internal class FormattingFilter : AbstractFilter
    {
        private string[] _elementsToFilter = { "font", "u", "em", "b", "big", "center",
                                               "hr", "i", "small", "strong", "tt",
                                               "nobr", "c", "basefont", "blink", "cite",
                                               "code", "span" };

        public override void filterHTML(HtmlNodeCollection htmlTree_, DatabaseHandler.DatabaseHandler dataHandler_)
        {
            foreach (string element in _elementsToFilter)
            {
                base.filterTag(htmlTree_, element, true);
            }
        }
    }
}
