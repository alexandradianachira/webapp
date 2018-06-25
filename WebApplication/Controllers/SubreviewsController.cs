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
            User user = (User)Session["User"];
            List<Subreview> subreviews = new List<Subreview>();
            List<Subreview> subs = (from u in db.Subreviews where u.Subreviewer.id_user == user.id_user select u).ToList();

            //foreach(Subreview sub in subs)
            // {
            //     if()
            // }

            return View(subs);



        }

        //subreviews pt chair la fiecare paper/conf in parte
        public ActionResult SubreviewsForChair()
        {
            User user = (User)Session["User"];
            List<Conference> confs = (from u in db.Conferences where u.Chair.id_user == user.id_user select u).ToList();


            return View(db.Subreviews.OrderBy(p => p.Subreviewer.PaperAssignment.Paper.id_conference).ToList());
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

        //adaugare subreview
        public ActionResult AddSubreview(int? id)
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
            return View();


        }

        // POST: Subreviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubreview([Bind(Include = "id_subreview,id_subreviewer,grade,confidence,comment,comment_to_edit")] Subreview subreview, int? id)
        {
            if (ModelState.IsValid)
            {
                User user = (User)Session["User"];

                Subreviewer subreviewer = db.Subreviewers.Find(id);

                subreview.Subreviewer = subreviewer;
                subreview.id_subreviewer = (Int32)id;
                subreview.date_submitted = DateTime.Now;
                subreview.comment_to_edit = "";
                db.Subreviews.Add(subreview);
                db.SaveChanges();
                // return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //editare subreview 
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
            var old = subreview.comment;
            Session["Old"] = (String)old;
            return View(subreview);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subreview,id_subreviewer,grade,confidence,comment,date_submitted")] Subreview subreview, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subreview).State = EntityState.Modified;
                subreview.comment_to_edit = (String)Session["Old"];
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
