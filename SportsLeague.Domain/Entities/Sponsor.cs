using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    // Representa un patrocinador dentro del sistema
    // Hereda de AuditBase para manejar Id, CreatedAt y UpdatedAt
    public class Sponsor : AuditBase
    {
        // Nombre del patrocinador (ej: Nike, Adidas)
        public string Name { get; set; } = string.Empty;

        // Correo de contacto principal
        public string ContactEmail { get; set; } = string.Empty;

        // Teléfono opcional
        public string? Phone { get; set; }

        // Página web opcional
        public string? WebsiteUrl { get; set; }

        // Categoría del sponsor (Main, Gold, etc.)
        public SponsorCategory Category { get; set; }

        // Relación muchos a muchos con torneos
        // Un sponsor puede participar en varios torneos
        public ICollection<TournamentSponsor> TournamentSponsors { get; set; }
            = new List<TournamentSponsor>();
    }
}