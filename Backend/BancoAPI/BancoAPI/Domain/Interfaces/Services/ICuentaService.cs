using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface ICuentaService
    {
        Task<IEnumerable<CuentaDto>> ObtenerTodasAsync();
        Task<CuentaDto> CrearAsync(CuentaDto cuenta);
        Task<CuentaDto> ActualizarAsync(long cuentaId, CuentaDto cuenta);
        Task<bool> EliminarAsync(long cuentaId);

    }
}

