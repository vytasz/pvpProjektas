using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult FormView(int? formId)
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            Higher _higher = new Higher();
            if (formId == null)
            {
                return RedirectToAction("Dashboard");
            }
            Form form = _higher.ReadForm((int)formId);
            return View(form);
        }

        // GET: Home
        public ActionResult FormInput(int? formId)
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            Higher _higher = new Higher();
            if (formId == null)
            {
                return RedirectToAction("Dashboard");
            }
            Form form = _higher.ReadForm((int)formId);
            ICacheService cache = new CacheService();
            cache.AddItemToCache(formId.ToString(), form, 600);
            return View(form);
        }

        [HttpPost]
        public ActionResult FormInputPost(int formId)
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            Higher _higher = new Higher();
            ICacheService cache = new CacheService();
            Form form = (Form)cache.GetItemFromCache(formId.ToString());
            if (form == null)
            {
                form = _higher.ReadForm(formId);
            }

            for (int i = 0; i < form.questions.Count(); i++)
            {
                string answer = "";
                if (form.questions[i].qType == Question.QuestionType.Multiple)
                {
                    List<string> listmultiple = new List<string>();
                    for (int j = 0; j < form.questions[i].answers.Count; j++)
                    {
                        string id = $"{i}choice{j}";
                        string txt = Request.Form[id];
                        if (txt == "true")
                        {
                            if (answer == "")
                            {
                                answer += form.questions[i].answers[j];
                            }
                            else
                            {
                                answer += ", " + form.questions[i].answers[j];
                            }
                        }
                    }
                }
                else answer = Request.Form[i.ToString()];
                form.questions[i].qAnswer = answer;
            }
            _higher.DeleteFormFormatted(form);
            _higher.WriteFormFormatted(form);
            _higher.UpdateForm(formId, form);
            return RedirectToAction("FormView", new { formId });
        }


        public ActionResult Upload()
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            Form form;
            if (file != null && file.ContentLength > 0)
            {
                form = FileReader.TextFromWord(file.InputStream);
                Higher higher = new Higher();
                higher.WriteForm((int)userID, form);
            }
            return RedirectToAction("FormInput");
        }

        public ActionResult Dashboard()
        {
            Login login = new Login();
            var cookie = Request.Cookies["JWT"];
            if (cookie == null) RedirectToAction("Index", "Login");
            var userID = login.CheckJWTToken(cookie.Value);
            if (userID == null) RedirectToAction("Index", "Login");
            Higher higher = new Higher();
            Dictionary<string, int> formMap = higher.ReadUserFormList((int)userID);
            return View(formMap);
        }
    }
}