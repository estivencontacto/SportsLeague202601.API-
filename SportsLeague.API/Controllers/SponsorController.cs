using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;


namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[Controller])")]
    public class SponsorController : ControllerBase//creo la clase controlador 
    {
        private readonly ISponsorService _sponsorService;//hago el enlace con el isponsor
        private readonly IMapper _mapper;


        public SponsorController(//inyecto todo lo necesario para el buen funcionamiento 

        ISponsorService sponsorService,
        IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()//consigo todos los sponsors registrados 

        {
            var sponsor = await _sponsorService.GetAllAsync();//hace la consulta del metodo desde sponsorservice
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsor));//te devuelve lo que te deberia mostrar el response del dto
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)//te permite buscar segun la id
        {

            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null)//si no hay una id igul registrada
                return NotFound(new { message = $"Sponsor con ID {id} no encontrado" });//te salta mensaje 
            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));// si si la hay, te muestra la respuesta 
            
        }
        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)//crea nuevos sponsors 
        {
            try

            {
                var sponsor = _mapper.Map<Sponsor>(dto);//guarda todo en una variable
                var created = await _sponsorService.CreateAsync(sponsor);//recarga todo en el service
                var responseDto = _mapper.Map<SponsorResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);//si sale bien permite la creacion



            }
            catch (InvalidOperationException ex)//en caso de error muestra mensaje 

            {

                return BadRequest(new { ex.Message });

            }



        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> Update(int id, SponsorRequestDTO dto)//permite actualizar 
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);//guarda todo en la variable
                await _sponsorService.UpdateAsync(id, sponsor);//hace la consulta desde el service con el metodo 
                return NoContent();//da la validez de la accion





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
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)//elimina sponsor por su id
        {
            try
            {
                await _sponsorService.DeleteAsync(id);//realiza la accion por medio del metodo
                return NoContent();//valida la accion
                   



            }
            catch  (KeyNotFoundException ex)
            {

                return NotFound(new { message = ex.Message });

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });


            }

        }

        [HttpPatch("{id}/Category")]
        public async Task<ActionResult> UpdateCategoryAsync(int id, SponsorRequestDTO dto)//actualiza la categoria 
        {
            try
            {
                await _sponsorService.UpdateCategoryAsync(id, dto.Category);//realiza la tarea por medio del metodo 
                return NoContent();//da validez a la accion

            }
            catch(KeyNotFoundException ex)
            {

                return NotFound(new { message = ex.Message });
                
            }
            catch(InvalidOperationException ex)
            {


                return Conflict(new { message = ex.Message });
            }


        }

        [HttpPost("{id}/AddTournament")]
        public async Task <ActionResult> AddtoTounramentAsync(int SponsorId, int TournamentId)//permite asociar un sponsor al torneo 
        {
            try
            {
                await _sponsorService.AddToTournamentAsync(SponsorId, TournamentId);
                return NoContent();


            } 
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });

            }
            catch(InvalidOperationException ex)
            {


                return Conflict(new { message = ex.Message });
            }





        }
    }
}
