using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Pages.WorkCulture
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showWorkCulture();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showWorkCulture()
        {
            Control workCulture = LoadControl("/Assets/Controls/WorkCulture.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(workCulture);
        }
    }
}