using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MateriaaliVarasto.Models;

namespace MateriaaliVarasto.Controllers
{
    public class ProductController : Controller
    {
        MatskuniDBEntities1 db = new MatskuniDBEntities1();
        // GET: Product
        public ActionResult Index()
        {
            List<Tuotteet> model = db.Tuotteet.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();
            ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuotteet.RyhmäID);
            ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuotteet.MateriaaliID);
            ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuotteet.ValmistajaID);
            return View(tuotteet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä")] Tuotteet tuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuote).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuote.RyhmäID);
                ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuote.MateriaaliID);
                ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuote.ValmistajaID);
                return RedirectToAction("Index");
            }
            return View(tuote);
        }

        public ActionResult Create()
        {
            ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä");
            ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali");
            ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä")] Tuotteet tuote)
        {
            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuote);
                db.SaveChanges();
                ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuote.RyhmäID);
                ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuote.MateriaaliID);
                ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuote.ValmistajaID);
                return RedirectToAction("Index");
            }
            return View(tuote);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();
            return View(tuotteet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tuotteet tuotteet = db.Tuotteet.Find(id); db.Tuotteet.Remove(tuotteet); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}