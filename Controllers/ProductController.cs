using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MateriaaliVarasto.Models;

namespace MateriaaliVarasto.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            MatskuniDBEntities1 db = new MatskuniDBEntities1();
            List<Tuotteet> model = db.Tuotteet.ToList();
            db.Dispose();
            return View(model);
        }
    }
}