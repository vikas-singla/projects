(function ($) {
    $.fn.emptyText = function (txt) {
        var defClass = arguments[1];
        if ($(this).val() == null || $.trim($(this).val()) == "") {
            if (defClass) $(this).addClass(defClass);
            $(this).val(txt);
        }
        $(this).attr("emptytext", txt);
        $(this).focus(function () {
            if ($(this).val() == txt) {
                if (defClass) $(this).removeClass(defClass);
                $(this).val("")
            }
        });
        $(this).blur(function () {
            if ($(this).val() == null || $.trim($(this).val()) == "") {
                if (defClass) $(this).addClass(defClass);
                $(this).val(txt)
            }
        })
    }
})(jQuery);

function clearDefaultIfEmpty(controlId, txt) {
    var element = $('#' + controlId);
    if (element.val() == txt) {
        element.val("");
        element.removeClass("emptytextstyle");
    }
}