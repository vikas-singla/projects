var gutterWidth = 32;
var columnWidth = 320;
var newtopicpostemptytext = "Type here to share your opinion, start a discussion, or write something interesting.";

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
    document.viewPortWidth = (actualColCount * columnWidth + gutterWidth - 2);
}

function populateTopicPageOverview() {
    var viewportWidth = document.viewPortWidth;

    $('#header .hcontainer').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.topic .theader').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.topic .tcontent').attr('style', 'width:' + (viewportWidth) + 'px');

    adjustPopulateTopicMediaPreview(((viewportWidth - 15) * 1.0 / 2));
}

function adjustPopulateTopicMediaPreview(topicPreviewWidth) {
    topicPreviewWidth = topicPreviewWidth - 4;
    var photoSize = 110;
    var count = 0;
    var widthVariance = (topicPreviewWidth % photoSize);

    while (widthVariance > 15) {
        photoSize += 1;
        var widthVariance = (topicPreviewWidth % photoSize);
        if (photoSize > 155) {
            break;
        }
    }

    count = Math.floor(((topicPreviewWidth * 1.0) / photoSize));

    $('.collagepreview .photocollagepreview').load('/topic/PhotoPreview?topicId=40&numPhotos=' + count, function (response, status, xhr) {
        $('.photocollagepreview .photopreview .prevphotocont').each(function (key) {
            $(this).attr('style', 'width:' + (photoSize - 6) + 'px;height:' + (photoSize - 6) + 'px;');
        });
        $('.photocollagepreview .photopreview').attr('style', 'width:' + (photoSize * count) + 'px;');
    });
    $('.collagepreview .videocollagepreview').load('/topic/VideoPreview?topicId=40&numVideos=' + count, function (response, status, xhr) {
        $('.videocollagepreview .photopreview .prevphotocont').each(function (key) {
            $(this).attr('style', 'width:' + (photoSize - 6) + 'px;height:' + (photoSize - 6) + 'px;');
        });
        $('.videocollagepreview .photopreview').attr('style', 'width:' + (photoSize * count) + 'px;');
    });
}

$(document).ready(function () {
    setViewPortWidth();
    populateTopicPageOverview();

    $(".userwall .newtopicwallpost #newtopicpostinput").emptyText(newtopicpostemptytext, "emptytextstyle");

    if ($('#topicarticlessect').is(":visible")) {
        if (!document.layoutTopicArticlesCollage) {
            document.layoutTopicArticlesCollage = 1;
            layoutArticlesCollage('#topicarticlessect', loadNewTopicArticles);
            formatArticleContents('#topicarticlessect .articlemaincontainer');
        }
    }

    if ($('#topicvideosect').is(":visible")) {
        if (!document.layoutTopicVideoCollage) {
            layoutVideosCollage('#topicvideosect', loadNewTopicVideos);
            document.layoutTopicVideoCollage = 1;
        }
    }

    if ($('#topicphotosect').is(":visible")) {
        if (!document.layoutTopicPhotoCollage) {
            layoutPhotosCollage('#topicphotosect', loadNewTopicPhotos);
            document.layoutTopicPhotoCollage = 1;
        }
    }

    $('#topicsectiontabs li .topicsectionlink').click(function () {
        $('#topicsectiontabs li .topicsectionlink').each(function () {
            $(this).parent().attr("class", "");
            var relToHide = $(this).attr("rel") + "";
            hide(relToHide);
        });

        var rel = $(this).attr("rel") + "";
        $(this).parent().attr("class", "selected");
        $(rel).show();

        if ($('#topicarticlessect').is(":visible")) {
            if (!document.layoutTopicArticlesCollage) {
                document.layoutTopicArticlesCollage = 1;
                layoutArticlesCollage('#topicarticlessect', loadNewTopicArticles);
            }

            $('#topicarticlessect .articlemaincontainer').isotope('reLayout');
        }

        if ($('#topicvideosect').is(":visible")) {
            if (!document.layoutTopicVideoCollage) {
                layoutVideosCollage('#topicvideosect', loadNewTopicVideos);
                document.layoutTopicVideoCollage = 1;
            }

            $('#topicvideosect .photocollage').isotope('reLayout');
        }

        if ($('#topicphotosect').is(":visible")) {
            if (!document.layoutTopicPhotoCollage) {
                layoutPhotosCollage('#topicphotosect', loadNewTopicPhotos);
                document.layoutTopicPhotoCollage = 1;
            }

            $('#topicphotosect .photocollage').isotope('reLayout');
        }

        return false;
    });

    $('#topicfilterbyfriendslink').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }
    });

    $('.followtopicbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var topicId = $(this).attr("data-topic-id") + "";
        addUserAsTopicFollower(topicId);

        return false;
    });

    $('.unfollowtopicbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var topicId = $(this).attr("data-topic-id") + "";
        removeUserAsTopicFollower(topicId);

        return false;
    });
});

function loadNewTopicPhotos(data) {
    if ($('#topicphotosect .photocollage') != 'undefined' && $('#topicphotosect .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#topicphotosect .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#topicphotosect').append(data);
        layoutPhotosCollage('#topicphotosect', loadNewTopicPhotos);
    }
}

function loadNewTopicVideos(data) {
    if ($('#topicvideosect .photocollage') != 'undefined' && $('#topicvideosect .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#topicvideosect .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#topicvideosect').append(data);
        layoutVideosCollage('#topicvideosect', loadNewTopicVideos);
    }
}

function loadNewTopicArticles(data) {
    if ($('#topicarticlessect .articlemaincontainer') != 'undefined' && $('#topicarticlessect .articlemaincontainer').length > 0) {
        var newElements = $('.articlecontainer', data);
        $('#topicarticlessect .articlemaincontainer').append(newElements).isotope('appended', newElements);

        formatArticleContents('#uarticles .articlemaincontainer');
    }
    else {
        $('#topicarticlessect').append(data);

        formatArticleContents('#topicarticlessect .articlemaincontainer');
        layoutArticlesCollage('#topicarticlessect', loadNewTopicArticles);
    }
}

function addUserAsTopicFollower(topicId) {
    $.ajax(
    {
        type: "POST",
        url: "/Topic/FollowTopic",
        data: {
            topicId: topicId
        },
        success: function (result) {
            showSuccessNotification('You have been added as a follower to the tag.');
            $('.rightpane #topicfollowerpanel').load("/Topic/GetTopicFollowers?topicId=" + topicId);
            $('.followtopicbtn').attr("class", "btnundo unfollowtopicbtn");
            $('.unfollowtopicbtn span').html("Unfollow");
            $('.unfollowtopicbtn img').attr("src", "/images/tick.png");
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to add you as a follower to the tag.');
        }
    });
}

function removeUserAsTopicFollower(topicId) {
    $.ajax(
    {
        type: "POST",
        url: "/Topic/UnfollowTopic",
        data: {
            topicId: topicId
        },
        success: function (result) {
            showSuccessNotification('You have been removed as a follower to the tag.');
            $('.rightpane #topicfollowerpanel').load("/Topic/GetTopicFollowers?topicId=" + topicId);
            $('.unfollowtopicbtn').attr("class", "btnstyle followtopicbtn");
            $('.followtopicbtn span').html("Follow");
            $('.followtopicbtn img').attr("src", "/images/plus.gif");
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to remove you as a follower from the tag.');
        }
    });
}