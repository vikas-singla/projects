using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Assets.Controls
{
    public partial class DiscussionThreadList : System.Web.UI.UserControl
    {
        /// <summary>
        /// Delegate invoked to signal what article to load on the page based on user click
        /// </summary>
        /// <param name="articleId_"></param>
        internal delegate void LoadDiscussion(int discussionId_);
        internal LoadDiscussion NotifyLoadDiscussion;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void link_Command(object sender, CommandEventArgs e)
        {
            int discussionId = 0; //TEMP VALUE

            NotifyLoadDiscussion(discussionId);
        }

        protected void DiscussionTopic_Click(object sender, EventArgs e)
        {
            int discussionId = 0; //TEMP VALUE

            NotifyLoadDiscussion(discussionId);
        }
    }
}