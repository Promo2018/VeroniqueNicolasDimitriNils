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
    public class Authentifications1Controller : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Authentifications1
        public ActionResult Index()
        {
            var authentifications = db.Authentifications.Include(a => a.Statuts);
            return View(authentifications.ToList());
        }

        // GET: Authentifications1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authentifications authentifications = db.Authentifications.Find(id);
            if (authentifications == null)
            {
                return HttpNotFound();
            }
            return View(authentifications);
        }

        // GET: Authentifications1/Create
        public ActionResult Create()
        {
            ViewBag.statut = new SelectList(db.Statuts, "statut", "statut");
            return View();
        }

        // POST: Authentifications1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email,mot_de_passe,statut")] Authentifications authentifications)
        {
            if (ModelState.IsValid)
            {
                db.Authentifications.Add(authentifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.statut = new SelectList(db.Statuts, "statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // GET: Authentifications1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authentifications authentifications = db.Authentifications.Find(id);
            if (authentifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.statut = new SelectList(db.Statuts, "statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // POST: Authentifications1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,mot_de_passe,statut")] Authentifications authentifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authentifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.statut = new SelectList(db.Statuts, "statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // GET: Authentifications1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authentifications authentifications = db.Authentifications.Find(id);
            if (authentifications == null)
            {
                return HttpNotFound();
            }
            return View(authentifications);
        }

        // POST: Authentifications1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Authentifications authentifications = db.Authentifications.Find(id);
            db.Authentifications.Remove(authentifications);
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
