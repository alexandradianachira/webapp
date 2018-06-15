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
    public class SubreviewersController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: Subreviewers
        public ActionResult Index()
        {
            return View(db.Subreviewers.ToList());
        }

        // GET: Subreviewers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreviewer subreviewer = db.Subreviewers.Find(id);
            if (subreviewer == null)
            {
                return HttpNotFound();
            }
            return View(subreviewer);
        }

        // GET: Subreviewers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subreviewers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_subreviewer,id_paper_assignment,id_user,invitation_send_date,invitation_ack,is_accepted")] Subreviewer subreviewer)
        {
            if (ModelState.IsValid)
            {
                db.Subreviewers.Add(subreviewer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subreviewer);
        }

        // GET: Subreviewers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreviewer subreviewer = db.Subreviewers.Find(id);
            if (subreviewer == null)
            {
                return HttpNotFound();
            }
            return View(subreviewer);
        }

        // POST: Subreviewers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subreviewer,id_paper_assignment,id_user,invitation_send_date,invitation_ack,is_accepted")] Subreviewer subreviewer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subreviewer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subreviewer);
        }

        // GET: Subreviewers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreviewer subreviewer = db.Subreviewers.Find(id);
            if (subreviewer == null)
            {
                return HttpNotFound();
            }
            return View(subreviewer);
        }

        // POST: Subreviewers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subreviewer subreviewer = db.Subreviewers.Find(id);
            db.Subreviewers.Remove(subreviewer);
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
