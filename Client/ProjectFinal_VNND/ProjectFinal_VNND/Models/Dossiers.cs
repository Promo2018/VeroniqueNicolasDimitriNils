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
            this.Assurances = new HashSet<Assurances>();
            this.Personnes1 = new HashSet<Personnes>();
        }
    
        public int id_dossier { get; set; }
        public string numero_carte_bancaire { get; set; }
        public string raison_annulation { get; set; }
        public string etat { get; set; }
        public int voyage { get; set; }
        public int numero_client { get; set; }
        public Nullable<System.DateTime> dernier_suivi { get; set; }
    
        public virtual Raisons_Annulations Raisons_Annulations { get; set; }
        public virtual Etats_Dossiers Etats_Dossiers { get; set; }
        public virtual Personnes Personnes { get; set; }
        public virtual Voyages Voyages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assurances> Assurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personnes> Personnes1 { get; set; }
    }
}