using BancoAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class ReporteRequestDto
    {
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public long ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        // Usando el enum para mayor seguridad
        [Required(ErrorMessage = "El formato es requerido")]
        public ReporteFormato Formato { get; set; } = ReporteFormato.JSON;
    }
}
