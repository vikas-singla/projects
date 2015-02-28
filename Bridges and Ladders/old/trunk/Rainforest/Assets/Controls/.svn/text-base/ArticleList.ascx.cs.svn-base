using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Assets.Controls
{
    public partial class ArticleList : System.Web.UI.UserControl
    {
       
        /// <summary>
        /// Delegate invoked to signal what article to load on the page based on user click
        /// </summary>
        /// <param name="articleId_"></param>
        internal delegate void LoadArticle(int articleId_);
        internal LoadArticle NotifyLoadArticle;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_Article_1_Click(object sender, EventArgs e)
        {
            int articleId = 0; //TEMP VALUE

            NotifyLoadArticle(articleId);
        }
    }
}