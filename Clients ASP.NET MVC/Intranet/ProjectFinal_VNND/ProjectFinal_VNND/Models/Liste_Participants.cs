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
    
    public partial class Liste_Participants
    {
        public int id_listparticipant { get; set; }
        public int participant { get; set; }
        public int dossier { get; set; }
    
        public virtual Dossiers Dossiers { get; set; }
        public virtual Personnes Personnes { get; set; }
    }
}
