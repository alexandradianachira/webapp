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


        //subreviewer cu reviwes
        public ActionResult SubreviewerDetails(int? id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Users");

            List<Subreviewer> subreviewerUser = new List<Subreviewer>();
            List<PaperAssignment> pas = new List<PaperAssignment>();
            User user = (User)Session["User"];
            List<Subreviewer> db_subreviewers = (from u in db.Subreviewers where u.id_user == id select u).ToList();
            foreach (Subreviewer sub in db_subreviewers)
            {
                pas.Add(sub.PaperAssignment);
            }


            return View(pas);
        }


        // GET: PaperAssignments
        public ActionResult Index()
        {
            return View(db.PaperAssignments.ToList());
        }

       


        public ActionResult ConferencePaper()
        {//conf chair-ului cu conf si paper si reviewer
            List<PaperAssignment> pas = new List<PaperAssignment>();
            User user =(User) Session["User"];
            foreach(PaperAssignment pa in db.PaperAssignments)
            {
                if(pa.PCmember.Conference.Chair.id_user==user.id_user)
                {
                    pas.Add(pa);
                }
            }
            return View(pas);
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
