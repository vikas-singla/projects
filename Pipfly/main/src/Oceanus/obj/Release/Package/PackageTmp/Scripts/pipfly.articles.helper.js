function formatArticleContents(sel) {
    try {
        $(sel + ' .articlewidthrect .oceanus-content img').each(function () {
            var ratio = ($(this).width() * 1.0) / ($(this).height() * 1.0);
            if (ratio <= 1) {
                var adjWidth = ((170 * 1.0) / $(this).height()) * $(this).width();
                var adjHeight = ((170 * 1.0) / $(this).width()) * $(this).height();

                if (adjWidth < 170) {
                    $(this).height(170);
                    $(this).css('width', adjWidth);
                }
                else {
                    $(this).width(170);
                    $(this).css('height', adjHeight);
                }
            }
            else {
                var adjHeight = ((170 * 1.0) / $(this).width()) * $(this).height();
                var adjWidth = ((170 * 1.0) / $(this).height()) * $(this).width();

                if (adjHeight < 170) {
                    $(this).width(170);
                    $(this).css('height', adjHeight);
                }
                else {
                    $(this).height(170);
                    $(this).css('width', adjWidth);
                }
            }
        });

        $(sel + ' .articlewidthsmallsquare .oceanus-content img').each(function () {
            var ratio = ($(this).width() * 1.0) / ($(this).height() * 1.0);
            if (ratio <= 1) {
                var adjWidth = ((80 * 1.0) / $(this).height()) * $(this).width();
                var adjHeight = ((125 * 1.0) / $(this).width()) * $(this).height();

                if (adjWidth < 125) {
                    $(this).height(80);
                    $(this).css('width', adjWidth);
                }
                else {
                    $(this).width(125);
                    $(this).css('height', adjHeight);
                }
            }
            else {
                var adjHeight = ((125 * 1.0) / $(this).width()) * $(this).height();
                var adjWidth = ((80 * 1.0) / $(this).height()) * $(this).width();

                if (adjHeight < 80) {
                    $(this).width(125);
                    $(this).css('height', adjHeight);
                }
                else {
                    $(this).height(80);
                    $(this).css('width', adjWidth);
                }
            }
        });

        $(sel + ' .articlecontainer .articlecontent .oceanus-content').each(function () {
            var artContainer = $(this).closest('.articlecontent');
            var addLinkToArticleTitle = true;

            $('a', this).each(function () {
                var newNode = document.createElement('span');
                try {
                    if ($('.oceanus-title', this).length == 0) {
                        newNode.innerHTML = this.innerHTML;
                        this.parentNode.replaceChild(newNode, this);
                    }
                    else {
                        if (artContainer != null && artContainer != 'undefined') {
                            var articleId = $(artContainer).attr("data-article-id");
                            $(this).attr("href", "/a/" + articleId);
                            addLinkToArticleTitle = false;
                        }
                    }
                }
                catch (e) {
                }
            });

            if (addLinkToArticleTitle) {
                $('.oceanus-title', this).each(function () {
                    var newNode = document.createElement('a');
                    try {
                        if (artContainer != null && artContainer != 'undefined') {
                            var articleId = $(artContainer).attr("data-article-id");
                            $(newNode).attr("href", "/a/" + articleId);
                            this.parentNode.replaceChild(newNode, this);
                            newNode.appendChild(this);
                        }
                    }
                    catch (e) {
                    }
                });
            }

            var width = $(this).parent().width();
            var height = $(this).parent().height();

            var imageCount = 0;

            $('img', this).each(function () {
                ++imageCount;
                if (imageCount > 1) {
                    $(this).remove();
                }
                else {
                    var imageHeight = $(this).height();
                    var removeImage = false;

                    if (imageHeight > height) {
                        removeImage = true;
                    }
                    else if ($('.oceanus-title', artContainer).length > 0) {
                        if (imageHeight + $(this).height() > height) {
                            removeImage = true;
                        }
                    }

                    if (removeImage) {
                        $(this).remove();
                    }
                    else {
                        try {
                            if (artContainer != null && artContainer != 'undefined') {
                                var articleId = $(artContainer).attr("data-article-id");

                                var newLink = document.createElement('a');
                                $(newLink).attr("href", "/a/" + articleId);
                                this.parentNode.replaceChild(newLink, this);
                                newLink.appendChild(this);
                            }
                        }
                        catch (e) {
                        }
                    }
                }
            });

            $('table', this).each(function () {
                $(this).remove();
            });

            var articleElementText = getInnerText(this, false);
            var targetElementTextLength = articleElementText.length;

            if (artContainer != null && artContainer != 'undefined') {
                var trimArticle = $(artContainer).attr("data-shorten-article");
                if (trimArticle != "false") {
                    targetElementTextLength = targetElementTextLength * 0.5;
                }

                $(artContainer).attr("data-shorten-article", "false");
            }

            var node = $('p', this);
            if (node.length > 0) {
                var firstNode = node[0];
                $(firstNode).attr("style", "margin-top:0;");
            }

            var currHtml = null;
            while (height > 0 && $(this).height() > height && currHtml != this.innerHTML) {
                currHtml = this.innerHTML;
                trimArticleContents(this, this, height);
                var updatedHtml = this.innerHTML;
            }
            articleElementText = getInnerText(this, false);

            var currArticleMarkup = null;
            while (articleElementText.length > targetElementTextLength && currArticleMarkup != this.innerHTML) {
                currArticleMarkup = this.innerHTML;
                trimArticleContents(this, this, null);
                articleElementText = getInnerText(this, false);
            }

            var horizontalRuleRegEx = /(<hr[^>]*>[ \n\r\t]*){1,}/gi;
            this.innerHTML = this.innerHTML.replace(horizontalRuleRegEx, '');
        });
    }
    catch (e) {
    }
}

function trimArticleContents(root, origRoot, targetHeight) {
    //trim content from article
    var lastParagraph = null;
    var paragraph = $('p, span, div, h3, h4, h5, h6', root);
    if (paragraph.length <= 0) {
        lastParagraph = root;
    }
    else {
        lastParagraph = paragraph[paragraph.length - 1];

        if ($.trim(lastParagraph.innerHTML) == "") {
            lastParagraph.parentNode.removeChild(lastParagraph);
            trimArticleContents(root, origRoot, targetHeight);
            return;
        }
    }

    for (var i = lastParagraph.childNodes.length - 1; i >= 0; i--) {
        var childNode = lastParagraph.childNodes[i];
        if (childNode.nodeType == Node.TEXT_NODE) {
            if (targetHeight == 'undefined' || targetHeight == null || targetHeight <= 0) {
                var text = childNode.nodeValue;
                if ($.trim(text) == "" || $.trim(text) == "...") {
                    childNode.parentNode.removeChild(childNode);
                }
                else {
                    text = text.replace(/\s*\S+\s*$/, "...");
                    childNode.nodeValue = text;
                    break;
                }
            }
            else {
                while (targetHeight > 0 && $(origRoot).height() > targetHeight) {
                    var text = childNode.nodeValue;
                    if ($.trim(text) == "" || $.trim(text) == "...") {
                        childNode.parentNode.removeChild(childNode);
                        break;
                    }
                    else {
                        text = text.replace(/\s*\S+\s*$/, "...");
                        childNode.nodeValue = text;
                    }
                }

                if ($(origRoot).height() < targetHeight) {
                    break;
                }
            }
        }
        else {
            if (childNode.innerHTML == "") {
                childNode.parentNode.removeChild(childNode);
            }
            else {
                trimArticleContents(childNode, origRoot, targetHeight);
            }
        }
    }

    if ($.trim(lastParagraph.innerHTML) == "") {
        lastParagraph.parentNode.removeChild(lastParagraph);
    }
}

function getInnerText(e, normalizeSpaces) {
    var textContent = "";
    var trimRe = /^\s+|\s+$/g;
    var normalizeRe = /\s{2,}/g;

    normalizeSpaces = (typeof normalizeSpaces == 'undefined') ? true : normalizeSpaces;
    if (navigator.appName == "Microsoft Internet Explorer")
        textContent = e.innerText.replace(trimRe, "");
    else
        textContent = e.textContent.replace(trimRe, "");

    if (normalizeSpaces)
        return textContent.replace(normalizeRe, " ");
    else
        return textContent;
}

$(document).ready(function () {
    $('.articlecontainer .btnsect .actionbtns .likeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var articleId = $(this).attr("data-article-id") + "";
        likeArticle(this, articleId);
    });

    $('.articlecontainer .btnsect .actionbtns .unlikeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var articleId = $(this).attr("data-article-id") + "";
        unlikeArticle(this, articleId);
    });

    $(".articlecontainer .shareaction").live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }
    });

    if (loggedin) {
        $(".articlecontainer .shareaction").fancybox({
            'modal': false,
            'type': 'ajax',
            'padding': 0,
            'scrolling': 'no',
            'fitToView': false,
            'transitionIn': 'fade',
            'transitionOut': 'fade',
            'afterLoad': function () {
                try {
                    $("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("http://www.pipfly.com/webpage/GetTokenSearchShareLinkUsers", {
                        propertyToSearch: "name",
                        emptyText: "Find people or type email address...",
                        hintText: "Type here to search by name or provide an email address...",
                        resultsFormatter: function (item) { return "<li>" + "<img src='" + item.userprofileimageurl + "' title='" + item.name + "' height='25px' width='25px' />" + "<div style='display: inline-block; padding-left: 10px;'><div class='full_name'>" + item.name + "</div><div class='email'>" + item.email + "</div></div></li>" },
                        tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }
                    });

                    $('textarea').elastic();
                }
                catch (err) {
                }
            }
        });
    }
});

function likeArticle(linkRef, articleId) {
    $.ajax(
    {
        type: "POST",
        url: "/article/LikeArticle",
        data: {
            articleId: articleId
        },
        success: function (result) {
            showSuccessNotification('Your like for the article has been saved.');

            $(linkRef).attr("class", "action unlikeaction");
            $(linkRef).html("<img class='actionbtnimg' src='/images/heart_filled_sm.png' alt /><span>Liked</span>");

            if ($('#articlelikes') != null && $('#articlelikes') != 'undefined') {
                $('#articlelikes').load("/article/GetArticleLikes?articleId=" + articleId);
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to save your like. Please try again later.');
        }
    });
}

function unlikeArticle(linkRef, articleId) {
    $.ajax(
    {
        type: "POST",
        url: "/article/UnikeArticle",
        data: {
            articleId: articleId
        },
        success: function (result) {
            showSuccessNotification('Your like for the article has been removed.');

            $(linkRef).attr("class", "action likeaction");
            $(linkRef).html("<img class='actionbtnimg' src='/images/heart_empty_sm.png' alt /><span>Like</span>");

            if ($('#articlelikes') != null && $('#articlelikes') != 'undefined') {
                $('#articlelikes').load("/article/GetArticleLikes?articleId=" + articleId);
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to save your like. Please try again later.');
        }
    });
}