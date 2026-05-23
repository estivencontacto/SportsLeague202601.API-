using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/match")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _matchLineupService;
    private readonly IMapper _mapper;

    public MatchLineupController(
        IMatchLineupService matchLineupService,
        IMapper mapper)
    {
        _matchLineupService = matchLineupService;
        _mapper = mapper;
    }

    // Consulta la alineacion completa de un partido.
    [HttpGet("{matchId}/lineup")]
    public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetLineup(int matchId)
    {
        try
        {
            var lineup = await _matchLineupService.GetByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Registra un titular o suplente en la alineacion del partido.
    [HttpPost("{matchId}/lineup")]
    public async Task<ActionResult<MatchLineupDTO>> AddPlayerAsync(
        int matchId,
        CreateMatchLineupDTO dto)
    {
        try
        {
            var lineup = _mapper.Map<MatchLineup>(dto);
            var created = await _matchLineupService.AddPlayerToLineupAsync(matchId, lineup);
            var responseDto = _mapper.Map<MatchLineupDTO>(created);

            return CreatedAtAction(nameof(GetLineup), new { matchId }, responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // Filtra la alineacion de un partido por equipo.
    [HttpGet("{matchId}/lineup/team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetByMatchAndTeamAsync(
        int matchId,
        int teamId)
    {
        try
        {
            var lineup = await _matchLineupService.GetByMatchAndTeamAsync(matchId, teamId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Elimina un jugador registrado en la alineacion del partido.
    [HttpDelete("{matchId}/lineup/{id}")]
    public async Task<ActionResult> DeleteLineupAsync(int matchId, int id)
    {
        try
        {
            await _matchLineupService.DeleteLineupAsync(matchId, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
