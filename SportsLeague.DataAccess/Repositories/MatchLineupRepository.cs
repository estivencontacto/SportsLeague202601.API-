using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;


namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
    {
        public MatchLineupRepository(LeagueDbContext context) : base(context)
        {

        }

        public async Task<bool> ExistsByMatchAndPlayer(int matchId, int playerId)
        {
            return await _dbSet
                 .AnyAsync(ml => ml.MatchId == matchId && ml.PlayerId == playerId);

        }

        // Carga la alineacion de un equipo con datos del jugador y su equipo.
        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int MatchId, int TeamId)
        {
            return await _dbSet
                 .AsNoTracking()
                 .Include(ml => ml.Player)
                 .ThenInclude(p => p.Team)//incluyo la clave foranea para el equipo
                 .Include(ml => ml.Match)
                 .Where(ml => ml.MatchId == MatchId && ml.Player.TeamId == TeamId)
                 .ToListAsync();
            
        }

        // Carga la alineacion completa del partido.
        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int MatchId)
        {
            return await _dbSet
                  .AsNoTracking()
                  .Include(ml => ml.Player)
                  .ThenInclude(p => p.Team)//incluyo la clave foranea para pero tomando en cuenta el jugador, patra que se incluya el nombre del equipo
                  .Include(ml => ml.Match)  
                  .Where(ml => ml.MatchId == MatchId)
                  .ToListAsync();

        }
    }
}
