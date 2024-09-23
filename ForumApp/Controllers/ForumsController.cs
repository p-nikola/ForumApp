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




    }
}