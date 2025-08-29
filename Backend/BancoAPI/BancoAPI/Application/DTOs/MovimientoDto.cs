using BancoAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class MovimientoDto
    {
        public long id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de movimiento no debe exceder los 20 caracteres")]
        public TipoMovimiento tipoMovimiento { get; set; } // "Debito" o "Credito"

        [Required(ErrorMessage = "El valor es requerido")]
        public decimal valor { get; set; }

        [Required(ErrorMessage = "El saldo es requerido")]
        public decimal saldo { get; set; }

        [Required(ErrorMessage = "La cuenta es requerida")]
        public CuentaResumida cuenta { get; set; }
        public class CuentaResumida
        {
            public long id { get; set; }

            [Required(ErrorMessage = "El número de cuenta es requerido")]
            [StringLength(50, ErrorMessage = "El número de cuenta no debe exceder los 50 caracteres")]
            public string numeroCuenta { get; set; }
        }
    }
}
