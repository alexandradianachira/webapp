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

        public ActionResult PaperForChair(int? id)

        {
            List<Paper> papers = new List<Paper>();
            List<Conference> conferences = new List<Conference>();

            User user = (User)Session["User"];
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


        public ActionResult SeePaper(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = (from u in db.Papers where u.id_conference == id select u).FirstOrDefault();


            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }


        //lucrarile utilizatorului de pe session
        public ActionResult Index()
        {
            User user = (User)Session["User"];
            //se extrage autorul care e pe sesiune
            List<Author> authors = db.Authors.ToList();
            List<Paper> papersList = db.Papers.ToList();
            List<Paper> papersShow = new List<Paper>();
            //se primesc toti autorii 
            foreach (Author author in authors)
            {
                if (user.id_user == author.id_user)
                {
                    Author a = db.Authors.Find(author.id_author);
                    foreach (Paper paper in papersList)
                    {
                        if (paper.Author.id_author == a.id_author)
                        {
                            papersShow.Add(paper);
                        }
                    }
                }

            }
            if (papersShow == null)
            {
                ViewBag.Message = "You don't have any papers";

            }
            return View(papersShow);
        }



        //conf unde chair-ul poate adauga decisions
        public ActionResult AddDecision(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference con = db.Conferences.Find(id);
            Paper paper = (from u in db.Papers where u.id_conference == id select u).FirstOrDefault();
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDecision(Boolean decision, String decision_text, Paper paper)
        {
            Paper p = db.Papers.Find(paper.id_paper);

            if (ModelState.IsValid)
            {

                p.decision = paper.decision;
                p.decision_text = paper.decision_text;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("ConferenceChair", "Conferences");
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

        //toate lucrarile din bd  PT ADMINISTRATOR
        public ActionResult AllPapers()
        {
            return View(db.Papers.ToList());
        }


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


        public ActionResult GetFile(int? id)
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
            return View();
        }
        [HttpPost]
        public ActionResult GetFile(HttpPostedFileBase pdf, int id)
        {

            string path = Server.MapPath("~/PaperFiles/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Paper paper = db.Papers.Find(id);
            if (pdf != null)
            {
                string fileName = Path.GetFileName(pdf.FileName);
                pdf.SaveAs(path + fileName);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                paper.pdf = (String)fileName;
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();

            }



            return RedirectToAction("Index");


        }
        public ActionResult SubmitPaper()
        {
            return View();
        }
        //adaugare lucrare + pcmember(as author) 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitPaper([Bind(Include = "id_paper,title,email,contributions,text")] Paper paper, int id_conference)
        {
            //var file = paper.pdf;
            PCmember newPCmember = new PCmember();
            User user = (User)Session["User"];
            Author a = (from u in db.Authors where (u.id_user).Equals(user.id_user) select u).FirstOrDefault();
            Author au = db.Authors.Find(a.id_author);
            paper.id_conference = id_conference;
            paper.Author = au;
            paper.pdf = "";
            paper.date_submitted = DateTime.Now;
            paper.is_submitted = true;
            paper.decision_date = DateTime.Now.AddHours(48);
            paper.decision_text = "";
            paper.decision = false;
            a.id_paper = paper.id_paper;
            paper.PaperAssignments = new List<PaperAssignment>();
            User us = db.Users.Find(user.id_user);
            newPCmember.User = us;
            newPCmember.id_user = us.id_user;
            Conference C = db.Conferences.Find(id_conference);
            newPCmember.Conference = C;
            newPCmember.id_conference = id_conference;
            newPCmember.is_chair = false;
            newPCmember.date_invitation_acc = DateTime.Now;
            newPCmember.date_invitation_sent = DateTime.Now;
            newPCmember.is_valid = false;


            if (ModelState.IsValid)
            {
                db.Papers.Add(paper);
                db.PCmembers.Add(newPCmember);
                try
                {
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
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

                return RedirectToAction("GetFile", new { id = paper.id_paper });
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
