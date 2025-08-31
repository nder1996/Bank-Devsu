using BancoAPI.Domain.Enums;

namespace BancoAPI.Application.DTOs
{
    public class ReporteListadoDto
    {
        public long Id { get; set; }
        public string Cliente { get; set; } = "";
        public string Periodo { get; set; } = "";
        public ReporteFormato Formato { get; set; }
    }
}