using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private UserModelContainer db = new UserModelContainer();


        //email si parola- verific daca exista deja in bz 
        //daca nu, - redirect to new user controller/create


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String password, String email)
        {
            var users = db.Users.ToList();

            //Store the products to a session

            Session["users"] = users;

            //To get what you have stored to a session

            // var users = Session["users"] as List<User>;

            //to clear the session value

            //Session["products"] = null;
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
                    if (Session["User"] != null)
                    {

                    }
                    // se pune userul user pe session 
                    // cand pe session avem un user, inseamna ca exista user logat
                    return RedirectToAction("Welcoming", "Login");
                }
                else
                    ViewBag.Message = "Your password is incorect, try again";


            }

            return View();
        }

        //public ActionResult Welcoming()
        //{
        //    return View();
        //}


        public ActionResult Welcoming()
        {



            return View();
        }
    }
}
