using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DatabaseHandler.DatabaseProcessors
{
    internal class TaleoDatabaseProcessor : DatabaseProcessor
    {
        /// <summary>
        /// Reference to the singleton instance of this class
        /// </summary>
        private static TaleoDatabaseProcessor _classInstance;
        /// <summary>
        /// Queue of information records that are pending processing from the crawler
        /// </summary>
        private Queue<TaleoJobCrawlerInfo> _infoRecordsUnprocessed;
        /// <summary>
        /// Queue of information records that have been processed by the crawler
        /// </summary>
        private Queue<TaleoJobCrawlerInfo> _infoRecordsProcessed;

        protected TaleoDatabaseProcessor(SqlConnection dbConnection_)
            : base(dbConnection_)
        {
            _infoRecordsUnprocessed = new Queue<TaleoJobCrawlerInfo>();
            _infoRecordsProcessed = new Queue<TaleoJobCrawlerInfo>();
        }

        /// <summary>
        /// Get the singleton instance of this class
        /// </summary>
        /// <param name="dbConnection_">The DB connection to use for this database processor class</param>
        /// <returns>Reference to instance of TaleoDatabaseProcessor</returns>
        public static TaleoDatabaseProcessor getInstance(SqlConnection dbConnection_)
        {
            if (_classInstance == null)
            {
                _classInstance = new TaleoDatabaseProcessor(dbConnection_);
            }

            return _classInstance;
        }

        /// <summary>
        /// Retrieve the next unprocessed Taleo job info record that has not yet been processed by the crawler
        /// </summary>
        /// <returns>Reference to the next record if it exists, null otherwise</returns>
        public TaleoJobCrawlerInfo dbRetrieveNextUnprocessedTaleoJobInfoRecord()
        {
            TaleoJobCrawlerInfo result = null;

            if (_infoRecordsUnprocessed.Count == 0 && _infoRecordsProcessed.Count == 0)
            {
                _infoRecordsUnprocessed = dbRetrieveTaleoJobInfoRecords();
                _infoRecordsProcessed.Clear();
            }

            if (_infoRecordsUnprocessed.Count > 0) //did we find anything in DB?
            {
                result = _infoRecordsUnprocessed.Dequeue();
                _infoRecordsProcessed.Enqueue(result);
            }

            return result;
        }

        /// <summary>
        /// Retrieve job info records for the Taleo crawler to use
        /// </summary>
        /// <returns>Reference to the found records</returns>
        internal Queue<TaleoJobCrawlerInfo> dbRetrieveTaleoJobInfoRecords()
        {
            Queue<TaleoJobCrawlerInfo> results = new Queue<TaleoJobCrawlerInfo>();

            try
            {
                string cmdText = "SELECT info.WebCrawlerTaleoInfoId, info.urlToIdentifyJobs, info.urlToExtractJobDetails, " + Environment.NewLine;
                cmdText += "info.locationTags, info.companyId, comp.Name, comp.IndustryId, ind.IndustryName " + Environment.NewLine;
                cmdText += "FROM TBL_WebCrawler_TaleoInfo info, " + Environment.NewLine;
                cmdText += "TBL_Company comp, TBL_Industry ind WHERE " + Environment.NewLine;
                cmdText += "info.companyId = comp.companyId AND comp.IndustryId = ind.IndustryId";

                SqlDataAdapter adapter = new SqlDataAdapter(cmdText, _dbConnection);

                // initialize the database connection
                initConnection();

                adapter.SelectCommand.CommandTimeout = 600;
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    int infoId = int.Parse(dr["WebCrawlerTaleoInfoId"] + string.Empty);
                    string urlToIdentifyJobs = dr["urlToIdentifyJobs"] + string.Empty;
                    string urlToExtractJobDetails = dr["urlToExtractJobDetails"] + string.Empty;
                    int companyId = int.Parse(dr["companyId"] + string.Empty);
                    string companyName = dr["Name"] + string.Empty;
                    string locationTagsRaw = dr["locationTags"] + string.Empty;
                    string[] locationTags = locationTagsRaw.Trim().Split(';');

                    List<string> locationTagList = new List<string>(locationTags);                    

                    /**
                    int locationId = int.Parse(dr["HQLocationId"] + string.Empty);
                    string city = dr["city"] + string.Empty;
                    string country = dr["country"] + string.Empty;
                    string addressLine1 = dr["AddressLine1"] + string.Empty;
                    string addressLine2 = dr["AddressLine2"] + string.Empty;
                    string addressLine3 = dr["AddressLine3"] + string.Empty;
                    string postalCode = dr["PostalCode"] + string.Empty;**/
                    int industryId = int.Parse(dr["IndustryId"] + string.Empty);
                    string industryName = dr["IndustryName"] + string.Empty;

                    Industry industry = new Industry(industryId, industryName);
                    //Location hqLocation = new Location(locationId, city, country, addressLine1, addressLine2, addressLine3, postalCode);
                    Company company = new Company(companyId, companyName, industry);
                    TaleoJobCrawlerInfo info = new TaleoJobCrawlerInfo(infoId, urlToIdentifyJobs, urlToExtractJobDetails, company, locationTagList);

                    results.Enqueue(info);
                }
            }
            catch (SqlException e)
            {
                System.Diagnostics.Debug.WriteLine("TaleoDatabaseProcessor.cs / Failed to retrieve taleo job info records from db: " + e.Message);
            }
            finally
            {
                closeConnection();

            }

            return results;
        }
    }
}
