using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using MateriaaliVarasto.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using WebMatrix.WebData;



namespace MateriaaliVarasto.Controllers
{
    
    public class ProductController : Controller
    {
        

        // GET: Product
        public  ActionResult Index(string sortProd, string currentFilter1,  string searchString1, string MateriaaliRyhma, string currentMateriaaliRyhma, int? page, int? pagesize)
        {
            ViewBag.CurrentSort = sortProd;
            ViewBag.ProdNameSortPara = string.IsNullOrEmpty(sortProd) ? "productname_desc" : "";

            if (searchString1 != null)
            {
                page = 1;
            }
            else
            {
                searchString1 = currentFilter1;
            }
            ViewBag.currentFilter1 = searchString1;

            if ((MateriaaliRyhma != null) && (MateriaaliRyhma != "0"))
            {
                page = 1;
            }
            else
            {
                MateriaaliRyhma = currentMateriaaliRyhma;
            }

            ViewBag.currentMateriaaliRyhma = MateriaaliRyhma;

            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                
                MatskuniDBEntities1 db = new MatskuniDBEntities1();

                var tuotteet = from t in db.Tuotteet
                               select t;
                
                
               
                if (!String.IsNullOrEmpty(MateriaaliRyhma) && (MateriaaliRyhma != "0"))
                {
                    int para = int.Parse(MateriaaliRyhma);
                    tuotteet = tuotteet.Where(p => p.MateriaaliID == para);
                }

                if (!String.IsNullOrEmpty(searchString1))
                {
                    switch (sortProd)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(t => t.Tuotenimi.Contains(searchString1)).OrderByDescending(t => t.Tuotenimi);
                            break;

                        default:
                            tuotteet = tuotteet.Where(t => t.Tuotenimi.Contains(searchString1)).OrderBy(t => t.Tuotenimi);
                            break;
                    }
                }
                else if (!String.IsNullOrEmpty(MateriaaliRyhma) && (MateriaaliRyhma != "0")) //Jos käytössä on tuoteryhmärajaus, niin käytetään sitä ja sen lisäksi lajitellaan tulokset 
                {
                    int para = int.Parse(MateriaaliRyhma);
                    switch (sortProd)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.MateriaaliID == para).OrderByDescending(p => p.Tuotenimi);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.MateriaaliID == para).OrderBy(p => p.Tuotenimi);
                            break;
                    }
                }
                else
                {
                    switch (sortProd)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.OrderByDescending(t => t.Tuotenimi);
                            break;

                        default:
                            tuotteet = tuotteet.OrderBy(t => t.Tuotenimi);
                            break;
                    }
                };

                List<Materiaalit> lstMaterials = new List<Materiaalit>();

                var materiaaliLista = from mat in db.Materiaalit
                                      select mat;

                Materiaalit tyhjaMateriaali = new Materiaalit();
                tyhjaMateriaali.MateriaaliID = 0;
                tyhjaMateriaali.Materiaali = "";
                tyhjaMateriaali.MateriaaliIDMateriaali = "";
                lstMaterials.Add(tyhjaMateriaali);

                foreach (Materiaalit materiaali in materiaaliLista)
                {
                    Materiaalit uusiMateriaali = new Materiaalit();
                    uusiMateriaali.MateriaaliID = materiaali.MateriaaliID;
                    uusiMateriaali.Materiaali = materiaali.Materiaali;
                    uusiMateriaali.MateriaaliIDMateriaali = materiaali.MateriaaliID.ToString() + " - " + materiaali.Materiaali;
                    lstMaterials.Add(uusiMateriaali);
                }
                ViewBag.MateriaaliID = new SelectList(lstMaterials, "MateriaaliID", "MateriaaliIDMateriaali", MateriaaliRyhma);


                int pageSize = (pagesize ?? 10);
                int pageNumber = (page ?? 1);
                
                return View(tuotteet.ToPagedList(pageNumber, pageSize));
            }


        }


        
        public ActionResult _ModalEdit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();

                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Tuotteet tuotteet = db.Tuotteet.Find(id);
                if (tuotteet == null) return HttpNotFound();
                ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuotteet.RyhmäID);
                ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuotteet.MateriaaliID);
                ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuotteet.ValmistajaID);
                //ViewBag.LoginID = new Logins().LoginId;
                return PartialView("_ModalEdit", tuotteet);
            }
        }
        public ActionResult _ModalEdit([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä,LoginId")] Tuotteet tuote)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();

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
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();

                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Tuotteet tuotteet = db.Tuotteet.Find(id);
                if (tuotteet == null) return HttpNotFound();
                ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuotteet.RyhmäID);
                ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuotteet.MateriaaliID);
                ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuotteet.ValmistajaID);
                return View(tuotteet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä,LoginId")] Tuotteet tuote)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();

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
        }
        public ActionResult Create2()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();
                ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä");
                ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali");
                ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja");
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä,LoginId")] Tuotteet tuote)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();
               
                if (ModelState.IsValid)
                {
                    tuote.LoginId = Convert.ToInt32(Session["LoginID"]);
                    db.Tuotteet.Add(tuote);
                    db.SaveChanges();
                    ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuote.RyhmäID);
                    ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuote.MateriaaliID);
                    ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuote.ValmistajaID);
                   
                    return RedirectToAction("Index");
                }
                return View(tuote);
            }
        }
        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Tuotteet tuotteet = db.Tuotteet.Find(id);
                if (tuotteet == null) return HttpNotFound();
                return View(tuotteet);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                MatskuniDBEntities1 db = new MatskuniDBEntities1();
                Tuotteet tuotteet = db.Tuotteet.Find(id); db.Tuotteet.Remove(tuotteet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}