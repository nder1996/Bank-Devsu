using BancoAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class ReporteDto
    {
        public long id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public long clienteId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime fechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime fechaFin { get; set; }

        [Required(ErrorMessage = "El formato es requerido")]
        public ReporteFormato formato { get; set; }

        public DateTime fechaGeneracion { get; set; }

        public string? rutaArchivo { get; set; }

        public string? nombreArchivo { get; set; }

        public bool activo { get; set; } = true;

        public ReporteClienteResumido cliente { get; set; } = new ReporteClienteResumido();

        public class ReporteClienteResumido
        {
            public long id { get; set; }
            public string nombre { get; set; } = "";
        }
    }
}