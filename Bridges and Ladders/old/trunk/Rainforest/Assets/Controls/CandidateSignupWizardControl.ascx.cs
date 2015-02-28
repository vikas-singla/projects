using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Rainforest.Assets.Controls
{
    public partial class CandidateSignupWizard : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                step1.Visible = true;
                step2.Visible = false;
                step3.Visible = false;

                // Iniatialize rows for Educational Qualifications
                SetInitialRow_Step1();

                // Iniatialize rows for Tech Skills
                SetInitialRow_Step3();
            }

        }

        #region Iniatalize Rows For Adding Educational Details For Step 1

        /// <summary>
        /// Initialize the rows of the Education Details grid so that the grid is visible on the page
        /// </summary>
        private void SetInitialRow_Step1()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Institute_Column", typeof(string)));
            dt.Columns.Add(new DataColumn("Degree_Column", typeof(string)));
            dt.Columns.Add(new DataColumn("StartDate_Column", typeof(string)));
            dt.Columns.Add(new DataColumn("EndDate_Column", typeof(string)));

            dr = dt.NewRow();

            dr["Institute_Column"] = string.Empty;
            dr["Degree_Column"] = string.Empty;
            dr["StartDate_Column"] = string.Empty;
            dr["EndDate_Column"] = string.Empty;

            dt.Rows.Add(dr);

            //Store the DataTable in ViewState

            ViewState["EducationDetailsTable"] = dt;

            Gridview_EducationalQuals.DataSource = dt;
            Gridview_EducationalQuals.DataBind();

        }

        /// <summary>
        /// Add new row on the click of the add button
        /// </summary>
        private void AddNewRowToGrid_Step1()
        {
            int rowIndex = 0;

            if (ViewState["EducationDetailsTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["EducationDetailsTable"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        // Extract the TextBox values
                        TextBox box1 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[1].FindControl("Txt_Institute");
                        TextBox box2 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[2].FindControl("Txt_Degree");
                        TextBox box3 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[3].FindControl("Txt_StartDate");
                        TextBox box4 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[3].FindControl("Txt_EndDate");

                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Institute_Column"] = box1.Text;
                        drCurrentRow["Degree_Column"] = box2.Text;
                        drCurrentRow["StartDate_Column"] = box3.Text;
                        drCurrentRow["EndDate_Column"] = box4.Text;

                        rowIndex++;
                    }

                    // Add new row to DataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    // Store the current data to ViewState
                    ViewState["EducationDetailsTable"] = dtCurrentTable;

                    // Re-bind the Grid with the current data
                    Gridview_EducationalQuals.DataSource = dtCurrentTable;
                    Gridview_EducationalQuals.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            // Set Previous Data on Postbacks
            SetPreviousData();
        }

        /// <summary>
        /// Set previous data
        /// </summary>
        private void SetPreviousData()
        {
            int rowIndex = 0;

            if (ViewState["EducationDetailsTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["EducationDetailsTable"];

                if (dt.Rows.Count > 0)
                {

                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        // Extract the TextBox values
                        TextBox box1 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[1].FindControl("Txt_Institute");
                        TextBox box2 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[2].FindControl("Txt_Degree");
                        TextBox box3 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[3].FindControl("Txt_StartDate");
                        TextBox box4 = (TextBox)Gridview_EducationalQuals.Rows[rowIndex].Cells[3].FindControl("Txt_EndDate");

                        box1.Text = dt.Rows[i]["Institute_Column"].ToString();
                        box2.Text = dt.Rows[i]["Degree_Column"].ToString();
                        box3.Text = dt.Rows[i]["StartDate_Column"].ToString();
                        box4.Text = dt.Rows[i]["EndDate_Column"].ToString();

                        rowIndex++;
                    }

                }

            }

        }

        #endregion

        #region Iniatalize Rows For Adding Skills In Step 3

        /// <summary>
        /// Initialize the rows of the Education Details grid so that the grid is visible on the page
        /// </summary>
        private void SetInitialRow_Step3()
        {
            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("TechSkills_Column", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfYearsWOrked_Column", typeof(string)));
            dt.Columns.Add(new DataColumn("ProficiencyLevel_Column", typeof(string)));

            dr = dt.NewRow();

            dr["TechSkills_Column"] = string.Empty;
            dr["NoOfYearsWOrked_Column"] = string.Empty;
            dr["ProficiencyLevel_Column"] = string.Empty;

            dt.Rows.Add(dr);

            //Store the DataTable in ViewState

            ViewState["SkillsTable"] = dt;

            Gridview_TechSkills.DataSource = dt;
            Gridview_TechSkills.DataBind();
        }

        /// <summary>
        /// Add new row on the click of the add button
        /// </summary>
        private void AddNewRowToGrid_Step3()
        {
            int rowIndex = 0;

            if (ViewState["SkillsTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["SkillsTable"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        // Extract the Dropdown values
                        DropDownList ddl1 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[1].FindControl("Ddl_TechSkills");
                        DropDownList ddl2 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[2].FindControl("Ddl_NoOfYearsWOrked");
                        DropDownList ddl3 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[3].FindControl("Ddl_ProficiencyLevel");
    
                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["TechSkills_Column"] = ddl1.Text;
                        drCurrentRow["NoOfYearsWOrked_Column"] = ddl2.Text;
                        drCurrentRow["ProficiencyLevel_Column"] = ddl3.Text;
 
                        rowIndex++;
                    }

                    // Add new row to DataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    // Store the current data to ViewState
                    ViewState["SkillsTable"] = dtCurrentTable;

                    // Re-bind the Grid with the current data
                    Gridview_TechSkills.DataSource = dtCurrentTable;
                    Gridview_TechSkills.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            // Set Previous Data on Postbacks
            SetPreviousData_Step3();
        }

        /// <summary>
        /// Set previous data
        /// </summary>
        private void SetPreviousData_Step3()
        {
            int rowIndex = 0;

            if (ViewState["SkillsTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["SkillsTable"];

                if (dt.Rows.Count > 0)
                {

                    for (int i =1; i < dt.Rows.Count; i++)
                    {
                        // Extract the Dropdown values
                        DropDownList ddl1 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[1].FindControl("Ddl_TechSkills");
                        DropDownList ddl2 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[2].FindControl("Ddl_NoOfYearsWOrked");
                        DropDownList ddl3 = (DropDownList)Gridview_TechSkills.Rows[rowIndex].Cells[3].FindControl("Ddl_ProficiencyLevel");

                        ddl1.Text = dt.Rows[i]["TechSkills_Column"].ToString();
                        ddl2.Text = dt.Rows[i]["NoOfYearsWOrked_Column"].ToString();
                        ddl3.Text = dt.Rows[i]["ProficiencyLevel_Column"].ToString();
 
                        rowIndex++;
                    }

                }

            }

        }

        #endregion

        /// <summary>
        /// Function to add more educational qualifications once the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_AddMoreQualification_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid_Step1();
        }

        /// <summary>
        /// Function to add more tech skills once the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_AddMoreTechSkills_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid_Step3();
        }

        /// <summary>
        /// STEP 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgButton_GeneralDetails_Click(object sender, ImageClickEventArgs e)
        {
            step1.Visible = true;
            step2.Visible = false;
            step3.Visible = false;
        }

        /// <summary>
        /// STEP 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgButton_QualificationAndExperience_Click(object sender, ImageClickEventArgs e)
        {
            step1.Visible = false;
            step2.Visible = true;
            step3.Visible = false;
        }

        /// <summary>
        /// STEP 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgButton_IntellectualProperty_Click(object sender, ImageClickEventArgs e)
        {
            step1.Visible = false;
            step2.Visible = false;
            step3.Visible = true;
        }

        /// <summary>
        /// STEP 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgButton_VideoOrAudioPitch_Click(object sender, ImageClickEventArgs e)
        {

        }

        /// <summary>
        /// STEP 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgButton_OtherCandidateInfo_Click(object sender, ImageClickEventArgs e)
        {

        }

    }
}