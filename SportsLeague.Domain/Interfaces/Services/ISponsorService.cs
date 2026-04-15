using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> AddAsync(Sponsor sponsor);
        Task UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);

        Task<TournamentSponsor> LinkToTournamentAsync(int sponsorId, int tournamentId, decimal amount);
        Task<IEnumerable<TournamentSponsor>> GetSponsorTournamentsAsync(int sponsorId);
        Task UnlinkFromTournamentAsync(int sponsorId, int tournamentId);
    }
}