$(document).ready(function () {
    $(".topicsection #edittopics #addtopicinput").tokenInput("http://www.pipfly.com/home/GetTokenSearchPageTags", {
        propertyToSearch: "name",
        emptyText: "Type topic here...",
        hintText: "Type to search for topics",
        resultsFormatter: function (item) { return "<li>" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div></div></li>" },
        tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
    });

    $('.topicsection #edittopics #addPageTopic').click(function () {
        var webPageId = $(this).attr("data-page-id") + "";
        var topic = $('.topicsection #edittopics #addtopicinput').tokenInput('get');

        addPageTopic(webPageId, topic);
        return false;
    });

    $('.topicsection #edittopics .topicdelbtn').click(function () {
        var webPageId = $(this).attr("data-page-id") + "";
        var topicId = $(this).attr("data-tid") + "";

        deletePageTopic(webPageId, topicId);
        return false;
    });

    $('.topicsection #addWebPageTopicsBtn').click(function () {
        $('.topicsection #edittopics').show();
        $('.topicsection #readonlytopics').hide();
        return false;
    });

    $('.topicsection #edittopics #editPageTopicDone').click(function () {
        $('.topicsection #edittopics').hide();
        $('.topicsection #readonlytopics').show();
        return false;
    });

    if ($('.discussionsect').height() < $('.othercontentsect').height()) {
        $('.discussionsect').height($('.othercontentsect').height());
    }
});

function addPageTopic(webPageId, topic) {
    $.ajax(
    {
        type: "POST",
        url: "/WebPage/AddPageTopic",
        data: {
            webPageId: webPageId,
            topic: JSON.stringify(topic)
        },
        async: false,
        success: function (result) {
            $('.othercontentsect #webpagetopics').load("/webpage/GetWebPageTopics?pageId=" + webPageId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Topic could not be added. Please try again later.');
        }
    });
}

function deletePageTopic(webPageId, topicId) {
    $.ajax(
    {
        type: "POST",
        url: "/WebPage/DeletePageTopic",
        data: {
            webPageId: webPageId,
            topicId: topicId
        },
        async: false,
        success: function (result) {
            $('.othercontentsect #webpagetopics').load("/webpage/GetWebPageTopics?pageId=" + webPageId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Topic could not be deleted. Please try again later.');
        }
    });
}