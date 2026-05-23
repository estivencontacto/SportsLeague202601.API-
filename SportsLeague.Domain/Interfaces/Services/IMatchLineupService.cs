using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
        Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup);//añadiremos un jugador a la alineacion
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);//muestra la alineacion completa de un partido
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);//muestra la alineacion de un equipo especifico 
        Task DeleteLineupAsync(int matchId, int lineupId);//Borramos jugador de alineacion
    }
}
