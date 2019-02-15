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
    public class PersonnesController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Personnes
        public ActionResult Index()
        {
            var personnes = db.Personnes.Include(p => p.Civilites);
            return View(personnes.ToList());
        }

        // GET: Personnes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnes personnes = db.Personnes.Find(id);
            if (personnes == null)
            {
                return HttpNotFound();
            }
            return View(personnes);
        }

        // GET: Personnes/Create
        public ActionResult Create()
        {
            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite");
            return View();
        }

        // POST: Personnes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_personne,civilite,nom,prenom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                db.Personnes.Add(personnes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite", personnes.civilite);
            return View(personnes);
        }

        // GET: Personnes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnes personnes = db.Personnes.Find(id);
            if (personnes == null)
            {
                return HttpNotFound();
            }
            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite", personnes.civilite);
            return View(personnes);
        }

        // POST: Personnes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_personne,civilite,nom,prenom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personnes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite", personnes.civilite);
            return View(personnes);
        }

        // GET: Personnes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnes personnes = db.Personnes.Find(id);
            if (personnes == null)
            {
                return HttpNotFound();
            }
            return View(personnes);
        }

        // POST: Personnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personnes personnes = db.Personnes.Find(id);
            db.Personnes.Remove(personnes);
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
