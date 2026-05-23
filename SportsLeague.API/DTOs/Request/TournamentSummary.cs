using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsLeague.API.DTOs.Request
{
    public class TournamentSummary
    {
        public int Id { get; set; }
        public string name { get; set; } =string.Empty;
    }
}
