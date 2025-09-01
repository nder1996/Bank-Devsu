using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Application.DTOs
{
    public class EstadoCuentaDto
    {
        [Required(ErrorMessage = "La fecha es requerida")]
        public string Fecha { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "El número de cuenta es requerido")]
        [Display(Name = "Numero Cuenta")]
        public string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El saldo inicial es requerido")]
        [Display(Name = "Saldo Inicial")]
        public decimal SaldoInicial { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El movimiento es requerido")]
        public decimal Movimiento { get; set; }

        [Required(ErrorMessage = "El saldo disponible es requerido")]
        [Display(Name = "Saldo Disponible")]
        public decimal SaldoDisponible { get; set; }
    }

    public class ReporteEstadoCuentaRequestDto
    {
        [Required(ErrorMessage = "El cliente ID es requerido")]
        public long ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        public string Formato { get; set; } = "json";
    }

    public class ReporteEstadoCuentaResponseDto
    {
        public IEnumerable<EstadoCuentaDto> Datos { get; set; } = new List<EstadoCuentaDto>();
        public string? PdfBase64 { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal TotalCreditos { get; set; }
        public int TotalMovimientos { get; set; }
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    }
}