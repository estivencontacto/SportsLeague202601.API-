using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    // Define operaciones específicas para Sponsor
    // Extiende el repositorio genérico
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        // Verifica si ya existe un sponsor con el mismo nombre
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);

        // Obtiene un sponsor junto con sus torneos asociados
        Task<Sponsor?> GetSponsorWithTournamentsAsync(int id);
    }
}