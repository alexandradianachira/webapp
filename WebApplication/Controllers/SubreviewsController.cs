using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;

namespace WebApplication.Controllers
{
    public class SubreviewsController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: Subreviews
        public ActionResult Index()
        {
            return View(db.Subreviews.ToList());
        }

        // GET: Subreviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreview subreview = db.Subreviews.Find(id);
            if (subreview == null)
            {
                return HttpNotFound();
            }
            return View(subreview);
        }

        // GET: Subreviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subreviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_subreview,id_subreviewer,grade,confidence,comments,comment_to_edit,date_submitted")] Subreview subreview)
        {
            if (ModelState.IsValid)
            {
                db.Subreviews.Add(subreview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subreview);
        }

        // GET: Subreviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreview subreview = db.Subreviews.Find(id);
            if (subreview == null)
            {
                return HttpNotFound();
            }
            return View(subreview);
        }

        // POST: Subreviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subreview,id_subreviewer,grade,confidence,comments,comment_to_edit,date_submitted")] Subreview subreview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subreview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subreview);
        }

        // GET: Subreviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreview subreview = db.Subreviews.Find(id);
            if (subreview == null)
            {
                return HttpNotFound();
            }
            return View(subreview);
        }

        // POST: Subreviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subreview subreview = db.Subreviews.Find(id);
            db.Subreviews.Remove(subreview);
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
