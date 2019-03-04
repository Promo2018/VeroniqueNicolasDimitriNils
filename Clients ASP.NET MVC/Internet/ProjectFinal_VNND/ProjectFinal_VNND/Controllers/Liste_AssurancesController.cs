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
    public class Liste_AssurancesController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Liste_Assurances
        public ActionResult Index()
        {
            var liste_Assurances = db.Liste_Assurances.Include(l => l.Assurances).Include(l => l.Dossiers);
            return View(liste_Assurances.ToList());
        }

        // GET: Liste_Assurances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Assurances liste_Assurances = db.Liste_Assurances.Find(id);
            if (liste_Assurances == null)
            {
                return HttpNotFound();
            }
            return View(liste_Assurances);
        }

        // GET : Création d'une Liste_Assurance (création par défaut avec les datas récupérées en session, évite de générer une page de confirmation)
        public ActionResult Create([Bind(Include = "id_listassurance,assurance,dossier")] Liste_Assurances liste_Assurances)
        {
            if (Session["f_idassurance"] == null)
            {
                return RedirectToAction("DetailsConfirmation", "Dossiers", new { id = Session["f_idDossier"] });
            }
            if (ModelState.IsValid)
            {
                liste_Assurances.dossier = (int)Session["f_idDossier"];
                liste_Assurances.assurance = Convert.ToInt32(Session["f_idassurance"]);
                db.Liste_Assurances.Add(liste_Assurances);
                db.SaveChanges();

                //return RedirectToAction("Create", "Liste_Participants");
                return RedirectToAction("DetailsConfirmation", "Dossiers", new { id = Session["f_idDossier"] });
            }
            ViewBag.assurance = new SelectList(db.Assurances, "id_assurance", "libelle", liste_Assurances.assurance);
            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "id_dossier", liste_Assurances.dossier);
            return View(liste_Assurances);
        }

        // GET: Liste_Assurances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Assurances liste_Assurances = db.Liste_Assurances.Find(id);
            if (liste_Assurances == null)
            {
                return HttpNotFound();
            }
            ViewBag.assurance = new SelectList(db.Assurances, "id_assurance", "libelle", liste_Assurances.assurance);
            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "numero_carte_bancaire", liste_Assurances.dossier);
            return View(liste_Assurances);
        }

        // POST: Liste_Assurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_listassurance,assurance,dossier")] Liste_Assurances liste_Assurances)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liste_Assurances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.assurance = new SelectList(db.Assurances, "id_assurance", "libelle", liste_Assurances.assurance);
            ViewBag.dossier = new SelectList(db.Dossiers, "id_dossier", "numero_carte_bancaire", liste_Assurances.dossier);
            return View(liste_Assurances);
        }

        // GET: Liste_Assurances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liste_Assurances liste_Assurances = db.Liste_Assurances.Find(id);
            if (liste_Assurances == null)
            {
                return HttpNotFound();
            }
            return View(liste_Assurances);
        }

        // POST: Liste_Assurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Liste_Assurances liste_Assurances = db.Liste_Assurances.Find(id);
            db.Liste_Assurances.Remove(liste_Assurances);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET : Suppression de la liste d'assurance dans le cas de l'annulation d'une réservationen cours
        public ActionResult DossNonConf(int? id)
        {
            id = Convert.ToInt32(Session["f_idassurance"]);
            Liste_Assurances liste_Assurances = db.Liste_Assurances.Find(id);
            if (liste_Assurances == null)
            {
                return RedirectToAction("DossNonConf", "Dossiers", new { id = Session["f_idDossier"] });
            }
            db.Liste_Assurances.Remove(liste_Assurances);
            db.SaveChanges();
            return RedirectToAction("DossNonConf", "Dossiers", new { id = Session["f_idDossier"] });
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
