using ForumApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;

namespace ForumApp.Controllers
{
    [Authorize(Roles = "Admin,Moderator,User")]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts/Details/5
        // PostsController.cs - In the Details action
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var post = db.Posts
                .Include(p => p.Votes)
                .Include(p => p.Comments.Select(c => c.Votes))
                .FirstOrDefault(p => p.PostId == id);
            post.Content = String.Empty;

            if (post == null)
            {
                return HttpNotFound();
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(post.UserId);
            ViewBag.UserRole = roles.FirstOrDefault() ?? "User"; // Assign a default role if none is found

            var userId = User.Identity.GetUserId();
            ViewBag.UserPostVoteType = post.Votes.FirstOrDefault(v => v.UserId == userId)?.VoteType ?? 0;

            // Pass similar information for comments if needed

            //can you make a breadcrumb here?
            var breadcrumb= new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Forums", Url = Url.Action("Index","Forums") },
                new BreadcrumbItem { Title = post.Forum.Title, Url = Url.Action("Details", "Forums", new { id = post.ForumId }) },
                new BreadcrumbItem { Title = post.Title, IsActive=true}
            };

            ViewBag.Breadcrumbs = breadcrumb;    

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

                if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
                {
                    post.IsApproved = true;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Forums", new { id = post.ForumId}); 
                }
                post.IsApproved = false; // Post is not approved by default
                db.Posts.Add(post);
                db.SaveChanges();

                return RedirectToAction("Details", "Forums", new { id = post.ForumId, postSubmitted = true }); // Redirect to a confirmation page
            }
            ViewBag.ForumId = post.ForumId; // proveri ova zasto se prakja

            return View(post);
        }

        /* [HttpPost]
         public ActionResult Up



         Post(int postId)
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
 */
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Details", "Forums", new { id = post.ForumId });
        }


        [Authorize(Roles = "Admin")]
        public ActionResult PostPendingApproval()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult PendingApproval()
        {
            var pendingPosts = db.Posts.Where(p => !p.IsApproved).ToList();
            return View(pendingPosts);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int id)
        {
            var post = db.Posts.Find(id);
            if (post != null)
            {
                post.IsApproved = true;
                db.SaveChanges();
            }

            return RedirectToAction("PendingApproval"); // Redirect back to the pending posts list
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Disapprove(int id)
        {
            var post = db.Posts.Find(id);
            if (post != null)
            {
                db.Posts.Remove(post); // Remove the post from the database
                db.SaveChanges();
            }

            return RedirectToAction("PendingApproval"); // Redirect back to the pending posts list
        }



        [HttpPost]
        [Authorize]
        public ActionResult UpvotePost(int postId)
        {
            var userId = User.Identity.GetUserId();
            var existingVote = db.PostVotes.FirstOrDefault(v => v.PostId == postId && v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == 1)
                {
                    // User clicked upvote again, remove the vote
                    db.PostVotes.Remove(existingVote);
                }
                else
                {
                    // Change vote from downvote to upvote
                    existingVote.VoteType = 1;
                    existingVote.DateVoted = DateTime.Now;
                    db.Entry(existingVote).State = EntityState.Modified;
                }
            }
            else
            {
                // Add new upvote
                var vote = new PostVote
                {
                    PostId = postId,
                    UserId = userId,
                    VoteType = 1,
                    DateVoted = DateTime.Now
                };
                db.PostVotes.Add(vote);
            }

            db.SaveChanges();

            // Recalculate vote counts
            var post = db.Posts.Include(p => p.Votes).FirstOrDefault(p => p.PostId == postId);
            return Json(new { success = true, upvotes = post.Upvotes, downvotes = post.Downvotes });
        }


        [HttpPost]
        [Authorize]
        public ActionResult DownvotePost(int postId)
        {
            var userId = User.Identity.GetUserId();
            var existingVote = db.PostVotes.FirstOrDefault(v => v.PostId == postId && v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == -1)
                {
                    // User clicked downvote again, remove the vote
                    db.PostVotes.Remove(existingVote);
                }
                else
                {
                    // Change vote from upvote to downvote
                    existingVote.VoteType = -1;
                    existingVote.DateVoted = DateTime.Now;
                    db.Entry(existingVote).State = EntityState.Modified;
                }
            }
            else
            {
                // Add new downvote
                var vote = new PostVote
                {
                    PostId = postId,
                    UserId = userId,
                    VoteType = -1,
                    DateVoted = DateTime.Now
                };
                db.PostVotes.Add(vote);
            }

            db.SaveChanges();

            // Recalculate vote counts
            var post = db.Posts.Include(p => p.Votes).FirstOrDefault(p => p.PostId == postId);
            return Json(new { success = true, upvotes = post.Upvotes, downvotes = post.Downvotes });
        }






    }
}