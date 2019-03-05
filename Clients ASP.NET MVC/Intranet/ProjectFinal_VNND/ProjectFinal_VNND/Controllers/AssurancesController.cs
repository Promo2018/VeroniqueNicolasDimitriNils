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
            if (Session["login"] != null)
            {
                return View(db.Assurances.ToList());
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

        // GET: Assurances/Details/5
        public ActionResult Details(int? id)
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

        // GET: Assurances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Assurances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_assurance,libelle,prix,descriptif")] Assurances assurances)
        {
            if (ModelState.IsValid)
            {
                db.Assurances.Add(assurances);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assurances);
        }

        // GET: Assurances/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Assurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_assurance,libelle,prix,descriptif")] Assurances assurances)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assurances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assurances);
        }

        // GET: Assurances/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Assurances/Delete/5
        [HttpPost, ActionName("Delete")]
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
