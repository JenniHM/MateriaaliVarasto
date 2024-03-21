using MateriaaliVarasto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MateriaaliVarasto.Controllers
{
    public class ProducersController : Controller
    {
        MatskuniDBEntities1 db = new MatskuniDBEntities1();
        // GET: Producers
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return View(db.Valmistajat.ToList());
            }
        }
        public ActionResult Create()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Valmistajat valmistajat)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Valmistajat.Add(valmistajat);
                    db.SaveChanges();
                    return RedirectToAction("Create2", "Product");
                }
                return View(valmistajat);
            }

        }
        
    }
}