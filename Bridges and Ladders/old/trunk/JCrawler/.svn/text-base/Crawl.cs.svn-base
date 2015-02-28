using System;
using System.Collections.Generic;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace JCrawler
{
    public class Crawl
    {
        static void Crawler()
        {
            string strMainURL = "http://nokia.taleo.net/careersection/10120/jobdetail.ftl";
                         
            // The html retrieved from the page
            String strResult;
            WebResponse objResponse;
            WebRequest objRequest = System.Net.HttpWebRequest.Create(strMainURL);
            objResponse = objRequest.GetResponse();

            // The using keyword will automatically dispose the object once complete
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                strResult = sr.ReadToEnd();

                // Close and clean up the StreamReader
                sr.Close();
            }

            
        }

        private string[] JobIds(String webpage)
        { 

        }

    }
}