$(document).ready(function () {

    $('#contactusformbtn').click(function () {

        if ($('#contactusform').validationEngine('validate')) {
            $('#contactusform').validationEngine('hideAll');

            addContactUsMessage();
        }

        return false;
    });
});

function addContactUsMessage() {
    var messageRecipientID = "c66c9ff0-3ce6-40a6-a604-81d97a16ade7";
    var senderName = $.trim($("#contactusname").val());
    var senderEmail = $.trim($('#contactusemail').val());
    var messageSubject = $.trim($("#contactussubject").val());
    var messageBody = $.trim($("#contactusmsg").val());
    messageTo = new Array();

    $("#contactusname").val("");
    $("#contactusemail").val("");
    $("#contactussubject").val("");
    $("#contactusmsg").val("");

    messageBody = "Sender: " + senderName + "\nSender Email: " + senderEmail + "\n\n\nMessage:\n" + messageBody;

    user = new Object();
    user.FullName = "Admin";
    user.UserId = messageRecipientID;

    messageTo.push(user);

    $.ajax(
    {
        type: "POST",
        dataType: 'json',
        traditional: true,
        url: "/Message/AddMessage",
        data: {
            messageTo: JSON.stringify(messageTo),
            messageSubject: messageSubject,
            messageBody: messageBody
        },
        async: false,
        success: function (result) {
            showSuccessNotification('Thank you for your input. We will make every effort to reply to you within 24 hours.');
            $.fancybox.close();
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Your message could not be processed. Please try again later.');
        }
    });
    messageTo = new Array();
}
