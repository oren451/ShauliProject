using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShauliProject.Models;

namespace ShauliProject.Controllers
{
    public class FanClubsController : Controller
    {
        private BlogDbContext db = new BlogDbContext();

        // GET: FanClubs
        public ActionResult Index()
        {
            return View(db.Fan.ToList());
        }

        // GET: FanClubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FanClub fanClub = db.Fan.Find(id);
            if (fanClub == null)
            {
                return HttpNotFound();
            }
            return View(fanClub);
        }

        // GET: FanClubs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FanClubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,Seniority,Address")] FanClub fanClub)
        {
            if (ModelState.IsValid)
            {
                db.Fan.Add(fanClub);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fanClub);
        }

        // GET: FanClubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FanClub fanClub = db.Fan.Find(id);
            if (fanClub == null)
            {
                return HttpNotFound();
            }
            return View(fanClub);
        }

        // POST: FanClubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,Seniority,Address")] FanClub fanClub)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fanClub).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fanClub);
        }

        // GET: FanClubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FanClub fanClub = db.Fan.Find(id);
            if (fanClub == null)
            {
                return HttpNotFound();
            }
            return View(fanClub);
        }

        // POST: FanClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FanClub fanClub = db.Fan.Find(id);
            db.Fan.Remove(fanClub);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
