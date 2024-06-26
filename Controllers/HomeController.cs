﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MateriaaliVarasto.Models;
using WebMatrix.WebData;
using System.Configuration;

namespace MateriaaliVarasto.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.LoginError = 0;
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult OfUs()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Logins reg)
        {
            MatskuniDBEntities1 db = new MatskuniDBEntities1();
            if (ModelState.IsValid)
            {
                var userExist = db.Logins.Any(x => x.UserName == reg.UserName);
                if (userExist)
                {
                    ModelState.AddModelError("UserName", "Käyttäjätunnus on jo olemassa");
                    return View(reg);
                }
                else
                {
                    db.Logins.Add(reg);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
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
                ViewBag.LoginError = 0;
                Session["UserName"] = LoggedUser.UserName;
                Session["LoginID"] = LoggedUser.LoginId;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                ViewBag.LoggedStatus = " ";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Uloskirjautunut";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MustAuthorize()
        {
            ViewBag.LoginError = 1;
            return View("Index");
        }

    }
}