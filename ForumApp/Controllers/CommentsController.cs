using ForumApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumApp.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments/Create
        public ActionResult Create(int postId, int? parentCommentId = null)
        {
            var viewModel = new CommentViewModel
            {
                PostId = postId,
                ParentCommentId = parentCommentId
            };
            return View(viewModel);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = viewModel.Content,
                    DateCreated = DateTime.Now,
                    PostId = viewModel.PostId,
                    ParentCommentId = viewModel.ParentCommentId,
                    UserId = User.Identity.GetUserId()
                };

                db.Comments.Add(comment);
                db.SaveChanges();

                // Pass an empty view model to prevent form from being filled with previous data
                var emptyViewModel = new CommentViewModel { PostId = viewModel.PostId };

                if (Request.IsAjaxRequest())
                {
                    // Fetch updated comments list
                    var comments = db.Comments
                                     .Where(c => c.PostId == viewModel.PostId && c.ParentCommentId == null)
                                     .Include(c => c.Replies.Select(r => r.User))
                                     .Include(c => c.User)
                                     .ToList();

                   

                    // Return the updated comments section with a fresh view model
                    return PartialView("_CommentList", comments);
                }

                return RedirectToAction("Details", "Posts", new { id = viewModel.PostId });
            }


            // Handle validation errors
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ReplyForm", viewModel);
            }

            return View(viewModel);
        }




        [HttpPost]
        public ActionResult UpvoteComment(int commentId)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null)
            {
                comment.Upvotes++;
                db.SaveChanges();
                return Json(new { success = true, upvotes = comment.Upvotes });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DownvoteComment(int commentId)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null)
            {
                comment.Downvotes++;
                db.SaveChanges();
                return Json(new { success = true, downvotes = comment.Downvotes });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            var comment = db.Comments.Include(c => c.Replies).FirstOrDefault(c => c.CommentId == commentId);
            if (comment != null)
            {
                DeleteNestedComments(comment);
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Comment not found" });
        }

        private void DeleteNestedComments(Comment parentComment)
        {
            // Recursively delete all nested comments (replies)
            foreach (var reply in parentComment.Replies.ToList())  // Use .ToList() to avoid modification during iteration
            {
                DeleteNestedComments(reply);
                db.Comments.Remove(reply);
            }
        }


        [HttpPost]
        public ActionResult SoftDeleteComment(int commentId)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null && comment.UserId == User.Identity.GetUserId()) // Ensure the user is only deleting their own comments
            {
                comment.IsSoftDeleted = true;
                db.SaveChanges();
                return Json(new { success = true, message = "Comment successfully deleted." });
            }
            return Json(new { success = false, message = "Comment not found or unauthorized action." });
        }




    }
}