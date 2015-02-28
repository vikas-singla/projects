using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainforest.Assets.Controls;


namespace Rainforest.Pages.Discussion
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showDiscusionTopics();
            //showDiscusionDetails();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showDiscusionTopics()
        {
            Control discusionTopics = LoadControl("/Assets/Controls/DiscussionThreadList.ascx");

            DiscussionThreadList discusionTopicsControl = (DiscussionThreadList)discusionTopics;

            discusionTopicsControl.NotifyLoadDiscussion += new DiscussionThreadList.LoadDiscussion(this.loadDiscussion);

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(discusionTopics);
        }

        private void showDiscusionDetails()
        {
            Control discusionTopics = LoadControl("/Assets/Controls/DiscussionThreadDetailControl.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(discusionTopics);
        }


        internal void loadDiscussion(int discussionId_)
        {
            UpdatePanel_Body.ContentTemplateContainer.Controls.Clear();

            showDiscusionDetails();
        }
    }
}