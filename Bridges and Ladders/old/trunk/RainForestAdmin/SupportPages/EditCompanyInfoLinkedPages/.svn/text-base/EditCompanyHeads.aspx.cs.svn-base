using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RainForestAdmin.DbClasses;

namespace RainForestAdmin
{
    public partial class EditCompanyHeads : System.Web.UI.Page
    {
        DBClass dbops = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvEditHeads.SelectedIndex = 0;
                FillInfoInCompanyHeadsGrid();
            }

        }
        private void FillInfoInCompanyHeadsGrid()
        {
            DataTable dtLocation = dbops.FetchCompanyHeadDetails(Convert.ToInt32(Session["EditCompanyId"]));


            if (dtLocation.Rows.Count > 0)
            {
                gvEditHeads.DataSource = dtLocation;
                gvEditHeads.DataBind();


            }
            else
            {
                dtLocation.Rows.Add(dtLocation.NewRow());
                gvEditHeads.DataSource = dtLocation;
                gvEditHeads.DataBind();

                int TotalColumns = gvEditHeads.Rows[0].Cells.Count;
                gvEditHeads.Rows[0].Cells.Clear();
                gvEditHeads.Rows[0].Cells.Add(new TableCell());
                gvEditHeads.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvEditHeads.Rows[0].Cells[0].Text = "No Record Found";
            }
        }

        protected void gvEditHeads_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEditHeads.EditIndex = -1;
            FillInfoInCompanyHeadsGrid();
        }

        protected void gvEditHeads_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                TextBox txtHeadName = (TextBox)gvEditHeads.FooterRow.FindControl("txt_NewHeadName");
                TextBox txtDesignation = (TextBox)gvEditHeads.FooterRow.FindControl("txt_NewDesignation");

                dbops.InsertCompanyHeadsInfo(Convert.ToInt32(Session["EditCompanyId"]), txtHeadName.Text, txtDesignation.Text);
                FillInfoInCompanyHeadsGrid();
            }

        }

        protected void gvEditHeads_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //dbops.Delete(Convert.ToInt32(Session["EditCompanyId"]));
            FillInfoInCompanyHeadsGrid();
        }

        protected void gvEditHeads_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEditHeads.EditIndex = e.NewEditIndex;
            FillInfoInCompanyHeadsGrid();
        }

        protected void gvEditHeads_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtHeadName = (TextBox)gvEditHeads.Rows[e.RowIndex].FindControl("txt_HeadName");
            TextBox txtDesignation = (TextBox)gvEditHeads.Rows[e.RowIndex].FindControl("txt_Designation");

            dbops.UpdateHeadsInfo(Convert.ToInt32(gvEditHeads.DataKeys[e.RowIndex].Values[0].ToString()), txtHeadName.Text, txtDesignation.Text);
            gvEditHeads.EditIndex = -1;
            FillInfoInCompanyHeadsGrid();


        }
    }
}