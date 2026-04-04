namespace SportsLeague.Domain.Entities
{
    // Tabla intermedia para la relación N:M entre Tournament y Sponsor
    public class TournamentSponsor : AuditBase
    {
        // Id del torneo
        public int TournamentId { get; set; }

        // Id del sponsor
        public int SponsorId { get; set; }

        // Valor del contrato entre sponsor y torneo
        public decimal ContractAmount { get; set; }

        // Fecha en la que el sponsor se vinculó al torneo
        public DateTime JoinedAt { get; set; }

        // Navegación hacia Tournament
        public Tournament Tournament { get; set; } = null!;

        // Navegación hacia Sponsor
        public Sponsor Sponsor { get; set; } = null!;
    }
}