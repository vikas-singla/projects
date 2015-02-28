using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RainForestAdmin.BusinessObjectClasses;
using System.Collections;

namespace RainForestAdmin
{
    public partial class AddCompanyPictures : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddLocationsInCombo();
        }

        protected void btn_SavePictures_Click(object sender, EventArgs e)
        {
            CompanyPictures pics = new CompanyPictures();
            pics.City = ddl_Location.SelectedValue;
            pics.PicturePath = fupload_CompanyPictures.FileName;
            pics.VideoPath = fupload_video.FileName;
            GridView1.Visible = false;
            List<CompanyPictures> list;
            if (Session["CompanyPictures"] == null)
            {
                list = new List<CompanyPictures>();
            }
            else
            {
                GridView1.Visible = true;
                list = (List<CompanyPictures>)Session["CompanyPictures"];
            }
            list.Add(pics);
            Session["CompanyPictures"] = list;
            GridView1.DataSource = list;
            GridView1.DataBind();
            ddl_Location.SelectedIndex = -1;
            
        }
        private void AddLocationsInCombo()
        {
            List<CompanyLocationInfo> lst = null;
            Array arr = null;
            if (Session["CompanyLocationsInfo"] != null)
            {
                lst = (List<CompanyLocationInfo>)Session["CompanyLocationsInfo"];
                int nCount = lst.Count;
                arr = new String[nCount];
                int index = 0;
                foreach (CompanyLocationInfo info in lst)
                {

                    arr.SetValue(info.City, index);
                    index++;
                }

            }
            else
            {
                arr = new String[1] { "Unknown" };
                
            }
            ddl_Location.DataSource = arr;
            ddl_Location.DataBind();
        }
    }
}