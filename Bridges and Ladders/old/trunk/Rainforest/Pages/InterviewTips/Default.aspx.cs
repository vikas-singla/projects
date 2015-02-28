using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Pages.InterviewTips
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showInterviewTipsControl();
        }


        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showInterviewTipsControl()
        {
            Control interviewTipsControl = LoadControl("/Assets/Controls/InterviewTipsControl.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(interviewTipsControl);
        }
    }
}