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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            FillGridInfo();


        }

        protected void gvedit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvedit.EditIndex = -1;
            FillGridInfo(); 
        }

        protected void gvedit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {

                
                
                
            }
        }

        protected void gvedit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvedit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvedit.EditIndex = e.NewEditIndex;
            FillGridInfo(); 
        }

        protected void gvedit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
        }

        protected void gvedit_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        private void FillGridInfo()
        {
            DBClass db = new DBClass();
            DataSet ds = db.FetchCompanyBasicInfo(Convert.ToInt32(Session["EditCompanyId"]));
            DataTable table = new DataTable();
            switch (Request.QueryString["Sender"])
            {
                case "Pics":
                    {
                        table = ds.Tables[3];
                        

                        break;
                    }
                case "Location":
                    {

                        table = ds.Tables[1];
                       
                        
                        break;
                    }
                case "Heads":
                    {
                        
                        table = ds.Tables[2];
                                               
                        break;
                    }
                case "Compensation":
                    {
                        table = ds.Tables[4];
                        
                        break;
                    }
            }
            gvedit.DataSource = table;
            gvedit.DataBind();

        }
    }
    public class DataGridTemplate : ITemplate
    {
        ListItemType templateType;
        string columnName;

        public DataGridTemplate(ListItemType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            Literal lc = new Literal();
            switch (templateType)
            {
                case ListItemType.Header:
                    lc.Text = "<B>" + columnName + "</B>";
                    container.Controls.Add(lc);
                    break;
                case ListItemType.Item:
                    lc.Text = "Item " + columnName;
                    container.Controls.Add(lc);
                    break;
                case ListItemType.EditItem:
                    TextBox tb = new TextBox();
                    tb.Text = "";
                    container.Controls.Add(tb);
                    break;
                case ListItemType.Footer:
                    lc.Text = "<I>" + columnName + "</I>";
                    container.Controls.Add(lc);
                    break;
            }
        }
    }

}