$(document).ready(function () {
    $("#sl_frmWriteMsg #writeMessageToInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers", {
        propertyToSearch: "name",
        emptyText: "Find people or type email address...",
        hintText: "Type here to search by name or provide an email address...",
        resultsFormatter: function (item) { return "<li>" + "<img src='" + item.userprofileimageurl + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
        tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
    });

    $('#sl_frmWriteMsg #sl_sharelinkbtn').click(function () {
        var recipients = $("#writeMessageToInput").tokenInput('get');

        if (recipients != null && recipients.length > 0) {
            shareLink(recipients);
        }
    });
});

function shareLink(recipients) {
    var message = $('#sl_frmWriteMsg #writeMessageBodyInput').val();
    var pageTitle = $('#sl_frmWriteMsg #messagePageTitle').val();
    var pageUrl = $('#sl_frmWriteMsg #messagePageUrl').val();

    $.ajax(
    {
        type: "POST",
        url: "/Webpage/ShareLink",
        dataType: 'json',
        traditional: true,
        data: {
            recipients: JSON.stringify(recipients),
            pageTitle: pageTitle,
            pageUrl: pageUrl,
            message: message
        },
        success: function (result) {
            showSuccessNotification('Link has been shared.');
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Link could not be shared. Please try again later.');
        }
    });
}
