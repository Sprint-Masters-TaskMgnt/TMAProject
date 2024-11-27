
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskMgnt_MVC_.DTO;
using TaskMgnt_MVC_.Models;

namespace TaskMgnt_MVC_.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public HomeController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index()
        {
            SessionModel sessionModel = new SessionModel();
            Session.Add("UserName", User.Identity.Name);
            string email = User.Identity.Name;

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"https://localhost:7071/api/Users/GetEmail/{email}");
            string responseData = await httpResponseMessage.Content.ReadAsStringAsync();

            // Deserialize the response into a strongly-typed object
            var userResponse = JsonConvert.DeserializeObject<RoleUserDTO>(responseData);


            //if (User.IsInRole("Admin"))
            if (userResponse.RoleId == 1)
            {
                sessionModel.Role = "Admin";
                sessionModel.Address = "dsfdfsdfds";
                sessionModel.Name = User.Identity.Name;

                Session.Add("SessionModel", sessionModel);

                Session.Add("Role", "Admin");
                ViewBag.Role = "Admin";

                // Logic for admin users
                return RedirectToAction("Dashboard", "Admin", new { id = userResponse.Id });
            }
            else
            {
                Session.Add("Role", "User");
                ViewBag.Role = "User";

                //var json = new { UserId=userResponse.UserId };
                return RedirectToAction("Dashboard", "User", new { id = userResponse.Id });
            }
        }

        public ActionResult Home()
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