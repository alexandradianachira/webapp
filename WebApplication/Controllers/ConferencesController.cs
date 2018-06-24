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

        public ActionResult Index()
        {

            return View();
        }

        // GET: Conferences1
        public ActionResult ConferenceChair()
        {
            List<User> chairs = new List<WebApplication.User>();
            List<Conference > confChair = new List<Conference>();
            User x = (User)Session["User"];
            foreach (Conference conf in db.Conferences)
                     if (conf.Chair?.id_user==x.id_user)
                          confChair.Add(conf);


            return View(confChair.ToList());
        }

        public ActionResult ConferencePCmember()
        {
            List<PCmember> pcmembers = db.PCmembers.ToList();
            List<Conference> confPcmember = new List<Conference>();
            User x = (User)Session["User"];
            
            foreach(PCmember pc in pcmembers)
            {
                if(pc.id_user==x.id_user)
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
 
        public ActionResult SendInvite()
        {
            
            return View();
        }
        public ActionResult WriteText()
        {

            return View();
        }
        
      

        [HttpPost]
        public ActionResult WriteText(String text)
        {
            var getUser = (User) Session["getUser"];
            DateTime date_invitation_sent = DateTime.Now;
            mailSender(date_invitation_sent,getUser.id_user, getUser.email,getUser.first_name,text,getUser.verification_key);
            Session["date_invitation_sent"] = DateTime.Now.ToString();
            return RedirectToAction("SendInvite", "Conferences");
        }

        public void mailSender(DateTime date_invitation_sent, int id_user, String email, String first_name, String text, String verification_key)
        {
            var conference =(Conference) Session["Conference"];
            int id_conference = conference.id_conference;
           

            MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
            MailAddress toAddress = new MailAddress(email);


            const string fromPassword = "piqejhrgxidzojsf";
            const string subject = "Invitation for review";
            var body = string.Format("Dear, {0} <BR/>  <BR/> Thank you for your registration. <BR/><b> Message:</b> {2} <BR/> </br>  Click on the below link to accept the chair's invitation : <a href =\"{1}\" title =\"Accept or decline\">{1}</a>", first_name, Url.Action("AcceptDecline", "PCmembers" ,new { verification_key, id_conference, id_user }, Request.Url.Scheme), text);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_conference,conference_name,location,start_date,end_date")] Conference conference)
        {
            if (ModelState.IsValid)
            {

                var c = (User)Session["User"];
                conference.Chair = db.Users.Find(c.id_user);
                //PCmember pc = new PCmember();
                //pc.Conference = conference;
                //pc.id_conference = conference.id_conference;
                //pc.User = c;
                //pc.id_user = c.id_user;
                //pc.date_invitation_acc = DateTime.Now;
                //pc.date_invitation_sent = DateTime.Now;
                //pc.is_chair = true;

                db.Conferences.Add(conference);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(conference);
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
