var newclippostemptytext = "Type here to write a comment";
$(document).ready(function () {
    $('textarea').elastic();

    $('#cliploginbtn').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
        }
        return false;
    });

    $('.viewclip .userwall #clipcommentpostinput').live('keydown', function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        switch (event.keyCode) {
            case 13:
                var clipId = $(this).attr("data-clip-id") + "";
                addClipComment(clipId);
                break;
            default:
                break;
        }
    });

    $('.viewclip .clipbtns .likeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var clipId = $(this).attr("data-clip-id") + "";
        likeClip(this, clipId);
    });

    $('.viewclip .clipbtns .unlikeaction').live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var clipId = $(this).attr("data-clip-id") + "";
        unlikeClip(this, clipId);
    });

    $(".viewclip .shareclipaction").live("click", function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }
    });

    if (loggedin) {
        $(".viewclip .shareclipaction").fancybox({
            'modal': false,
            'type': 'ajax',
            'padding': 0,
            'fitToView': false,
            'scrolling': 'no',
            'transitionIn': 'fade',
            'transitionOut': 'fade'
        });
    }

    $('.viewclip .userwall #clipcommentpostinput').emptyText(newclippostemptytext, "emptytextstyle");
});

function addClipComment(clipId) {
    var commentText = $('.userwall #clipcommentpostinput').val();

    $.ajax(
    {
        type: "POST",
        url: "/clip/AddClipComment",
        data: {
            commentText: commentText,
            clipId: clipId
        },
        success: function (result) {
            $('.viewclip #clipCommentsPostContainer').load("/clip/GetClipComments?clipId=" + clipId, function () {
                $('.viewclip .userwall #clipcommentpostinput').emptyText(newclippostemptytext, "emptytextstyle");            
            });
        },
        error: function (req, status, error) {
            $.fancybox.close();
            showErrorNotification('Error: Comment could not be published. Please try again later.');
        }
    });
}

function likeClip(linkRef, clipId) {
    $.ajax(
    {
        type: "POST",
        url: "/clip/LikeClip",
        data: {
            clipId: clipId
        },
        success: function (result) {
            showSuccessNotification('Your like for the clip has been saved.');

            $(linkRef).attr("class", "unlikeaction btnundo");
            $(linkRef).html("<img class='actionbtnimg' src='/images/heart_filled_sm.png' height='10px' alt />Liked");
            $('.viewclip #cliplikescontainer').load("/clip/GetClipLikes?clipId=" + clipId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to save your like. Please try again later.');
        }
    });
}

function unlikeClip(linkRef, clipId) {
    $.ajax(
    {
        type: "POST",
        url: "/clip/UnikeClip",
        data: {
            clipId: clipId
        },
        success: function (result) {
            showSuccessNotification('Your like for the clip has been removed.');

            $(linkRef).attr("class", "likeaction btnstyle");
            $(linkRef).html("<img class='actionbtnimg' src='/images/heart_empty_sm.png' height='10px' alt />Like");
            $('.viewclip #cliplikescontainer').load("/clip/GetClipLikes?clipId=" + clipId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Error occured trying to save your like. Please try again later.');
        }
    });
}