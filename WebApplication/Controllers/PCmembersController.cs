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
    public class PCmembersController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: PCmembers
        public ActionResult Index()
        {
            return View(db.PCmembers.ToList());
        }

        // GET: PCmembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }

        // GET: PCmembers/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_pcmember,id_user,id_conference,is_chair,date_invitation_sent,date_invitation_acc,is_valid")] PCmember pCmember)
        {
            if (ModelState.IsValid)
            {
                db.PCmembers.Add(pCmember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pCmember);
        }

        // GET: PCmembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_pcmember,id_user,id_conference,is_chair,date_invitation_sent,date_invitation_acc,is_valid")] PCmember pCmember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pCmember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pCmember);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PCmember pCmember = db.PCmembers.Find(id);
            db.PCmembers.Remove(pCmember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult AcceptDecline()
        {

            return View();

        }


        //metoda dupa click pe link-ul din e-mail; utilizatorul decide daca doreste sa faca review la lucrare
        //se adauga un nou pcmember + paperassignment + subreviewer 
        [HttpPost]
        public ActionResult AcceptDecline([Bind(Include = "verification_key")]User user, int id_user, int id_conference, String submit)
        {

            if (Request.Form["Accept"] != null)
            {
                Paper paper = (from u in db.Papers where u.id_conference == id_conference select u).FirstOrDefault();

                PCmember newPcMember = new PCmember();
                newPcMember.User = db.Users.Find(id_user);
                newPcMember.Conference = db.Conferences.Find(id_conference);
                String confName = newPcMember.Conference.conference_name;
                newPcMember.id_user = newPcMember.User.id_user;
                int id = newPcMember.id_user;
                newPcMember.id_conference = newPcMember.Conference.id_conference;
                newPcMember.is_chair = false;
                newPcMember.is_valid = false;
                newPcMember.date_invitation_acc = DateTime.Now;
                string invitation_sent = (String)Session["date_invitation_sent"];
                DateTime myDate = DateTime.Parse(invitation_sent);
                newPcMember.date_invitation_sent = (DateTime)myDate;
                db.PCmembers.Add(newPcMember);
                db.SaveChanges();

                PCmember pcm = db.PCmembers.Find(newPcMember.id_pcmember);

                PaperAssignment newPaperAssignment = new PaperAssignment();
                Conference conf = db.Conferences.Find(id_conference);
                newPaperAssignment.Paper = paper;
                newPaperAssignment.PCmember = pcm;
                newPaperAssignment.id_paper = paper.id_paper;
                newPaperAssignment.id_pcmember = pcm.id_pcmember;


                // de refacut - date_assigned - date_submitted
                newPaperAssignment.date_assigned = paper.date_submitted;
                //termen de 2 zile pana cand mai poate reincarca lucrarea
                newPaperAssignment.date_due = newPaperAssignment.date_assigned.AddHours(48);
                newPaperAssignment.is_delegated = false;
                db.PaperAssignments.Add(newPaperAssignment);
                db.SaveChanges();


                PaperAssignment pp = db.PaperAssignments.Find(newPaperAssignment.id_paper_assignment);
                Subreviewer newSubreviewer = new Subreviewer();

                newSubreviewer.PaperAssignment = pp;
                newSubreviewer.id_paper_assignment = pp.id_paper_assignment;
                PaperAssignment p = db.PaperAssignments.Find(pp.id_paper_assignment);
                PCmember pc = p.PCmember;
                User us = db.Users.Find(pc.id_user);
                newSubreviewer.User = us;
                newSubreviewer.id_user = us.id_user;

                newSubreviewer.date_invitation_answer = newPcMember.date_invitation_acc;
                newSubreviewer.date_invitation_send = newPcMember.date_invitation_sent;
                newSubreviewer.is_accepted = true;
                newSubreviewer.PaperAssignment = p;
                newSubreviewer.id_paper_assignment = p.id_paper_assignment;

                db.Subreviewers.Add(newSubreviewer);
                db.SaveChanges();


            }
            else if (Request.Form["Decline"] != null)
            {
                return RedirectToAction("Reconsideration", "PCmembers");

            }
            return RedirectToAction("SubreviewerDetails", "PaperAssignments", new { id = id_user });
        }
        public ActionResult Reconsideration()
        {
            return View();
        }

        //lista cu toti pc membersii detaliata
        public ActionResult AllPCmembers()
        {
            ViewBag.Users = db.Users.ToList();
            ViewBag.Conferences = db.Conferences.ToList();

            return View(db.PCmembers.ToList());
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
