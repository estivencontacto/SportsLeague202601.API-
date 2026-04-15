using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepo;
        private readonly ITournamentSponsorRepository _linkRepo;
        private readonly ITournamentRepository _tournamentRepo;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepo,
            ITournamentSponsorRepository linkRepo,
            ITournamentRepository tournamentRepo,
            ILogger<SponsorService> logger)
        {
            _sponsorRepo = sponsorRepo;
            _linkRepo = linkRepo;
            _tournamentRepo = tournamentRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepo.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _sponsorRepo.GetSponsorWithTournamentsAsync(id);
        }

        public async Task<Sponsor> AddAsync(Sponsor sponsor)
        {
            if (await _sponsorRepo.ExistsByNameAsync(sponsor.Name))
                throw new InvalidOperationException("Sponsor already exists");

            sponsor.CreatedAt = DateTime.UtcNow;

            return await _sponsorRepo.AddAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor updated)
        {
            var existing = await _sponsorRepo.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Sponsor not found");

            existing.Name = updated.Name;
            existing.ContactEmail = updated.ContactEmail;
            existing.Phone = updated.Phone;
            existing.WebsiteUrl = updated.WebsiteUrl;
            existing.Category = updated.Category;
            existing.UpdatedAt = DateTime.UtcNow;

            await _sponsorRepo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _sponsorRepo.ExistsAsync(id))
                throw new KeyNotFoundException("Sponsor not found");

            await _sponsorRepo.DeleteAsync(id);
        }

        public async Task<TournamentSponsor> LinkToTournamentAsync(int sponsorId, int tournamentId, decimal amount)
        {
            var sponsor = await _sponsorRepo.GetByIdAsync(sponsorId);
            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);

            if (sponsor == null || tournament == null)
                throw new KeyNotFoundException("Sponsor or Tournament not found");

            var link = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = amount,
                JoinedAt = DateTime.UtcNow
            };

            return await _linkRepo.AddAsync(link);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorTournamentsAsync(int sponsorId)
        {
            return await _linkRepo.GetBySponsorIdAsync(sponsorId);
        }

        public async Task UnlinkFromTournamentAsync(int sponsorId, int tournamentId)
        {
            var link = await _linkRepo.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (link == null)
                throw new KeyNotFoundException("Relation not found");

            await _linkRepo.DeleteAsync(link.Id);
        }
    }
}