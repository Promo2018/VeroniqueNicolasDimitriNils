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

        // Affichage de tout les détails de la réservation du voyage avant le payement, afin que le client puisse voir le prix
        public ActionResult DetailsConfirmation(int? id)
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

            string confimer = Request.Form["Conf"];
            if (confimer == "Oui")
            {
                return RedirectToAction("Confirmation", "Dossiers", new { id = Session["f_idDossier"] });
            }
            if (confimer == "Non")
            {
                return RedirectToAction("DossNonConf", "Liste_Participants", new { id = Session["f_idDossier"] });
            }

            return View(dossiers);
        }

        // GET : Création d'un dossier (création par défaut avec les datas récupérées en session, évite de générer une page de confirmation)
        public ActionResult Create([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                dossiers.numero_carte_bancaire = "";
                dossiers.etat = 1;
                dossiers.dernier_suivi = DateTime.Now;
                dossiers.voyage = (int)Session["f_idvoyage"];
                dossiers.client = (Session["client"] as Personnes).id_personne;
                db.Dossiers.Add(dossiers);
                db.SaveChanges();
                Session["f_idDossier"] = dossiers.id_dossier;
                return RedirectToAction("Create", "Liste_Participants");
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


        // confirmation du dossier avec l'entrée de la CB qui est en fait un Edit au niveau du dossier avec uniquement le champ de la CB. Le dossier a été crée avant dans le processus
        // de réservation d'un voyage (création d'un dossier)
        public ActionResult Confirmation(int? id)
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
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "id_personne", dossiers.client);
            ViewBag.voyage = new SelectList(db.Voyages, "id_voyage", "id_voyage", dossiers.voyage);
            return View(dossiers);
        }

        // POST: Dossiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation([Bind(Include = "id_dossier,numero_carte_bancaire,raison_annulation,etat,voyage,client,dernier_suivi")] Dossiers dossiers)
        {
            if (ModelState.IsValid)
            {
                dossiers.etat = 1;
                dossiers.dernier_suivi = DateTime.Now;
                dossiers.voyage = (int)Session["f_idvoyage"];
                dossiers.client = (Session["client"] as Personnes).id_personne;
                db.Entry(dossiers).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ChangeStatutClient", "Personnes", new { id = (int)Session["f_idvoyage"] });
            }
            ViewBag.raison_annulation = new SelectList(db.Raisons_Annulations, "id_annul", "annulation_raison", dossiers.raison_annulation);
            ViewBag.etat = new SelectList(db.Etats_Dossiers, "id_etat", "etat", dossiers.etat);
            ViewBag.client = new SelectList(db.Personnes, "id_personne", "prenom", dossiers.client);
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


        //GET : Suppression du dossier dans le cas de l'annulation d'une réservation en cours (réservation = dossier)
        public ActionResult DossNonConf(int? id)
        {
            

            if (Session["f_idDossier"] == null)
            {
                Session.Remove("f_idassurance");
                Session.Remove("f_idvoyage");
                Session.Remove("f_voyage");
                Session.Remove("listParticipant");
                Session.Remove("f_place");
                Session.Remove("nbParticipant");

                return RedirectToAction("Index", "Voyages");
            }
            id = (int)Session["f_idDossier"];
            Dossiers dossiers = db.Dossiers.Find(id);
            db.Dossiers.Remove(dossiers);
            db.SaveChanges();

            Session.Remove("f_idassurance");
            Session.Remove("f_idvoyage");
            Session.Remove("f_voyage");
            Session.Remove("listParticipant");
            Session.Remove("f_place");
            Session.Remove("nbParticipant");
            Session.Remove("f_idDossier");

            return RedirectToAction("Index", "Voyages");
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
