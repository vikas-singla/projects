$(document).ready(function () {
    if ($('#sl_frmClipImage') != 'undefined' && $('#sl_frmClipImage') != null) {
        $("#sl_frmClipImage #shareRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers", {
            preventDuplicates: true,
            propertyToSearch: "name",
            emptyText: "Find people or type email address...",
            hintText: "Type here to search by name or provide an email address...",
            resultsFormatter: function (item) { return "<li>" + "<img src='" + (item.userprofileimageurl == null ? "/Images/user_white.png" : item.userprofileimageurl) + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
            tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
        });
    }

    if ($('#sl_frmClipStory') != 'undefined' && $('#sl_frmClipStory') != null) {
        $("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers", {
            preventDuplicates: true,
            propertyToSearch: "name",
            emptyText: "Find people or type email address...",
            hintText: "Type here to search by name or provide an email address...",
            resultsFormatter: function (item) { return "<li>" + "<img src='" + (item.userprofileimageurl == null ? "/Images/user_white.png" : item.userprofileimageurl) + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
            tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
        });
    }

    $('#sl_frmClipImage #sl_addclipbtn').click(function () {
        $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
        $('#sl_frmClipImage #sl_processingcaption').show();
        $("#sl_frmClipImage :input").attr("disabled", true);
        $(this).hide();

        if ($('#sl_frmClipImage') != 'undefined' && $('#sl_frmClipImage') != null) {
            if ($('#sl_frmClipImage').validationEngine('validate')) {
                $('#sl_frmClipImage').validationEngine('hideAll');
                sl_addClipToScrapbook(this);
            }
        }

        if ($('#sl_frmClipStory') != 'undefined' && $('#sl_frmClipStory') != null) {
            if ($('#sl_frmClipStory').validationEngine('validate')) {
                $('#sl_frmClipStory').validationEngine('hideAll');
                sl_addClipToScrapbook(this);
            }
        }

        return false;
    });

    $('#sl_frmClipImage #sl_addclipdesc').focus(function () {
        $('#sl_frmClipImage #sl_processingcaption').hide();
        $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
        return false;
    });
    $('#sl_frmClipImage #token-input-shareRecipientInput').focus(function () {
        $('#sl_frmClipImage #sl_processingcaption').hide();
        $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
    });

    $('#sl_frmClipStory #sl_addarticlecomments').focus(function () {
        $('#sl_frmClipStory #sl_processingcaption').hide();
        $('#sl_frmClipStory #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
        return false;
    });
    $('#sl_frmClipStory #token-input-shareArticleRecipientInput').focus(function () {
        $('#sl_frmClipStory #sl_processingcaption').hide();
        $('#sl_frmClipStory #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
    });

    $('#sl_frmClipStory #sl_addarticleclipbtn').click(function () {
        $('#sl_frmClipStory #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');
        $('#sl_frmClipStory #sl_processingcaption').show();
        $("#sl_frmClipStory :input").attr("disabled", true);
        $(this).hide();

        sl_addArticleClipToScrapbook(this);
        return false;
    });
});

function sl_addClipToScrapbook(btn) {
    var imageUrl = $('#sl_frmClipImage #clipimagephototag').attr("src");
    var imageWidth = $('#sl_frmClipImage #clipimagephototag').attr("data-width");
    var imageHeight = $('#sl_frmClipImage #clipimagephototag').attr("data-height");
    var videoId = $('#sl_frmClipImage #sl_addclipbtn').attr("data-vid");
    var pageUrl = $('#sl_frmClipImage #sl_addclipbtn').attr("data-page-url");
    var pageTitle = $('#sl_frmClipImage #sl_addclipbtn').attr("data-page-title");
    var pageDesc = $('#sl_frmClipImage #sl_addclipbtn').attr("data-page-desc");
    var desc = $('#sl_frmClipImage #sl_addclipdesc').val();
    var recipients = $("#sl_frmClipImage #shareRecipientInput").tokenInput('get');

    var url = "/clip/ClipPhotoVideo";

    $.ajax(
    {
        type: "POST",
        url: url,
        data: {
            recipients: JSON.stringify(recipients),
            pageUrl: pageUrl,
            pageTitle: pageTitle,
            pageDescription: pageDesc,
            imageUrl: imageUrl,
            imageWidth: imageWidth,
            imageHeight: imageHeight,
            videoId: videoId,
            clipDesc: desc
        },
        success: function (result) {
            if (result.successful == "true") {
                if (recipients.length > 0) {
                    showSuccessNotification('Clip has been shared.');
                }
                else {
                    showSuccessNotification('Clip has been added.');
                }
                $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>');
            }
            else {
                showErrorNotification('Error: Clip could not be added. Please try again later.');
                $('#sl_frmClipImage #sl_processingcaption').hide();
            }
            $("#sl_frmClipImage :input").attr("disabled", false);
            $(btn).show();
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Clip could not be added. Please try again later.');
            $('#sl_frmClipImage #sl_processingcaption').hide();
            $("#sl_frmClipImage :input").attr("disabled", false);
            $(btn).show();
        }
    });
}

function sl_addArticleClipToScrapbook(btn) {
    var articleId = $('#sl_frmClipStory #sl_addarticleclipbtn').attr("data-article-id");
    var articleDesc = $('#sl_frmClipStory #sl_addarticlecomments').val();
    var articleType = $('#sl_frmClipStory #articletypenews').val();
    var recipients = $("#sl_frmClipStory #shareArticleRecipientInput").tokenInput('get');

    var url = "/article/PublishArticleClip";

    $.ajax(
    {
        type: "POST",
        url: url,
        data: {
            articleId: articleId,
            articleDesc: articleDesc,
            articleType: articleType,
            recipients: JSON.stringify(recipients)
        },
        success: function (result) {
            if (result.successful == "true") {
                showSuccessNotification('Clip has been added.');
                $('#sl_frmClipStory #sl_processingcaption').html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>');
            }
            else {
                showErrorNotification('Error: Clip could not be added. Please try again later.');
                $('#sl_frmClipStory #sl_processingcaption').hide();
            }
            $("#sl_frmClipStory :input").attr("disabled", false);
            $(btn).show();
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Clip could not be added. Please try again later.');
            $('#sl_frmClipStory #sl_processingcaption').hide();
            $("#sl_frmClipStory :input").attr("disabled", false);
            $(btn).show();
        }
    });
}