﻿using System;
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
            Authentifications qui = new Authentifications();
            ViewBag.message = "Login + Pass";
            return View("Index", qui);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(Authentifications auth)
        {

            if (Request.Form["f_login"] != "" && Request.Form["f_pass"] != "")
            {
                string login = Request.Form["f_login"];
                string password = Request.Form["f_pass"];

                auth.email = login;
                auth.mot_de_passe = password;

                Session["login"] = login;
                Session["password"] = password;
                //Request.Form["f_login"] = "";
                //Request.Form["f_pass"] = "";
                auth.email = "";
                auth.mot_de_passe = "";

                return RedirectToAction("../Voyages/Index");
            }
            else
            {
                ViewBag.message = "ATTENTION \nIf you want to proceed you must fill out all required fields";
                return View("Index", auth);
            }
        }

        // GET: Authentifications/Informations/5
        public ActionResult Informations(string id)
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

        // GET: Authentifications/Sauvegarder
        public ActionResult Sauvegarder()
        {
            ViewBag.statut = new SelectList(db.Statuts, "statut", "statut");
            return View();
        }

        // POST: Authentifications/Sauvegarder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sauvegarder([Bind(Include = "email,mot_de_passe,statut")] Authentifications authentifications)
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

        // GET: Authentifications/Modifier/5
        public ActionResult Modifier(string id)
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

        // POST: Authentifications/Modifier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "email,mot_de_passe,statut")] Authentifications authentifications)
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

        // GET: Authentifications/Supprimer/5
        public ActionResult Supprimer(string id)
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

        // POST: Authentifications/Supprimer/5
        [HttpPost, ActionName("Supprimer")]
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
