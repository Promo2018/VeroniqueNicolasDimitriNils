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


            if (Session["client"] != null)
            {
                var dossiers = db.Dossiers.Include(d => d.Raisons_Annulations).Include(d => d.Etats_Dossiers).Include(d => d.Personnes).Include(d => d.Voyages).Include(d => d.Raisons_Annulations.annulation_raison);
                return View(dossiers.ToList());
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
        public ActionResult Index(string nom, string prenom, string continent, string etat)
        {

            var dossier = from s in db.Dossiers.Include(d => d.Raisons_Annulations).Include(d => d.Etats_Dossiers).Include(d => d.Personnes).Include(d => d.Voyages)
                          select s;

            if (!String.IsNullOrEmpty(nom) || !String.IsNullOrEmpty(prenom) || !String.IsNullOrEmpty(continent) || !String.IsNullOrEmpty(etat))
            {
                dossier = dossier.Where(s => s.Personnes.nom.Contains(nom) && s.Personnes.prenom.Contains(prenom)
                && s.Voyages.Destinations.Continents.continent.Contains(continent));
            }
            return View(dossier.ToList());
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
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison");
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat");
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet");
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet");
            return View();
        }

        // POST: Dossiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                dossiers.etat = 1;
                dossiers.dernier_suivi = DateTime.Now;
                db.Dossiers.Add(dossiers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet", dossiers.voyage);
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
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet", dossiers.voyage);
            return View(dossiers);
        }

        // POST: Dossiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dossiers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet", dossiers.voyage);
            return View(dossiers);
        }

        // GET: Dossiers/Edit/5
        public ActionResult Suivi(int? id)
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

            Session["numero_carte_bancaire"] = dossiers.numero_carte_bancaire;
            Session["voyage"] = dossiers.voyage;
            Session["client"] = dossiers.client;

            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet", dossiers.voyage);
            return View(dossiers);
        }

        // POST: Dossiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suivi([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,client,dernier_suivi")] Dossiers dossiers)
        {

            if (ModelState.IsValid)
            {
                string radioB = Request.Form["radioB"];
                Session["radioB"] = radioB;
                dossiers.dernier_suivi = DateTime.Now;
                dossiers.numero_carte_bancaire = (string)Session["numero_carte_bancaire"];
                dossiers.voyage = (int)Session["voyage"];
                dossiers.client = (int)Session["client"];

                if (radioB == "nonSolvable")
                {
                    dossiers.etat = 3;
                    dossiers.raison_annulation = 1;
                    ViewBag.mess = "Client est insolvable, ce dossier est REJETÉ";
                    Session["radioB"] = null;
                }
                else if (radioB == "solvable")
                {
                    dossiers.etat = 2;
                    Session["radioB"] = null;

                }
                else if (radioB == "suff")
                {
                    dossiers.etat = 4;
                    Session["radioB"] = null;
                }
                else if (radioB == "insuff")
                {
                    dossiers.etat = 3;
                    dossiers.raison_annulation = 3;

                    Session["radioB"] = null;
                }
                else if (radioB == "annClient")
                {
                    dossiers.etat = 3;
                    dossiers.raison_annulation = 2;
                    ViewBag.mess = "Annulee par le client, ce dossier est REJETÉ";
                    Session["radioB"] = null;
                }

                db.Entry(dossiers).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Details", "Dossiers", new { id = dossiers.id_dossier });
            }
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "nomcomplet", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "voyagecomplet", dossiers.voyage);
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
