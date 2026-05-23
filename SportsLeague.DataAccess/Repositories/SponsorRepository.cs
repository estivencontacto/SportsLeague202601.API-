using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository//creamos la logica de implementacion del repositorio, donde solamente definimos la logica de los metodos propios
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<Sponsor?> ExistByNameAsync(string SponsorName)//Implementamos el metodo que permite ver si hay otro sponsor llamado igual
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SponsorName == SponsorName);

        }

        
        public async Task AddToTournamentAsync(int tournamentId, int sponsorId)
        {
            var tournamentSponsor = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                JoinedAt = DateTime.UtcNow
            };
            await _context.TournamentSponsors.AddAsync(tournamentSponsor);
            await _context.SaveChangesAsync();
        }
    }
}
