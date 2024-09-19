using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ForumApp.Models;
using Microsoft.AspNet.Identity;

namespace ForumApp.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Include(u => u.Posts)
                       .Include(u => u.Comments)
                       .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Calculate total upvotes and downvotes from posts
            var totalPostUpvotes = user.Posts.Sum(p => p.Upvotes);
            var totalPostDownvotes = user.Posts.Sum(p => p.Downvotes);

            // Calculate total upvotes and downvotes from comments
            var totalCommentUpvotes = user.Comments.Sum(c => c.Upvotes);
            var totalCommentDownvotes = user.Comments.Sum(c => c.Downvotes);

            // Total upvotes and downvotes
            user.TotalUpvotes = totalPostUpvotes + totalCommentUpvotes;
            user.TotalDownvotes = totalPostDownvotes + totalCommentDownvotes;

            return View(user);
        }


        // GET: Profile/Edit
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    //user.UserName = model.UserName;
                    user.ProfilePictureUrl = model.ProfilePictureUrl;

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index",new {id=userId});
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult UserPosts(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Include(u => u.Posts).FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserName = user.UserName;

            return View(user.Posts); // Return the list of posts to the view
        }

        [HttpGet]
        public ActionResult UserComments(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Include(u => u.Comments.Select(c => c.Post)).FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserName = user.UserName;


            return View(user.Comments); // Return the list of comments to the view
        }

    }
}
