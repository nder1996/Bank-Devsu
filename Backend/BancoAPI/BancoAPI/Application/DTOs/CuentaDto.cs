using BancoAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class CuentaDto
    {
        public long id { get; set; }

        [Required(ErrorMessage = "El número de cuenta es requerido")]
        [StringLength(50, ErrorMessage = "El número de cuenta no debe exceder los 50 caracteres")]
        public string numeroCuenta { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de cuenta no debe exceder los 20 caracteres")]
        public TipoCuenta tipoCuenta { get; set; }

        [Required(ErrorMessage = "El saldo inicial es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor o igual a cero")]
        public decimal saldoInicial { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool estado { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        public ClienteResumido cliente { get; set; }

        public class ClienteResumido
        {
            public long id { get; set; }

            [Required(ErrorMessage = "El nombre es requerido")]
            [StringLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres")]
            public string nombre { get; set; }
        }
    }
}
