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
            List<Materiaalit> model = db.Materiaalit.ToList();
            return View(model);
        }

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); Shippers shippers = db.Shippers.Find(id);
        //    if (shippers == null) return HttpNotFound(); returnView(shippers);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken] //Katsohttps://go.microsoft.com/fwlink/?LinkId=317598
        //public ActionResult Edit([Bind(Include = "ShipperID,CompanyName,Phone")] Shippersshipper)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(shipper).State = EntityState.Modified; db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    returnView(shipper);
        //}
    }
}