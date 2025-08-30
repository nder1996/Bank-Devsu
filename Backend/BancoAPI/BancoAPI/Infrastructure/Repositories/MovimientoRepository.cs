using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Enums;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Infrastructure.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {

        private readonly BancoDbContext _context;

        public MovimientoRepository(BancoDbContext context)
        {
            _context = context;
        }

        async Task<Movimiento> IMovimientoRepository.CreateAsync(Movimiento movimiento)
        {
            await _context.Movimientos.AddAsync(movimiento);
            await _context.SaveChangesAsync();
            return movimiento;
        }

        async Task<bool> IMovimientoRepository.DeleteAsync(long id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
                return false;

            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();
            return true;
        }  

        async Task<IEnumerable<Movimiento>> IMovimientoRepository.GetAllAsync()
        {
            return await _context.Movimientos
               .Include(m => m.Cuenta) 
               .ToListAsync();
        }

        async Task<IEnumerable<Movimiento>> IMovimientoRepository.GetByCuentaIdAsync(long cuentaId)
        {
            return await _context.Movimientos
            .Where(m => m.CuentaId == cuentaId)
            .ToListAsync();
        }

        async Task<Movimiento?> IMovimientoRepository.GetByIdAsync(long id)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        async Task<IEnumerable<Movimiento>> IMovimientoRepository.GetDebitosDiariosByCuentaIdAsync(long cuentaId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Movimientos
            .Where(m => m.CuentaId == cuentaId &&
                  m.TipoMovimiento == TipoMovimiento.Debito &&
                  m.Fecha >= fechaInicio &&
                  m.Fecha < fechaFin)
            .ToListAsync();
        }

        async Task<Movimiento> IMovimientoRepository.UpdateAsync(Movimiento movimiento)
        {
            var existingMovimiento = await _context.Movimientos.FindAsync(movimiento.Id);
            if (existingMovimiento == null)
            {
                throw new Exception("Movimiento not found");
            }

            // Actualizamos las propiedades
            existingMovimiento.Fecha = movimiento.Fecha;
            existingMovimiento.TipoMovimiento = movimiento.TipoMovimiento;
            existingMovimiento.Valor = movimiento.Valor;
            existingMovimiento.Saldo = movimiento.Saldo;
            existingMovimiento.CuentaId = movimiento.CuentaId;

            _context.Movimientos.Update(existingMovimiento);
            await _context.SaveChangesAsync();
            return existingMovimiento;
        }

        public async Task<IEnumerable<Movimiento>> GetByClienteIdAndDateRangeAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .Where(m => m.Cuenta.ClienteId == clienteId &&
                           m.Fecha >= fechaInicio &&
                           m.Fecha <= fechaFin)
                .OrderBy(m => m.Fecha)
                .ToListAsync();
        }
    }
}
