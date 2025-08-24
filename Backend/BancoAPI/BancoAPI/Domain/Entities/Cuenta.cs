using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BancoAPI.Domain.Enums;

namespace BancoAPI.Domain.Entities
{
    public class Cuenta
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "El número de cuenta es requerido")]
        [StringLength(50, ErrorMessage = "El número de cuenta no debe exceder los 50 caracteres")]
        public string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de cuenta no debe exceder los 20 caracteres")]
        public TipoCuenta TipoCuenta { get; set; }

        [Required(ErrorMessage = "El saldo inicial es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor o igual a cero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        public long ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente? ClienteNavigation { get; set; }

        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    }
}
