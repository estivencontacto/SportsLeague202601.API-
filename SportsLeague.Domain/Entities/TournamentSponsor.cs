using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor:AuditBase//Creamos la clase sponsor con los atributos de la auditbase
    {
        public int TournamentId { get; set; }
        public int SponsorId { get; set; }
        public decimal ContractAmount { get; set; } 

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;


        //Agregamos propiedades de navegacion
        public Sponsor Sponsor { get; set; } = null!;
        public Tournament Tournament { get; set; } = null!;

    }
}
