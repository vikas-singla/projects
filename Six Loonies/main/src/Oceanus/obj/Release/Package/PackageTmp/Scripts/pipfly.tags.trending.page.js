var tagsGutterWidth = 32;
var tagsColumnWidth = 239;

$(document).ready(function () {
    if ($('#spotlightcontainer').length == 0) {
        setTagsViewPortWidth();
    }

    $('.tagcollage').isotope({
        itemSelector: '.tagcollageitem',
        masonry: { tagsColumnWidth: 1 },
        layoutMode: 'masonry',
        resizable: true,
        animationEngine: 'best-available'
    });

    $(window).load(function () {
        $('.tagcollage').isotope('reLayout');
    });

    $('.followtopicbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var topicId = $(this).attr("data-topic-id") + "";
        addUserAsTopicFollower(this, topicId);

        return false;
    });

    $('.unfollowtopicbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var topicId = $(this).attr("data-topic-id") + "";
        removeUserAsTopicFollower(this, topicId);

        return false;
    });
});

function setTagsViewPortWidth() {
    var viewportWidth = $('.page').width();

    if (viewportWidth <= 1024) {
        viewportWidth = 1024;
    }

    viewportWidth = viewportWidth - 20;

    var absColsCount = viewportWidth / tagsColumnWidth;
    absColsCount = Math.floor(absColsCount);
    var actualColCount = absColsCount;
    var absBlankSpaceWidth = viewportWidth % tagsColumnWidth;

    if (absBlankSpaceWidth < tagsGutterWidth) {
        actualColCount = actualColCount - 1;
    }
    document.viewPortWidth = (actualColCount * tagsColumnWidth + tagsGutterWidth);

    viewportWidth = document.viewPortWidth;

    $('#header .hcontainer').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.breadcomb').attr('style', 'width:' + (viewportWidth) + 'px');
    $('.page .tagcollagecontainer').attr('style', 'width:' + (viewportWidth - tagsGutterWidth) + 'px');
    $('.page .trendingheader').attr('style', 'width:' + (viewportWidth) + 'px');
}

function addUserAsTopicFollower(btn, topicId) {
    $.ajax(
    {
        type: "POST",
        url: "/Topic/FollowTopic",
        data: {
            topicId: topicId
        },
        success: function (result) {
            showSuccessNotification('You have been added as a follower to the tag.');
            $(btn).attr("class", "btnundo unfollowtopicbtn");
            $('span', btn).html("Unfollow");
            $('img', btn).attr("src", "/images/tick.png");

            if ($('#spotlightcontainer') != 'undefined') {
                var followCount = $('#spotlightcontainer').attr('data-follow-count');
                followCount = followCount + 1;

                if (followCount <= 0) {
                    $('#spotlightcontainer .optionbar').hide();
                    $('#spotlightcontainer .spotlightoverview').show();
                }
                else {
                    $('#spotlightcontainer .optionbar').show();
                    $('#spotlightcontainer .spotlightoverview').hide();
                }
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to add you as a follower to the tag.');
        }
    });
}

function removeUserAsTopicFollower(btn, topicId) {
    $.ajax(
    {
        type: "POST",
        url: "/Topic/UnfollowTopic",
        data: {
            topicId: topicId
        },
        success: function (result) {
            showSuccessNotification('You have been removed as a follower to the tag.');
            $(btn).attr("class", "btnstyle followtopicbtn");
            $('span', btn).html("Follow");
            $('img', btn).attr("src", "/images/plus.gif");

            if ($('#spotlightcontainer') != 'undefined') {
                var followCount = $('#spotlightcontainer').attr('data-follow-count');
                followCount = followCount - 1;

                if (followCount <= 0) {
                    $('#spotlightcontainer .optionbar').hide();
                    $('#spotlightcontainer .spotlightoverview').show();
                }
                else {
                    $('#spotlightcontainer .optionbar').show();
                    $('#spotlightcontainer .spotlightoverview').hide();
                }
            }
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to remove you as a follower from the tag.');
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
