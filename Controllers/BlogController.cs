using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using ShauliProject.Models;

namespace ShauliProject.Controllers
{
    public class BlogController : Controller
    {
        public static ApplicationDbContext getDbContext()
        {
            return new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            ApplicationDbContext db = getDbContext();
            ViewBag.authorList = new SelectList(db.Users.Select(x => x.Name).Distinct().OrderBy(x => x)).Items;

            List<PostUserViewModel> list = (from u in db.Users join p in db.Posts
                                           on u.Id equals p.UserId
                                           select new PostUserViewModel { post = p, user = u}).ToList();
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Management()
        {
            ApplicationDbContext db = getDbContext();
            List<PostUserViewModel> list = (from u in db.Users
                                            join p in db.Posts
                                            on u.Id equals p.UserId
                                            select new PostUserViewModel { post = p, user = u }).ToList();
            return View(list);
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = getDbContext().Posts.Single(m => m.PostId == id);
            ViewBag.name = getDbContext().Users.Where(i => i.Id.Equals(id)).Select(i => new {i.Name}).Single();

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            string key = HttpContext.User.Identity.Name;
            ViewBag.name = getDbContext().Users.Where(i => i.UserName.Equals(key)).Select(i => new { i.Name });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = getDbContext();
                string currentUserName = HttpContext.User.Identity.Name;
                post.UserId = (from i in db.Users where i.UserName.Equals(currentUserName) select i.Id).Single();
                post.PublishDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                System.Threading.Thread.Sleep(2000);
                return RedirectToAction("Management");
            }

            return View(post);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = getDbContext().Posts.Single(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = getDbContext();
                post.PublishDate = DateTime.Now;
                db.Posts.AddOrUpdate(post);
                db.SaveChanges();
                return RedirectToAction("Management");
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment)
        {
            ApplicationDbContext db = getDbContext();

            if (ModelState.IsValid)
            {
                db.Comments.AddOrUpdate(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["PostId"] = new SelectList(db.Posts, "PostId", "Post", comment.PostId);
            return View(comment);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = getDbContext().Posts.Single(p => p.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationDbContext db = getDbContext();
            Post post = db.Posts.Single(m => m.PostId == id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                getDbContext().Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddComment()
        {
            ViewData["PostId"] = new SelectList(getDbContext().Posts, "PostId", "Post");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            ApplicationDbContext db = getDbContext();
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["PostId"] = new SelectList(db.Posts, "PostId", "Post", comment.PostId);
            return RedirectToAction("Index");
        }

        [ActionName("DeleteComment")]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Comment comment = getDbContext().Comments.Single(m => m.CommentId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            ApplicationDbContext db = getDbContext();
            Comment comment = db.Comments.Single(m => m.CommentId == id);
            int temp = comment.PostId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("CommentDetails", new {id = temp});
        }

        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var comments = getDbContext().Comments.Where(m => m.PostId == id).ToList();

            if (comments.Count == 0)
            {
                return HttpNotFound();
            }

            return View(comments);
        }

        public ActionResult Search(string title, string author, string content)
        {
            ApplicationDbContext db = getDbContext();
            List<Post> posts = db.Posts.Where(c =>
                !(title == null || title.Trim() == string.Empty) && c.Title.Contains(title)).ToList();

            if (author == null || author.Trim() == string.Empty)
            {
                posts.AddRange(from p in db.Posts
                    join u in db.Users on p.UserId equals u.Id
                    where u.Name.Equals(author)
                    select p);
            }

            posts.AddRange(db.Posts.Where(c =>
                !(content == null || content.Trim() == string.Empty) && c.Content.Contains(content)).ToList());

            List<Post> list = posts.Distinct().ToList();
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (Post p in list)
            {
                if (!users.Exists(i => i.Id.Equals(p.UserId)))
                {
                    users.Add(db.Users.Where(i => i.Id.Equals(p.UserId)).Single());
                }
            }

            return View(list);

        }
    }
}