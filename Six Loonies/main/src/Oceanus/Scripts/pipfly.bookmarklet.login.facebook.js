﻿$(document).ready(function () {
    $('#facebooksigninbtn').click(function () {
        fblogin();
        return false;
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
        async: false,
        success: function (result) {
            loggedin = true;
            refreshBookmarkletForLogin();
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Your profile could not be registered.');
        }
    });
}
