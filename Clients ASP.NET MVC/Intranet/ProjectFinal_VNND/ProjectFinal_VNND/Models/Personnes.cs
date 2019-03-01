//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectFinal_VNND.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Personnes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personnes()
        {
            this.Dossiers = new HashSet<Dossiers>();
            this.Liste_Participants = new HashSet<Liste_Participants>();
        }

        public int id_personne { get; set; }
        public int civilite { get; set; }
        public string prenom { get; set; }
        public string nom { get; set; }
        [DataType(DataType.MultilineText)]
        [UIHint("DisplayPostalAddr")]
        public string adresse { get; set; }
        public string telephone { get; set; }
        public System.DateTime date_naissance { get; set; }
        public Nullable<int> client { get; set; }
        public Nullable<int> participant { get; set; }
        public string email { get; set; }

        public virtual Civilites Civilites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dossiers> Dossiers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Liste_Participants> Liste_Participants { get; set; }
        public virtual OuisNons OuisNons { get; set; }
        public virtual OuisNons OuisNons1 { get; set; }
        //calcul de l'age
        public int age
        {
            get
            {
                int age2 = DateTime.Now.Year - date_naissance.Year;
                if (DateTime.Now.Month < date_naissance.Month || DateTime.Now.Month == date_naissance.Month && DateTime.Now.Day < date_naissance.Day)
                { age2--; }
                else
                { };
                return age2;
            }
        }
        //calcul de la reduction
        public int reduction { get { int red = 0; if (age < 12) { red = 40; } return red; } }
        //affichage pour la droplist
        public string nomcomplet { get { string complet = id_personne.ToString() + ". " + Civilites.civilite +" "+prenom+" "+NOMCAP; return complet; } }
        public string NOMCAP { get { return nom.ToUpper(); } set { NOMCAP = nom.ToUpper(); } }
    }
}
