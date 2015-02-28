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

function populateSearchPageOverview() {
    var viewportWidth = document.viewPortWidth;

    $('#header .hcontainer').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.topic .theader').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.topic .tcontent').attr('style', 'width:' + (viewportWidth) + 'px');

    adjustPopulateSearchMediaPreview(((viewportWidth - 15) * 1.0 / 2));
}

function adjustPopulateSearchMediaPreview(topicPreviewWidth) {
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
    populateSearchPageOverview();

    $(".userwall .newtopicwallpost #newtopicpostinput").emptyText(newtopicpostemptytext, "emptytextstyle");

    if ($('#topicarticlessect').is(":visible")) {
        if (!document.layoutTopicArticlesCollage) {
            document.layoutTopicArticlesCollage = 1;
            layoutArticlesCollage('#topicarticlessect', loadNewSearchArticles);
            formatArticleContents('#topicarticlessect .articlemaincontainer');
        }
    }

    if ($('#topicvideosect').is(":visible")) {
        if (!document.layoutTopicVideoCollage) {
            layoutVideosCollage('#topicvideosect', loadNewSearchVideos);
            document.layoutTopicVideoCollage = 1;
        }
    }

    if ($('#topicphotosect').is(":visible")) {
        if (!document.layoutTopicPhotoCollage) {
            layoutPhotosCollage('#topicphotosect', loadNewSearchPhotos);
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
                layoutArticlesCollage('#topicarticlessect', loadNewSearchArticles);
            }

            $('#topicarticlessect .articlemaincontainer').isotope('reLayout');
        }

        if ($('#topicvideosect').is(":visible")) {
            if (!document.layoutTopicVideoCollage) {
                layoutVideosCollage('#topicvideosect', loadNewSearchVideos);
                document.layoutTopicVideoCollage = 1;
            }

            $('#topicvideosect .photocollage').isotope('reLayout');
        }

        if ($('#topicphotosect').is(":visible")) {
            if (!document.layoutTopicPhotoCollage) {
                layoutPhotosCollage('#topicphotosect', loadNewSearchPhotos);
                document.layoutTopicPhotoCollage = 1;
            }

            $('#topicphotosect .photocollage').isotope('reLayout');
        }

        if ($('#tagsect').is(":visible")) {
            if (!document.layoutTagCollage) {
                layoutTagsCollage('#tagsect', loadNewSearchTags);
                document.layoutTagCollage = 1;
            }

            $('#tagsect .tagcollage').isotope('reLayout');
        }

        return false;
    });

    $('#topicfilterbyfriendslink').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }
    });
});

function loadNewSearchTags(data) {
    if ($('#tagsect .tagcollage') != 'undefined' && $('#tagsect .tagcollage').length > 0) {
        var newElements = $('.tagcollageitem', data);
        $('#tagsect .tagcollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#tagsect').append(data);
        layoutTagsCollage('#tagsect', loadNewSearchTags);
    }
}

function loadNewSearchPhotos(data) {
    if ($('#topicphotosect .photocollage') != 'undefined' && $('#topicphotosect .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#topicphotosect .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#topicphotosect').append(data);
        layoutPhotosCollage('#topicphotosect', loadNewSearchPhotos);
    }
}

function loadNewSearchVideos(data) {
    if ($('#topicvideosect .photocollage') != 'undefined' && $('#topicvideosect .photocollage').length > 0) {
        var newElements = $('.collagephoto', data);
        $('#topicvideosect .photocollage').append(newElements).isotope('appended', newElements);
    }
    else {
        $('#topicvideosect').append(data);
        layoutVideosCollage('#topicvideosect', loadNewSearchVideos);
    }
}

function loadNewSearchArticles(data) {
    if ($('#topicarticlessect .articlemaincontainer') != 'undefined' && $('#topicarticlessect .articlemaincontainer').length > 0) {
        var newElements = $('.articlecontainer', data);
        $('#topicarticlessect .articlemaincontainer').append(newElements).isotope('appended', newElements);

        formatArticleContents('#uarticles .articlemaincontainer');
    }
    else {
        $('#topicarticlessect').append(data);

        formatArticleContents('#topicarticlessect .articlemaincontainer');
        layoutArticlesCollage('#topicarticlessect', loadNewSearchArticles);
    }
}