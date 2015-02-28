var gutterWidth = 32;
var columnWidth = 320;

$(document).ready(function () {
    setViewPortWidth();

    if (!document.layoutUserTagCollage) {
        if ($('#utags').is(":visible")) {
            document.layoutUserTagCollage = 1;
            $('.tagcollage').isotope({
                itemSelector: '.tagcollageitem',
                masonry: { columnWidth: 1 },
                layoutMode: 'masonry',
                resizable: true,
                animationEngine: 'best-available'
            });

            $(window).load(function () {
                $('.tagcollage').isotope('reLayout');
            });
        }
    }

    $('#profilesectiontabs li a').click(function () {
        $('#profilesectiontabs li a').each(function () {
            $(this).parent().attr("class", "");
            var relToHide = $(this).attr("rel") + "";
            hide(relToHide);
        });

        var rel = $(this).attr("rel") + "";
        $(this).parent().attr("class", "selected");
        show(rel);

        if ($('#utags').is(":visible")) {
            $('.tagcollage').isotope('reLayout');
        }

        if (!document.layoutUserPhotoCollage) {
            if ($('#uphotos').is(":visible")) {
                document.layoutUserPhotoCollage = 1;
                layoutPhotosCollage('#uphotos', loadNewUserPhotos);
            }
        }

        if ($('#uvideos').is(":visible")) {
            if (!document.layoutUserVideoCollage) {
                document.layoutUserVideoCollage = 1;
                layoutVideosCollage('#uvideos', loadNewUserVideos);
            }
        }

        if ($('#uarticles').is(":visible")) {
            if (!document.layoutUserArticlesCollage) {
                document.layoutUserArticlesCollage = 1;
                layoutArticlesCollage('#uarticles', loadNewUserArticles);
            }
        }

        return false;
    });

    $('#dashboardfriendslink').click(function () {
        $('#friendstablink').click();
    });

    $('.unfollowuserbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var uid = $(this).attr("data-uid") + "";
        if (uid != null) {
            unfollowUser(uid);
        }

        return false;
    });

    $('.followuserbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var uid = $(this).attr("data-uid") + "";
        if (uid != null) {
            followUser(uid);
        }

        return false;
    });

    $('.btnbluestyle').click(function () {
        var sid = $(this).attr("data-sid") + "";

        if (sid == "addfbslfriend") {
            if (!loggedin) {
                $('#loginbtn').click();
                return false;
            }

            var uid = $(this).attr("data-uid") + "";
            if (uid != null) {
                sendFriendRequest(uid);
            }

            return false;
        }
    });

    $('#uploadprofilephotobtn').click(function () {
        var userProfileId = $(this).attr("data-upid") + "";

        $('#userprofilephotouploadcontent').load("/account/GetUserProfilePhotoUploadForm?userProfileId=" + htmlEncode(userProfileId));

        return false;
    });

    $('#sendMsgToUserBtn').click(function () {
        var recipientId = $(this).attr("data-uid") + "";

        $('#userprofilesendmsgformcontent').load("/message/GetPredefinedWriteMessageUI?recipientUID=" + htmlEncode(recipientId));

        return false;
    });

    $('#addfriendbtn').click(function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var uid = $(this).attr("data-uid") + "";
        if (uid != null) {
            sendFriendRequest(uid);
        }

        return false;
    });

    $('#acceptfriendrequestbtn').click(function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var uid = $(this).attr("data-uid") + "";
        if (uid != null) {
            acceptFriendRequest(uid);
        }

        return false;
    });

    $('#rejectfriendrequestbtn').click(function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var uid = $(this).attr("data-uid") + "";
        if (uid != null) {
            rejectFriendRequest(uid);
        }

        return false;
    });

    if (loggedin) {
        $("#uploadprofilephotobtn").fancybox({
            'autoSize': true,
            'modal': false,
            'padding': 0,
            'transitionIn': 'fade',
            'transitionOut': 'fade',
            'scrolling': 'no'
        });

        $("#sendMsgToUserBtn").fancybox({
            'autoSize': true,
            'modal': false,
            'padding': 0,
            'transitionIn': 'fade',
            'transitionOut': 'fade',
            'scrolling': 'no'
        });

        $("#writemessagelink").fancybox({
            'autoSize': true,
            'modal': false,
            'padding': 0,
            'transitionIn': 'fade',
            'transitionOut': 'fade',
            'scrolling': 'no'
        });
    }
});

function setViewPortWidth() {
    var viewportWidth = $('.page').width();

    if (viewportWidth <= 1024) {
        viewportWidth = 1024;
    }

    viewportWidth = viewportWidth - 20;

    var absColsCount = viewportWidth / columnWidth;
    absColsCount = Math.floor(absColsCount);
    var actualColCount = absColsCount;
    var absBlankSpaceWidth = viewportWidth % columnWidth;

    if (absBlankSpaceWidth < gutterWidth) {
        actualColCount = actualColCount - 1;
    }
    document.viewPortWidth = (actualColCount * columnWidth + gutterWidth);

    viewportWidth = document.viewPortWidth;

    $('#header .hcontainer').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.breadcomb').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.page .profile').attr('style', 'width:' + (viewportWidth) + 'px');

    $('.profile .userprofilewall').attr('style', 'width:' + (document.viewPortWidth - 334 - gutterWidth) + 'px');
    $('.userwall .wallpost .postcontent').attr('style', 'width:' + ((document.viewPortWidth - 334 - gutterWidth) - 100) + 'px');
}

function loadNewUserPhotos(data) {
    if ($('#uphotos .photocollage') != 'undefined' && $('#uphotos .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#uphotos .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#uphotos').append(data);
        layoutPhotosCollage('#uphotos', loadNewUserPhotos);

        $('#uphotos .photocollage').isotope('reLayout');
    }
}

function loadNewUserVideos(data) {
    if ($('#uvideos .photocollage') != 'undefined' && $('#uvideos .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#uvideos .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#uvideos').append(data);
        layoutVideosCollage('#uvideos', loadNewUserVideos);

        $('#uvideos .photocollage').isotope('reLayout');
    }
}

function loadNewUserArticles(data) {
    if ($('#uarticles .articlemaincontainer') != 'undefined' && $('#uarticles .articlemaincontainer').length > 0) {
        var newElements = $('.articlecontainer', data);
        $('#uarticles .articlemaincontainer').append(newElements).isotope('appended', newElements);

        formatArticleContents('#uarticles .articlemaincontainer');
    }
    else {
        $('#uarticles').append(data);

        formatArticleContents('#uarticles .articlemaincontainer');
        layoutArticlesCollage('#uarticles', loadNewUserArticles);

        $('#uarticles .articlemaincontainer').isotope('reLayout');
    }
}

function rejectFriendRequest(friendUserId) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/RejectFriendRequest",
        data: {
            friendId: friendUserId
        },
        async: false,
        success: function (result) {
            window.location.reload(true);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Friend request could not be rejected.');
        }
    });
}

function acceptFriendRequest(friendUserId) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/AcceptFriendRequest",
        data: {
            friendId: friendUserId
        },
        async: false,
        success: function (result) {
            window.location.reload(true);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Friend request could not be accepted.');
        }
    });
}

function sendFriendRequest(indicatedFriendId) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/SendSixLooniesFriendRequest",
        data: {
            indicatedFriend: indicatedFriendId
        },
        async: false,
        success: function (result) {
            window.location.reload(true);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: You could not be added as a follower.');
        }
    });
}

function followUser(userToFollowId) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/FollowUser",
        data: "userToFollowId=" + htmlEncode(userToFollowId),
        async: false,
        success: function (result) {
            showSuccessNotification('You have been added as a follower.');
            $('.userdetails #userstats').load("/Account/GetUserStats?userid=" + userToFollowId);
            $('.followuserbtn').attr("class", "btnundo unfollowuserbtn");
            $('.unfollowuserbtn span').html("Unfollow");
            $('.unfollowuserbtn img').attr("src", "/images/tick.png");
        },
        error: function (req, status, error) {
            showErrorNotification('Error: You could not be added as a follower.');
        }
    });
}

function unfollowUser(userToFollowId) {
    $.ajax(
    {
        type: "POST",
        url: "/Account/UnfollowUser",
        data: "followedUserId=" + htmlEncode(userToFollowId),
        async: false,
        success: function (result) {
            showSuccessNotification('You have been removed as a follower.');
            $('.userdetails #userstats').load("/Account/GetUserStats?userid=" + userToFollowId);
            $('.unfollowuserbtn').attr("class", "btnstyle followuserbtn");
            $('.followuserbtn span').html("Follow");
            $('.followuserbtn img').attr("src", "/images/plus.gif");
        },
        error: function (req, status, error) {
            showErrorNotification('Error: You could not be removed as a follower.');
        }
    });
}


$.Isotope.prototype._getCenteredMasonryColumns = function () {
    this.width = this.element.width();

    var parentWidth = this.element.parent().width();

    // i.e. options.masonry && options.masonry.columnWidth
    var colW = this.options.masonry && this.options.masonry.columnWidth ||
    // or use the size of the first item
                  this.$filteredAtoms.outerWidth(true) ||
    // if there's no items, use size of container
                  parentWidth;

    var cols = Math.floor(parentWidth / colW);
    cols = Math.max(cols, 1);

    // i.e. this.masonry.cols = ....
    this.masonry.cols = cols;
    // i.e. this.masonry.columnWidth = ...
    this.masonry.columnWidth = colW;
};

$.Isotope.prototype._masonryReset = function () {
    // layout-specific props
    this.masonry = {};
    // FIXME shouldn't have to call this again
    this._getCenteredMasonryColumns();
    var i = this.masonry.cols;
    this.masonry.colYs = [];
    while (i--) {
        this.masonry.colYs.push(0);
    }
};

$.Isotope.prototype._masonryResizeChanged = function () {
    var prevColCount = this.masonry.cols;
    // get updated colCount
    this._getCenteredMasonryColumns();
    return (this.masonry.cols !== prevColCount);
};

$.Isotope.prototype._masonryGetContainerSize = function () {
    var unusedCols = 0,
        i = this.masonry.cols;
    // count unused columns
    while (--i) {
        if (this.masonry.colYs[i] !== 0) {
            break;
        }
        unusedCols++;
    }

    return {
        height: Math.max.apply(Math, this.masonry.colYs),
        // fit container to columns that have been used;
        width: (this.masonry.cols - unusedCols) * this.masonry.columnWidth
    };
};
