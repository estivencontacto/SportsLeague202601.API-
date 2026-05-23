using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IGoalRepository : IGenericRepository<Goal>

    {

        Task<IEnumerable<Goal>> GetByMatchAsync(int matchId);

        Task<IEnumerable<Goal>> GetByMatchWithDetailsAsync(int matchId);

    }
}
