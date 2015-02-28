using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace RainForestAdmin.DbClasses
{
    public class DBClass
    {
        private string dbconnectionString;

        public string DBConnectionString
        {
            get { return dbconnectionString; }
            set { dbconnectionString = value; }
        }
        public DBClass()
        {
            DBConnectionString = ConfigurationManager.ConnectionStrings["rainforest_dbConnectionString"].ConnectionString;

        }
        public int InsertIntoCompanyInfoTable(string CompanyName, string CompanyLogo, int CompanyTypeId, int CompanyOriginId,
           int IndustryId, string companyurl, string contactinfo, bool Fortune1000, int overallemployees, int indianemployees,
           string Workculture, string RecruitmentProcess, string day2dayexp, string benefits, string pros, string cons, string material, string uploadmaterial)
        {
            //    string[] xmlnodes = new string[14] {"CompanyName","IndustryId","CompanyLogo","CompanyTypeId","CompanyOriginId","CompanyUrl",
            //"ContactInfo","Fortune1000","OverallEmployees","OverallEmployeesIndia","Workcultrue","RecruitmentProcess","day2dayexp","Benefits"};
            //    BuildXmlStringForSQLSP
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                cmd.Parameters.Add(new SqlParameter("@IndustryId", IndustryId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyLogo", CompanyLogo));
                cmd.Parameters.Add(new SqlParameter("@CompanyTypeId", CompanyTypeId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyOriginId", CompanyOriginId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyUrl", companyurl));
                cmd.Parameters.Add(new SqlParameter("@ContactInfo", contactinfo));
                cmd.Parameters.Add(new SqlParameter("@Fortune1000", Fortune1000));
                cmd.Parameters.Add(new SqlParameter("@OverallEmployees", overallemployees));
                cmd.Parameters.Add(new SqlParameter("@IndiaEmployees", indianemployees));
                cmd.Parameters.Add(new SqlParameter("@WorkCulture", Workculture));
                cmd.Parameters.Add(new SqlParameter("@RecruitmentProcess", RecruitmentProcess));
                cmd.Parameters.Add(new SqlParameter("@Day2DayExp", day2dayexp));
                cmd.Parameters.Add(new SqlParameter("@Benefits", benefits));
                cmd.Parameters.Add(new SqlParameter("@Pros", pros));
                cmd.Parameters.Add(new SqlParameter("@Cons", cons));
                cmd.Parameters.Add(new SqlParameter("@Material", material));
                cmd.Parameters.Add(new SqlParameter("@UploadedMaterial", uploadmaterial));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@ReturnCompanyId", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);

            }
        }
        public int InsertCompanyLocationInfo(int companyid, string address1, string address2, string address3, string state, string city, string country, string postalcode)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertCompanyLocations";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@AddressLine1", address1));
                cmd.Parameters.Add(new SqlParameter("@AddressLine2", address2));
                cmd.Parameters.Add(new SqlParameter("@AddressLine3", address3));
                cmd.Parameters.Add(new SqlParameter("@City", city));
                cmd.Parameters.Add(new SqlParameter("@State", state));
                cmd.Parameters.Add(new SqlParameter("@Country", country));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", postalcode));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@RowsAffected", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);
            }
        }
        public int InsertGrowthPathCompensationInfo(int CompanyId, int positionid, string positionname, Int64 payscale)
        {

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertGrowthPathCompensationInfo";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
                cmd.Parameters.Add(new SqlParameter("@PositionId", positionid));
                cmd.Parameters.Add(new SqlParameter("@PositionName", positionname));
                cmd.Parameters.Add(new SqlParameter("@Payscale", payscale));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@RowsAffected", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);
            }
        }
        public int InsertCompanyHeadsInfo(int companyid, string headname, string desgination)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertCompanyHeadsInfo";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@HeadName", headname));
                cmd.Parameters.Add(new SqlParameter("@Designation", desgination));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@RowsAffected", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);
            }
        }
        public int InsertCompanyPicVideo(int companyid, string picpath, string videopath)
        {

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertCompanyPicsVideos";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", companyid));
                cmd.Parameters.Add(new SqlParameter("@PicturePath", picpath));
                cmd.Parameters.Add(new SqlParameter("@VideoPath", videopath));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@RowsAffected", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);
            }
        }

        //can be used to pass list as xml to sql SP's
        //public static string BuildXmlStringForSQLSP(string xmlRootName,string objectname,string[] xmlnodes, string[] values)
        //{
        //    StringBuilder xmlString = new StringBuilder();

        //    xmlString.AppendFormat("<{0}><{1}>", xmlRootName, objectname);

        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        xmlString.AppendFormat("<{0}>{1}</{0}>",xmlnodes[i],values[i]);
        //    }
        //    xmlString.AppendFormat("</{0}></{1}>", objectname,xmlRootName);

        //    return xmlString.ToString();
        //}
        public DataSet FetchCompanyBasicInfo(int companyid)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", companyid));
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(ds);
                conn.Close();
                return ds;

            }
        }
        public void UpdateLocation(int locid, string add1, string add2, string add3, string city, string state, string country, string postalcode)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_UpdateLocationDetails";
                cmd.Parameters.Add(new SqlParameter("@LocationID", locid));
                cmd.Parameters.Add(new SqlParameter("@Addr1", add1));
                cmd.Parameters.Add(new SqlParameter("@Addr2", add2));
                cmd.Parameters.Add(new SqlParameter("@Addr3", add3));
                cmd.Parameters.Add(new SqlParameter("@City", city));
                cmd.Parameters.Add(new SqlParameter("@State", state));
                cmd.Parameters.Add(new SqlParameter("@Country", country));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", postalcode));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }

        
        public DataTable FetchCompanyLocation(int nCompanyId)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyLocationDetails";
                cmd.Parameters.Add(new SqlParameter("@CompanyId", nCompanyId));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
        }


       
        public int UpdateCompanyDetails(int CompanyId,string CompanyName, string CompanyLogo, int CompanyTypeId, int CompanyOriginId,
           int IndustryId, string companyurl, string contactinfo, bool Fortune1000, int overallemployees, int indianemployees,
           string Workculture, string RecruitmentProcess, string day2dayexp, string benefits, string pros, string cons, string material, string uploadmaterial)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_UpdateCompanyInfo";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
                cmd.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                cmd.Parameters.Add(new SqlParameter("@IndustryId", IndustryId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyLogo", CompanyLogo));
                cmd.Parameters.Add(new SqlParameter("@CompanyTypeId", CompanyTypeId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyOriginId", CompanyOriginId + 1));
                cmd.Parameters.Add(new SqlParameter("@CompanyUrl", companyurl));
                cmd.Parameters.Add(new SqlParameter("@ContactInfo", contactinfo));
                cmd.Parameters.Add(new SqlParameter("@Fortune1000", Fortune1000));
                cmd.Parameters.Add(new SqlParameter("@OverallEmployees", overallemployees));
                cmd.Parameters.Add(new SqlParameter("@IndiaEmployees", indianemployees));
                cmd.Parameters.Add(new SqlParameter("@WorkCulture", Workculture));
                cmd.Parameters.Add(new SqlParameter("@RecruitmentProcess", RecruitmentProcess));
                cmd.Parameters.Add(new SqlParameter("@Day2DayExp", day2dayexp));
                cmd.Parameters.Add(new SqlParameter("@Benefits", benefits));
                cmd.Parameters.Add(new SqlParameter("@Pros", pros));
                cmd.Parameters.Add(new SqlParameter("@Cons", cons));
                cmd.Parameters.Add(new SqlParameter("@Material", material));
                cmd.Parameters.Add(new SqlParameter("@UploadedMaterial", uploadmaterial));
                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("@ReturnCompanyId", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                return Convert.ToInt32(returnValue.Value);

            }
        }
        public DataTable FetchCompanyHeadDetails(int nCompanyId)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyHeadDetails";
                cmd.Parameters.Add(new SqlParameter("@CompanyId", nCompanyId));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
        }
        public void UpdateHeadsInfo(int headid, string name, string designation)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_UpdateHeadsDetails";
                cmd.Parameters.Add(new SqlParameter("@HeadID", headid));
                cmd.Parameters.Add(new SqlParameter("@Name", name));
                cmd.Parameters.Add(new SqlParameter("@Designation", designation));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }
        public DataTable FetchCompanyPicDetails(int nCompanyId)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyPicsDetails";
                cmd.Parameters.Add(new SqlParameter("@CompanyId", nCompanyId));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
        }
        public void UpdatePicsInfo(int picid, string picpath, string videopath)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_UpdatePicsDetails";
                cmd.Parameters.Add(new SqlParameter("@PicId", picid));
                cmd.Parameters.Add(new SqlParameter("@PicPath", picpath));
                cmd.Parameters.Add(new SqlParameter("@VideoPath", videopath));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }

       

    }

}



    
