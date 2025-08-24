using BancoAPI.Domain.Entities;
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

        Task<bool> IMovimientoRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }  

        async Task<IEnumerable<Movimiento>> IMovimientoRepository.GetAllAsync()
        {
            return await _context.Movimientos
               .Include(m => m.Cuenta) 
               .ToListAsync();
        }

        async Task<Movimiento?> IMovimientoRepository.GetByIdAsync(int id)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
