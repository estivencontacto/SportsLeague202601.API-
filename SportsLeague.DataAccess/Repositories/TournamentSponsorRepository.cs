using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository//Implementamos el repository con los metodos del generic y del ITournamentSponsor
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<TournamentSponsor?> GetByTournamentAndSponsor(int tournamentId, int sponsorId)//logramos buscar por cualquiera de las dos ids
        {
            return await _dbSet
                .Where(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId)
                .FirstOrDefaultAsync();
        }

        
    }
}

