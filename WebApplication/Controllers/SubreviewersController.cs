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


        //lista cu papers, conf ca si evaluator - session user
        // GET: Subreviewers
        public ActionResult Index(int? id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Users");

            List<Subreviewer> subreviewerUser = new List<Subreviewer>();
            User user = (User)Session["User"];
            List<Subreviewer> db_subreviewers = (from u in db.Subreviewers where u.id_user == id select u).ToList();
            if (id != null)
            {
                foreach (Subreviewer sub in db_subreviewers)
                {

                    subreviewerUser.Add(sub);


                    PaperAssignment paperAssignment = db.PaperAssignments.Find(sub.id_paper_assignment);
                    Paper paper = db.Papers.Find(paperAssignment.id_paper);
                    ViewBag.Paper = paper;
                    Conference conference = db.Conferences.Find(paper.id_conference);
                    ViewBag.Conference = conference;
                }

            }

            return View(subreviewerUser);
        }

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
