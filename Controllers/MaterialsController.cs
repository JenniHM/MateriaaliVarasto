using MateriaaliVarasto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MateriaaliVarasto.Controllers
{
    public class MaterialsController : Controller
    {
        MatskuniDBEntities1 db = new MatskuniDBEntities1();
        // GET: Materials
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                List<Materiaalit> model = db.Materiaalit.ToList();
                db.Dispose();
                return View(model);
            }
        }

       
    }
}