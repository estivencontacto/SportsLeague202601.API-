using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{ 
    public interface ISponsorRepository : IGenericRepository<Sponsor>// creo el IRepository donde hereda los metodos del generic 
    {
        Task<Sponsor?> ExistByNameAsync(string SponsorName);//para ver si es repetido el nombre del sponsor 
        
        
        Task AddToTournamentAsync(int tournamentId, int sponsorId);
    }
}
