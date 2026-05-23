using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    



    public interface IMatchResultRepository : IGenericRepository<MatchResult>

    {

        Task<MatchResult?> GetByMatchIdAsync(int matchId);

    }
}
