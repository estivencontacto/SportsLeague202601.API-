using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    // Controlador REST para manejar Sponsors
    [ApiController]
    [Route("api/sponsors")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _service;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // =========================
        // GET ALL SPONSORS
        // =========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetSponsors()
        {
            var data = await _service.GetAllAsync();
            var response = _mapper.Map<IEnumerable<SponsorResponseDTO>>(data);

            return Ok(response);
        }

        // =========================
        // GET SPONSOR BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetSponsor(int id)
        {
            var sponsor = await _service.GetByIdAsync(id);

            if (sponsor == null)
                return NotFound(new { error = $"Sponsor ID {id} not found" });

            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
        }

        // =========================
        // CREATE SPONSOR
        // =========================
        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> CreateSponsor(SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);

                var created = await _service.AddAsync(entity);

                var result = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetSponsor), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // =========================
        // UPDATE SPONSOR
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSponsor(int id, SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);

                await _service.UpdateAsync(id, entity);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // =========================
        // DELETE SPONSOR
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSponsor(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // =========================
        // GET TOURNAMENTS BY SPONSOR
        // =========================
        [HttpGet("{id}/tournaments")]
        public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetTournaments(int id)
        {
            try
            {
                var data = await _service.GetSponsorTournamentsAsync(id);

                var response = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(data);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // =========================
        // LINK SPONSOR TO TOURNAMENT
        // =========================
        [HttpPost("{id}/tournaments")]
        public async Task<ActionResult<TournamentSponsorResponseDTO>> AddSponsorToTournament(
            int id,
            [FromBody] TournamentSponsorRequestDTO dto)
        {
            try
            {
                var link = await _service.LinkToTournamentAsync(id, dto.TournamentId, dto.ContractAmount);

                var response = _mapper.Map<TournamentSponsorResponseDTO>(link);

                return CreatedAtAction(nameof(GetTournaments), new { id }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // =========================
        // UNLINK SPONSOR
        // =========================
        [HttpDelete("{id}/tournaments/{tournamentId}")]
        public async Task<IActionResult> RemoveSponsorFromTournament(int id, int tournamentId)
        {
            try
            {
                await _service.UnlinkFromTournamentAsync(id, tournamentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}