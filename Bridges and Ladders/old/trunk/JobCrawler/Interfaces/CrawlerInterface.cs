using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobCrawler.Interfaces
{
    interface CrawlerInterface
    {
        /// <summary>
        /// Main method of execution by the crawler
        /// </summary>
        void doWork();
    }
}
