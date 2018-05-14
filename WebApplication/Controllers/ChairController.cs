
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebApplication.Controllers
{
    public class ChairController : Controller
    {
        // GET: Chair
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendInvites()
        {
            return View();
        }
        public ActionResult RSVP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RSVP(Button Accept, Button Decline)
        {
            //inca s logata pe contu chair - ului 
            if (Request.Form["Accept"] != null)
            {
                ViewBag.Message = "You need to log in or make an account";
                return RedirectToAction("Index", "Login");
            }
            else if (Request.Form["Decline"] != null)
            {
                ViewBag.Message = "Thanks !";
                return RedirectToAction("Index", "Home");
            }
            return View();
          
        }
        [HttpPost]
        public ActionResult SendInvites(String first_name, String surname, String email, String text)
        {

            //NUMAI CAND CHAIR-UL ESTE LOGAT 
            User user =(User) Session["User"];
            //if user este chair ??
           // String fromEmail = user.email;
            


            MailAddress fromAddress = new MailAddress("peer.review.confirmation@gmail.com");
            MailAddress toAddress = new MailAddress(email);

            const string fromPassword = "piqejhrgxidzojsf";
            const string subject = ("Invitation for review");
           // var body = text;
            var body = string.Format("<b>{0} {1} sent you an invitation</b> <br/><i> {2}</i><br> <div> Please RSVP here  <a href =\"{3}\" title =\"RSVP\">{3}</a></div></br>", user.surname, user.first_name, text, Url.Action("RSVP", "Chair", new {  }, Request.Url.Scheme));
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
            return View(user);
        }

    }

    
}