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
    public class AuthentificationsController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Authentifications
        public ActionResult Index()
        {
            var authentifications = db.Authentifications.Include(a => a.Statuts);
            return View(authentifications.ToList());
        }


        public ActionResult Connexion()
        {
            Authentifications qui = new Authentifications();
            ViewBag.message = "Login + Pass";
            return View("Connexion", qui);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Connexion(Authentifications auth, Statuts stat)
        {

            if (Request.Form["f_login"] != "" && Request.Form["f_pass"] != "")
            {
                string login = Request.Form["f_login"];
                string password = Request.Form["f_pass"];

                auth.email = login;
                auth.mot_de_passe = password;

                Session["login"] = login;
                Session["password"] = password;
                auth.email = "";
                auth.mot_de_passe = "";

                
                var radioB = Request.Form["radioB"];
                stat.statut = radioB;
                Session["radioB"] = radioB;

                return RedirectToAction("../Voyages/Index");
            }
            else
            {
                ViewBag.message = "ATTENTION \nIf you want to proceed you must fill out all required fields";
                return View("Connexion", auth);
            }
        }

        // GET: Authentifications/Details/5
        public ActionResult Details(int? id)
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

        // GET: Authentifications/Create
        public ActionResult Create()
        {
            ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut");
            return View();
        }

        // POST: Authentifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_auth,email,mot_de_passe,statut")] Authentifications authentifications)
        {
            if (ModelState.IsValid)
            {
                db.Authentifications.Add(authentifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // GET: Authentifications/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // POST: Authentifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_auth,email,mot_de_passe,statut")] Authentifications authentifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authentifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut", authentifications.statut);
            return View(authentifications);
        }

        // GET: Authentifications/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Authentifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
