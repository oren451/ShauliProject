using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ShauliProject.Models;

namespace ShauliProject.Controllers
{
    public class FanClubsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: FanClubs
        public ActionResult Index()
        {
            return View(_db.Users.Where(m => m.Address != null).ToList());
        }

        // GET: FanClubs/Details/5
        public ActionResult Details(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser fanClub = _db.Users.Find(id);

            if (fanClub == null)
            {
                return HttpNotFound();
            }

            return View(fanClub);
        }

        // GET: FanClubs/Create
        public ActionResult Create()
        {
            ApplicationUser user = _db.Users.Where(m => m.UserName.Equals(HttpContext.User.Identity.Name)).Single();
            ViewBag.isFan = user.Seniority != 0 ? true : false;
            return View();
        }

        // POST: FanClubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string dateOfBirth, int seniority, string address)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user = _db.Users.Where(m => m.UserName.Equals(HttpContext.User.Identity.Name)).Single();
                user.DateOfBirth = dateOfBirth;
                user.Seniority = seniority;
                user.Address = address;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: FanClubs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser fanClub = _db.Users.Find(id);
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
        public ActionResult Edit(string dateOfBirth, int seniority, string address)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user = _db.Users.Where(i => i.UserName.Equals(HttpContext.User.Identity.Name)).Single();
                user.DateOfBirth = dateOfBirth;
                user.Seniority = seniority;
                user.Address = address;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: FanClubs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser fanClub = _db.Users.Find(id);
            if (fanClub == null)
            {
                return HttpNotFound();
            }
            return View(fanClub);
        }

        // POST: FanClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = _db.Users.Find(id);
            user.DateOfBirth = null;
            user.Seniority = 0;
            user.Address = null;
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
