﻿$(document).ready(function () {
    $('#facebooksigninbtn').live("click", function () {
        fblogin();
        return false;
    });

    $('#facebooksigninbtn2').live("click", function () {
        fblogin();
        return false;
    });

    $('#facebooksigninbtn3').live("click", function () {
        fblogin();
        return false;
    });

    $('#facebooksigninbtn4').live("click", function () {
        fblogin();
        return false;
    });

    $('.btnbluestyle').live("click", function () {
        var sid = $(this).attr("data-sid") + "";

        if (sid == "invitefbuser") {
            if (!loggedin) {
                $('#loginbtn').click();
                return false;
            }

            var fbUID = $(this).attr("data-fbuid") + "";
            inviteFacebookUser(fbUID);
        }
    });
});

window.fbAsyncInit = function () {
    FB.init({ appId: '167589106690868', status: false, cookie: true,
        xfbml: true,
        channelURL: '//www.pipfly.com/channel.html', // channel.html file
        oauth: true // enable OAuth 2.0
    });

    FB.Event.subscribe('auth.login', function (response) {
        if (!loggedin) {
            if (response.status === 'connected') {
                if (!loggedin) {
                    var uid = response.authResponse.userID;
                    var accessToken = response.authResponse.accessToken;
                    registerFacebookUser(uid, accessToken);

                    return;
                }
            } else {
                //not logged in facebook
            }
        }
    });
};

// Load the SDK Asynchronously
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
} (document));

function fblogin() {
    if (!loggedin) {
        FB.login(function (response) {
            if (response.authResponse) {
                // user is logged in and granted some permissions.
                // perms is a comma separated list of granted permissions
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;
                registerFacebookUser(uid, accessToken);
            } else {
                // user is not logged in
            }
        }, { scope: 'email,user_location,publish_stream' });
    }
}

function registerFacebookUser(uid, accessToken) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/FacebookUserRegister",
        data: {
            uid: uid,
            token: accessToken
        },
        success: function (result) {
            if (result.isnewuser == "true") {
                window.location.href = "/welcome/1";
            }
            else {
                window.location.reload(true);
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Your profile could not be registered.');
            window.location.reload(true);
        }
    });
}

function inviteFacebookUser(fbUID) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/SendInviteToFacebookUser",
        data: {
            friendFBUID: fbUID
        },
        success: function (result) {
            if ($('#expandnetworksect') != 'undefined') {
                showSuccessNotification('Your friend has been invited to Pipfly via a Facebook Wall Post.');
                $('#expandnetworksect').load('/account/UserProfileExpandNetwork');
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Your friend could not be invited.');
        }
    });
}
