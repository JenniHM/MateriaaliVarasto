using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using MateriaaliVarasto.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using WebMatrix.WebData;
using static System.Net.WebRequestMethods;



namespace MateriaaliVarasto.Controllers
{

    public class ProductController : Controller
    {
        MatskuniDBEntities1 db = new MatskuniDBEntities1();

        // GET: Product
        public ActionResult Index(string sortProd, string currentFilter1, string searchString1, string MateriaaliRyhma, string currentMateriaaliRyhma, int? page, int? pagesize)
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
               
                int LoginId = Convert.ToInt32(Session["LoginID"]);

                var tuotteet = from t in db.Tuotteet
                               where t.LoginId == LoginId
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

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
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
        public ActionResult Edit([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä,Kuva,LoginId")] Tuotteet tuote, HttpPostedFileBase File1)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    
                    if (File1 != null && File1.ContentLength > 0)
                    {
                        tuote.Kuva = new byte[File1.ContentLength];
                        File1.InputStream.Read(tuote.Kuva, 0, File1.ContentLength);
                    }                 
                    tuote.LoginId = Convert.ToInt32(Session["LoginID"]);
                    db.Entry(tuote).State = EntityState.Modified;
                    if (tuote.Kuva == null)
                    {
                        var dbValues = db.Entry(tuote).GetDatabaseValues();
                        tuote.Kuva = (byte[])dbValues["Kuva"];
                    }
                    db.SaveChanges();
                        ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuote.RyhmäID);
                        ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuote.MateriaaliID);
                        ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuote.ValmistajaID);
                        return RedirectToAction("Index");
                    }
                }
                return View(tuote);
                
            }
      

            [HttpGet]
            public ActionResult Create2()
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä");
                    ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali");
                    ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja");
                    return View();
                }
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create2([Bind(Include = "TuoteID,Tuotenimi,ValmistajaID,RyhmäID,MateriaaliID,Pesty,Määrä,Kuva,LoginId")] Tuotteet tuote, HttpPostedFileBase File1)
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("index", "home");
                }
                else 
                { 
                    if (ModelState.IsValid)
                    {
                    try
                    {
                        if (File1 != null && File1.ContentLength > 0)
                        {
                            tuote.Kuva = new byte[File1.ContentLength];
                            File1.InputStream.Read(tuote.Kuva, 0, File1.ContentLength);
                        }
                        tuote.LoginId = Convert.ToInt32(Session["LoginID"]);
                        db.Tuotteet.Add(tuote);
                        db.SaveChanges();
                        ViewBag.RyhmäID = new SelectList(db.Ryhmät, "RyhmäID", "Ryhmä", tuote.RyhmäID);
                        ViewBag.MateriaaliID = new SelectList(db.Materiaalit, "MateriaaliID", "Materiaali", tuote.MateriaaliID);
                        ViewBag.ValmistajaID = new SelectList(db.Valmistajat, "ValmistajaID", "Valmistaja", tuote.ValmistajaID);
                    }
                     catch (Exception)
                    {
                        ViewBag.Message = "Tallennus epäonnistui";
                    }  
                    }              
                    return RedirectToAction("Index");
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
                    Tuotteet tuotteet = db.Tuotteet.Find(id); db.Tuotteet.Remove(tuotteet);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

      
        }
    }
