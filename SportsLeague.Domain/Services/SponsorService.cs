using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(
        ISponsorRepository sponsorRepository,
        ITournamentRepository tournamentRepository,
        ITournamentSponsorRepository tournamentSponsorRepository,
        ILogger<SponsorService> logger)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentRepository = tournamentRepository;
        _tournamentSponsorRepository = tournamentSponsorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Sponsor>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all sponsors");
        return await _sponsorRepository.GetAllAsync();
    }

    public async Task<Sponsor?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving sponsor with ID {SponsorId}", id);
        return await _sponsorRepository.GetByIdAsync(id);
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        await EnsureSponsorNameIsAvailableAsync(sponsor.SponsorName);

        sponsor.SponsorName = sponsor.SponsorName.Trim();
        sponsor.ContactEmail = sponsor.ContactEmail.Trim();
        sponsor.Phone = sponsor.Phone?.Trim();
        sponsor.WebSiteURl = sponsor.WebSiteURl?.Trim();

        _logger.LogInformation("Creating sponsor {SponsorName}", sponsor.SponsorName);
        return await _sponsorRepository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existing = await _sponsorRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro el sponsor con ID {id}");

        var duplicatedName = await _sponsorRepository.ExistByNameAsync(sponsor.SponsorName);
        if (duplicatedName != null && duplicatedName.Id != id)
            throw new InvalidOperationException($"Ya existe un sponsor con el nombre {sponsor.SponsorName}");

        existing.SponsorName = sponsor.SponsorName.Trim();
        existing.ContactEmail = sponsor.ContactEmail.Trim();
        existing.Phone = sponsor.Phone?.Trim();
        existing.WebSiteURl = sponsor.WebSiteURl?.Trim();
        existing.Category = sponsor.Category;

        _logger.LogInformation("Updating sponsor {SponsorId}", id);
        await _sponsorRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _sponsorRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontro el sponsor con ID {id}");

        _logger.LogInformation("Deleting sponsor {SponsorId}", id);
        await _sponsorRepository.DeleteAsync(id);
    }

    public async Task UpdateCategoryAsync(int id, SponsorCategory newCategory)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro el sponsor con ID {id}");

        sponsor.Category = newCategory;

        _logger.LogInformation("Updating sponsor {SponsorId} category to {Category}", id, newCategory);
        await _sponsorRepository.UpdateAsync(sponsor);
    }

    public async Task AddToTournamentAsync(int tournamentId, int sponsorId)
    {
        var sponsorExists = await _sponsorRepository.ExistsAsync(sponsorId);
        if (!sponsorExists)
            throw new KeyNotFoundException($"No se encontro el sponsor con ID {sponsorId}");

        var tournamentExists = await _tournamentRepository.ExistsAsync(tournamentId);
        if (!tournamentExists)
            throw new KeyNotFoundException($"No se encontro el torneo con ID {tournamentId}");

        var existingRelation = await _tournamentSponsorRepository
            .GetByTournamentAndSponsor(tournamentId, sponsorId);
        if (existingRelation != null)
            throw new InvalidOperationException(
                $"El sponsor {sponsorId} ya esta asociado al torneo {tournamentId}");

        await _sponsorRepository.AddToTournamentAsync(tournamentId, sponsorId);
        _logger.LogInformation(
            "Sponsor {SponsorId} added to tournament {TournamentId}", sponsorId, tournamentId);
    }

    private async Task EnsureSponsorNameIsAvailableAsync(string sponsorName)
    {
        var existing = await _sponsorRepository.ExistByNameAsync(sponsorName);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un sponsor con el nombre {sponsorName}");
    }
}
