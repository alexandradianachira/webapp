﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private UserModelContainer db = new UserModelContainer();
       public ActionResult ex()
        {
            return View();
        }
        public ActionResult Index()
        {
           
           
                return View();
        }
        public ActionResult HCarousel()
        {


            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}