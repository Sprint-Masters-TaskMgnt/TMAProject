﻿
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TMAWebAPI.Controllers
//{
//    public class AdminController : Controller
//    {
//        // GET: Admin
//        public ActionResult Dashboard()
//        {
//            return View();
//        }
//        public ActionResult Users()
//        {
//            return View();
//        }
//        public ActionResult Projects()
//        {
//            return View();
//        }
//        public ActionResult Tasks()
//        {
//            return View();
//        }
//        public ActionResult ProjectManagement()
//        {
//            return View();
//        }

//        public ActionResult ProjectReport()
//        {
//            return View();
//        }

//        public ActionResult TaskReport()
//        {
//            return View();
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMgnt_MVC_.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard(int? id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult Users(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult Projects(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult Tasks(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult ProjectManagement(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }

        public ActionResult ProjectReport(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }

        public ActionResult TaskReport(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult UserProductivity(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult UpdateUserRole(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }


    }
}