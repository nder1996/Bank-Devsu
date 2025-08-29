using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface ICuentaService
    {
        Task<IEnumerable<CuentaDto>> ObtenerTodasAsync();
        Task<CuentaDto> ObtenerPorIdAsync(long cuentaId);
        Task<CuentaDto> CrearAsync(CuentaDto cuenta);
        Task<bool> EliminarAsync(long cuentaId);
        Task<bool> ActualizarEstadoAsync(long cuentaId, bool nuevoEstado);
        Task<bool> ActualizarTipoCuentaAsync(long cuentaId, string nuevoTipo);
        Task<CuentaDto> ObtenerPorNumeroCuentaAsync(string numeroCuenta);
        Task<IEnumerable<CuentaDto>> ObtenerPorClienteIdAsync(long clienteId);
    }
}

