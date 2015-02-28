$(document).ready(function () {
    $('.article .btnsect .likeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var articleId = $(this).attr("data-article-id") + "";
        likeArticle(this, articleId);
    });

    $('.article #articlepgloginbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
        }
        return false;
    });

    $('.article .btnsect .unlikeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var articleId = $(this).attr("data-article-id") + "";
        unlikeArticle(this, articleId);
    });

    $('.article .userwall .postbtns #addnewuserpostbtn').click(function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var articleId = $(this).attr("data-article-id") + "";
        addArticleComment(articleId);
        return false;
    });

    $('.article .oceanus-content a').each(function () {
        $(this).attr("target", "_blank");
    });

    var articleElement = document.getElementById("articlemarkup");
    var articleElementText = "";

    articleElementText = getInnerText(articleElement, false);
    var targetElementTextLength = articleElementText.length * 0.5;

    if (targetElementTextLength > 1500) {
        targetElementTextLength = 1500;
    }

    var currArticleMarkup = null;
    while (articleElementText.length > targetElementTextLength && currArticleMarkup != articleElement.innerHTML) {
        currArticleMarkup = articleElement.innerHTML;
        trimArticleContents(articleElement, articleElement, null);
        articleElementText = getInnerText(articleElement, false);
    }

    var horizontalRuleRegEx = /(<hr[^>]*>[ \n\r\t]*){2,}/gi;
    articleElement.innerHTML = articleElement.innerHTML.replace(horizontalRuleRegEx, '<hr>');

    $('.article .oceanus-content .oceanus-title').each(function () {
        try {
            var artLink = $(this).closest('a');
            if (artLink == null || artLink == 'undefined' || artLink.length == 0) {
                var newNode = document.createElement('a');
                var artContainer = $(this).closest('#articlemarkup');
                if (artContainer != null && artContainer != 'undefined') {
                    var webPageUrl = $(artContainer).attr("data-web-id");
                    $(newNode).attr("href", webPageUrl);
                    $(newNode).attr("target", "_blank");
                    this.parentNode.replaceChild(newNode, this);
                    newNode.appendChild(this);
                }
            }
        }
        catch (e) {
        }
    });
});

function addArticleComment(articleId) {
    var commentText = $('.userwall #newwallpostinput').val();

    $.ajax(
    {
        type: "POST",
        url: "/article/AddArticleComment",
        data: {
            commentText: commentText,
            articleId: articleId
        },
        success: function (result) {
            showSuccessNotification('Comment has been published.');
            $('#userwallposts').load("/article/GetArticleCommentPosts?articleId=" + articleId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Comment could not be published. Please try again later.');
        }
    });
}
