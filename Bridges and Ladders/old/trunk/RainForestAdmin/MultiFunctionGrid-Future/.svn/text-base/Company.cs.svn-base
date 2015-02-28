using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace RainForestAdmin
{
    public class Company
    {
        private string cnstr = ConfigurationManager.ConnectionStrings["RAINFOREST_DB_CONNECTSTR"].ConnectionString;
        

        public Company()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void InsertIntoCompanyInfoTable(string CompanyName, string CompanyLogo, int CompanyTypeId, int CompanyOriginId,
            int IndustryId,string companyurl,string contactinfo,bool Fortune1000,int overallemployees, int indianemployees,
            string Workculture, string RecruitmentProcess,string day2dayexp,string benefits)
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_InsertCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                cmd.Parameters.Add(new SqlParameter("@IndustryId", IndustryId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyLogo", CompanyLogo));
                cmd.Parameters.Add(new SqlParameter("@CompanyTypeId", CompanyTypeId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyOriginId", CompanyOriginId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyUrl", companyurl));
                cmd.Parameters.Add(new SqlParameter("@ContactInfo", contactinfo));
                cmd.Parameters.Add(new SqlParameter("@Fortune1000", Fortune1000));
                cmd.Parameters.Add(new SqlParameter("@OverallEmployees", overallemployees));
                cmd.Parameters.Add(new SqlParameter("@IndiaEmployees", indianemployees));
                cmd.Parameters.Add(new SqlParameter("@WorkCulture", Workculture));
                cmd.Parameters.Add(new SqlParameter("@RecruitmentProcess", RecruitmentProcess));
                cmd.Parameters.Add(new SqlParameter("@Day2DayExp", day2dayexp));
                cmd.Parameters.Add(new SqlParameter("@Benefits", benefits));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public DataTable FetchCompanyDetails()
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
        }



        public void Update(string CompanyName, string CompanyLogo, int CompanyTypeId, int CompanyOriginId,
            int IndustryId, string companyurl, string contactinfo, bool Fortune1000, int overallemployees, int indianemployees,
            string Workculture, string RecruitmentProcess, string day2dayexp, string benefits)
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_UpdateCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                cmd.Parameters.Add(new SqlParameter("@IndustryId", IndustryId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyLogo", CompanyLogo));
                cmd.Parameters.Add(new SqlParameter("@CompanyTypeId", CompanyTypeId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyOriginId", CompanyOriginId+1));
                cmd.Parameters.Add(new SqlParameter("@CompanyUrl", companyurl));
                cmd.Parameters.Add(new SqlParameter("@ContactInfo", contactinfo));
                cmd.Parameters.Add(new SqlParameter("@Fortune1000", Fortune1000));
                cmd.Parameters.Add(new SqlParameter("@OverallEmployees", overallemployees));
                cmd.Parameters.Add(new SqlParameter("@IndiaEmployees", indianemployees));
                cmd.Parameters.Add(new SqlParameter("@WorkCulture", Workculture));
                cmd.Parameters.Add(new SqlParameter("@RecruitmentProcess", RecruitmentProcess));
                cmd.Parameters.Add(new SqlParameter("@Day2DayExp", day2dayexp));
                cmd.Parameters.Add(new SqlParameter("@Benefits", benefits));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Delete(int CompanyId)
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_DeleteCompanyDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public DataTable GetCompanyTypes()
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyTypesDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
              
            }
        }
        public DataTable GetCompanyOrigin()
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchCompanyOriginDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;

            }
        }
        public DataTable GetIndustry()
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "np_FetchIndustryDetails";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;

            }
        }
        public DataTable FetchCompanyLocation(int nCompanyId)
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
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
        public void UpdateLocation(int locid, string add1, string add2, string add3, string city, string state, string country, string postalcode)
        {
            using (SqlConnection conn = new SqlConnection(cnstr))
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

        public void InsertLocationDetails(int compid, string add1, string add2, string add3, string city, string state, string country, string postalcode)
        {
 
        }
    }
}