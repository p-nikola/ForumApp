// comments.js

// Define global functions for upvoting and downvoting
function upvotePost(postId) {
    $.post(upvoteCommentUrl, { postId: postId }, function (data) {
        if (data.success) {
            $('#post-upvotes').text(data.upvotes);
            $('#post-downvotes').text(data.downvotes);

            var upvoteButton = $('#upvote-post-' + postId);
            var downvoteButton = $('#downvote-post-' + postId);

            // Toggle the selected-vote class
            if (upvoteButton.hasClass('selected-vote')) {
                // Remove upvote
                upvoteButton.removeClass('selected-vote');
            } else {
                // Add upvote and remove downvote if it exists
                upvoteButton.addClass('selected-vote');
                downvoteButton.removeClass('selected-vote');
            }
        } else {
            alert('An error occurred while upvoting the post.');
        }
    });
}


function downvotePost(postId) {
    $.post('@Url.Action("DownvotePost", "Posts")', { postId: postId }, function (data) {
        if (data.success) {
            $('#post-upvotes').text(data.upvotes);
            $('#post-downvotes').text(data.downvotes);

            var upvoteButton = $('#upvote-post-' + postId);
            var downvoteButton = $('#downvote-post-' + postId);

            if (downvoteButton.hasClass('selected-vote')) {
                // Remove downvote
                downvoteButton.removeClass('selected-vote');
            } else {
                // Add downvote and remove upvote if it exists
                downvoteButton.addClass('selected-vote');
                upvoteButton.removeClass('selected-vote');
            }
        } else {
            alert('An error occurred while downvoting the post.');
        }
    });
}


// Similarly update upvoteComment and downvoteComment functions
function upvoteComment(commentId) {
    $.post('@Url.Action("UpvoteComment", "Comments")', { commentId: commentId }, function (data) {
        if (data.success) {
            $('#comment-upvotes-' + commentId).text(data.upvotes);
            $('#comment-downvotes-' + commentId).text(data.downvotes);

            var upvoteButton = $('#upvote-comment-' + commentId);
            var downvoteButton = $('#downvote-comment-' + commentId);

            if (upvoteButton.hasClass('selected-vote')) {
                upvoteButton.removeClass('selected-vote');
            } else {
                upvoteButton.addClass('selected-vote');
                downvoteButton.removeClass('selected-vote');
            }
        } else {
            alert('An error occurred while upvoting the comment.');
        }
    });
}

function downvoteComment(commentId) {
    $.post('@Url.Action("DownvoteComment", "Comments")', { commentId: commentId }, function (data) {
        if (data.success) {
            $('#comment-upvotes-' + commentId).text(data.upvotes);
            $('#comment-downvotes-' + commentId).text(data.downvotes);

            var upvoteButton = $('#upvote-comment-' + commentId);
            var downvoteButton = $('#downvote-comment-' + commentId);

            if (downvoteButton.hasClass('selected-vote')) {
                downvoteButton.removeClass('selected-vote');
            } else {
                downvoteButton.addClass('selected-vote');
                upvoteButton.removeClass('selected-vote');
            }
        } else {
            alert('An error occurred while downvoting the comment.');
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
