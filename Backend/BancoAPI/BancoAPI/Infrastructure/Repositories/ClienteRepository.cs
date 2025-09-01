using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {


        private readonly BancoDbContext _context;

        public ClienteRepository(BancoDbContext context)
        {
            _context = context;
        }


        async Task<Cliente> IClienteRepository.ActualizarAsync(Cliente cliente)
        {
            // Desanexar el cliente si ya está siendo rastreado
            var localCliente = _context.Clientes.Local.FirstOrDefault(e => e.Id == cliente.Id);
            if (localCliente != null)
            {
                _context.Entry(localCliente).State = EntityState.Detached;
            }

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }



        async Task<bool> IClienteRepository.ActualizarEstadoAsync(int clienteId, bool nuevoEstado)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);
            if (cliente == null)
                return false;

            cliente.Estado = nuevoEstado;
            _context.Clientes.Update(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        async Task<Cliente> IClienteRepository.CrearAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        async Task<bool> IClienteRepository.EliminarAsync(long clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return false;
            cliente.Estado = false;
            _context.Clientes.Update(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        async Task<Cliente> IClienteRepository.ObtenerClienteConCuentasYMovimientosAsync(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Clientes
               .Where(c => c.Id == clienteId)
               .Include(c => c.Cuentas)
                   .ThenInclude(cuenta => cuenta.Movimientos.Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin))
               .FirstOrDefaultAsync();
        }

        async Task<Cliente> IClienteRepository.ObtenerPorIdAsync(long clienteId)
        {
            return await _context.Clientes
               .Include(c => c.Cuentas)
               .FirstOrDefaultAsync(c => c.Id == clienteId);
        }

        async Task<Cliente> IClienteRepository.ObtenerPorIdentificacionAsync(string identificacion)
        {
            return await _context.Clientes
                .Include(c => c.Cuentas)
                .FirstOrDefaultAsync(c => c.Identificacion == identificacion);
        }

        async Task<IEnumerable<Cliente>> IClienteRepository.ObtenerTodosAsync()
        {
            return await _context.Clientes
            .Where(c => c.Estado == true)
            .Include(c => c.Cuentas)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
