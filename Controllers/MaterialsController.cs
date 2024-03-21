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
                return RedirectToAction("index", "home");
            }
            else
            {         
                return View(db.Materiaalit.ToList());
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
        public ActionResult Create(Materiaalit materiaalit)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Materiaalit.Add(materiaalit);
                    db.SaveChanges();
                    return RedirectToAction("Create2", "Product");
                }
                return View(materiaalit);
            }
        }
       
    }
}