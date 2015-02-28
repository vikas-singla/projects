using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DatabaseHandler.DatabaseProcessors
{
    internal class FilterAdLinkProcessor : DatabaseProcessor
    {
        /// <summary>
        /// Reference to the singleton instance of this class
        /// </summary>
        private static FilterAdLinkProcessor _classInstance;
        /// <summary>
        /// Reference to the ad link list
        /// </summary>
        private List<string> _adLinkRepository;

        protected FilterAdLinkProcessor(SqlConnection dbConnection_)
            : base(dbConnection_)
        {
            _adLinkRepository = new List<string>();
        }

        /// <summary>
        /// Get the singleton instance of this class
        /// </summary>
        /// <param name="dbConnection_">The DB connection to use for this database processor class</param>
        /// <returns>Reference to instance of FilterAdLinkProcessor</returns>
        public static FilterAdLinkProcessor getInstance(SqlConnection dbConnection_)
        {
            if (_classInstance == null)
            {
                _classInstance = new FilterAdLinkProcessor(dbConnection_);
            }

            return _classInstance;
        }

        /// <summary>
        /// Retrieve teh list of ad links (used for HTML filter) from DB
        /// </summary>
        internal ReadOnlyCollection<string> dbRetrieveAdLinks()
        {
            if(_adLinkRepository.Count == 0)
            {
                try
                {
                    string cmdText = null;

                    cmdText = @"SELECT ADFILTER.regularExpression" + Environment.NewLine;
                    cmdText += @"FROM TBL_Filter_Advertisement_Links as ADFILTER" + Environment.NewLine;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdText, _dbConnection);
                    adapter.SelectCommand.CommandTimeout = 600;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        _adLinkRepository.Add(dr["regularExpression"] + string.Empty);
                    }
                }
                catch (SqlException e)
                {
                    System.Diagnostics.Debug.WriteLine("FilterAdLinkProcessor.cs / Failed fetching ad link repository data from db: " + e.Message);
                }
                finally
                {
                    closeConnection();
                }
            }

            return _adLinkRepository.AsReadOnly();
        }
    }
}
