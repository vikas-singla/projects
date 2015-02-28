using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.BusinessObjectClasses;
using RainForestAdmin.DbClasses;
using System.Data.SqlClient;
using System.EnterpriseServices;


namespace RainForestAdmin
{
    public partial class BasicCompanyInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                PopulateCombos();
                
            }
            if (Session["HeadsInfo"] != null)
            {
                gv_heads.DataSource = (List<AddCompanyHeadsClass>)Session["HeadsInfo"];
                gv_heads.DataBind();
            }

            if (Session["CompanyLocationsInfo"] != null)
            {
                gv_Locations.DataSource = (List<CompanyLocationInfo>)Session["CompanyLocationsInfo"];
                gv_Locations.DataBind();
            }
            if (Session["CompanyPictures"] != null)
            {
                gv_picsvideos.DataSource = (List<CompanyPictures>)Session["CompanyPictures"];
                gv_picsvideos.DataBind();
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

        protected void btnAddGrowthPath_Click(object sender, EventArgs e)
        {
            List<GrowthLadder> growthlist = null;
            GrowthLadder ladder = new GrowthLadder();
            ladder.Designation = txt_PositionName.Text;
            ladder.CompensationRange = txt_Compensation.Text;
            ladder.Position = ddl_PositionGrowthPath.SelectedValue;
            
            if (Session["GrowthList"] != null)
            {
                growthlist = (List<GrowthLadder>)Session["GrowthList"];
                
            }
            else
            {
                growthlist = new List<GrowthLadder>();
                
            }
            if (growthlist.Count == 10)
            {
                ddl_PositionGrowthPath.Enabled = false;
                txt_Compensation.Enabled = false;
                txt_PositionName.Enabled = false;
                btnAddGrowthPath.Enabled = false;
            }
            else
            growthlist.Add(ladder);
            Session["GrowthList"] = growthlist;
            GridView1.DataSource = growthlist;
            GridView1.DataBind();
            ddl_PositionGrowthPath.SelectedItem.Enabled = false;
            
            ddl_PositionGrowthPath.SelectedIndex = -1;
            txt_PositionName.Text = String.Empty;
            txt_Compensation.Text = String.Empty;
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            SaveCompanyInfo();
        }
        
        private void SaveCompanyInfo()
        {
            DBClass dbobject = new DBClass();
            int compid = dbobject.InsertIntoCompanyInfoTable(txt_CompanyName.Text, fupload_CompanyLogo.FileName, ddl_CompanyType.SelectedIndex, ddl_CompanyOrigin.SelectedIndex
                , ddl_Industry.SelectedIndex, txt_CompanyUrl.Text, txt_ContactInfo.Text, (bool)(ddl_Fortune1000.SelectedValue == "Yes"),
                Convert.ToInt32(txt_NumberOfEmpOverall.Text), Convert.ToInt32(txt_NumOfEmpIndia.Text), txt_wrkculture.Text, txt_RecruitmentProcess.Text,
                txt_d2dExperience.Text, txt_Benefits.Text,txt_Pros.Text,txt_Cons.Text,txt_materialInterview.Text,fupload_material.FileName);
            if (compid != -1)
            {
                Session["CompanyId"] = compid;
                Session["LastCompanyInserted"] = txt_CompanyName.Text;
                if(Session["HeadsInfo"] != null)
                SaveCompanyHeadsInfo();
                if(Session["CompanyLocationsInfo"]!= null)
                SaveCompanyLocations();
                if(Session["CompanyPictures"]!=null)
                SaveCompanyPicsVideosInfo();
                if (Session["GrowthList"] != null)
                    SaveCompanyGrowthPath();
                
                Session["HeadsInfo"] = null;
                Session["CompanyLocationsInfo"] = null;
                Session["CompanyPictures"] = null;

                Response.Redirect("~/NotificationPages/DBOperationSucceeded.aspx?CompanyName=" + txt_CompanyName.Text + "&Operation=Inserted");
            }
        }

        private bool SaveCompanyGrowthPath()
        {
            int nRet = 0;
            bool bSuccess = false;
            DBClass dbobject = new DBClass();

            List<GrowthLadder> list = (List<GrowthLadder>)Session["GrowthList"];
            foreach (GrowthLadder growth in list)
            {
                int nVal = dbobject.InsertGrowthPathCompensationInfo(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(growth.Position),growth.Designation,Convert.ToInt32(growth.CompensationRange));
                nRet += nVal;
            }
            if (nRet == list.Count)
            {
                bSuccess = true;
                Session["GrowthList"] = null;
            }
            return bSuccess;
        }
        private bool SaveCompanyHeadsInfo()
        {
            int nRet = 0;
            bool bSuccess = false;
            DBClass dbobject = new DBClass();
           
                List<AddCompanyHeadsClass> list = (List<AddCompanyHeadsClass>)Session["HeadsInfo"];
                foreach (AddCompanyHeadsClass heads in list)
                {
                    int nVal = dbobject.InsertCompanyHeadsInfo(Convert.ToInt32(Session["CompanyId"]), heads.HeadName, heads.HeadDesignation);
                    nRet += nVal;
                }
                if (nRet == list.Count)
                {
                    bSuccess = true;
                    Session["HeadsInfo"] = null;
                }
            
            return bSuccess;
        }
        private bool SaveCompanyLocations()
        {
            int nRet = 0;
            bool bSuccess = false;
            DBClass dbobject = new DBClass();
            List<CompanyLocationInfo> list = (List<CompanyLocationInfo>)Session["CompanyLocationsInfo"];
            foreach(CompanyLocationInfo loc in list)
            {
                int nVal = dbobject.InsertCompanyLocationInfo(Convert.ToInt32(Session["CompanyId"]),loc.AddLine1,loc.AddLine2,loc.AddLine3,loc.State,loc.City,loc.Country,loc.Pincode);
                nRet += nVal;
            }
            if (nRet == list.Count)
            {
                bSuccess = true;
                Session["CompanyLocationsInfo"] = null;
            }
            return bSuccess;
                      
        }
        private bool SaveCompanyPicsVideosInfo()
        {
            int nRet = 0;
            bool bSuccess = false;
            DBClass dbobject = new DBClass();
            List<CompanyPictures> list = (List<CompanyPictures>)Session["CompanyPictures"];
            foreach (CompanyPictures pic in list)
            {
                int nVal = dbobject.InsertCompanyPicVideo(Convert.ToInt32(Session["CompanyId"]),pic.PicturePath,pic.VideoPath);
                nRet += nVal;
            }
            if (nRet == list.Count)
            {
                bSuccess = true;
                Session["CompanyPictures"] = null;
            }
            return bSuccess;
        }

        protected void txt_Compensation_TextChanged(object sender, EventArgs e)
        {
            
        }


    }
}