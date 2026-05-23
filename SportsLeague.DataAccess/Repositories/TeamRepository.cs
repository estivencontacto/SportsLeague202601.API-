using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories;


public class TeamRepository : GenericRepository<Team>, ITeamRepository

{

    public TeamRepository(LeagueDbContext context) : base(context)

    {

    }


    public async Task<Team?> GetByNameAsync(string name)

    {

        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name);

    }


    public async Task<IEnumerable<Team>> GetByCityAsync(string city)

    {

        return await _dbSet
            .AsNoTracking()
            .Where(t => t.City == city)
            .ToListAsync();

    }

}
