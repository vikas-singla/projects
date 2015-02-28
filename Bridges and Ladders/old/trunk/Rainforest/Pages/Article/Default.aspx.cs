using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainforest.Assets.Controls;

namespace Rainforest.Pages.Articles
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();

            showArticleList();
            //showArticleDetails();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showArticleList()
        {
            Control articleListControl = LoadControl("/Assets/Controls/ArticleList.ascx");

            ArticleList articleList = (ArticleList)articleListControl;

            articleList.NotifyLoadArticle += new ArticleList.LoadArticle(this.loadArticle);

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(articleListControl);
        }

        public void showArticleDetails()
        {
            Control discusionTopics = LoadControl("/Assets/Controls/DetailedArticle.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(discusionTopics);
        }

        internal void loadArticle(int articleId_)
        {
            UpdatePanel_Body.ContentTemplateContainer.Controls.Clear();

            showArticleDetails();
        }
    }
}