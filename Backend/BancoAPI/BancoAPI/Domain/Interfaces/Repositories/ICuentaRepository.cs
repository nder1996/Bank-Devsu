using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Repositories
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> ObtenerTodasAsync();
        Task<Cuenta> ObtenerPorIdAsync(long cuentaId);
        Task<Cuenta> CrearAsync(Cuenta cuenta);
        Task<Cuenta> ActualizarAsync(Cuenta cuenta);
        Task<bool> EliminarAsync(long cuentaId);
        Task<bool> ActualizarEstadoAsync(long cuentaId, bool nuevoEstado);
        Task<bool> ActualizarTipoCuentaAsync(long cuentaId, string nuevoTipo);
        Task<Cuenta> ObtenerPorNumeroCuentaAsync(string numeroCuenta);
        Task<IEnumerable<Cuenta>> ObtenerPorClienteIdAsync(long clienteId);
    }
}

