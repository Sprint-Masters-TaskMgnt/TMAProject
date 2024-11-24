using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMgntWebApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult Projects()
        {
            return View();
        }
        public ActionResult Tasks()
        {
            return View();
        }
        public ActionResult ProjectManagement()
        {
            return View();
        }

        public ActionResult ProjectReport()
        {
            return View();
        }

        public ActionResult TaskReport()
        {
            return View();
        }
    }
}