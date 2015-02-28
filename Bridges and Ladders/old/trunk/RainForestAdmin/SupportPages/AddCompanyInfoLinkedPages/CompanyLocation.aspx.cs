using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.BusinessObjectClasses;

namespace RainForestAdmin
{
    public partial class CompanyLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_SaveAddress_Click(object sender, EventArgs e)
        {
            CompanyLocationInfo locations = new CompanyLocationInfo();
            locations.AddLine1 = txt_AddressLine1.Text;
            locations.AddLine2 = txt_AddressLine2.Text;
            locations.AddLine3 = txt_AddressLine3.Text;
            locations.City = txtCity.Text;
            locations.Country = txtCountry.Text;
            locations.State = txtState.Text;
            locations.Pincode = txtPinCode.Text;
            //heads.HeadName = txt_headname.Text;
            //heads.HeadDesignation = txt_designation.Text;
            List<CompanyLocationInfo> list;
            if (Session["CompanyLocationsInfo"] == null)
            {
                list = new List<CompanyLocationInfo>();
            }
            else
            {
                list = (List<CompanyLocationInfo>)Session["CompanyLocationsInfo"];
                
            }
            list.Add(locations);
            Session["CompanyLocationsInfo"] = list;
            GridView1.DataSource = list;
            GridView1.DataBind();
            txt_AddressLine1.Text = String.Empty;
            txt_AddressLine2.Text = String.Empty;
            txt_AddressLine3.Text = String.Empty;
            txtCity.Text = String.Empty;
            txtCountry.Text = String.Empty;
            txtState.Text = String.Empty;
            txtPinCode.Text = String.Empty;
        }

        protected void btn_Reset_Click(object sender, EventArgs e)
        {

        }
    }
}