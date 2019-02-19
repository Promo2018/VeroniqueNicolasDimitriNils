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
            var personnes = db.Personnes.Include(p => p.Civilites).Include(p => p.OuisNons).Include(p => p.OuisNons1);
            return View(personnes.ToList());
        }

        [HttpPost]
        public ActionResult Index(string nom, string prenom)
        {

            var personnes = from s in db.Personnes
                            select s;

            if (!String.IsNullOrEmpty(nom) || !String.IsNullOrEmpty(prenom))
            {
                personnes = personnes.Where(s => s.nom.Contains(nom) && s.prenom.Contains(prenom));
            }

            return View(personnes.ToList());
        }

        // GET: Personnes/Informations/5
        public ActionResult Informations(int? id)
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

        // GET: Personnes/Sauvegarder
        public ActionResult Sauvegarder()
        {
            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite");
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            return View();
        }

        // POST: Personnes/Sauvegarder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sauvegarder([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                db.Personnes.Add(personnes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // GET: Personnes/Modifier/5
        public ActionResult Modifier(int? id)
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
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // POST: Personnes/Modifier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personnes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.civilite = new SelectList(db.Civilites, "civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // GET: Personnes/Supprimer/5
        public ActionResult Supprimer(int? id)
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

        // POST: Personnes/Supprimer/5
        [HttpPost, ActionName("Supprimer")]
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
