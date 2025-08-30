using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BancoAPI.Domain.Enums;

namespace BancoAPI.Domain.Entities
{
    public class Reporte
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public long ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El formato es requerido")]
        public ReporteFormato Formato { get; set; }

        [Required(ErrorMessage = "La fecha de generación es requerida")]
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "La ruta del archivo no debe exceder los 500 caracteres")]
        public string? RutaArchivo { get; set; }

        [StringLength(100, ErrorMessage = "El nombre del archivo no debe exceder los 100 caracteres")]
        public string? NombreArchivo { get; set; }

        public bool Activo { get; set; } = true;

        // Propiedades de navegación
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
    }
}