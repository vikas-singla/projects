using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.Business_Objects;

using System.IO;


namespace RainForestAdmin
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
               
            }
        }
        
        protected void Btn_SaveCompanyInfo_Click(object sender, EventArgs e)
        {
            
            AddToDB();
            
            

        }
        private void AddToDB()
        {
            //CompanyProfileEntry entry = new CompanyProfileEntry();
            //entry.CompanyClass = Ddl_CompanyOrigin.SelectedValue;
            //entry.CompanyLogoPath = Fupload_CompanyLogo.FileName;
            //entry.CompanyRegisteredName = txt_CompanyName.Text; 
            //entry.CompanyType = Ddl_CompanyType.SelectedValue;
            //entry.CompanyUrl = txt_CompanyUrl.Text; ;
            //entry.CompnyContactInfo = txt_ContactInfo.Text; ;
            //entry.DailyExperience = txt_dailyexperience.Text; ;
            //entry.Fortune1000 = ddl_Fortune1000.SelectedValue;
            ////entry.GrowthPath = imgmap_GrowthPath.ImageUrl;
            //entry.Industry = ddl_Industry.SelectedValue;
            ////entry.InsiderScoop = radio_InsiderSoop.SelectedValue;
            //entry.MaterialsUrl = txt_material.Text;
            //entry.NumberofEmployeesInIndia = txt_NumberOfEmpInIndia.Text;
            //entry.NumberofEmployeesWW = txt_NumberOfEmpOverall.Text; ;
            //entry.RecruitmentProcess = txt_recruitmentProcess.Text; ;
            //entry.WorkCulture = txt_WorkCulture.Text;
            //entry.VideoUrlEmbed = fUpload_Videos.FileName;
            ////entry.CompanyHeads = (List<CompanyHeads>)ViewState["CompanyHeadInfo"];
            //List<CompanyHeads> heads = (List<CompanyHeads>)ViewState["CompanyHeadInfo"];
            //entry.CompanyHeads = heads;
            ////still to work for 
            ////entry.OtherBenefits = ;
            ////entry.PayComparision= ;
            ////entry.PostionAndCompensation = ;
            
        }

        protected void btn_Undo_Click(object sender, EventArgs e)
        {
            //foreach (Control s in Page.Controls)
            //{
            //    if (s is TextBox)
            //    {
            //        (s as TextBox).Text = String.Empty;
            //    }
            //    else if (s is DropDownList)
            //    {
            //        (s as DropDownList).SelectedIndex = -1;
            //    }
            //    else if (s is AddCompanyHeads)
            //    {
            //        (s as AddCompanyHeads).ClearAll();
            //    }
            //}
        }

        protected void btnAddGrowthPath_Click(object sender, EventArgs e)
        {
            List<GrowthPath> lstgpath = null;
            GrowthPath gpath = new GrowthPath();
            gpath.Position= ddl_PositionGrowthPath.SelectedValue;
            gpath.Desingnation = txt_PositionName.Text;
            if (ViewState["GrowthPath"] == null)
                lstgpath = new List<GrowthPath>();
            else
                lstgpath = (List<GrowthPath>)ViewState["GrowthPath"];
            lstgpath.Add(gpath);
            ViewState["GrowthPath"] = (object)lstgpath;
            ddl_PositionGrowthPath.SelectedIndex = -1;
            txt_PositionName.Text = String.Empty;
        }

        protected void btnAddCompensation_Click(object sender, EventArgs e)
        {
            List<Compensation> lstcompen = null;
            Compensation compen = new Compensation();
            compen.Position = ddl_PositionCompensation.SelectedValue;
            compen.CompensationRange = txt_Compensation.Text;
            if (ViewState["Compensation"] == null)
                lstcompen = new List<Compensation>();
            else
                lstcompen = (List<Compensation>)ViewState["Compensation"];
            lstcompen.Add(compen);
            ViewState["Compensation"] = (object)lstcompen;
            ddl_PositionCompensation.SelectedIndex = -1;
            txt_Compensation.Text = String.Empty;
        }

       
        

        
       
    }
}
