using System;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Login loginService = new Login();
            var jwt = loginService.BuildJWTToken(username, password);
            if (jwt == null) return View();
            var loginCookie = new HttpCookie("JWT")
            {
                Value = jwt,
                Expires = DateTime.Now.AddMinutes(15)
            };
            Response.Cookies.Add(loginCookie);
            return RedirectToAction("Dashboard", "Home");
        }
    }
}