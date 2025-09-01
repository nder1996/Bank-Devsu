using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Repositories
{
    public interface IReporteRepository
    {
        Task<IEnumerable<EstadoCuentaDto>> GetEstadoCuentaAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
        Task<(decimal totalDebitos, decimal totalCreditos)> GetTotalesMovimientosAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
    }
}