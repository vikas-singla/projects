$(document).ready(function () {
    document.fbInviteTabLoaded = 0;

    $("#invitefriendscontainer :input").focus(function () {
        $('#invitefriendscontainer #sl_processingcaption').hide();
        $('#sl_frmClipImage #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Sending Invite...</span>');
    });

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
        $(this).hide();
        $('#invitefriendscontainer #sl_processingcaption').html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Sending Invite...</span>');        
        $('#invitefriendscontainer #sl_processingcaption').show();
        $("#invitefriendscontainer :input").attr("disabled", true);

        var recipients = $("#invitefriendscontainer #writeMessageToInput").tokenInput('get');

        if (recipients != null && recipients.length > 0) {
            emailInviteFriend(recipients, this);
        }
        else {
            alert('Please provide atleast 1 invite recipient');
        }
    });
});

function emailInviteFriend(recipients, btn) {
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
                $('#invitefriendscontainer #sl_processingcaption').html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Invite Sent</span>');            
            }
            else {
                showErrorNotification('Error: Invite could not be sent.');
                $('#invitefriendscontainer #sl_processingcaption').hide();
            }
            $("#invitefriendscontainer :input").attr("disabled", false);
            $(btn).show();
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Invite could not be sent.');
        }
    });
}
