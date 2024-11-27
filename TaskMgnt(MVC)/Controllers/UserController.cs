
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TMAWebAPI.Controllers
//{
//    public class UserController : Controller
//    {
//        public ActionResult DashBoard()
//        {
//            return View();
//        }

//        public ActionResult TaskTbl()
//        {
//            return View();
//        }

//        public ActionResult UpdateProfile()
//        {
//            return View();
//        }
//        public ActionResult AssignTask()
//        {
//            return View();
//        }
//        public ActionResult UpdateTask()
//        {
//            return View();
//        }
//        public ActionResult ProjectManagement()
//        {
//            return View();
//        }
//        public ActionResult TaskReport()
//        {
//            return View();
//        }
//        public ActionResult MyReport()
//        {
//            return View();
//        }
//        public ActionResult UserProductivity()
//        {
//            return View();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace TaskMgnt_MVC_.Controllers
{
    public class UserController : Controller
    {
        public ActionResult DashBoard(int? id)
        {
            ViewBag.Message = id;
            return View(id);
        }

        public ActionResult TaskTbl()
        {

            return View();
        }

        public ActionResult UpdateProfile(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult AssignTask(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult UpdateTask(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult ProjectManagement(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }
        public ActionResult TaskReport(int id)
        {
            ViewBag.Message = id;
            return View(id);
        }

    }
}
