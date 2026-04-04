using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IRefereeService
    {
        Task<IEnumerable<Referee>> GetAllAsync();
        Task<Referee?> GetByIdAsync(int id);
        Task<Referee> AddAsync(Referee referee);
        Task UpdateAsync(int id, Referee referee);
        Task DeleteAsync(int id);
    }

}
