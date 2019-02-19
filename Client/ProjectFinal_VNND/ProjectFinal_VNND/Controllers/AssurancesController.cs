using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectFinal_VNND.Models;

namespace ProjectFinal_VNND.Controllers
{
    public class AssurancesController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Assurances
        public ActionResult Index()
        {
            return View(db.Assurances.ToList());
        }

        [HttpPost]
        public ActionResult Index(string type)
        {

            var assurances = from s in db.Assurances
                            select s;

            if (!String.IsNullOrEmpty(type))
            {
                assurances = assurances.Where(s => s.libelle.Contains(type));
            }

            return View(assurances.ToList());
        }

        // GET: Assurances/Informations/5
        public ActionResult Informations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assurances assurances = db.Assurances.Find(id);
            if (assurances == null)
            {
                return HttpNotFound();
            }
            return View(assurances);
        }

        // GET: Assurances/Sauvegarder
        public ActionResult Sauvegarder()
        {
            return View();
        }

        // POST: Assurances/Sauvegarder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sauvegarder([Bind(Include = "id_assurance,libelle,prix,descriptif")] Assurances assurances)
        {
            if (ModelState.IsValid)
            {
                db.Assurances.Add(assurances);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assurances);
        }

        // GET: Assurances/Modifier/5
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assurances assurances = db.Assurances.Find(id);
            if (assurances == null)
            {
                return HttpNotFound();
            }
            return View(assurances);
        }

        // POST: Assurances/Modifier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id_assurance,libelle,prix,descriptif")] Assurances assurances)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assurances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assurances);
        }

        // GET: Assurances/Supprimer/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assurances assurances = db.Assurances.Find(id);
            if (assurances == null)
            {
                return HttpNotFound();
            }
            return View(assurances);
        }

        // POST: Assurances/Supprimer/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assurances assurances = db.Assurances.Find(id);
            db.Assurances.Remove(assurances);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
