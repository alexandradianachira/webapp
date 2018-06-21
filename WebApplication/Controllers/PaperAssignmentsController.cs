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
    public class PaperAssignmentsController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: PaperAssignments
        public ActionResult Index()
        {
            return View(db.PaperAssignments.ToList());
        }

        public ActionResult Assign()
        {

            return View();
        }

        // GET: PaperAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaperAssignment paperAssignment = db.PaperAssignments.Find(id);
            if (paperAssignment == null)
            {
                return HttpNotFound();
            }
            return View(paperAssignment);
        }

        // GET: PaperAssignments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaperAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_paper_assignment,id_paper,id_pcmember,date_assigned,date_due,is_delegated")] PaperAssignment paperAssignment)
        {
            if (ModelState.IsValid)
            {
                db.PaperAssignments.Add(paperAssignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paperAssignment);
        }

        // GET: PaperAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaperAssignment paperAssignment = db.PaperAssignments.Find(id);
            if (paperAssignment == null)
            {
                return HttpNotFound();
            }
            return View(paperAssignment);
        }

        // POST: PaperAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_paper_assignment,id_paper,id_pcmember,date_assigned,date_due,is_delegated")] PaperAssignment paperAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paperAssignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paperAssignment);
        }

        // GET: PaperAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaperAssignment paperAssignment = db.PaperAssignments.Find(id);
            if (paperAssignment == null)
            {
                return HttpNotFound();
            }
            return View(paperAssignment);
        }

        // POST: PaperAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaperAssignment paperAssignment = db.PaperAssignments.Find(id);
            db.PaperAssignments.Remove(paperAssignment);
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
