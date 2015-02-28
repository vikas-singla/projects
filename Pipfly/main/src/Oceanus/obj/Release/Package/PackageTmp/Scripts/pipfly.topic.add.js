$(document).ready(function () {
    $('#posttypemenu a').live('click', function () {
        $('#posttypemenu a').each(function () {
            $(this).attr("class", "posttypemenuitem");
            var relToHide = $(this).attr("rel") + "";
            hide(relToHide);
        });

        var rel = $(this).attr("rel") + "";
        $(this).attr("class", "posttypemenuitem selposttypemenuitem");
        show(rel);

        return false;
    });
});