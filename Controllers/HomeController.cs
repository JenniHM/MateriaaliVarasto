﻿using System;
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
    }
}