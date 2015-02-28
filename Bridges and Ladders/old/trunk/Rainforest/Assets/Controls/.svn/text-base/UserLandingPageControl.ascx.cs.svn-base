using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Assets.Controls
{
    public partial class UserLandingPageControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadLandingPgHomeContent();

            loadViewJobDetailsControl();
        }

        private void loadViewJobDetailsControl()
        {
            Control viewJobDetailsControl = LoadControl("/Assets/Controls/ViewJobDetailsControl.ascx");

            UpdatePanel_LandingPg_Content.ContentTemplateContainer.Controls.Clear();
            UpdatePanel_LandingPg_Content.ContentTemplateContainer.Controls.Add(viewJobDetailsControl);
        }

        private void loadLandingPgHomeContent()
        {
            Control userLandingPageControl = LoadControl("/Assets/Controls/BasicSearchControl.ascx");
            Image dividerImg = new Image();
            dividerImg.ImageUrl = "~/Assets/Images/TopLine_MasterPage.png";
            dividerImg.Width = Unit.Percentage(100.00);

            Table contentTable = new Table();
            TableRow searchControlRow = new TableRow();
            TableCell searchControlCell = new TableCell();
            TableRow navContentRow = new TableRow();
            TableCell navContentCell = new TableCell();

            searchControlRow.VerticalAlign = VerticalAlign.Top;
            searchControlRow.HorizontalAlign = HorizontalAlign.Center;
            navContentRow.VerticalAlign = VerticalAlign.Top;
            navContentRow.HorizontalAlign = HorizontalAlign.Center;

            searchControlRow.Cells.Add(searchControlCell);
            navContentRow.Cells.Add(navContentCell);
            contentTable.Rows.Add(searchControlRow);
            contentTable.Rows.Add(navContentRow);

            searchControlCell.Controls.Add(userLandingPageControl);
            searchControlCell.Controls.Add(dividerImg);

            UpdatePanel_LandingPg_Content.ContentTemplateContainer.Controls.Add(contentTable);
        }
    }
}