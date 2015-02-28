using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DatabaseHandler;
using JobCrawler.Interfaces;
using JobCrawler.HTMLParser;
using JobCrawler.HTMLParser.HTMLElements;
using Common;

namespace JobCrawler.CrawlerHelpers
{
    public class TaleoCrawler : CrawlerInterface
    {
        /// <summary>
        /// The ID number of the crawler
        /// </summary>
        private int _crawlerNumber = -1;
        /// <summary>
        /// The name of the crawler
        /// </summary>
        private string _crawlerName = "Taleo Crawler";
        /// <summary>
        /// Reference to the database handler
        /// </summary>
        private DatabaseHandler.DatabaseHandler _databaseHandler;
        /// <summary>
        /// Reference to teh browser used to crawl web pages
        /// </summary>
        private WebBrowser _browser;
        /// <summary>
        /// Queue of found JOB IDs by the crawler
        /// </summary>
        private Queue<string> _foundJobIds;
        /// <summary>
        /// Reference to the information record that the crawler is currently processing
        /// </summary>
        private TaleoJobCrawlerInfo _crawlerInfoRecord;
        /// <summary>
        /// Flag for the crawler to indicate whether the focus is on identifying job details
        /// </summary>
        private bool _identifyJobDetails;

        internal TaleoCrawler(int crawlerNumber_, DatabaseHandler.DatabaseHandler databaseHandler_, WebBrowser browser_)
        {
            _crawlerNumber = crawlerNumber_;
            _databaseHandler = databaseHandler_;
            _browser = browser_;
            _foundJobIds = new Queue<string>();
        }

        /// <summary>
        /// Main method of execution by the crawler
        /// </summary>
        public void doWork()
        {
            TaleoJobCrawlerInfo infoRecord = _databaseHandler.retrieveNextUnprocessedTaleoJobInfoRecord();
            _browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(browser_ProgressChanged);


            if (infoRecord != null && !infoRecord.URLToIdentifyJobs.Trim().Equals(string.Empty))
            {
                _crawlerInfoRecord = infoRecord;

                try
                {
                    _identifyJobDetails = false;

                    _browser.Navigate(infoRecord.URLToIdentifyJobs);

                    //object loc = infoRecord.URLToIdentifyJobs; 
                    //object null_obj_str = "";
                    //System.Object null_obj = 0;
                    //_browser.Navigate2(ref loc, ref null_obj, ref null_obj, ref null_obj_str, ref null_obj_str);

                    //while (_browser.ReadyState != WebBrowserReadyState.Complete)
                    //{
                    //    Application.DoEvents();
                    //}

                    // this.processWebPage();

                    /**
                    WebClient wc = new WebClient();
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    Byte[] returnBytes = wc.DownloadData(infoRecord.URLToIdentifyJobs);
                        
                    htmlSource = Encoding.UTF8.GetString(returnBytes).Trim(); **/
                }
                catch (WebException e)
                {
                    System.Diagnostics.Debug.WriteLine("TaleoCrawler.cs / Failed to retrieve web HTML source for link (" + infoRecord.URLToIdentifyJobs +
                    "): " + e.Message);
                }

                //_browser.DocumentComplete -= new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.browser_DocumentComplete);
            }
        }

        private void browser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (_browser.ReadyState == WebBrowserReadyState.Complete)
            {
                //get HTML source
                System.Windows.Forms.HtmlDocument webDoc = _browser.Document;
                string htmlSource = string.Empty;

                if (!_identifyJobDetails) //are we looking for JOB IDs or job details?
                {
                    System.Windows.Forms.HtmlElement element = _browser.Document.GetElementById("requisitionListInterface");
                    if (element != null)
                    {
                        htmlSource = element.OuterHtml;
                    }

                    //find JOB IDs in the current web page
                    processHTMLDocumentForJobIDs(htmlSource);

                    //trigger pagination if there are more jobs
                    HtmlElementCollection anchorRefs = webDoc.Links;

                    bool nextLinkFound = false;
                    /**
                    foreach (System.Windows.Forms.HtmlElement anchor in anchorRefs)
                    {
                        string anchorText = anchor.InnerText;

                        if (anchorText != null && anchorText.Trim().ToLower().Equals("next"))
                        {
                            nextLinkFound = true;
                            object obj = anchor.DomElement;
                            System.Reflection.MethodInfo mi = obj.GetType().GetMethod("click");
                            mi.Invoke(obj, new object[0]);
                        }
                    }**/

                    if (!nextLinkFound)
                    {
                        //no further pagination 
                        retrieveJobDetailsForIDsFound();
                        //reset crawler for next web source
                        _crawlerInfoRecord = null;
                    }
                }
                else
                {
                    processHTMLDocumentForJobDetails(webDoc);
                }
            }
        }

        /// <summary>
        /// Retrieve job details for the JOB IDs that have been found by the crawler
        /// </summary>
        private void retrieveJobDetailsForIDsFound()
        {
            if (_crawlerInfoRecord != null && _crawlerInfoRecord.URLToExtractJobDetails != null &&
                !_crawlerInfoRecord.URLToExtractJobDetails.Equals(string.Empty))
            {
                string keyword = System.Configuration.ConfigurationManager.AppSettings["urlToExtractJobKeyword"];

                string jobID = _foundJobIds.Dequeue();

                string urlToNavigateTo = _crawlerInfoRecord.URLToExtractJobDetails.Replace(keyword, jobID);

                _identifyJobDetails = true;

                _browser.Navigate(urlToNavigateTo);
            }
        }
        /// <summary>
        /// Process the provided HTML source for Job Details as per Taleo format
        /// </summary>
        /// <param name="webDoc">Reference to the source web document</param>
        private void processHTMLDocumentForJobDetails(System.Windows.Forms.HtmlDocument webDoc_)
        {
            List<string> attributeValuesToMatch = new List<string>();
            attributeValuesToMatch.Add("subtitle");
            attributeValuesToMatch.Add("text");
            attributeValuesToMatch.Add("titlepage");

            List<System.Windows.Forms.HtmlElement> relevantElements = FindByAttributeNameAndValue(webDoc_.Body.All, "span", "class",
                attributeValuesToMatch, true);

            int x = 0;
            //string decodedHtmlSource = WebUtility.HtmlDecode(sourceHTMLDoc_);
        }

        /// <summary>
        /// This will search though this collection of nodes for the specified element with an
        /// attribute with the given name and containing one of the specified attribute values. 
        /// </summary>
        /// <param name="sourceElements_">Reference to the source web HTML elements</param>
        /// <param name="elementName_">The name of the element to look in</param>
        /// <param name="attributeName_">The name of the attribute to find</param>
        /// <param name="attributeValues_">List of attribute values that are accepted</param>
        /// <param name="searchChildren_">True if you want to search sub-nodes, False to
        /// only search this collection.</param>
        /// <returns>A list of all the elements that match.</returns>
        public List<System.Windows.Forms.HtmlElement> FindByAttributeNameAndValue(System.Windows.Forms.HtmlElementCollection sourceElements_, string elementName_, string attributeName_, List<string> attributeValues_, bool searchChildren_)
        {
            List<System.Windows.Forms.HtmlElement> results = new List<System.Windows.Forms.HtmlElement>();

            if (sourceElements_ != null && sourceElements_.Count > 0)
            {
                attributeName_ = attributeName_.ToLower();

                foreach (System.Windows.Forms.HtmlElement element in sourceElements_)
                {
                    if (element.Name.Trim().ToLower().Equals(elementName_.Trim().ToLower()))
                    {
                        string attributeVal = element.GetAttribute(attributeName_);

                        if (!attributeVal.Equals(string.Empty))
                        {
                            foreach (string acceptedAttributeVal in attributeValues_)
                            {
                                if (acceptedAttributeVal.Trim().ToLower().Equals(attributeVal.Trim().ToLower()))
                                {
                                    results.Add(element);
                                }
                            }
                        }

                        if (searchChildren_)
                        {
                            List<System.Windows.Forms.HtmlElement> childrenResults = FindByAttributeNameAndValue(element.All, elementName_, attributeName_,
                                attributeValues_, searchChildren_);

                            if (childrenResults.Count > 0)
                            {
                                results.AddRange(childrenResults);
                            }
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Process the provided HTML source for JOB IDs as per Taleo format
        /// </summary>
        /// <param name="sourceHTMLDoc_">The source HTML</param>
        private void processHTMLDocumentForJobIDs(string sourceHTMLDoc_)
        {
            string decodedHtmlSource = WebUtility.HtmlDecode(sourceHTMLDoc_);

            //Regex regex = new Regex(@"\(Job\sNumber:\s\w*\)", RegexOptions.IgnoreCase);
            Regex regex = new Regex("<div(\\s*)id=('*)(\"*)(\\w+)('*)(\"*)(\\s*)class=('*)(\"*)iconcontentpanel('*)(\"*)(\\s*)title=([\\w'\"]*)>", RegexOptions.IgnoreCase);

            MatchCollection matches = regex.Matches(decodedHtmlSource);

            foreach (Match match in matches)
            {
                string matchStr = match.Value;
                int colonIndex = matchStr.IndexOf("id=");
                matchStr = matchStr.Remove(0, colonIndex + 3);

                int spaceIndex = matchStr.IndexOf(' ');
                matchStr = matchStr.Substring(0, spaceIndex);
                matchStr = matchStr.Replace("'", "");
                matchStr = matchStr.Replace("\"", "");
                matchStr = matchStr.Trim();

                if (!_foundJobIds.Contains(matchStr))
                {
                    _foundJobIds.Enqueue(matchStr);
                }
            }
        }
    }
}