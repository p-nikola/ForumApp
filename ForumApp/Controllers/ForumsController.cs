using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumApp.Controllers
{

    public class ForumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Forums
        public ActionResult Index()
        {
            var breadcrum = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Forums", IsActive = true }
            };

            ViewBag.Breadcrumbs = breadcrum;

            return View(db.Forums.ToList());
        }

        // GET: Forums/Details/5
        public ActionResult Details(int id)
        {
            var forum = db.Forums.Include("Posts").FirstOrDefault(f => f.ForumId == id);
            if (forum == null)
            {
                return HttpNotFound();
            }

            var breadcrumb = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Forums", Url = Url.Action("Index") },
                new BreadcrumbItem { Title = forum.Title, Url = Url.Action("Details", "Forums", new { id = forum.ForumId }), IsActive = true }
            };

            ViewBag.Breadcrumbs = breadcrumb;

            return View(forum);
        }

        // GET: Forums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Forum forum)
        {
            if (ModelState.IsValid)
            {
                db.Forums.Add(forum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forum);
        }


        public ActionResult DeleteForum(int id)
        {
            Forum forum = db.Forums.Find(id);
            db.Forums.Remove(forum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Forums/GetPosts")]
        public ActionResult GetPosts(int draw, int start, int length, string search, string sortColumn, string sortDirection, int forumId)
        {
            // Filter the posts to only those from the current forum
            var query = db.Posts.Where(p => p.ForumId == forumId && p.IsApproved);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
            }

            var orderColumnIndex = Request["order[0][column]"];  // Get the column index to sort by
            var orderDirection = Request["order[0][dir]"];       // Get the sort direction (asc or desc)

            // Map the column index to the actual database column
            switch (orderColumnIndex)
            {
                case "0":
                    sortColumn = "Title";
                    break;
                case "1":
                    sortColumn = "Content";
                    break;
                case "2":
                    sortColumn = "DateCreated";
                    break;
                case "3":
                    sortColumn = "CommentsCount";
                    break;
                default:
                    sortColumn = "PostId";  // Default sorting
                    break;
            }

            // Total records before filtering
            var totalRecords = query.Count();

            // Apply sorting logic based on the DataTables request
            switch (sortColumn)
            {
                case "Title":
                    query = orderDirection == "asc" ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title);
                    break;
                case "DateCreated":
                    query = orderDirection == "asc" ? query.OrderBy(p => p.DateCreated) : query.OrderByDescending(p => p.DateCreated);
                    break;
                case "CommentsCount":
                    query = orderDirection == "asc" ? query.OrderBy(p => p.Comments.Count) : query.OrderByDescending(p => p.Comments.Count);
                    break;
                default:
                    query = query.OrderBy(p => p.PostId); // Default sorting
                    break;
            }

            // Pagination: Skip the first "start" records and take "length" number of records
            var posts = query.Skip(start).Take(length).ToList();

            // Prepare data for DataTables
            var data = posts.Select(p => new
            {
                p.PostId,
                p.Title,
                p.Content,
                DateCreated = p.DateCreated.ToString("MMMM dd, yyyy"),
                CommentsCount = p.Comments.Count,
                User = new
                {
                    p.UserId, // Include the UserId here for proper linking
                    p.User.UserName,
                    p.User.ProfilePictureUrl
                }
            }).ToList();

            // Return result as JSON
            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,  // Change if filtering is applied
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetForums(int draw, int start, int length)
        {
            // Retrieve search value from the request
            string searchValue = Request["search[value]"];

            // Fetch all forums from the database
            var query = db.Forums.AsQueryable();

            // Apply search filter if necessary
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.Title.Contains(searchValue) || f.Description.Contains(searchValue));
            }

            // Sorting (DataTables uses column index for sorting)
            var sortColumnIndex = Request["order[0][column]"];
            var sortDirection = Request["order[0][dir]"];

            switch (sortColumnIndex)
            {
                case "0":
                    query = sortDirection == "asc" ? query.OrderBy(f => f.Title) : query.OrderByDescending(f => f.Title);
                    break;
                case "1":
                    query = sortDirection == "asc" ? query.OrderBy(f => f.Description) : query.OrderByDescending(f => f.Description);
                    break;
                case "2":
                    query = sortDirection == "asc" ? query.OrderBy(f => f.Posts.Count(p => p.IsApproved)) : query.OrderByDescending(f => f.Posts.Count(p => p.IsApproved));
                    break;
                default:
                    query = query.OrderBy(f => f.ForumId); // Default sort
                    break;
            }

            // Get total records count before filtering
            var totalRecords = query.Count();

            // Pagination
            var forums = query.Skip(start).Take(length).ToList();

            // Prepare data for DataTables
            var data = forums.Select(f => new
            {
                f.ForumId,
                f.Title,
                f.Description,
                PostsCount = f.Posts.Count(p => p.IsApproved)
            }).ToList();

            // Return result as JSON in DataTables format
            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords, // Total number of records before filtering
                recordsFiltered = totalRecords, // Total number of records after filtering
                data = data
            }, JsonRequestBehavior.AllowGet);
        }


    }



}