using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BancoAPI.Domain.Enums;

namespace BancoAPI.Domain.Entities
{
    public class Movimiento
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        public TipoMovimiento TipoMovimiento { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "El saldo es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "La cuenta es requerida")]
        public long CuentaId { get; set; }

        // Propiedades de navegación
        [ForeignKey("CuentaId")]
        public virtual Cuenta Cuenta { get; set; }
    }
}
