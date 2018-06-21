using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;

namespace WebApplication.Controllers
{
    public class PapersController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: Papers
       public ActionResult Index()
        {
            User user = (User)Session["User"];
            //se extrage autorul care e pe sesiune
            Author au = (from u in db.Authors where (user.id_user).Equals(u.id_user) select u).FirstOrDefault();
            Author a = db.Authors.Find(au.id_author);
            List<Paper> papersList = db.Papers.ToList();
            List<Paper> papersShow = new List<Paper>();

            //se ia id-ul autorului
            //se afiseaza toate lucrarile lui 
            foreach (Paper paper in papersList)
            {
                if(paper.Author==a)
                {
                    papersShow.Add(paper);
                }
            }
            return View(papersShow);
        }
        // GET: Papers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // GET: Papers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Papers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_paper,id_conference,title,pdf,date_submitted,is_submitted,decision,decision_text,decision_date,email,contributions")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                db.Papers.Add(paper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paper);
        }

        // GET: Papers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Papers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_paper,id_conference,title,pdf,date_submitted,is_submitted,decision,decision_text,decision_date,email,au")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paper);
        }

        // GET: Papers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paper paper = db.Papers.Find(id);
            db.Papers.Remove(paper);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SubmitPaper()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitPaper([Bind(Include = "id_paper,title,pdf,email,contributions,text")] Paper paper, int id_conference, HttpPostedFileBase file)
        {
                PCmember newPCmember = new PCmember();
                User user = (User)Session["User"];
                Author a = (from u in db.Authors where (u.id_user).Equals(user.id_user) select u).FirstOrDefault();
                Author au = db.Authors.Find(a.id_author);
                paper.id_conference = id_conference;
                paper.Author = au;
                paper.date_submitted = DateTime.Now;
                paper.is_submitted = true;
                paper.decision_date = DateTime.Now.AddHours(48);
                paper.decision_text = "";
                paper.decision = false;
                a.id_paper = paper.id_paper;
                paper.PaperAssignments = new List<PaperAssignment>();
                newPCmember.id_user = user.id_user;
                newPCmember.id_conference = id_conference;
                newPCmember.is_chair = false;
                newPCmember.date_invitation_acc = DateTime.Now;
                newPCmember.date_invitation_sent = DateTime.Now;
                newPCmember.is_valid = false;

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/PaperFiles"), fileName);
                file.SaveAs(path);
            }


            if (ModelState.IsValid)
                {
                    db.Papers.Add(paper);
                try
                {
                    db.SaveChanges();

                }catch(DbEntityValidationException e)
{
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                //conferences cu dropdown list

                return RedirectToAction("Index");
                }



            return View(paper);
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
