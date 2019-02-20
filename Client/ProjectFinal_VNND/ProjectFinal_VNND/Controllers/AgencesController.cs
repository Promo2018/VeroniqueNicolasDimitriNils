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
    public class AgencesController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Agences
        public ActionResult Index()
        {
            return View(db.Agences.ToList());
        }

        [HttpPost]
        public ActionResult Index(string nomAgence)
        {

            var agences = from s in db.Agences
                          select s;

            if (!String.IsNullOrEmpty(nomAgence))
            {
                agences = agences.Where(s => s.agence.Contains(nomAgence));
            }


            return View(agences.ToList());
        }

        // GET: Agences/Informations/5
        public ActionResult Informations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agences agences = db.Agences.Find(id);
            if (agences == null)
            {
                return HttpNotFound();
            }
            return View(agences);
        }

        // GET: Agences/Sauvegarder
        public ActionResult Sauvegarder()
        {
            return View();
        }

        // POST: Agences/Sauvegarder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sauvegarder([Bind(Include = "id_agence,agence")] Agences agences)
        {
            if (ModelState.IsValid)
            {
                db.Agences.Add(agences);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agences);
        }

        // GET: Agences/Modifier/5
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agences agences = db.Agences.Find(id);
            if (agences == null)
            {
                return HttpNotFound();
            }
            return View(agences);
        }

        // POST: Agences/Modifier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id_agence,agence")] Agences agences)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agences).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agences);
        }

        // GET: Agences/Supprimer/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agences agences = db.Agences.Find(id);
            if (agences == null)
            {
                return HttpNotFound();
            }
            return View(agences);
        }

        // POST: Agences/Supprimer/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agences agences = db.Agences.Find(id);
            db.Agences.Remove(agences);
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
