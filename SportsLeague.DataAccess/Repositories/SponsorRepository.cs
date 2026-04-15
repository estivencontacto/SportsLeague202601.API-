using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    // Implementación concreta del repositorio de Sponsor
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        // Verifica si ya existe un sponsor con el mismo nombre
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            var normalizedName = name.ToLower().Trim();

            var query = _dbSet.AsQueryable();

            // Si estamos actualizando, excluimos el mismo ID
            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return await query.AnyAsync(s => s.Name.ToLower() == normalizedName);
        }

        // Trae un sponsor junto con los torneos asociados
        public async Task<Sponsor?> GetSponsorWithTournamentsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.TournamentSponsors)
                    .ThenInclude(ts => ts.Tournament)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}