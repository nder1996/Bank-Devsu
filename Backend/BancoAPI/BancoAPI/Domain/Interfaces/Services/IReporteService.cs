using BancoAPI.Application.DTOs;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IReporteService
    {
        Task<ReporteEstadoCuentaResponseDto> GenerarEstadoCuentaAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> GenerarEstadoCuentaPdfAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
        Task<string> GenerarEstadoCuentaJsonAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
    }
}