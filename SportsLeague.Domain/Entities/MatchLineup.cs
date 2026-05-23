using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class MatchLineup:AuditBase
    {
        // Relaciona el jugador con el partido donde sera convocado.
        public int MatchId { get; set; }
        public int PlayerId { get; set; }

        // Define si el jugador inicia como titular y su posicion asignada.
        public bool IsStarter { get; set; }
        public string Position { get; set; } = string.Empty;

        // Propiedades de navegacion usadas por EF Core y AutoMapper.
        public Match Match { get; set; } = null!;
        public Player Player { get; set; } = null!;

    }
}
