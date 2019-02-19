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
    public class DestinationsController : Controller
    {
        private BoVoyage_VNNDEntities db = new BoVoyage_VNNDEntities();

        // GET: Destinations
        public ActionResult Index()
        {
            var destinations = db.Destinations.Include(d => d.Continents);
            return View(destinations.ToList());
        }

        [HttpPost]
        public ActionResult Index(string continent, string pays, string region)
        {

            var destination = from s in db.Destinations
                              select s;

            if (!String.IsNullOrEmpty(continent) || !String.IsNullOrEmpty(pays) || !String.IsNullOrEmpty(region))
            {
                destination = destination.Where(s => s.continent.Contains(continent) && s.pays.Contains(pays) && s.region.Contains(region));
            }

            return View(destination.ToList());
        }

        // GET: Destinations/Informations/5
        public ActionResult Informations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinations destinations = db.Destinations.Find(id);
            if (destinations == null)
            {
                return HttpNotFound();
            }
            return View(destinations);
        }

        // GET: Destinations/Sauvegarder
        public ActionResult Sauvegarder()
        {
            ViewBag.continent = new SelectList(db.Continents, "continent", "continent");
            return View();
        }

        // POST: Destinations/Sauvegarder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sauvegarder([Bind(Include = "id_destination,continent,pays,region,descriptif")] Destinations destinations)
        {
            if (ModelState.IsValid)
            {
                db.Destinations.Add(destinations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.continent = new SelectList(db.Continents, "continent", "continent", destinations.continent);
            return View(destinations);
        }

        // GET: Destinations/Modifier/5
        public ActionResult Modifier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinations destinations = db.Destinations.Find(id);
            if (destinations == null)
            {
                return HttpNotFound();
            }
            ViewBag.continent = new SelectList(db.Continents, "continent", "continent", destinations.continent);
            return View(destinations);
        }

        // POST: Destinations/Modifier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more Informations see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifier([Bind(Include = "id_destination,continent,pays,region,descriptif")] Destinations destinations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(destinations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.continent = new SelectList(db.Continents, "continent", "continent", destinations.continent);
            return View(destinations);
        }

        // GET: Destinations/Supprimer/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinations destinations = db.Destinations.Find(id);
            if (destinations == null)
            {
                return HttpNotFound();
            }
            return View(destinations);
        }

        // POST: Destinations/Supprimer/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Destinations destinations = db.Destinations.Find(id);
            db.Destinations.Remove(destinations);
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
