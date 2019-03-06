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

            if (Session["login"] != null)
            {
                return View(db.Agences.OrderBy(a => a.id_agence).ToList());
            }
            else
            //redirection vers connexion si pas connecté
            {
                string message = "Veuillez vous connecter pour acceder à cette page";
                Authentifications auth = new Authentifications();
                return RedirectToAction("Connexion", "Authentifications", new { auth, message }); ;
            }

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

            return View(agences.OrderBy(a => a.id_agence).ToList());
        }

        // GET: Agences/Details/5
        public ActionResult Details(int? id)
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

        // GET: Agences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_agence,agence")] Agences agences)
        {
            if (ModelState.IsValid)
            {
                db.Agences.Add(agences);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agences);
        }

        // GET: Agences/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Agences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_agence,agence")] Agences agences)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agences).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agences);
        }

        // GET: Agences/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Agences/Delete/5
        [HttpPost, ActionName("Delete")]
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
