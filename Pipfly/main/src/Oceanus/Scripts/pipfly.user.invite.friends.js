$(document).ready(function () {
    document.fbInviteTabLoaded = 0;

    $('#invitefriendscontainer #writeMessageToInput').tokenInput([], {
        preventDuplicates: true,
        propertyToSearch: "name",
        emptyText: "Type an email address here",
        noResultsText: "Type an email address here",
        hintText: "Type an email address here",
        searchingText: "Type an email address here",
        resultsFormatter: function (item) { return "<li></li>" },
        tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
    });

    $('#invitefriendscontainer .invitemenu .invitemenuitem').click(function () {
        $('#invitefriendscontainer .invitemenu .invitemenuitem').each(function () {
            $(this).attr("class", "invitemenuitem");
            var relToHide = $(this).attr("rel") + "";
            hide(relToHide);
        });

        var rel = $(this).attr("rel") + "";
        $(this).attr("class", "invitemenuitem selinvitemenuitem");
        $(rel).show();
    });

    $('#invitefriendscontainer #sendemailinvitebtn').click(function () {
        var recipients = $("#invitefriendscontainer #writeMessageToInput").tokenInput('get');

        if (recipients != null && recipients.length > 0) {
            emailInviteFriend(recipients);
        }
        else {
            alert('Please provide atleast 1 invite recipient');
        }
    });
});

function emailInviteFriend(recipients) {
    var message = $('#invitefriendscontainer #writeMessageBodyInput').val();

    $.ajax(
    {
        type: "POST",
        url: "/Account/SendEmailInvite",
        dataType: 'json',
        traditional: true,
        data: {
            recipients: JSON.stringify(recipients),
            message: message
        },
        success: function (result) {
            if (result.successful == "true") {
                showSuccessNotification('An email invite has been sent.');
                $("#invitefriendscontainer #writeMessageToInput").tokenInput("clear");
                $.fancybox.close();
            }
            else {
                showErrorNotification('Error: Invite could not be sent.');
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Invite could not be sent.');
        }
    });
}
