using MateriaaliVarasto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MateriaaliVarasto.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
                
            }
            else ViewBag.LoggedStatus = "In";
            ViewBag.Selain = Request.UserAgent;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Tietoja sovelluksesta.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Yhteystiedot.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            MatskuniDBEntities1 db = new MatskuniDBEntities1();
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Kirjautuminen onnistunut!";
                ViewBag.LoggedStatus = "Sisäänkirjautunut";
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                ViewBag.LoggedStatus = " ";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Login", LoginModel);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Uloskirjautunut";
            return RedirectToAction("Index", "Home");
        }

    }
}