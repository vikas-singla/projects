(function () {
    if ($('#sl_frmClipImage') != 'undefined' && $('#sl_frmClipImage') != null) {
        $("#sl_frmClipImage #shareClipRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers", {
            preventDuplicates: true,
            propertyToSearch: "name",
            emptyText: "Find people or type email address...",
            hintText: "Search by name or provide an email address...",
            resultsFormatter: function (item) { return "<li>" + "<img src='" + (item.userprofileimageurl == null ? "/Images/user_white.png" : item.userprofileimageurl) + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
            tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
        });

        $('#sl_frmClipImage #sl_shareclipbtn').click(function () {
            $(this).hide();
            $('#sl_frmClipImage #sl_processingcaption').show();
            $("#sl_frmClipImage :input").attr("disabled", true);

            sl_sharePhotoVideo(this);
            return false;
        });
    }

    if ($('#sl_frmClipStory') != 'undefined' && $('#sl_frmClipStory') != null) {
        $("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers", {
            preventDuplicates: true,
            propertyToSearch: "name",
            emptyText: "Find people or type email address...",
            hintText: "Search by name or provide an email address...",
            resultsFormatter: function (item) { return "<li>" + "<img src='" + (item.userprofileimageurl == null ? "/Images/user_white.png" : item.userprofileimageurl) + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
            tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
        });

        $('#sl_frmClipStory #sl_sharearticleclipbtn').click(function () {
            $(this).hide();
            $('#sl_frmClipStory #sl_processingcaption').show();
            $("#sl_frmClipStory :input").attr("disabled", true);

            sl_shareArticle(this);
            return false;
        });

        if ($('#sl_frmClipStory #sharearticlemarkup').attr('data-trim-value') == "true") {
            $('#sl_frmClipStory #sharearticlemarkup').each(function () {
                var imageCount = 0;
                $('img', this).each(function () {
                    ++imageCount;
                    if (imageCount > 1) {
                        $(this).remove();
                    }
                });

                $('table', this).each(function () {
                    $(this).remove();
                });
            });

            var articleElement = document.getElementById("sharearticlemarkup");
            var articleElementText = "";

            articleElementText = getInnerText(articleElement, false);
            var targetElementTextLength = articleElementText.length * 0.5;

            if (targetElementTextLength > 600) {
                targetElementTextLength = 600;
            }

            var currArticleMarkup = null;
            while (articleElementText.length > targetElementTextLength && currArticleMarkup != articleElement.innerHTML) {
                currArticleMarkup = articleElement.innerHTML;
                trimArticleContents(articleElement, articleElement, null);
                articleElementText = getInnerText(articleElement, false);
            }

            var horizontalRuleRegEx = /(<hr[^>]*>[ \n\r\t]*){1,}/gi;
            articleElement.innerHTML = articleElement.innerHTML.replace(horizontalRuleRegEx, '');
        }
    }
})();

function sl_shareArticle(btn) {
    var articleId = $('#sl_frmClipStory').attr("data-article-id");
    var comments = $('#sl_sharearticlecomments').val();
    var recipients = $("#shareArticleRecipientInput").tokenInput('get');
    var articleMarkup = $(".sharearticlesect #sharearticlemarkup").html();

    var url = "/article/ShareArticleClip";

    $.ajax(
    {
        type: "POST",
        url: url,
        data: {
            recipients: JSON.stringify(recipients),
            articleId: articleId,
            comments: comments,
            articleMarkup: articleMarkup
        },
        success: function (result) {
            if (result.successful == "true") {
                showSuccessNotification('Article has been shared.');
                $('#sl_frmClipStory #sl_processingcaption').html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Shared</span>');
                $.fancybox.close();
            }
            else {
                showErrorNotification('Error: Article could not be shared. Please try again later.');
                $('#sl_frmClipStory #sl_processingcaption').hide();
                $(btn).show();
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Article could not be shared. Please try again later.');
        }
    });
}

function sl_sharePhotoVideo(btn) {
    var clipId = $('#sl_frmClipImage').attr("data-clip-id");
    var comments = $('#sl_shareclipdesc').val();
    var recipients = $("#shareClipRecipientInput").tokenInput('get');

    var url = "/clip/SharePhotoVideoClip";

    $.ajax(
    {
        type: "POST",
        url: url,
        data: {
            recipients: JSON.stringify(recipients),
            clipId: clipId,
            comments: comments
        },
        success: function (result) {
            if (result.successful == "true") {
                showSuccessNotification('Clip has been shared.');
                $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Shared</span>');
                $.fancybox.close();
            }
            else {
                showErrorNotification('Error: Clip could not be shared. Please try again later.');
                $('#sl_frmClipImage #sl_processingcaption').hide();
                $(btn).show();
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Clip could not be shared. Please try again later.');
        }
    });
}

