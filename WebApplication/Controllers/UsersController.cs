using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private UserModelContainer db = new UserModelContainer();
        List<PCmember> pcmembers = new List<PCmember>();


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(String password, String email)
        {
      
            if (ModelState.IsValid)
            {
                User user = (from u in db.Users
                             where
                                   u.password.Equals(password) &&
                                   u.email.Equals(email)
                             select u).FirstOrDefault();
                if (user != null)
                {
                    Session["User"] = user;
                    //
                   // if (Session["User"] != null)
                
                        return RedirectToAction("Welcoming", "Users");
                   
                    // se pune userul user pe session 
                    // cand pe session avem un user, inseamna ca exista user logat
                    //else
                  
                }
                else
                    ViewBag.Message = "Your password or e-mail is incorrect, try again";

            }

            return View();
        }

        public ActionResult Logout()
        {
            //var a = Session["User"];
            Session["User"] = null;
            Session.Remove("User");
            var a = Session["User"];
            //Session.Clear();
            return RedirectToAction("HCarousel", "Home");
        }



        public ActionResult Welcoming()
        {

            return View();
        }
        public ActionResult SendEmail()
        {
            return View();
        }


        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "id_user,surname,first_name,email,institution")] User user)
        {
            user.password = "password";
            user.verified_account = DateTime.Now;
            user.date_active = DateTime.Now.AddHours(24);
            user.date_verification_send = DateTime.Now;
            user.verification_key = KeyGenerator.GetUniqueKey(8);




            if (ModelState.IsValid)
            {
                //var errors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                db.Users.Add(user);
                emailSender(user.verification_key, user.email, user.first_name);
                db.SaveChanges();

            }

            return RedirectToAction("SendEmail");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void emailSender(String verification_key, String email, String first_name)
        {

            MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
            MailAddress toAddress = new MailAddress(email);


            const string fromPassword = "piqejhrgxidzojsf";
            const string subject = "Account confirmation";
            // var body = "Verification Key - Enter into link below " + verification_key;
            var body = string.Format("Dear, {0} <BR/>  <br/> Thank you for your registration, please click on the below link to complete your registration:  <a href =\"{1}\" title =\"User Email Confirm\">{1}</a>", first_name, Url.Action("EnterVerificationKey", "Users", new { verification_key, email }, Request.Url.Scheme));

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


        [HttpGet]
        public ActionResult EnterVerificationKey(String verification_key, String email)
        {

            DateTime now = DateTime.Now;
            DateTime now24 = now.AddHours(-24);

            User user = (from u in db.Users
                         where u.verification_key.Equals(verification_key) &&
                               u.email.Equals(email) &&
                               now.CompareTo(u.date_verification_send) > 0 &&
                               now24.CompareTo(u.date_verification_send) > 0
                         select u).FirstOrDefault();

            //User nuser = db.Users.Where(
            //u => u.verification_key.Equals(verification_key) && u.email.Equals(email)
            //&& u.date_verification_send < now && now < u.date_verification_send.AddHours(24)).FirstOrDefault();


            int? x = (from u in db.Users
                      where u.verification_key.Equals(verification_key) &&
                       u.email.Equals(email)
                      select u.id_user).FirstOrDefault();

            using (var context = new UserModelContainer())
            {
                User userById = context.Users.Find(x);

                if (x != null)
                {
                    Session["verification_key"] = userById.verification_key;
                    return RedirectToAction("SetNewPassword", new RouteValueDictionary(
                     new { action = "SetNewPassword", id_user = x }));

                }
                else
                    return RedirectToAction("ErrorVerificationKey");
            }

            // acuma userul trebuie validat si trebuie sa isi dea o parola
            // trebuie redirectionat catre view-ul unde isi introduce parola
            // la redirectionare transmitem in camp ascuns now
            // in controllerul de la set new password trebuie sa identifici un user in bd dat de data aceasta dupa cheia primara
        }

        public ActionResult SetNewPassword()
        {
            return View();
        }

        public ActionResult Index()
        {
            User u = (User)Session["User"];
            User x = db.Users.Find(u.id_user);
            return View(x);
        }
        [HttpPost]
        public ActionResult SetNewPassword(String password, String confirmPassword)
        {
            var verification_key = (string)Session["verification_key"];
            User user = (from u in db.Users
                         where u.verification_key.Equals(verification_key)
                         select u).FirstOrDefault();



            if (ModelState.IsValid)

                // var errors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                if (password == confirmPassword)
                {
                    user.password = password;
                    // se seteaza verfied account si date active din bd la data curenta
                    // din acest moment user account-ul este activ si poate face login
                    user.verified_account = DateTime.Now;
                    user.date_active = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    ViewBag.Message = "Your passwords are not coresponding";

                } return View();
        }

        //public ActionResult ErrorVerificationKey()
        //{
        //    return View();
        //}

        public ActionResult Edit()
        {
            User x = (User)Session["User"];
            int? id = x.id_user;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,surname,first_name,email,institution,password")] User user)
        {
            User u = db.Users.Find(user.id_user);
            if (ModelState.IsValid)
            {
                u.surname = user.surname;
                u.first_name = user.first_name;
                u.email = user.email;
                u.institution = user.institution;
                u.password = user.password;

                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return View(user);
            return RedirectToAction("Login", "Users");

        }

        public ActionResult Details()
        {
            User x = (User)Session["User"];
            int? id = x.id_user; 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           User user = db.Users.Find(x.id_user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


    }

}




