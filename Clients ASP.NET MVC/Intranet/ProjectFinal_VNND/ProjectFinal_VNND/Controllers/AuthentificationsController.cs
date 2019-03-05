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
            if (Session["login"] != null)
            {
                var authentifications = db.Authentifications.Include(a => a.Statuts);
                return View(authentifications.ToList());

            }
            else
            {
                string message = "Veuillez vous connecter pour acceder à cette page";
                Authentifications auth = new Authentifications();
                return RedirectToAction("Connexion", "Authentifications", new { auth, message }); ;
            }

        }
        // GET: Connexion
        public ActionResult Connexion()
        {
            Authentifications qui = new Authentifications();

            return View("Connexion", qui);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Connexion(Authentifications auth)
        {
            Authentifications auth2 = new Authentifications();
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
                            return View("Connexion", auth2);
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
                                if (emailvalid.Statuts.statut == "Client")
                                {
                                    ViewBag.message = "### Veuillez utiliser un \"Compte employé\" pour vous connecter à l'application BoVoyage Intranet ###";
                                    return View("Connexion", auth2);
                                }

                                else if (emailvalid.Statuts.statut == "Commercial" || emailvalid.Statuts.statut == "Administrateur" || emailvalid.Statuts.statut == "Marketing")
                                {
                                    Session["login"] = emailvalid.email;
                                    Session["statut"] = emailvalid.Statuts.statut;
                                    return RedirectToAction("../Voyages/Index");

                                }
                                else
                                {
                                    ViewBag.message = "### Erreur statut ! Ni client Ni Employé! ###";
                                    return View("Connexion", auth2);
                                }
                            }
                            else
                            {
                                ViewBag.message = "Mot de Passe incorrecte";
                                return View("Connexion", auth2);
                            }
                        }
                        else
                        {
                            ViewBag.message = "### Erreur plusieurs correspondances pour cet e-mail dans la base de données ###";
                            return View("Connexion", auth2);

                        }

                    }
                    else
                    {
                        ViewBag.message = "Ce compte n'existe pas, veuillez vous inscrire pour créer un compte ou corrigez votre identifiant";
                        return View("Connexion", auth2);

                    }

                }
                else
                {
                    ViewBag.message = "Rentrez un mot de Passe et un E-mail pour vous connecter";
                    return View("Connexion", auth2);
                }


            }
            catch (Exception erreur)
            {
                ViewBag.message = "#### Erreur de connexion inconnue ###" + "\n\n\t\t" + erreur;
                return View("Connexion", auth2);

            }


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
                return View("Connection", auth);
            }
        }

        public ActionResult Connection()
        {
            Authentifications qui = new Authentifications();
            return View("Connection", qui);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Connection(Authentifications auth)
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

                return RedirectToAction("../Voyages/Index");
            }
            else
            {
                ViewBag.message = "ATTENTION \nIf you want to proceed you must fill out all required fields";
                return View("Connection", auth);
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

        public ActionResult LogOff(Authentifications authentifications)
        {
            Session.Clear();
            string message = "Vous êtes déconnecté";
            return RedirectToAction("../Authentifications/Connexion", message);
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

