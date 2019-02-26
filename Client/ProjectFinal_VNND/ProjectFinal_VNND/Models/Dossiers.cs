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
    
    public partial class Dossiers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dossiers()
        {
            this.Liste_Assurances = new HashSet<Liste_Assurances>();
            this.Liste_Participants = new HashSet<Liste_Participants>();
        }
    
        public int id_dossier { get; set; }
        public string numero_carte_bancaire { get; set; }
        public Nullable<int> raison_annulation { get; set; }
        public int etat { get; set; }
        public int voyage { get; set; }
        public int client { get; set; }
        public Nullable<System.DateTime> dernier_suivi { get; set; }
    
        public virtual Raisons_Annulations Raisons_Annulations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Liste_Assurances> Liste_Assurances { get; set; }
        public virtual Etats_Dossiers Etats_Dossiers { get; set; }
        public virtual Personnes Personnes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Liste_Participants> Liste_Participants { get; set; }
        public virtual Voyages Voyages { get; set; }
        public float PrixTotal
        {
            get
            {
                float prixTotal2 = CalculPrix();            // Get de prixTotal 
                return prixTotal2;
            }
        }



        public float CalculPrix()
        {
            var prixTC = Voyages.tarif_tout_compris;       // Price of voyage from Voyages table
            var reductionPrix = Convert.ToDecimal(0.6);     // Declaration of 60% reduction
            decimal numVoyageurs = 0;       // Number of travelers
            decimal reductionT = 0; 
            foreach (Liste_Participants p in Liste_Participants)     // Liste_Participants - list of Participants 
            {
                numVoyageurs++;
                reductionT = (reductionT + p.Personnes.reduction)/100;
            }
            
            decimal totalAssurance = 0;
            foreach (Liste_Assurances n in Liste_Assurances)
            {
                totalAssurance = totalAssurance + Convert.ToDecimal(n.Assurances.prix);        //  Calculation of ALL Assurances chosen (if any)  
            }

            decimal prixTotal3 = prixTC*(numVoyageurs + 1 - reductionT) * (1 + totalAssurance); //Le +1 correspond au client ajouté

            float price = (float)prixTotal3;

            return price;
        }


    }
}
