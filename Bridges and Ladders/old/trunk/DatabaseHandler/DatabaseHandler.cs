using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using DatabaseHandler.DatabaseProcessors;
using Common;


namespace DatabaseHandler
{
    public class DatabaseHandler
    {
        /// <summary>
        /// Reference to the singleton of this class
        /// </summary>
        private static DatabaseHandler _classInstance;
        /// <summary>
        /// Reference to the SQL database connection
        /// </summary>
        private SqlConnection _dbConnection;
        /// <summary>
        /// Reference to the TaleoDatabaseProcessor class
        /// </summary>
        private TaleoDatabaseProcessor _taleoDatabaseProcessor;
        /// <summary>
        /// Reference to the FilterAdLinkProcessor class
        /// </summary>
        private FilterAdLinkProcessor _filterAdLinkProcessor;

        protected DatabaseHandler()
        {
            string conn = System.Configuration.ConfigurationManager.AppSettings["dbConnectionString"];
            if (conn == null)
            {
                // not run from current project and so retrieve from equivalent web.config file in the startup project
                conn = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            }

            _dbConnection = new SqlConnection(conn);

            _taleoDatabaseProcessor = TaleoDatabaseProcessor.getInstance(_dbConnection);
            _filterAdLinkProcessor = FilterAdLinkProcessor.getInstance(_dbConnection);
        }

        /// <summary>
        /// Get the singleton instance of this class
        /// </summary>
        /// <returns>Reference to instance of DatabaseHandler</returns>
        public static DatabaseHandler getInstance()
        {
            if (_classInstance == null)
            {
                _classInstance = new DatabaseHandler();
            }

            return _classInstance;
        }

        /// <summary>
        /// Retrieve the next unprocessed Taleo job info record that has not yet been processed by the crawler
        /// </summary>
        /// <returns>Reference to the next record if it exists, null otherwise</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public TaleoJobCrawlerInfo retrieveNextUnprocessedTaleoJobInfoRecord()
        {
            return _taleoDatabaseProcessor.dbRetrieveNextUnprocessedTaleoJobInfoRecord();
        }

        /// <summary>
        /// Retrieve teh list of ad links (used for HTML filter) from DB
        /// </summary>
        public ReadOnlyCollection<string> retrieveAdLinks()
        {
            return _filterAdLinkProcessor.dbRetrieveAdLinks();
        }
        
    }
}
