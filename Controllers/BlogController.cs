using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShauliProject.Models;

namespace ShauliProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext db = new BlogDbContext();

        public ActionResult Index()
        {
            return View(db.Posts.Include(c => c.Comments).ToList());
        }

        public ActionResult Management()
        {
            return View(db.Posts.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Single(m => m.Id == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PublishDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
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
            Post post = db.Posts.Single(m => m.Id == id);
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
            Post post = db.Posts.Single(p => p.Id == id);
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
            Post post = db.Posts.Single(m => m.Id == id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddComment()
        {
            ViewData["PostId"] = new SelectList(db.Posts, "PostId", "Post");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
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

            Comment comment = db.Comments.Single(m => m.CommentId == id);
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
            Comment comment = db.Comments.Single(m => m.CommentId == id);
            int temp = comment.PostId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("CommentDetails", new { id = temp });
        }

        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

    
            var comments = db.Comments.Where(m => m.PostId == id).ToList();
            if (comments == null)
            {
                return HttpNotFound();
            }

            return View(comments);
        }
    }
}