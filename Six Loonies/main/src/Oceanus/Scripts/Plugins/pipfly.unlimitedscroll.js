$.fn.unlimitedscroll = function (options) {
    var defaults = {
        overlay: "#overlay",
        load_callback: function (data) { }
    };
    var finished = false;
    var isDuringAjax = false;
    var options = $.extend(defaults, options);
    var currentPage = $('.paginationlink', this).attr('data-page-id') - 1;
    var url = $('.paginationlink', this).attr('href');
    var load = function () {
        var queryStr = "&page=" + encodeURIComponent(currentPage);
        var post_url = url + queryStr;
        var divContainer = document.createElement('div');
        $(divContainer).load(post_url, function (data) {
            if (options.item_selector != "") {
                if ($(options.item_selector, data).length == 0) {
                    finished = true;
                }
            }

            options.load_callback(data);
            $(options.overlay).fadeOut();
            isDuringAjax = false;
        });
    };
    return this.each(function () {
        obj = $(this);
        overlay = $(options.overlay);

        $(this).show(function () {
            if (!finished && $(this).is(':visible') && !isDuringAjax) {
                var pixelsFromNavToBottom = $(document).height() - $('#footer').offset().top;
                var pixelsFromWindowBottomToBottom = 0 + $(document).height() - $(window).scrollTop() - $(window).height();
                if ((pixelsFromWindowBottomToBottom - 50) < pixelsFromNavToBottom) {
                    isDuringAjax = true;
                    currentPage++;
                    overlay.fadeIn();
                    load();
                }
            }
        });

        $(window).scroll(function () {
            if (!finished && $(this).is(':visible') && !isDuringAjax) {
                var pixelsFromNavToBottom = $(document).height() - $('#footer').offset().top;
                var pixelsFromWindowBottomToBottom = 0 + $(document).height() - $(window).scrollTop() - $(window).height();
                if ((pixelsFromWindowBottomToBottom - 50) < pixelsFromNavToBottom) {
                    isDuringAjax = true;
                    currentPage++;
                    overlay.fadeIn();
                    load();
                }
            }
        });
    });
}