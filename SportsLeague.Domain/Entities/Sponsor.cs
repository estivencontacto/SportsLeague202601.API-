using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase// creamos la clase sponsor
    {
        public string SponsorName { get; set; } = String.Empty;
        public string ContactEmail { get; set; } = String.Empty;
        public string? Phone { get; set; } 
        public string? WebSiteURl { get; set; }
        public SponsorCategory Category { get; set; } //hacemos el enlace con los enums de la categoria

        public ICollection<TournamentSponsor> tournamentSponsors { get; set; } = new List<TournamentSponsor>();//agregamos las navegation properties adecuadas
    }
}
