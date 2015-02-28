using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.Business_Objects;

namespace RainForestAdmin.UserControls
{
    public partial class AddCompanyHeads : System.Web.UI.UserControl
    {
        private string HeadName;
        private string HeadPosition;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                HeadName = txt_HeadName.Text;
                HeadPosition = txt_HeadPosition.Text;
                
            }
           
        }

        protected void btn_AddNewHeads_Click(object sender, EventArgs e)
        {
            List<CompanyHeads> lstcompheads = null;
            CompanyHeads cheads = new CompanyHeads();
            cheads.Name = HeadName;
            cheads.Position = HeadPosition;
            if(ViewState["CompanyHeadInfo"] == null)
                lstcompheads = new List<CompanyHeads>();
            else
                lstcompheads = (List<CompanyHeads>)ViewState["CompanyHeadInfo"];
            lstcompheads.Add(cheads);
            ViewState["CompanyHeadInfo"]=(object)lstcompheads;
            txt_HeadName.Text = string.Empty;
            txt_HeadPosition.Text = string.Empty;
            
        }
        public void ClearAll()
        {
            txt_HeadName.Text = string.Empty;
            txt_HeadPosition.Text = string.Empty;
            List<CompanyHeads> lstcompheads = null;
            if (ViewState["CompanyHeadInfo"] != null)
            {
                lstcompheads = (List<CompanyHeads>)ViewState["CompanyHeadInfo"];
                lstcompheads.Clear();
            }
        }
        
               
    }
}
