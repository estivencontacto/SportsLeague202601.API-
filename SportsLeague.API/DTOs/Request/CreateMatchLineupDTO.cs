using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request
{
    public class CreateMatchLineupDTO 
    {
        // Datos minimos que envia el cliente para registrar una convocatoria.
        [Range(1, int.MaxValue, ErrorMessage = "El jugador es obligatorio")]
        public int PlayerId { get; set; }

        public bool IsStarter { get; set; }

        [Required(ErrorMessage = "La posición es obligatoria")]
        [StringLength(10, ErrorMessage = "La posición no puede superar 10 caracteres")]
        public string Position { get; set; } = string.Empty;
    }
}
