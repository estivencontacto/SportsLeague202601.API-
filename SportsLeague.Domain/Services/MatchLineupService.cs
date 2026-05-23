using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _matchLineupRepository;
    private readonly MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchLineupService> _logger;
    private readonly ITeamRepository _teamRepository;
    private readonly IMatchRepository _matchRepository;

    public MatchLineupService(
        IMatchLineupRepository matchLineupRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchLineupService> logger,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository)
    {
        _matchLineupRepository = matchLineupRepository;
        _validationHelper = validationHelper;
        _logger = logger;
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
    }

    public async Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup)
    {
        // Valida reglas base de la rubrica: partido, estado y pertenencia del jugador.
        var match = await _validationHelper.ValidateMatchForLineupAsync(matchId);
        var player = await _validationHelper.ValidatePlayerInMatchAsync(lineup.PlayerId, match);

        // Evita duplicar un jugador dentro de la misma alineacion.
        var alreadyRegistered = await _matchLineupRepository.ExistsByMatchAndPlayer(
            matchId,
            lineup.PlayerId);
        if (alreadyRegistered)
            throw new InvalidOperationException(
                "El jugador ya esta registrado en la alineacion de este partido");

        // La regla de maximo 11 solo aplica para titulares.
        if (lineup.IsStarter)
        {
            var teamLineup = await _matchLineupRepository.GetByMatchAndTeamAsync(
                matchId,
                player.TeamId);

            var startersCount = teamLineup.Count(l => l.IsStarter);
            if (startersCount >= 11)
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 titulares registrados en este partido");
        }

        lineup.MatchId = matchId;
        lineup.Position = lineup.Position.Trim().ToUpperInvariant();

        _logger.LogInformation(
            "Registering lineup player {PlayerId} for match {MatchId}", lineup.PlayerId, matchId);

        return await _matchLineupRepository.CreateAsync(lineup);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        // Confirma que el partido exista antes de consultar su alineacion.
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontro el partido con ID {matchId}");

        return await _matchLineupRepository.GetByMatchAsync(matchId);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
    {
        // Valida que el filtro por partido y equipo sea consistente.
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontro el partido con ID {matchId}");

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new KeyNotFoundException($"No se encontro el equipo con ID {teamId}");

        return await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
    }

    public async Task DeleteLineupAsync(int matchId, int lineupId)
    {
        // Verifica que la alineacion exista y pertenezca al partido indicado.
        var exists = await _matchLineupRepository.ExistsAsync(lineupId);
        if (!exists)
            throw new KeyNotFoundException($"No se encontro la alineacion con ID {lineupId}");

        var matchLineup = await _matchLineupRepository.GetByMatchAsync(matchId);
        var belongsToMatch = matchLineup.Any(l => l.Id == lineupId);
        if (!belongsToMatch)
            throw new KeyNotFoundException(
                $"La alineacion con ID {lineupId} no pertenece al partido {matchId}");

        _logger.LogInformation(
            "Deleting lineup {LineupId} from match {MatchId}", lineupId, matchId);

        await _matchLineupRepository.DeleteAsync(lineupId);
    }
}
