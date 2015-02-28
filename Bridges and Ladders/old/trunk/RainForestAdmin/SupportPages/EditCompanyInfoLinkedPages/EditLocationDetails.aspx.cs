using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RainForestAdmin.DbClasses;
using System.Drawing;

namespace RainForestAdmin
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        DBClass dbops = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvEditLoc.SelectedIndex = 0;
                FillInfoInLocationGrid();
            }
            
        }
        private void FillInfoInLocationGrid()
        {
            DataTable dtLocation = dbops.FetchCompanyLocation(Convert.ToInt32(Session["EditCompanyId"]));


            if (dtLocation.Rows.Count > 0)
            {
                gvEditLoc.DataSource = dtLocation;
                gvEditLoc.DataBind();


            }
            else
            {
                dtLocation.Rows.Add(dtLocation.NewRow());
                gvEditLoc.DataSource = dtLocation;
                gvEditLoc.DataBind();

                int TotalColumns = gvEditLoc.Rows[0].Cells.Count;
                gvEditLoc.Rows[0].Cells.Clear();
                gvEditLoc.Rows[0].Cells.Add(new TableCell());
                gvEditLoc.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvEditLoc.Rows[0].Cells[0].Text = "No Record Found";
            }
        }

        protected void gvEditLoc_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEditLoc.EditIndex = -1;
            FillInfoInLocationGrid();
        }

        protected void gvEditLoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                TextBox txtNewAdd1 = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewAddr1");
                TextBox txtNewAdd2 = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewAddr2");
                TextBox txtNewAdd3 = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewAddr3");
                TextBox txtNewCity = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewCity");
                TextBox txtNewState = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewState");
                TextBox txtNewCountry = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewCountry");
                TextBox txtNewPC = (TextBox)gvEditLoc.FooterRow.FindControl("txt_NewPC");
                dbops.InsertCompanyLocationInfo(Convert.ToInt32(Session["EditCompanyId"]), txtNewAdd1.Text, txtNewAdd2.Text, txtNewAdd3.Text, txtNewCity.Text, txtNewState.Text, txtNewCountry.Text, txtNewPC.Text);
                FillInfoInLocationGrid();
            }
            
        }

        protected void gvEditLoc_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //dbops.Delete(Convert.ToInt32(Session["EditCompanyId"]));
            FillInfoInLocationGrid();
        }

        protected void gvEditLoc_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEditLoc.EditIndex = e.NewEditIndex;
            FillInfoInLocationGrid(); 
        }

        protected void gvEditLoc_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtNewAdd1 = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_Addr1");
            TextBox txtNewAdd2 = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_Addr2");
            TextBox txtNewAdd3 = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_Addr3");
            TextBox txtNewCity = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_City");
            TextBox txtNewState = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_State");
            TextBox txtNewCountry = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_Country");
            TextBox txtNewPC = (TextBox)gvEditLoc.Rows[e.RowIndex].FindControl("txt_PC");
            dbops.UpdateLocation(Convert.ToInt32(gvEditLoc.DataKeys[e.RowIndex].Values[0].ToString()), txtNewAdd1.Text, txtNewAdd2.Text, txtNewAdd3.Text, txtNewCity.Text, txtNewState.Text, txtNewCountry.Text, txtNewPC.Text);
            gvEditLoc.EditIndex = -1;
            FillInfoInLocationGrid();
            
            
        }

        protected void gvEditLoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}