using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using DatabaseHandler;
using JobCrawler.Interfaces;
using JobCrawler.CrawlerHelpers;

namespace JobCrawler
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Reference to the database handler to fetch data from the database
        /// </summary>
        private DatabaseHandler.DatabaseHandler _databaseHandler;
        /// <summary>
        /// Reference to the instantiated crawlers (by each type - eg. Taleo Crawler, etc.)
        /// </summary>
        private List<CrawlerInterface> _crawlers;

        public MainForm()
        {
            InitializeComponent();

            _databaseHandler = DatabaseHandler.DatabaseHandler.getInstance();
            _crawlers = new List<CrawlerInterface>();

            _crawlers.Add(new TaleoCrawler(0, _databaseHandler, crawlerBrowser));            
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            foreach (CrawlerInterface crawler in this._crawlers)
            {
                crawler.doWork();
            }
        }
    }
}
