using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto> CrearAsync(ClienteDto cliente);
        Task<ClienteDto> ActualizarAsync(ClienteDto cliente);
        Task<bool> EliminarAsync(long clienteId);
        Task<ClienteDto> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> ActualizarEstadoAsync(int clienteId, bool nuevoEstado);
    }
}
