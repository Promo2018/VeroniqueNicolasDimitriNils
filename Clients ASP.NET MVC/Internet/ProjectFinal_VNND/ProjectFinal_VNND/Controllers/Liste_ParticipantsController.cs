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
    public class Liste_ParticipantsController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Liste_Participants
        public ActionResult Index()
        {
            var liste_Participants = db.Liste_Participants.Include(l => l.Dossiers).Include(l => l.Personnes);
            return View(liste_Participants.ToList());
        }

        // GET: Liste_Participants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Participants liste_Participants = db.Liste_Participants.Find(id);
            if (liste_Participants == null)
            {
                return HttpNotFound();
            }
            return View(liste_Participants);
        }

        // GET : Création d'une Liste_Participants (création par défaut avec les datas récupérées en session, évite de générer une page de confirmation)
        public ActionResult Create([Bind(Include = "id_listparticipant,participant,dossier")] Liste_Participants liste_Participants)
        {
            if (ModelState.IsValid)
            {
                liste_Participants.dossier = (int)Session["f_idDossier"];
                foreach (var id in Session["listParticipant"] as List<int>)
                {
                    liste_Participants.participant = id;
                    db.Liste_Participants.Add(liste_Participants);
                    db.SaveChanges();

                }
                return RedirectToAction("Create", "Liste_Assurances");
                //return RedirectToAction("Details", "Dossiers", new { id = Session["f_idDossier"] } );
            }

            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "numero_carte_bancaire", liste_Participants.dossier);
            ViewBag.participant = new SelectList(db.Personnes, "id_personne", "prenom", liste_Participants.participant);
            return View(liste_Participants);
        }

        // GET: Liste_Participants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Participants liste_Participants = db.Liste_Participants.Find(id);
            if (liste_Participants == null)
            {
                return HttpNotFound();
            }
            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "numero_carte_bancaire", liste_Participants.dossier);
            ViewBag.participant = new SelectList(db.Personnes, "id_personne", "prenom", liste_Participants.participant);
            return View(liste_Participants);
        }

        // POST: Liste_Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_listparticipant,participant,dossier")] Liste_Participants liste_Participants)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liste_Participants).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "numero_carte_bancaire", liste_Participants.dossier);
            ViewBag.participant = new SelectList(db.Personnes, "id_personne", "prenom", liste_Participants.participant);
            return View(liste_Participants);
        }

        // GET: Liste_Participants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Participants liste_Participants = db.Liste_Participants.Find(id);
            if (liste_Participants == null)
            {
                return HttpNotFound();
            }
            return View(liste_Participants);
        }

        // POST: Liste_Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Liste_Participants liste_Participants = db.Liste_Participants.Find(id);
            db.Liste_Participants.Remove(liste_Participants);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET : Suppression de la liste de participants dans le cas de l'annulation d'une réservation en cours 
        public ActionResult DossNonConf(int? id)
        {
            int idDossier = (int)Session["f_idDossier"];

            List<int> listeid = new List<int>();
            foreach (Liste_Participants a in db.Liste_Participants)
            {
                if (a.dossier == idDossier)
                {
                    listeid.Add(a.id_listparticipant);
                }
            }

            foreach (var idrecup in listeid)
            {
                Liste_Participants liste_Participants = db.Liste_Participants.Find(idrecup);
                if (liste_Participants == null)
                {
                    return RedirectToAction("DossNonConf", "Personnes", new { id = Session["f_idDossier"] });
                }
                db.Liste_Participants.Remove(liste_Participants);

            }
            db.SaveChanges();
            return RedirectToAction("DossNonConf", "Personnes", new { id = Session["f_idDossier"] });

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
