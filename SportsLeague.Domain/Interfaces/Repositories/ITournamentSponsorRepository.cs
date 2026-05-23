using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository: IGenericRepository<TournamentSponsor>// creo el Irepository donde hereda los metodos del generic
    {
        Task<TournamentSponsor?> GetByTournamentAndSponsor(int TournamentId, int SponsorId);

       

    }
}
