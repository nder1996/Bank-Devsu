using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();
        Task<Cliente> ObtenerPorIdAsync(int clienteId);
        Task<Cliente> CrearAsync(Cliente cliente);
        Task<Cliente> ActualizarAsync(Cliente cliente);
        Task<bool> EliminarAsync(int clienteId);
        Task<Cliente> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> ActualizarEstadoAsync(int clienteId, bool nuevoEstado);
    }
}
