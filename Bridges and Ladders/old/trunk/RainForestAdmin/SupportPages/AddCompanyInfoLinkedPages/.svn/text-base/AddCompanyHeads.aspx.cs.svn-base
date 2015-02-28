using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.BusinessObjectClasses;

namespace RainForestAdmin
{
    public partial class AddCompanyHeads : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            AddCompanyHeadsClass heads = new AddCompanyHeadsClass();
            heads.HeadName = txt_headname.Text;
            heads.HeadDesignation = txt_designation.Text;
            List<AddCompanyHeadsClass> list; 
            if (Session["HeadsInfo"] == null)
            {
                list = new List<AddCompanyHeadsClass>();
            }
            else
            {
                list = (List<AddCompanyHeadsClass>)Session["HeadsInfo"];
               
            }
             list.Add(heads);
             
             Session["HeadsInfo"] = list;
             GridView1.DataSource = list;
             GridView1.DataBind();
             txt_designation.Text = String.Empty;
             txt_headname.Text = String.Empty;
            
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            txt_designation.Text = String.Empty;
            txt_headname.Text = String.Empty;
        }
    }
}