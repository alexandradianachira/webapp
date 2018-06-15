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
            //var c = (User)Session["User"];
            List<Conference > confChair = new List<Conference>();
            User x = (User)Session["User"];
            foreach (Conference conf in db.Conferences)
                     if (conf.Chair?.id_user==x.id_user)
                          confChair.Add(conf);


            return View(confChair.ToList());
        }

        public ActionResult ConferencePCmember()

        {
            List<PCmember> pcmembers= new List<WebApplication.PCmember>();
           // var c = (User)Session["User"];
            List<Conference> confPcmember = new List<Conference>();
            User x = (User)Session["User"];
            foreach (Conference conf in db.Conferences)
                if (conf.PCmembers != null)
                    foreach (PCmember PC in conf.PCmembers)
                    {
                        if (PC.id_user == x.id_user)
                            confPcmember.Add(conf);
                       
                    }
                else
                {
                    ViewBag.Message = "You don't have any conferences";
                    return RedirectToAction("Index", "Home");

                }



            return View(confPcmember.ToList());
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
            DateTime date_invitation_sent = DateTime.Now;
            mailSender(date_invitation_sent,getUser.id_user, getUser.email,getUser.first_name,user.text,getUser.verification_key);
            //usersAlreadyInvited.Add(getUser);
            //se adauga intr-o lista userii care deja au fost invitati 
            //usersAlreadyInvited.Add(user);
           
                //Session["date_invitation_sent"]=DateTime.Now;
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
            // var body = "Verification Key - Enter into link below " + verification_key;
            var body = string.Format("Dear, {0} <BR/>  <BR/> Thank you for your registration. <BR/><b> Message:</b> {2} <BR/> please click on the below link to accept the chair's invitation : <a href =\"{1}\" title =\"Accept or decline\">{1}</a>", first_name, Url.Action("AcceptDecline", "Conferences" ,new { verification_key, id_conference, id_user }, Request.Url.Scheme), text);

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
        public ActionResult AcceptDecline([Bind(Include="verification_key")]User user,int id_user, int id_conference, String submit )
        {

            // var confActuala =(Conference) Session["Conference"];
            //  confActuala = db.Conferences.Find(confActuala.id_conference);

            var conference = db.Conferences.Find(id_conference);
            var userFound = db.Users.Find(id_user);
            

            if (Request.Form["Accept"] != null)
            {
                PCmember newPcMember = new PCmember();
                 newPcMember.User= db.Users.Find(id_user);
                //id-ul conferintei pe care sunt
                newPcMember.Conference= db.Conferences.Find(id_conference);
                String confName = newPcMember.Conference.conference_name;
                newPcMember.id_user = newPcMember.User.id_user;
                newPcMember.id_conference = newPcMember.Conference.id_conference;
                newPcMember.is_chair = false;
                //newPcMember.User = userFound;
                newPcMember.is_valid = false;
                newPcMember.date_invitation_acc = DateTime.Now;
                var invitation_sent = Session["date_invitation_sent"];

                newPcMember.date_invitation_sent = DateTime.Now;
                //newPcMember.dateinvitationacc = (DateTime)invitation_acc;
               // newPcMember.Conference = conference;
                db.PCmembers.Add(newPcMember);
                db.SaveChanges();

            }
            else if (Request.Form["Decline"] != null)
            {
                return RedirectToAction("Reconsideration", "Conferences");
                
            }

        
           
        
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Reconsideration()
        {
            return View();
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
