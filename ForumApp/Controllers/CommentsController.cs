using ForumApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

                return RedirectToAction("Details", "Posts", new { id = viewModel.PostId });
            }
            else
            {

                return RedirectToAction("Details", "Posts", new { id = viewModel.PostId });
            }
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
    }
}