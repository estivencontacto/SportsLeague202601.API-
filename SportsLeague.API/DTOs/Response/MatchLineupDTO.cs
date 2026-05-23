namespace SportsLeague.API.DTOs.Response
{
    public class MatchLineupDTO //creamos el response con lo propio del dto(createMatchLineup)
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public bool IsStarter { get; set; }
        public string Position { get; set; } = string.Empty;

    }
}
