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
    public class DossiersController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Dossiers
        public ActionResult Index()
        {
            var dossiers = db.Dossiers.Include(d => d.Raisons_Annulations).Include(d => d.Etats_Dossiers).Include(d => d.Personnes).Include(d => d.Voyages);
            return View(dossiers.ToList());
        }

        // GET: Dossiers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dossiers dossiers = db.Dossiers.Find(id);
            if (dossiers == null)
            {
                return HttpNotFound();
            }
            return View(dossiers);
        }

        // GET: Dossiers/Create
        public ActionResult Create()
        {
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "annulation_raison", "annulation_raison");
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "etat_dossier", "etat_dossier");
            ViewBag.numero_client = new SelectList(db.Personnes, "id_personne", "civilite");
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "id_voyage");
            return View();
        }

        // POST: Dossiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,numero_client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                db.Dossiers.Add(dossiers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "annulation_raison", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "etat_dossier", "etat_dossier", dossiers.etat);
            ViewBag.numero_client = new SelectList(db.Personnes, "id_personne", "civilite", dossiers.numero_client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "id_voyage", dossiers.voyage);
            return View(dossiers);
        }

        // GET: Dossiers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dossiers dossiers = db.Dossiers.Find(id);
            if (dossiers == null)
            {
                return HttpNotFound();
            }
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "annulation_raison", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "etat_dossier", "etat_dossier", dossiers.etat);
            ViewBag.numero_client = new SelectList(db.Personnes, "id_personne", "civilite", dossiers.numero_client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "id_voyage", dossiers.voyage);
            return View(dossiers);
        }

        // POST: Dossiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,numero_client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dossiers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "annulation_raison", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "etat_dossier", "etat_dossier", dossiers.etat);
            ViewBag.numero_client = new SelectList(db.Personnes, "id_personne", "civilite", dossiers.numero_client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "id_voyage", dossiers.voyage);
            return View(dossiers);
        }

        // GET: Dossiers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dossiers dossiers = db.Dossiers.Find(id);
            if (dossiers == null)
            {
                return HttpNotFound();
            }
            return View(dossiers);
        }

        // POST: Dossiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dossiers dossiers = db.Dossiers.Find(id);
            db.Dossiers.Remove(dossiers);
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
