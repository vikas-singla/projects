var newwallpostemptytext = "Type here to share your opinion, start a discussion, or write something interesting.";
$(document).ready(function () {
    $(".newwallpost #newwallpostinput").emptyText(newwallpostemptytext, "emptytextstyle");

    $('.newwallpost #newwallpostinput').focus(function () {
        $('.newwallpost .postbtns').show();
    });

    $('.wallpostcomment .postcommentcontent .newpostcommentinput').live('keydown', function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var postId = $(this).attr("data-post-id") + "";
        var commentText = $(this).val();

        switch (event.keyCode) {
            case 13:
                addUserWallPostComment(postId, commentText);
                break;
            default:
                break;
        }
    });

    $('.newwallpost .postbtns #addnewuserpostbtn').click(function () {
        if (!loggedin) {
            $('#loginbtn').click();
            return false;
        }

        var pageId = $(this).attr("data-page-id");
        addUserWallPost(pageId);
        return false;
    });

    $('.wallpost .showpostcommentsbtn').click(function () {
        var commentsBlock = $(this).attr("data-post-comments");
        $("." + commentsBlock).toggle();
        return false;
    });
});

function addUserWallPost(pageId) {
    var postText = $('.newwallpost #newwallpostinput').val();

    $.ajax(
    {
        type: "POST",
        url: "/Activity/AddWallTextPost",
        data: {
            postText: postText,
            pageId: pageId
        },
        success: function (result) {
            showSuccessNotification('Post has been published.');
            $('#sl_discussionsect').load("/activity/GetBookmarkletUserWallPosts?pageId=" + pageId);
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Post could not be published. Please try again later.');
        }
    });
}

function addUserWallPostComment(postId, commentText) {
    $.ajax(
    {
        type: "POST",
        url: "/Activity/AddWallTextPostComment",
        data: {
            wallPostId: postId,
            commentText: commentText
        },
        success: function (result) {
            showSuccessNotification('Post comment has been published.');
            $('.pcb' + postId).load("/Activity/GetUserWallPostComments?wallPostId=" + postId, function () {
                $('.wallpostcomment .postcommentcontent .newpostcommentinput').elastic();
            });
        },
        error: function (req, status, error) {
            showErrorNotification('Error: Post comment could not be published. Please try again later.');
        }
    });
}