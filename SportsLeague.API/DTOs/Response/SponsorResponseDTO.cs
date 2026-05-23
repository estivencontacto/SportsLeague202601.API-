using Microsoft.AspNetCore.Http.HttpResults;
using SportsLeague.API.DTOs.Request;
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class SponsorResponseDTO
    {
       public int Id {  get; set; }
        public string SponsorName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebSiteURl { get; set; }

        public SponsorCategory Category { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }= string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
