using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente> ObtenerPorIdAsync(int clienteId);
        Task<Cliente> CrearAsync(Cliente cliente);
        Task<Cliente> ActualizarAsync(Cliente cliente);
        Task<bool> EliminarAsync(int clienteId);
        Task<Cliente> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> ActualizarEstadoAsync(int clienteId, bool nuevoEstado);
        Task<Cliente> ObtenerClienteConCuentasYMovimientosAsync(int clienteId, DateTime fechaInicio, DateTime fechaFin);
    }
}
