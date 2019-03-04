using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            if (Session["client"] != null)
            {
                Authentifications authcli=new Authentifications();
                Personnes client = Session["client"] as Personnes;
                foreach (Authentifications a in db.Authentifications)
                {
                    if (a.email == client.email)
                    {
                        authcli = a;
                    }
                }

                List<Authentifications> moi = new List<Authentifications>();
                moi.Add(authcli);

            return View(moi); }
            else
            {   
                ViewBag.message="Veuillez vous connecter pour acceder à cette page";
                Authentifications auth = new Authentifications();
                return RedirectToAction("../Authentifications/Connexion", auth); ;
            }
        }

        public ActionResult Connexion()
        {
            Authentifications qui = new Authentifications();

            return View("Connexion", qui);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Connexion(Authentifications auth)
        {
            try
            {
                if (Request.Form["f_login"] != "" && Request.Form["f_pass"] != "")
                {
                    //on recupere identifiant et mdp entrés dans la page
                    string login = Request.Form["f_login"];
                    string password = Request.Form["f_pass"];
                    //on aurait aussi pu faire ca : 
                    //auth.email = login;
                    /*auth.mot_de_passe = password;
                     */

                    //certains caracteres font bugger le champ mot de passe
                    List<string> interdits = new List<string>() { "<", ">" };
                    foreach (string i in interdits)
                    {
                        if (password.Contains(i))
                        {
                            ViewBag.message = "### Votre mot de passe ne doit pas contenir le caractère " + i + " ###";
                            return View("Connexion", auth);
                        }

                    }
                    /*
                    liste complete des authentification */
                    var listauth = db.Authentifications;
                    //on verifie que l'email existe en tant que login dans la table des authentifications
                    if (listauth.Any(i => i.email == login))
                    {
                        // on recupere l'objet authentification dans une liste
                        List<Authentifications> listauth2 = new List<Authentifications>();
                        foreach (Authentifications a in listauth.ToList() as List<Authentifications>)
                        {
                            if (a.email == login)
                            {
                                listauth2.Add(a);
                            }
                        }
                        //on verifie que cet e-mail ne correspond qu'a un login dans la BDD
                        if (listauth2.Count == 1)
                        {
                            // on recupere l'objet authentification
                            Authentifications emailvalid = listauth2[0];
                            //on verifie que 
                            if (emailvalid.mot_de_passe == password)
                            {
                                // on veridfie que c'est bien un client qui se connecte
                                if (emailvalid.Statuts.statut != "Client")
                                {
                                    ViewBag.message = "### Veuillez utiliser un \"Compte client\" pour vous connecter à l'application pour les clients ###";
                                    return View("Connexion", auth);
                                }

                                else
                                {
                                    // on verifie que le client qui se connecte existe dans la table "personnes"
                                    if (db.Personnes.Any(i => i.email == emailvalid.email))
                                    {
                                        // on recupere l'objet personne qui correspond au client sous forme de liste
                                        List<Personnes> clients = new List<Personnes>();
                                        foreach (Personnes p in db.Personnes.Include(p=>p.Civilites).Include(p => p.OuisNons).Include(p => p.OuisNons1))
                                        {
                                            if (p.email == emailvalid.email)
                                            {
                                                clients.Add(p);
                                            }
                                        }
                                        // on verifie l'unicité de l'email dans la BDD
                                        if (clients.Count == 1)
                                        {

                                            Personnes client = clients[0];
                                            // on recupere toutes les infos dans la session
                                            Session["login"] = emailvalid.email;
                                            Session["client"] = client;
                                            return RedirectToAction("../Voyages/Index");

                                        }
                                        else
                                        {
                                            ViewBag.message = " Erreur aucune ou plusieurs personnes avec cet e-mail dans la BDD###";
                                            return View("Connexion", auth);
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.message = "### Erreur pas de Personne correspondante dans la BDD ###";
                                        return View("Connexion", auth);

                                    }
                                }
                            }
                            else
                            {
                                ViewBag.message = "Mot de Passe incorrecte";
                                return View("Connexion", auth);
                            }
                        }
                        else
                        {
                            ViewBag.message = "### Erreur plusieurs correspondances pour cet e-mail dans la base de données ###";
                            return View("Connexion", auth);

                        }

                    }
                else
                    {
                    ViewBag.message = "Ce compte n'existe pas, veuillez vous inscrire pour créer un compte ou corriger votre identifiant";
                    return View("Connexion", auth);

                    }

                }
                else
                {
                ViewBag.message = "ATTENTION \n Vous devez entrer un mot de passe et un email!";
                return View("Connexion", auth);

                }

            }
            catch (Exception erreur)
            {
                ViewBag.message = "#### Erreur de connexion inconnue ###"+"\n\n\t\t"+erreur;
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

            try
            {

                if (ModelState.IsValid)
                {

                    if (authentifications.email == "" || authentifications.email == null || authentifications.mot_de_passe == "" || authentifications.mot_de_passe == null)
                    {
                        ViewBag.Message = "Veuillez insérer un identifiant et un mot de passe!";
                        ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut");
                        return View(authentifications);
                    }

                    else if (db.Personnes.Any(i => i.email == authentifications.email))
                    {
                        ViewBag.Message4 = "Cette personne existe déjà dans notre base de données, veuillez utiliser un autre email ou contacter notre service clientèle ...";
                        ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut");
                        return View(authentifications);
                    }

                    else
                    {
                        authentifications.statut = 1;                       // forcing status = CLIENT (statut = 1)

                        db.Authentifications.Add(authentifications);
                        db.SaveChanges();
                        Session["login"] = authentifications.email;
                        return RedirectToAction("../Personnes/Create");    // create/personnes

                    }
                }
                else
                {
                    ViewBag.statut = new SelectList(db.Statuts, "id_statut", "statut");
                    return View(authentifications);
                }

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ViewBag.Message2 = "Cet email est déjà enregistré dans la base de données, veuillez insérer un autre email s'il vous plaît...";
                return View(authentifications);
            }
            //catch (System.Data.Entity.Validation.DbEntityValidationException)
            //{
            //    ViewBag.Message = "Veuillez insérer un identifiant et un mot de passe!";
            //    return View(authentifications);
            //}
            catch (Exception)
            {
                ViewBag.Message3 = "Unknown (yet) Error";
                return View(authentifications);
            }

        }

        public ActionResult LogOff(Authentifications authentifications)
        {
            Session.Clear();

            return RedirectToAction("Index", "Voyages");
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
                authentifications.statut = 1;
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
            return RedirectToAction("LogOff", "Authentifications");
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
