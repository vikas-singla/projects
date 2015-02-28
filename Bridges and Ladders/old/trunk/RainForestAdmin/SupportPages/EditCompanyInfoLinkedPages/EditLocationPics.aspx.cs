using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.DbClasses;
using System.Data;

namespace RainForestAdmin
{
    public partial class EditLocationPics : System.Web.UI.Page
    {
        DBClass dbops = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvEditPics.SelectedIndex = 0;
                FillInfoInCompanyPicsGrid();
            }

        }
        private void FillInfoInCompanyPicsGrid()
        {
            DataTable dtLocation = dbops.FetchCompanyPicDetails(Convert.ToInt32(Session["EditCompanyId"]));


            if (dtLocation.Rows.Count > 0)
            {
                gvEditPics.DataSource = dtLocation;
                gvEditPics.DataBind();


            }
            else
            {
                dtLocation.Rows.Add(dtLocation.NewRow());
                gvEditPics.DataSource = dtLocation;
                gvEditPics.DataBind();

                int TotalColumns = gvEditPics.Rows[0].Cells.Count;
                gvEditPics.Rows[0].Cells.Clear();
                gvEditPics.Rows[0].Cells.Add(new TableCell());
                gvEditPics.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvEditPics.Rows[0].Cells[0].Text = "No Record Found";
            }
        }

        protected void gvEditPics_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEditPics.EditIndex = -1;
            FillInfoInCompanyPicsGrid();
        }

        protected void gvEditPics_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                FileUpload fuPicPath = (FileUpload)gvEditPics.FooterRow.FindControl("fu_NewPicPath");
                FileUpload fuVideoPath = (FileUpload)gvEditPics.FooterRow.FindControl("fu_NewVideoPath");

                dbops.InsertCompanyPicVideo(Convert.ToInt32(Session["EditCompanyId"]), fuPicPath.FileName, fuVideoPath.FileName);
                FillInfoInCompanyPicsGrid();
            }

        }

        protected void gvEditPics_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //dbops.Delete(Convert.ToInt32(Session["EditCompanyId"]));
            FillInfoInCompanyPicsGrid();
        }

        protected void gvEditPics_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEditPics.EditIndex = e.NewEditIndex;
            FillInfoInCompanyPicsGrid();
        }

        protected void gvEditPics_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            FileUpload fuPicPath = (FileUpload)gvEditPics.Rows[e.RowIndex].FindControl("fu_PicPath");
            FileUpload fuVideoPath = (FileUpload)gvEditPics.Rows[e.RowIndex].FindControl("fu_VideoPath");

            dbops.UpdatePicsInfo(Convert.ToInt32(gvEditPics.DataKeys[e.RowIndex].Values[0].ToString()), fuPicPath.FileName, fuVideoPath.FileName);
            gvEditPics.EditIndex = -1;
            FillInfoInCompanyPicsGrid();


        }
    
    }
}