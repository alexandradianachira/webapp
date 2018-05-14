﻿using System;
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

        // GET: Conferences1
        public ActionResult Index()
        {
            List<User> chairs = new List<WebApplication.User>();
            var c = (User)Session["User"];
            List<Conference > confChair = new List<Conference>();
            User x = (User)Session["User"];
            foreach (Conference conf in db.Conferences)
                     if (conf.Chair?.id_user==x.id_user)
                          confChair.Add(conf);


            return View(confChair.ToList());
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
           //ViewBag.Users= db.Users.Where(x => !usersAlreadyInvited.Contains(x.PCmembers)).ToList();
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
        public ActionResult WriteText([Bind(Include = "text")] User user)
        {
            //List<User> usersAlreadyInvited = new List<User>();
            var getUser = (User) Session["getUser"];

            mailSender(getUser.email,getUser.first_name,user.text,getUser.verification_key);
            //usersAlreadyInvited.Add(getUser);
            //se adauga intr-o lista userii care deja au fost invitati 
            //usersAlreadyInvited.Add(user);
            return RedirectToAction("SendInvite", "Conferences");
        }

        public void mailSender(String email, String first_name, String text, String verification_key)
        {
            var conference =(Conference) Session["Conference"];
            int id_conference = conference.id_conference;

            MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
            MailAddress toAddress = new MailAddress(email);


            const string fromPassword = "piqejhrgxidzojsf";
            const string subject = "Invitation for review";
            // var body = "Verification Key - Enter into link below " + verification_key;
            var body = string.Format("Dear, {0} <BR/>  <BR/> Thank you for your registration. <BR/><b> Message:</b> {2} <BR/> please click on the below link to accept the chair's invitation : <a href =\"{1}\" title =\"Accept or decline\">{1}</a>", first_name, Url.Action("AcceptDecline", "Conferences" ,new { verification_key, id_conference }, Request.Url.Scheme), text);

            //  var body = GetFormattedMessageHTML(email, first_name,verification_key);

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


        public ActionResult AcceptDecline()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcceptDecline([Bind(Include="verification_key")]User user, int id_conference, String submit )
        {

            // var confActuala =(Conference) Session["Conference"];
            //  confActuala = db.Conferences.Find(confActuala.id_conference);

            var conference = db.Conferences.Find(id_conference);

            var userFound = db.Users.Find(user.verification_key);
            if (Request.Form["Accept"] != null)
            {
                PCmember newPcMember = new PCmember();

                //id-ul conferintei pe care sunt
                newPcMember.id_conference= id_conference;
                newPcMember.id_user = userFound.id_user;
                newPcMember.is_chair = false;
                newPcMember.User = userFound;
                newPcMember.is_valid = false;

                db.PCmembers.Add(newPcMember);
                db.SaveChanges();
            }
            else if (Request.Form["Decline"] != null)
            {
                // daca refuza
            }

        
           
        
            return RedirectToAction("Index", "Home");
        }

      
        
        // GET: Conferences1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conferences1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: Conferences1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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


        //public ActionResult SendEmail()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SendInv(String email, string first_name)
        //{

        //    emailSender(email, first_name);
        //    return View();
        //}

        //public void emailSender(String email, String first_name)
        //{

        //    MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
        //    MailAddress toAddress = new MailAddress(email);


        //    const string fromPassword = "piqejhrgxidzojsf";
        //    const string subject = "Invitation for review";
        //    // var body = "Verification Key - Enter into link below " + verification_key;
        //    var body = string.Format("Dear, {0} <BR/>  <br/> Invitation for reviewing, please click on the link to accept or decline  <a href =\"{1}\" title =\"User Email Confirm\">{1}</a>", first_name, Url.Action("AcceptDecline", "Conferences", new {email }, Request.Url.Scheme));

        //    //  var body = GetFormattedMessageHTML(email, first_name,verification_key);

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //    };

        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = subject,
        //        Body = body,

        //    })

        //    {
        //        message.IsBodyHtml = true;
        //        smtp.Send(message);
        //    };
        //}

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