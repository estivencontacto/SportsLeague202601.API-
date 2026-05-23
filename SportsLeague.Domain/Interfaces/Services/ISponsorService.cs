using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService//CReamos el iservice de sponsor primero con sus acciones propias de la aplicacion
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);

        Task UpdateCategoryAsync(int id, SponsorCategory newCategory);//Permite subir de categoria de patrocinador
        Task AddToTournamentAsync(int tournamentId, int sponsorId);//permite asociar sponsor a torneo 
        
    }
}
