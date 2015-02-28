using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using JobCrawler.HTMLParser.HTMLElements;
using JobCrawler.HTMLParser.HTMLContentFilters.Filters;
using DatabaseHandler;

namespace JobCrawler.HTMLParser.HTMLContentFilters.Filters
{
    /// <summary>
    /// This class will filter out ad links (appropriately through thorough analysis) from the HTML source
    /// </summary>
    internal class LinkFilter : AbstractFilter
    {
        private string[] _elementsToFilter = { "a" };

        public override void filterHTML(HtmlNodeCollection htmlTree_, DatabaseHandler.DatabaseHandler dataHandler_)
        {            
            ReadOnlyCollection<string> adLinks = dataHandler_.retrieveAdLinks();

            foreach (string elementName in _elementsToFilter)
            {
                //retrieve associated element nodes from html tree
                List<HtmlElement> elementList = htmlTree_.FindByName(elementName, true);

                foreach (HtmlElement element in elementList)
                {
                    //retrieve element href attribute
                    HtmlAttribute attribute = element.Attributes["href"];

                    if (attribute != null && attribute.Value != null)
                    {
                        bool linkIsAd = false;

                        //attribute was found!
                        foreach (string repoAdLink in adLinks)
                        {
                            try
                            {
                                if (System.Text.RegularExpressions.Regex.Match(attribute.Value, repoAdLink,
                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase).Length > 0)
                                {
                                    //there was a match. Hence, this link is an ad link
                                    linkIsAd = true;

                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                int x = 0;
                            }
                        }

                        if (linkIsAd)
                        {
                            //remove this ad link and it's associated children
                            element.Remove();
                        }
                        else
                        {
                            //this is not a ad link from what we can tell... just remove the html tag surrounding the text
                            base.filterElement(element, htmlTree_, true);
                        }
                    }             
                } 
            }
        }
    }
}
