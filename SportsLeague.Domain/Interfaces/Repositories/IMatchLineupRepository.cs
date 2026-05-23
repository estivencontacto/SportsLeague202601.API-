using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int id);//retorna jugadores de un partido
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int MatchId, int TeamId);//muestra solamente jugadores de un partido y de un equipo especifico
        Task <bool> ExistsByMatchAndPlayer(int matchId, int playerId);//Busca si el jugador hace parte de dicho partido
    }
}
