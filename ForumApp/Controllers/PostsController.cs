using ForumApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumApp.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts/Details/5
        public ActionResult Details(int id)
        {
            var post = db.Posts.Include("Comments").FirstOrDefault(p => p.PostId == id);
            post.Content = string.Empty;
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create(int forumId)
        {
            var forum = db.Forums.Find(forumId);
            if (forum == null)
            {
                return HttpNotFound();
            }

            ViewBag.ForumId = forumId;
            ViewBag.ForumTitle = forum.Title;
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.DateCreated = DateTime.Now;
                post.UserId = User.Identity.GetUserId();
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Details", "Forums", new { id = post.ForumId });
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult UpvotePost(int postId)
        {
            var post = db.Posts.Find(postId);
            if (post != null)
            {
                post.Upvotes++;
                db.SaveChanges();
                return Json(new { success = true, upvotes = post.Upvotes });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DownvotePost(int postId)
        {
            var post = db.Posts.Find(postId);
            if (post != null)
            {
                post.Downvotes++;
                db.SaveChanges();
                return Json(new { success = true, downvotes = post.Downvotes });
            }
            return Json(new { success = false });
        }

    }
}