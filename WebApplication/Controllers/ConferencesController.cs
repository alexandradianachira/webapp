using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication;

namespace WebApplication.Controllers
{
    public class ConferencesController : Controller
    {
        private UserModelContainer db = new UserModelContainer();


        //index - index conferinte de tip chair sau pcmembers
        public ActionResult Index()
        {

            return View();
        }


        //conferinte ca si chair
        public ActionResult ConferenceChair()
        {
            List<Conference> confChair = new List<Conference>();
            User x = (User)Session["User"];
            foreach (Conference conf in db.Conferences)
                if (conf.Chair.id_user == x.id_user)
                    confChair.Add(conf);


            return View(confChair.ToList());
        }

        //conferinte ca si pc member
        public ActionResult ConferencePCmember()
        {
            List<PCmember> pcmembers = db.PCmembers.ToList();
            List<Conference> confPcmember = new List<Conference>();
            User x = (User)Session["User"];

            foreach (PCmember pc in pcmembers)
            {
                if (pc.id_user == x.id_user)
                {
                    Conference conf = db.Conferences.Find(pc.id_conference);
                    confPcmember.Add(conf);
                }
            }

            return View(confPcmember);
        }

        // GET: Conferences1/Details/5
        public ActionResult Details(int? id)
        {

            var x = (User)Session["User"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            var y = conference;
            Session["Conference"] = y;
            if (conference == null)
            {
                return HttpNotFound();
            }

            ViewBag.Users = db.Users.ToList();
            return View(conference);
        }

        //lista useri pentru invite 
        public ActionResult ListUsers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var x = user;
            Session["getUser"] = x;
            return View(user);
        }

        //pagina cu mesaj ca a fost trimis mail 
        public ActionResult SendInvite()
        {

            return View();
        }

        //pagina de adaugare a mesajului
        public ActionResult WriteText()
        {

            return View();
        }


        //pagina de adaugare a mesajului
        [HttpPost]
        public ActionResult WriteText(String text)
        {
            var getUser = (User)Session["getUser"];
            DateTime date_invitation_sent = DateTime.Now;
            mailSender(date_invitation_sent, getUser.id_user, getUser.email, getUser.first_name, text, getUser.verification_key);
            Session["date_invitation_sent"] = DateTime.Now.ToString();
            return RedirectToAction("SendInvite", "Conferences");
        }

        public void mailSender(DateTime date_invitation_sent, int id_user, String email, String first_name, String text, String verification_key)
        {
            var conference = (Conference)Session["Conference"];
            int id_conference = conference.id_conference;


            MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
            MailAddress toAddress = new MailAddress(email);


            const string fromPassword = "piqejhrgxidzojsf";
            const string subject = "Invitation for review";
            var body = string.Format("Dear, {0} <BR/>  <BR/> Thank you for your registration. <BR/><b> Message:</b> {2} <BR/><br/> 1. First, please login <a href =\"{3}\" title =\"Login\">{3}</a> </br>  2.Second, click on the below link to accept the chair's invitation : <a href =\"{1}\" title =\"Accept or decline\">{1}</a>", first_name, Url.Action("AcceptDecline", "PCmembers", new { verification_key, id_conference, id_user }, Request.Url.Scheme), text, Url.Action("Login", "Users", new { }, Request.Url.Scheme));

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,

            })

            {
                message.IsBodyHtml = true;
                smtp.Send(message);
            };
        }
        //detalii conferinte pt pc members
        public ActionResult DetailsForPCmembers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }

            return View(conference);
        }


        // GET: Conferences1/Create
        public ActionResult Create()
        {
            return View();
        }

        //creare conf chair
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_conference,conference_name,location,start_date,end_date")] Conference conference)
        {
            if (ModelState.IsValid)
            {

                var c = (User)Session["User"];
                conference.Chair = db.Users.Find(c.id_user);
                db.Conferences.Add(conference);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(conference);
        }

        //lista conf pentru a scrie deciziile la fiecare paper
        public ActionResult ShowConferences()
        {
            User user = (User)Session["User"];
            User us = db.Users.Find(user.id_user);
            List<Conference> confs = new List<Conference>();
            List<PCmember> pc = (from u in db.PCmembers where us.id_user == u.id_user && u.is_chair == true select u).ToList();
            foreach (PCmember p in pc)
            {
                confs.Add(p.Conference);
            }

            return View(confs);
        }



        // GET: Conferences1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_conference,conference_name,location,start_date,end_date")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conference);
        }


        //toate conferintele din bd
        public ActionResult AllConferences()
        {
            return View(db.Conferences.ToList());
        }


        // GET: Conferences1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: Conferences1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conference conference = db.Conferences.Find(id);
            db.Conferences.Remove(conference);
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
