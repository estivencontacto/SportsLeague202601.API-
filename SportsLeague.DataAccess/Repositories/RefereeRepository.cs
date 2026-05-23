using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;




namespace SportsLeague.DataAccess.Repositories
{
    public class RefereeRepository : GenericRepository<Referee>,IRefereeRepository

    {

        public RefereeRepository(LeagueDbContext context) : base(context)

        {

        }


        public async Task<IEnumerable<Referee>> GetByNationalityAsync(string nationality)

        {

            return await _dbSet
                .AsNoTracking()
                .Where(r => r.Nationality == nationality)
                .ToListAsync();
        }
    }
}
