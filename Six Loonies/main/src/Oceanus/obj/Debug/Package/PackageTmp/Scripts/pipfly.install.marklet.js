$(document).ready(function () {
    var userAgent = navigator.userAgent.toLowerCase();

    // Figure out what browser is being used
    jQuery.browser = {
        version: (userAgent.match(/.+(?:rv|it|ra|ie|me)[\/: ]([\d.]+)/) || [])[1],
        chrome: /chrome/.test(userAgent),
        safari: /webkit/.test(userAgent) && !/chrome/.test(userAgent),
        opera: /opera/.test(userAgent),
        msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
        mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent)
    };

    if ($.browser.chrome) {
        $('.buttoncontainer #chrome').show();
    }
    else if ($.browser.safari) {
        $('.buttoncontainer #safari').show();
    }
    else if ($.browser.mozilla) {
        $('.buttoncontainer #firefox').show();
    }
    else if ($.browser.msie) {
        if ($.browser.version >= 9) {
            $('.buttoncontainer #ie9').show();
        }
        else if ($.browser.version >= 8 && $.browser.version < 9) {
            $('.buttoncontainer #ie7').show();
        }
        else {
            $('.buttoncontainer #notsupported').show();
            $('.buttoncontainer .btnshowcasecontainer').hide();
        }
    }
});