using BancoAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class MovimientoDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de movimiento no debe exceder los 20 caracteres")]
        public TipoMovimiento TipoMovimiento { get; set; } // "Debito" o "Credito"

        [Required(ErrorMessage = "El valor es requerido")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "El saldo es requerido")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "La cuenta es requerida")]
        public CuentaResumida Cuenta { get; set; }
        public class CuentaResumida
        {
            public long Id { get; set; }

            [Required(ErrorMessage = "El número de cuenta es requerido")]
            [StringLength(50, ErrorMessage = "El número de cuenta no debe exceder los 50 caracteres")]
            public string NumeroCuenta { get; set; }
        }
    }
}
