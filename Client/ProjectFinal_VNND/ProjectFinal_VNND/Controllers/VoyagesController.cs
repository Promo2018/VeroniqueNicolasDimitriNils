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
    public class VoyagesController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Voyages
        public ActionResult Index()
        {
            var voyages = db.Voyages.Include(v => v.Agences).Include(v => v.Destinations);
            return View(voyages.ToList());
        }

        // GET: Voyages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voyages voyages = db.Voyages.Find(id);
            if (voyages == null)
            {
                return HttpNotFound();
            }
            return View(voyages);
        }

        // GET: Voyages/Create
        public ActionResult Create()
        {
            ViewBag.agence = new SelectList(db.Agences, "id_agence", "agence");
            ViewBag.destination = new SelectList(db.Destinations, "id_destination", "continent");
            return View();
        }

        // POST: Voyages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_voyage,date_aller,date_retour,places_disponibles,tarif_tout_compris,agence,destination")] Voyages voyages)
        {
            if (ModelState.IsValid)
            {
                db.Voyages.Add(voyages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.agence = new SelectList(db.Agences, "id_agence", "agence", voyages.agence);
            ViewBag.destination = new SelectList(db.Destinations, "id_destination", "continent", voyages.destination);
            return View(voyages);
        }

        // GET: Voyages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voyages voyages = db.Voyages.Find(id);
            if (voyages == null)
            {
                return HttpNotFound();
            }
            ViewBag.agence = new SelectList(db.Agences, "id_agence", "agence", voyages.agence);
            ViewBag.destination = new SelectList(db.Destinations, "id_destination", "continent", voyages.destination);
            return View(voyages);
        }

        // POST: Voyages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_voyage,date_aller,date_retour,places_disponibles,tarif_tout_compris,agence,destination")] Voyages voyages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voyages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.agence = new SelectList(db.Agences, "id_agence", "agence", voyages.agence);
            ViewBag.destination = new SelectList(db.Destinations, "id_destination", "continent", voyages.destination);
            return View(voyages);
        }

        // GET: Voyages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voyages voyages = db.Voyages.Find(id);
            if (voyages == null)
            {
                return HttpNotFound();
            }
            return View(voyages);
        }

        // POST: Voyages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voyages voyages = db.Voyages.Find(id);
            db.Voyages.Remove(voyages);
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
