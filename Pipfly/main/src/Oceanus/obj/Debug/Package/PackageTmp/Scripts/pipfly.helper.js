var showingSignInDD = false;
var showingRegisterDD = false;
var showingUserSessionDD = false;
var showingCitySearchDD = false;
var notificationId = 1;
var globalsearchemptytext = "Type here to search...";

function htmlEncode(value) {
    if (value) {
        return encodeURIComponent(value);
    }
    else {
        return '';
    }
}

function loadVimeoThumb(id) {
    $.getJSON('http://www.vimeo.com/api/v2/video/' + id + '.json?callback=?', { format: "jsonp" }, function (data) {
        if (data != null && data[0] != null) {
            var id_img = "#vimeo-" + data[0].id;
            $(id_img).attr('src', data[0].thumbnail_medium);
        }
        else {
            var id_img = "#vimeo-" + id;
            $(id_img).attr('src', '/images/videothumbnail.gif');
        }
    });
}

function loadEditPortfolioVimeoThumb(id) {
    $.getJSON('http://www.vimeo.com/api/v2/video/' + id + '.json?callback=?', { format: "jsonp" }, function (data) {
        if (data != null && data[0] != null) {
            var id_img = "#editportfoliovimeo-" + data[0].id;
            $(id_img).attr('src', data[0].thumbnail_medium);
        }
        else {
            var id_img = "#editportfoliovimeo-" + id;
            $(id_img).attr('src', '/images/videothumbnail.gif');
        }
    });
}

function loadEditPortfolioDetailsVimeoThumb(id) {
    $.getJSON('http://www.vimeo.com/api/v2/video/' + id + '.json?callback=?', { format: "jsonp" }, function (data) {
        if (data != null && data[0] != null) {
            var id_img = "#editportfoliodetailsvimeo-" + data[0].id;
            $(id_img).attr('src', data[0].thumbnail_medium);
        }
        else {
            var id_img = "#editportfoliodetailsvimeo-" + id;
            $(id_img).attr('src', '/images/videothumbnail.gif');
        }
    });
}

function loadDeletePortfolioDetailsVimeoThumb(id) {
    $.getJSON('http://www.vimeo.com/api/v2/video/' + id + '.json?callback=?', { format: "jsonp" }, function (data) {
        if (data != null && data[0] != null) {
            var id_img = "#deleteportfoliodetailsvimeo-" + data[0].id;
            $(id_img).attr('src', data[0].thumbnail_medium);
        }
        else {
            var id_img = "#deleteportfoliodetailsvimeo-" + id;
            $(id_img).attr('src', '/images/videothumbnail.gif');
        }
    });
}

function showErrorNotification(text) {
    if (document.useJQueryNotify != 'undefined' && document.useJQueryNotify) {
        $.notify({
            inline: true,
            html: "<div style='clear:both;overflow:auto;'><div class='fancynotifyicon'><img src='/images/error.png' alt /></div><div class='fancynotifytext'><span style='font-size:18px;'>" + text + "</span></div></div>"
        }, 5000);
    }
    else {
        var html = "<div class='error' id='notifid" + notificationId + "'><span class='hpadtext'><span class='notificonalign'><img src='/images/error.png' alt /></span>" + text + "</span><span class='notifalignright'><a class='notifclosebtn' data-refid='" + notificationId + "'></a></span></div>";
        $('#notification').append(html);
        $('#scrollToTopLink').click();
        notificationId = notificationId + 1;
    }
}

function showSuccessNotification(text) {
    if (document.useJQueryNotify != 'undefined' && document.useJQueryNotify) {
        $.notify({
            inline: true,
            html: "<div style='clear:both;overflow:auto;'><div class='fancynotifyicon'><img src='/images/tick.png' alt /></div><div class='fancynotifytext'><span style='font-size:18px;'>" + text + "</span></div></div>"
        }, 5000);
    }
    else {
        var html = "<div class='success' id='notifid" + notificationId + "'><span class='hpadtext'><span class='notificonalign'><img src='/images/tick.png' alt /></span>" + text + "</span><span class='notifalignright'><a class='notifclosebtn' data-refid='" + notificationId + "'></a></span></div>";
        $('#notification').append(html);
        $('#scrollToTopLink').click();
        notificationId = notificationId + 1;
    }
}

$(document).ready(function () {
    $(".searchinputstyle").emptyText(globalsearchemptytext, "emptytextstyle");

    $('#scrollToTopLink').click(function (event) {
        event.preventDefault();
        var link = this;
        $.smoothScroll({
            scrollTarget: link.hash
        });
    });

    $('.searchinputstyle').focus(function () {
        $('#searchdisplay').attr('style', 'background:#fff;border: 1px solid #b9b9b9;color: #555;');
        $('.searchinputstyle').attr('style', 'background:#fff; border: none; color: #333;');
    });

    $('.searchinputstyle').blur(function () {
        $('#searchdisplay').attr('style', 'background:#fff;border: 1px solid #b9b9b9;color:#555');
        $('.searchinputstyle').attr('style', 'background:#fff; border: none; color: #555;');
    });

    $('#searchsiteinput').keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            window.location = '/s/' + encodeURIComponent($('#searchsiteinput').val());
        }
    });

    $('#searchsitebtn').click(function () {
        window.location = '/s/' + encodeURIComponent($('#searchsiteinput').val());
    });

    $("#loginbtn").fancybox({
        'modal': false,
        'padding': 0,
        'fitToView': false,
        'scrolling': 'no',
        'transitionIn': 'fade',
        'transitionOut': 'fade'
    });

    $('.cliplink').fancybox({
        'type': 'ajax',
        'padding': 0,
        'topRatio': 0,
        'nextEffect': 'none',
        'prevEffect': 'none',
        'fitToView': false,
        'scrolling': 'no'
    });

    $('.notifclosebtn').live('click', function () {
        var refid = $(this).attr("data-refid") + "";
        hide('#notifid' + refid);
        return false;
    });

    $('#cityddmenu').click(function () {
        if (!showingCitySearchDD) {
            show('#cityselectdropdown');
            $('#cityddmenu').attr('class', 'citysearchsel');
            showingCitySearchDD = true;
        }
        else {
            hide('#cityselectdropdown');
            $('#cityddmenu').attr('class', '');
            showingCitySearchDD = false;
        }
        return false;
    });

    $('#signindisplay').click(function () {
        if (!showingSignInDD) {
            show('#signindropdown');
            hide('#registerdropdown');
            hide('#usersessiondropdown');
            $('#signindisplay').attr('class', 'sessionsel');
            $('#joinowdisplay').attr('class', '');
            $('#userlogged').attr('class', '');
            showingSignInDD = true;
            showingRegisterDD = false;
            showingUserSessionDD = false;
        }
        else {
            hide('#signindropdown');
            $('#signindisplay').attr('class', '');
            showingSignInDD = false;
        }
        return false;
    });
    $('#joinowdisplay').click(function () {
        if (!showingRegisterDD) {
            show('#registerdropdown');
            hide('#signindropdown');
            hide('#usersessiondropdown');
            $('#signindisplay').attr('class', '');
            $('#joinowdisplay').attr('class', 'sessionsel');
            $('#userlogged').attr('class', '');
            showingRegisterDD = true;
            showingSignInDD = false;
            showingUserSessionDD = false;
        }
        else {
            hide('#registerdropdown');
            $('#joinowdisplay').attr('class', '');
            showingRegisterDD = false;
        }
        return false;
    });
    $('#userlogged').click(function () {
        if (!showingUserSessionDD) {
            show('#usersessiondropdown');
            hide('#registerdropdown');
            hide('#signindropdown');
            $('#signindisplay').attr('class', '');
            $('#joinowdisplay').attr('class', '');
            $('#userlogged').attr('class', 'sessionsel');
            showingUserSessionDD = true;
            showingRegisterDD = false;
            showingSignInDD = false;
        }
        else {
            hide('#usersessiondropdown');
            $('#userlogged').attr('class', '');
            showingUserSessionDD = false;
        }
        return false;
    });
    $(document).click(function (e) {

        if (!($(e.target).is('#signindropdown *') || $(e.target).is('#registerdropdown *') || $(e.target).is('#usersessiondropdown *'))) {
            hide('#usersessiondropdown');
            hide('#registerdropdown');
            hide('#signindropdown');
            $('#signindisplay').attr('class', '');
            $('#joinowdisplay').attr('class', '');
            $('#userlogged').attr('class', '');
            showingUserSessionDD = false;
            showingRegisterDD = false;
            showingSignInDD = false;
        }
    });
});
