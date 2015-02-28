function AddToList(list, item) {
    if (list == null) {
        list = new Array();
    }
    if (item != null) {
        item = $.trim(item);
        if ($.inArray(item, list) == -1) {
            list.push(item);
        }
    }
}

String.prototype.format = function () {
    var formatted = this;
    for (arg in arguments) {
        formatted = formatted.replace("{" + arg + "}", arguments[arg]);
    }
    return formatted;
};

function DisableEmptyFormFieldsForSubmission(form) {
    // disable all empty parameters and page parameter
    form.find(':input[value=""]').attr('disabled', true);
}

(function ($) {
    $.fn.serializeJSON = function () {
        var json = {};
        jQuery.map($(this).serializeArray(), function (n, i) {
            json[n['name']] = n['value'];
        });
        return json;
    };
})(jQuery);

function show(control)
{
    $(control).show(); 
}

function hide(control)
{
    $(control).hide();
}

function isIE()
{
    return $.browser.msie;
}