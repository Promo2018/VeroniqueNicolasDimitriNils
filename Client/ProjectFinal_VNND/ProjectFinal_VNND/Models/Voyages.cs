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
    
    public partial class Voyages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Voyages()
        {
            this.Dossiers = new HashSet<Dossiers>();
        }
    
        public int id_voyage { get; set; }
        public System.DateTime date_aller { get; set; }
        public System.DateTime date_retour { get; set; }
        public int places_disponibles { get; set; }
        public decimal tarif_tout_compris { get; set; }
        public int agence { get; set; }
        public int destination { get; set; }
    
        public virtual Agences Agences { get; set; }
        public virtual Destinations Destinations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dossiers> Dossiers { get; set; }
    }
}
