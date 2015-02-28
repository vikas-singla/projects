using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseHandler.DatabaseProcessors
{
    /// <summary>
    /// Base class for all process-type database classes
    /// </summary>
    internal class DatabaseProcessor
    {
        /// <summary>
        /// Reference to the SQL database connection
        /// </summary>
        protected SqlConnection _dbConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection_">The DB connection to use for this database processor class</param>
        protected DatabaseProcessor(SqlConnection dbConnection_)
        {
            _dbConnection = dbConnection_;
        }
        
        /// <summary>
        /// Initialize the database connection specified
        /// </summary>
        protected void initConnection()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
        }

        /// <summary>
        /// Close the database connection specified
        /// </summary>
        protected void closeConnection()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
            }
        }
    }
}
