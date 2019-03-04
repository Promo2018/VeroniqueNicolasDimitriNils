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
            
            int idclient = (int)(Session["client"] as Personnes).id_personne;
            return View(personnes.Where(i => i.id_personne == idclient).ToList());
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

        // GET: Personnes/Details/5
        public ActionResult Details(int? id)
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


        // Inscription d'un "Client"
        // GET: Personnes/Create
        public ActionResult Create()
        {
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite");
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            return View();
        }

        //// POST: Personnes/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        personnes.client = 1;
        //        personnes.participant = 1;
        //        db.Personnes.Add(personnes);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
        //    ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
        //    ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
        //    return View(personnes);
        //}
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant")] Personnes personnes)
        {
            Authentifications auth = new Authentifications();
            personnes.email = (string)Session["login"];     /*emailclient;*/

            if (ModelState.IsValid)
            {
                personnes.client = 1;
                personnes.participant = 1;
                db.Personnes.Add(personnes);
                db.SaveChanges();

                // on verifie que le client que l'on viens de creer existe dans la table "personnes"
                if (db.Personnes.Any(i => i.email == personnes.email))
                {
                    // on recupere l'objet personne qui correspond au client sous forme de liste
                    List<Personnes> clients = new List<Personnes>();
                    foreach (Personnes p in db.Personnes.Include(p => p.Civilites).Include(p => p.OuisNons).Include(p => p.OuisNons1))
                    {
                        if (p.email == personnes.email)
                        {
                            clients.Add(p);
                        }
                    }
                    // on verifie l'unicité de l'email dans la BDD
                    if (clients.Count == 1)
                    {

                        Personnes client = clients[0];
                        // on recupere toutes les infos dans la session
 
                        Session["client"] = client;

                        ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
                        ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
                        ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);

                        return RedirectToAction("../Voyages/Index");

                    }
                    else
                    {
                        ViewBag.message = " Erreur aucune ou plusieurs personnes avec cet e-mail dans la BDD###";
                        return RedirectToAction("../Voyages/Index");
                    }
                }
                else
                {
                    ViewBag.message = "### Erreur pas de Personne correspondante dans la BDD ###";
                    return RedirectToAction("../Voyages/Index");

                }

            }
            return RedirectToAction("../Voyages/Index");

        }

        //Ajout ou non du client dans la liste des participants qui est mise en session. Cela depend de sa réponse, soit il participe et paie le voyage, soit il paie le voyage uniquement.
        public ActionResult ClientPartic()
        {
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite");
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientPartic([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {

                List<int> Participants = Session["listParticipant"] as List<int>;
                string clientPart = Request.Form["f_reponse"];
                if (Participants == null)
                {
                    Participants = new List<int>();
                    if (clientPart == "Oui")
                    {
                        personnes.id_personne = (Session["client"] as Personnes).id_personne;
                        Participants.Add(personnes.id_personne);
                        Session["listParticipant"] = Participants;
                        Session["nbParticipant"] = Participants.Count;
                        return RedirectToAction("CreerParticipant");
                    }
                    if (clientPart == "Non")
                    {
                        return RedirectToAction("CreerParticipant");
                    }
                }

                return RedirectToAction("Create");
            }
            return View(personnes);
        }

        //Ajout des participants pour un voyage
        // GET: Personnes/Create
        public ActionResult CreerParticipant()
        {
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite");
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur");
            return View();
        }

        // POST: Personnes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreerParticipant([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {

                List<int> Participants = Session["listParticipant"] as List<int>;

                if (Participants == null)
                {
                    Participants = new List<int>();
                }

                if (personnes.email != null && personnes.email != "")
                {
                    if (db.Personnes.Any(i => i.email == personnes.email ))
                        {

                            ViewBag.message1 = " Erreur aucune ou plusieurs personnes avec cet e-mail dans la BDD###";
                            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
                            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
                            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
                            return View(personnes);
                        }
                }

                personnes.client = 1;
                personnes.participant = 2;
                db.Personnes.Add(personnes);
                db.SaveChanges();
                Participants.Add(personnes.id_personne);

                Session["listParticipant"] = Participants;
                Session["nbParticipant"] = Participants.Count;

                if (Participants.Count < 9)
                {
                    if (Participants.Count < (int)Session["f_place"])
                    {

                    }
                    else
                    {
                        ViewBag.message = " Attention : Vous ne pouvez plus ajouter de participants. Le nombre de place disponible serait insuffisant";
                        return View(personnes);
                    }

                }
                else
                {
                    ViewBag.message = " Attention : vous avez atteint le seuil maximal de 9 participants pour une réservation. Vous ne pouvez donc pas en ajouter plus. ";
                    return View(personnes);
                }
                return RedirectToAction("CreerParticipant");
            }
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // GET: Personnes/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // POST: Personnes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                personnes.id_personne = (Session["client"] as Personnes).id_personne;
                personnes.email = (Session["client"] as Personnes).email;
                personnes.client = (Session["client"] as Personnes).client;
                personnes.participant = (Session["client"] as Personnes).participant;
                db.Entry(personnes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }

        // POST: Personnes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        // GET : Modification automatique du statut client de Non à Oui lorsque la personne paie la réservation. 
        public ActionResult ChangeStatutClient([Bind(Include = "id_personne,civilite,prenom,nom,adresse,telephone,date_naissance,client,participant,email")] Personnes personnes)
        {
            if (ModelState.IsValid)
            {
                personnes.client = 2;              
                personnes.id_personne = (Session["client"] as Personnes).id_personne;
                personnes.civilite = (Session["client"] as Personnes).civilite;
                personnes.prenom = (Session["client"] as Personnes).prenom;
                personnes.nom = (Session["client"] as Personnes).nom;
                personnes.date_naissance = (Session["client"] as Personnes).date_naissance;
                personnes.adresse = (Session["client"] as Personnes).adresse;
                personnes.telephone = (Session["client"] as Personnes).telephone;
                personnes.email = (Session["client"] as Personnes).email;
                personnes.participant = (Session["client"] as Personnes).participant;

                db.Entry(personnes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SoustractionPlace", "Voyages", new { id = (int)Session["f_idvoyage"] });
            }
            ViewBag.civilite = new SelectList(db.Civilites, "id_civilite", "civilite", personnes.civilite);
            ViewBag.client = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.client);
            ViewBag.participant = new SelectList(db.OuisNons, "id_ouinon", "valeur", personnes.participant);
            return View(personnes);
        }


        // GET: Personnes/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Personnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personnes personnes = db.Personnes.Find(id);
            db.Personnes.Remove(personnes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET : Suppression des personnes créées (participants au voyage) dans le cas de l'annulation d'une réservation en cours
        public ActionResult DossNonConf(int? id)
        {
            if (Session["listParticipant"] == null)
            {
                return RedirectToAction("DossNonConf", "Liste_Assurances", new { id = Session["f_idDossier"] });
            }

            foreach (var idliste in Session["listParticipant"] as List<int>)
            {

                id = idliste;
                if (id != (Session["client"] as Personnes).id_personne)
                {
                    Personnes personnes = db.Personnes.Find(id);
                    if (personnes == null)
                    {
                        return RedirectToAction("DossNonConf", "Liste_Assurances", new { id = Session["f_idDossier"] });
                    }
                    db.Personnes.Remove(personnes);
                }
                db.SaveChanges();
            }
            return RedirectToAction("DossNonConf", "Liste_Assurances", new { id = Session["f_idDossier"] });
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
