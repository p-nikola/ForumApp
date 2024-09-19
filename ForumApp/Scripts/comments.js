// comments.js

// Define global functions for upvoting and downvoting
function upvoteComment(commentId) {
    console.log("Upvoting comment:", commentId);
    $.post('/Comments/UpvoteComment', { commentId: commentId }, function (data) {
        if (data.success) {
            $('#comment-upvotes-' + commentId).text(data.upvotes);
        }
    });
}

function downvoteComment(commentId) {
    console.log("Downvoting comment:", commentId);
    $.post('/Comments/DownvoteComment', { commentId: commentId }, function (data) {
        if (data.success) {
            $('#comment-downvotes-' + commentId).text(data.downvotes);
        }
    });
}

$(document).ready(function () {
    // Your existing reply form toggle code

    // Hide all reply form containers on page
    $('.reply-form-container').hide();

    // Toggle reply form visibility
    $(document).on('click', '.reply-button', function () {
        var target = $(this).data('target');
        $('#' + target).toggle();
    });

    // Cancel button functionality
    $(document).on('click', '.cancel-button', function () {
        var target = $(this).data('target');
        $('#' + target).hide();
    });
});
