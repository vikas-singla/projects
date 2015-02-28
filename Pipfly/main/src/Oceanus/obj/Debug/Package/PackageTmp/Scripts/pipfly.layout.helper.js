function layoutTagsCollage(sel, newdata_func) {
    if (!document.tagCollageLayoutDefined) {
        if ($(sel + ' .tagcollage') != 'undefined' && $(sel + ' .tagcollage').length > 0) {
            document.tagCollageLayoutDefined = 1;
            $(sel + ' .tagcollage').isotope({
                itemSelector: '.tagcollageitem',
                masonry: { columnWidth: 1 },
                layoutMode: 'masonry',
                resizable: false,
                animationEngine: 'best-available'
            });
            $(window).load(function () {
                $(sel + ' .tagcollage').isotope('reLayout');
            });
        }
    }

    if (!document.tagCollageScollLinked) {
        document.tagCollageScollLinked = 1;
        setTagInfiniteScroll(sel, newdata_func);
    }
}

function setTagInfiniteScroll(sel, newdata_func) {
    var since_date = $(sel).attr("data-since-date");

    $(sel).unlimitedscroll({
        load_callback: newdata_func,
        item_selector: '.tagcollageitem',
        overlay: "#loading_overlay"
    });
}

function layoutPhotosCollage(sel, newdata_func) {
    if (!document.photoCollageLayoutDefined) {
        if ($(sel + ' .photocollage') != 'undefined' && $(sel + ' .photocollage').length > 0) {
            document.photoCollageLayoutDefined = 1;
            $(sel + ' .photocollage').isotope({
                itemSelector: '.collagephoto',
                masonry: { columnWidth: 1 },
                layoutMode: 'masonry',
                resizable: false,
                animationEngine: 'best-available'
            });
            $(window).load(function () {
                $(sel + ' .photocollage').isotope('reLayout');
            });
        }
    }

    if (!document.photoCollageScollLinked) {
        document.photoCollageScollLinked = 1;
        setPhotoInfiniteScroll(sel, newdata_func);
    }
}

function setPhotoInfiniteScroll(sel, newdata_func) {
    var since_date = $(sel).attr("data-since-date");

    $(sel).unlimitedscroll({
        load_callback: newdata_func,
        item_selector: '.collagephoto',
        overlay: "#loading_overlay"
    });
}

function layoutArticlesCollage(sel, newdata_func) {
    if ($(sel + ' .articlemaincontainer') != 'undefined' && $(sel + ' .articlemaincontainer').length > 0) {
        if (!document.articleCollageLayoutDefined) {
            document.articleCollageLayoutDefined = 1;
            $(sel + ' .articlemaincontainer').isotope({
                itemSelector: '.articlecontainer',
                masonry: { columnWidth: 320 },
                layoutMode: 'masonry',
                resizable: false,
                animationEngine: 'best-available'
            });
        }
    }

    if (!document.articleCollageScollLinked) {
        document.articleCollageScollLinked = 1;
        setArticleInfiniteScroll(sel, newdata_func);
    }
}

function setArticleInfiniteScroll(sel, newdata_func) {
    var since_date = $(sel).attr("data-since-date");

    $(sel).unlimitedscroll({
        load_callback: newdata_func,
        item_selector: '.articlecontainer',
        overlay: "#loading_overlay"
    });
}

function layoutVideosCollage(sel, newdata_func) {
    if ($(sel + ' .photocollage') != 'undefined' && $(sel + ' .photocollage').length > 0) {
        var collageSects = $(sel + ' .collagephoto');

        collageSects.each(function () {
            $('img', this).each(function () {
                var ratio = ($(this).width() * 1.0) / ($(this).height() * 1.0);

                if (ratio >= 0.75 && ratio <= 1.34) {
                    if ($(this).width() > 200 && $(this).height() > 200) {
                        $(this).parent().parent().parent().attr("class", $(this).parent().parent().parent().attr("class") + " collagevideosquare");
                        $(this).width(225);
                        $(this).css("height", "");
                    }
                    else {
                        $(this).parent().parent().parent().attr("class", $(this).parent().parent().parent().attr("class") + " collagevideosquare");

                        if ($(this).width() > $(this).height()) {
                            $(this).height(170);
                            $(this).css("width", "");
                        }
                        else {
                            $(this).width(225);
                            $(this).css("height", "");
                        }
                    }
                }
                else if (ratio < 0.75) {
                    $(this).parent().parent().parent().attr("class", $(this).parent().parent().parent().attr("class") + " collagevideosquare");

                    var adjWidth = ((170 * 1.0) / $(this).height()) * $(this).width();

                    if (adjWidth < 225) {
                        $(this).width(225);
                        $(this).css("height", "");
                    }
                    else {
                        $(this).height(170);
                        $(this).css("width", "");
                    }
                }
                else {
                    $(this).parent().parent().parent().attr("class", $(this).parent().parent().parent().attr("class") + " collagevideosquare");

                    var adjHeight = ((225 * 1.0) / $(this).width()) * $(this).height();

                    if (adjHeight < 170) {
                        $(this).height(170);
                        $(this).css("width", "");
                    }
                    else {
                        $(this).width(225);
                        $(this).css("height", "");
                    }
                }
            });
        });
    }

    if (!document.videoCollageIsotopeLinked) {
        document.videoCollageIsotopeLinked = 1;

        $(sel + ' .photocollage').isotope({
            itemSelector: '.collagephoto',
            masonry: { columnWidth: 1 },
            layoutMode: 'masonry',
            resizable: false,
            animationEngine: 'best-available'
        });

        $(window).load(function () {
            $(sel + ' .photocollage').isotope('reLayout');
        });
    }

    if (!document.videoCollageScollLinked) {
        document.videoCollageScollLinked = 1;
        setVideoInfiniteScroll(sel, newdata_func);
    }
}

function setVideoInfiniteScroll(sel, newdata_func) {
    var since_date = $(sel).attr("data-since-date");

    $(sel).unlimitedscroll({
        load_callback: newdata_func,
        item_selector: '.collagephoto',
        overlay: "#loading_overlay"
    });
}
