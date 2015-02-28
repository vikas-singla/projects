using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.DbClasses;
using System.Data;
using RainForestAdmin.BusinessObjectClasses;

namespace RainForestAdmin
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DatabaseOperationsDataContext dbcontext = new DatabaseOperationsDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["rainforest_dbConnectionString"].ConnectionString);
            var compnames = from t in dbcontext.TBL_Companies
                           select t
                            ;
            ddl_CompanyNames.DataSource = compnames;
            if (!IsPostBack)
            {
                
                ddl_CompanyNames.DataBind();
                ddl_CompanyNames.SelectedIndex = 0;
                
            }
            if (Convert.ToInt32(Session["EditCompanyId"]) != Convert.ToInt32(ddl_CompanyNames.SelectedItem.Value))
            {
                Session["EditCompanyId"] = Convert.ToInt32(ddl_CompanyNames.SelectedItem.Value);
                LoadCompanyData(Convert.ToInt32(Session["EditCompanyId"]));
                PopulateCombos();
            }
            
            
        }
        private void PopulateCombos()
        {
            DatabaseOperationsDataContext dbcontext = new DatabaseOperationsDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["rainforest_dbConnectionString"].ConnectionString);
            var industry = from t in dbcontext.TBL_Industries
                           select t;
            ddl_Industry.DataSource = industry;
            ddl_Industry.DataBind();

            var companytype = from t in dbcontext.TBL_Company_Types
                              select t;
            ddl_CompanyType.DataSource = companytype;
            ddl_CompanyType.DataBind();

            var companyorigin = from t in dbcontext.TBL_Company_Origins
                                select t;
            ddl_CompanyOrigin.DataSource = companyorigin;
            ddl_CompanyOrigin.DataBind();

        }
      

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            DBClass dbop = new DBClass();
            int nRet = dbop.UpdateCompanyDetails(Convert.ToInt32(Session["EditCompanyId"]),txt_CompanyName.Text, fupload_CompanyLogo.FileName, ddl_CompanyType.SelectedIndex, ddl_CompanyOrigin.SelectedIndex
                , ddl_Industry.SelectedIndex, txt_CompanyUrl.Text, txt_ContactInfo.Text, (bool)(ddl_Fortune1000.SelectedValue == "Yes"),
                Convert.ToInt32(txt_NumberOfEmpOverall.Text), Convert.ToInt32(txt_NumOfEmpIndia.Text), txt_wrkculture.Text, txt_RecruitmentProcess.Text,
                txt_d2dExperience.Text, txt_Benefits.Text, txt_Pros.Text, txt_Cons.Text, txt_materialInterview.Text, fupload_material.FileName);
            if (nRet != -1)
            {
                Page.Session.Clear();

                Response.Redirect("~/NotificationPages/DBOperationSucceeded.aspx?CompanyName=" + txt_CompanyName.Text + "&Operation=Updated");
            }
        }
        private void LoadCompanyData(int nCompId)
        {

            DBClass db = new DBClass();
            DataSet ds = db.FetchCompanyBasicInfo(Convert.ToInt32(Session["EditCompanyId"]));
            DataTable dtable = ds.Tables[0];
            txt_CompanyName.Text = dtable.Rows[0]["CompanyName"].ToString();
            txt_Benefits.Text = dtable.Rows[0]["Benefits"].ToString();
            txt_CompanyUrl.Text = dtable.Rows[0]["companyurl"].ToString();
            txt_Cons.Text = dtable.Rows[0]["Cons"].ToString();
            txt_ContactInfo.Text = dtable.Rows[0]["contactinfo"].ToString();
            txt_d2dExperience.Text = dtable.Rows[0]["Experience"].ToString();
            txt_materialInterview.Text = dtable.Rows[0]["Prep-material"].ToString();
            txt_NumberOfEmpOverall.Text = dtable.Rows[0]["overallEmployees"].ToString();
            txt_NumOfEmpIndia.Text = dtable.Rows[0]["overallEmployeesIndia"].ToString();
            txt_Pros.Text = dtable.Rows[0]["Pros"].ToString();
            txt_RecruitmentProcess.Text = dtable.Rows[0]["RecruitmentProcess"].ToString();
            txt_wrkculture.Text = dtable.Rows[0]["WorkCulture"].ToString();
            int nCompanyType = Convert.ToInt32(dtable.Rows[0]["companyTypeId"])-1;
            int nCompanyOrigin = Convert.ToInt32(dtable.Rows[0]["companyOriginId"])-1;
            int nIndustryId = Convert.ToInt32(dtable.Rows[0]["IndustryId"])-1;
            bool bFortune1000 = Convert.ToBoolean(dtable.Rows[0]["Fortune1000"]);
            ddl_CompanyOrigin.SelectedIndex = nCompanyOrigin;
            ddl_CompanyType.SelectedIndex = nCompanyType;
            ddl_Industry.SelectedIndex = nIndustryId;
            ddl_Fortune1000.SelectedIndex = Convert.ToInt32(bFortune1000);

        }
        protected override void OnPreRender(EventArgs e)
        {
            
            base.OnPreRender(e);
        }

        

       

        
    }
}
