using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            var a=Session["User"];
            Session["User"] = null;
            return RedirectToAction("Index","Home");
        }
    }
}