using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace RainForestAdmin
{
    public partial class AdminPage : System.Web.UI.Page
    {
        Company company = new Company();
        protected override void OnInit(EventArgs e)
        {
            
            base.OnInit(e);
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gv_company.SelectedIndex = 0;
                FillInfoInCompanyGrid();
                FillInfoInLocationGrid();
            }
            
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillInfoInLocationGrid();
        }


        private void FillInfoInCompanyGrid()
        {
            DataTable dtCompany = company.FetchCompanyDetails();
            

            if (dtCompany.Rows.Count > 0)
            {
                gv_company.DataSource = dtCompany;
                gv_company.DataBind();
                
                
            }
            else
            {
                dtCompany.Rows.Add(dtCompany.NewRow());
                gv_company.DataSource = dtCompany;
                gv_company.DataBind();

                int TotalColumns = gv_company.Rows[0].Cells.Count;
                gv_company.Rows[0].Cells.Clear();
                gv_company.Rows[0].Cells.Add(new TableCell());
                gv_company.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gv_company.Rows[0].Cells[0].Text = "No Record Found";
            }
        }

        private void FillInfoInLocationGrid()
        {
            DataTable dtLocation = company.FetchCompanyLocation(Convert.ToInt32(gv_company.SelectedRow.Cells[3].Text));


            if (dtLocation.Rows.Count > 0)
            {
                gv_location.DataSource = dtLocation;
                gv_location.DataBind();


            }
            else
            {
                dtLocation.Rows.Add(dtLocation.NewRow());
                gv_location.DataSource = dtLocation;
                gv_location.DataBind();

                int TotalColumns = gv_location.Rows[0].Cells.Count;
                gv_location.Rows[0].Cells.Clear();
                gv_location.Rows[0].Cells.Add(new TableCell());
                gv_location.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gv_location.Rows[0].Cells[0].Text = "No Record Found";
            }
        }
        #region CompanyGrid

        protected void gvcompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                TextBox txtNewCompanyName = (TextBox)gv_company.FooterRow.FindControl("txt_NewCompany");
                DropDownList ddlNewFortune = (DropDownList)gv_company.FooterRow.FindControl("ddl_NewFortune");
                TextBox txtNewLogoPath = (TextBox)gv_company.FooterRow.FindControl("txt_NewLogoPath");
                TextBox txtNewOverallEmployees = (TextBox)gv_company.FooterRow.FindControl("txt_NewOverallEmployees");
                TextBox txtNewOverallEmployeesIndia = (TextBox)gv_company.FooterRow.FindControl("txt_NewOverallEmployeesIndia");
                TextBox txtNewComapnyUrl = (TextBox)gv_company.FooterRow.FindControl("txt_NewCompanyUrl");
                TextBox txtNewContactInfo = (TextBox)gv_company.FooterRow.FindControl("txt_NewContactInfo");
                TextBox txtNewWorkculture = (TextBox)gv_company.FooterRow.FindControl("txt_NewWorkculture");
                TextBox txtNewExperience = (TextBox)gv_company.FooterRow.FindControl("txt_NewExperience");
                TextBox txtNewRecruitment = (TextBox)gv_company.FooterRow.FindControl("txt_NewRecruitment");
                TextBox txtNewBenefits = (TextBox)gv_company.FooterRow.FindControl("txt_NewBenefits");
                DropDownList ddlNewIndustry = (DropDownList)gv_company.FooterRow.FindControl("ddl_NewIndustry");
                DropDownList ddlNewCompanyOrigin = (DropDownList)gv_company.FooterRow.FindControl("ddl_NewCompanyOrigin");
                DropDownList ddlNewCompanyType = (DropDownList)gv_company.FooterRow.FindControl("ddl_NewCompanyType");

                company.InsertIntoCompanyInfoTable(txtNewCompanyName.Text, txtNewLogoPath.Text, ddlNewCompanyType.SelectedIndex,
                    ddlNewCompanyOrigin.SelectedIndex, ddlNewIndustry.SelectedIndex, txtNewComapnyUrl.Text,
                    txtNewContactInfo.Text, (bool)(ddlNewFortune.SelectedValue == "Y"), Convert.ToInt32(txtNewOverallEmployees.Text), Convert.ToInt32(txtNewOverallEmployeesIndia.Text), txtNewWorkculture.Text, txtNewRecruitment.Text, txtNewExperience.Text, txtNewBenefits.Text);
                FillInfoInCompanyGrid();
            }
           
            
        }
        protected void gvcompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList cmbType = (DropDownList)e.Row.FindControl("ddl_CompanyType");
                DropDownList cmbType1 = (DropDownList)e.Row.FindControl("ddl_CompanyOrigin");
                DropDownList cmbType2 = (DropDownList)e.Row.FindControl("ddl_Industry");

                if (cmbType != null)
                {
                    cmbType.DataSource = company.GetCompanyTypes();
                    cmbType.DataBind();
                    cmbType.SelectedValue = gv_company.DataKeys[e.Row.RowIndex].Values[1].ToString();
                }
                if (cmbType1 != null)
                {
                    cmbType1.DataSource = company.GetCompanyOrigin();
                    cmbType1.DataBind();
                    cmbType1.SelectedValue = gv_company.DataKeys[e.Row.RowIndex].Values[2].ToString();
                }
                if (cmbType2 != null)
                {
                    cmbType2.DataSource = company.GetIndustry();
                    cmbType2.DataBind();
                    cmbType2.SelectedValue = gv_company.DataKeys[e.Row.RowIndex].Values[3].ToString();
                }
                
                
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList cmbNewType = (DropDownList)e.Row.FindControl("ddl_NewCompanyType");
                cmbNewType.DataSource = company.GetCompanyTypes();
                cmbNewType.DataBind();
            
                DropDownList cmbNewType1 = (DropDownList)e.Row.FindControl("ddl_NewCompanyOrigin");
                cmbNewType1.DataSource = company.GetCompanyOrigin();
                cmbNewType1.DataBind();

                DropDownList cmbNewType2 = (DropDownList)e.Row.FindControl("ddl_NewIndustry");
                cmbNewType2.DataSource = company.GetIndustry();
                cmbNewType2.DataBind();
            } 
           
        }
        protected void gvcompany_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_company.EditIndex = -1;
            FillInfoInCompanyGrid(); 
        }
        protected void gvcompany_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            company.Delete(Convert.ToInt32(gv_company.DataKeys[e.RowIndex].Values[0].ToString()));
            FillInfoInCompanyGrid();
        }
        protected void gvcompany_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_company.EditIndex = e.NewEditIndex;
            FillInfoInCompanyGrid(); 
        }
        protected void gvcompany_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtNewCompanyName = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_CompanyName");
            DropDownList ddlNewFortune = (DropDownList)gv_company.Rows[e.RowIndex].FindControl("ddl_Fortune");
            TextBox txtNewLogoPath = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_LogoPath");
            TextBox txtNewOverallEmployees = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_OverallEmployees");
            TextBox txtNewOverallEmployeesIndia = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_OverallEmployeesIndia");
            TextBox txtNewComapnyUrl = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_CompanyUrl");
            TextBox txtNewContactInfo = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_ContactInfo");
            TextBox txtNewWorkculture = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_Workculture");
            TextBox txtNewExperience = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_Experience");
            TextBox txtNewRecruitment = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_Recruitment");
            TextBox txtNewBenefits = (TextBox)gv_company.Rows[e.RowIndex].FindControl("txt_Benefits");
            DropDownList ddlNewIndustry = (DropDownList)gv_company.Rows[e.RowIndex].FindControl("ddl_Industry");
            DropDownList ddlNewCompanyOrigin = (DropDownList)gv_company.Rows[e.RowIndex].FindControl("ddl_CompanyOrigin");
            DropDownList ddlNewCompanyType = (DropDownList)gv_company.Rows[e.RowIndex].FindControl("ddl_CompanyType");

            company.Update(txtNewCompanyName.Text, txtNewLogoPath.Text, ddlNewCompanyType.SelectedIndex,
                    ddlNewCompanyOrigin.SelectedIndex, ddlNewIndustry.SelectedIndex, txtNewComapnyUrl.Text,
                    txtNewContactInfo.Text, (bool)(ddlNewFortune.SelectedValue == "True"), Convert.ToInt32(txtNewOverallEmployees.Text), Convert.ToInt32(txtNewOverallEmployeesIndia.Text), txtNewWorkculture.Text, txtNewRecruitment.Text, txtNewExperience.Text, txtNewBenefits.Text);
            FillInfoInCompanyGrid();
        }

        

        
        #endregion CompanyGrid

        //location
        #region locationGrid
        protected void gvlocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvlocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvlocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        protected void gvlocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvlocation_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void gvlocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        #endregion locationGrid


        //Company heads
        #region CompanyHeadsGrid
        protected void gvcompanyheads_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvcompanyheads_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvcompanyheads_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        protected void gvcompanyheads_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvcompanyheads_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void gvcompanyheads_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        #endregion CompanyHeadsGrid
        //CompensationGrid
        #region CompensationGrid
        protected void gvCompensation_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvCompensation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvCompensation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        protected void gvCompensation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvCompensation_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void gvCompensation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        #endregion CompensationGrid

        //Company pics and videos
        #region CompanyPicsVideosGrid
        protected void gvcompanypics_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvcompanypics_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvcompanypics_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        protected void gvcompanypics_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvcompanypics_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void gvcompanypics_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        #endregion CompanyPicsVideosGrid

       
    }
}