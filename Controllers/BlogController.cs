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
        private readonly BlogDbContext _db = new BlogDbContext();

        public ActionResult Index()
        {
            ViewBag.authorList = new SelectList(_db.Posts.Select(x => x.ApplicationUser.Name).Distinct().OrderBy(x => x)).Items;
            return View(_db.Posts.Include(c => c.Comments).ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Management()
        {
            return View(_db.Posts.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = _db.Posts.Single(m => m.PostId == id);

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
                post.ApplicationUserId = Int32.Parse(Membership.GetUser().ProviderUserKey.ToString());
                post.PublishDate = DateTime.Now;
                _db.Posts.Add(post);
                _db.SaveChanges();
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
            Post post = _db.Posts.Single(m => m.PostId == id);
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
                post.PublishDate = DateTime.Now;
                _db.Posts.AddOrUpdate(post);
                _db.SaveChanges();
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
                _db.Comments.AddOrUpdate(comment);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["PostId"] = new SelectList(_db.Posts, "PostId", "Post", comment.PostId);
            return View(comment);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _db.Posts.Single(p => p.PostId == id);
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
            Post post = _db.Posts.Single(m => m.PostId == id);
            _db.Posts.Remove(post);
            _db.SaveChanges();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddComment()
        {
            ViewData["PostId"] = new SelectList(_db.Posts, "PostId", "Post");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _db.Comments.Add(comment);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["PostId"] = new SelectList(_db.Posts, "PostId", "Post", comment.PostId);
            return RedirectToAction("Index");
        }

        [ActionName("DeleteComment")]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Comment comment = _db.Comments.Single(m => m.CommentId == id);
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
            Comment comment = _db.Comments.Single(m => m.CommentId == id);
            int temp = comment.PostId;
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return RedirectToAction("CommentDetails", new {id = temp});
        }

        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var comments = _db.Comments.Where(m => m.PostId == id).ToList();

            if (comments.Count == 0)
            {
                return HttpNotFound();
            }

            return View(comments);
        }

        public ActionResult Search(string title, string author, string content)
        {
            List<Post> posts = _db.Posts.Where(c =>
                !(title == null || title.Trim() == string.Empty) && c.Title.Contains(title)).ToList();
            
            posts.AddRange(_db.Posts.Where(c =>
                !(author == null || author.Trim() == string.Empty) && c.ApplicationUser.Name.Equals(author)).ToList());

            posts.AddRange(_db.Posts.Where(c =>
                !(content == null || content.Trim() == string.Empty) && c.Content.Contains(content)).ToList());

            return View(posts.Distinct().ToList());
        }
    }
}