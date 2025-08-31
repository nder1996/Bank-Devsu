using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Infrastructure.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly BancoDbContext _context;

        public ReporteRepository(BancoDbContext context)
        {
            _context = context;
        }

        public async Task<Reporte> CreateAsync(Reporte reporte)
        {
            // No crear reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // No eliminar reportes ya que la tabla no existe
            return false;
        }

        public async Task<IEnumerable<Reporte>> GetAllAsync()
        {
            // Devolver una lista vacía ya que la tabla no existe
            return new List<Reporte>();
        }

        public async Task<IEnumerable<Reporte>> GetByClienteIdAsync(long clienteId)
        {
            // Devolver una lista vacía ya que la tabla no existe
            return new List<Reporte>();
        }

        public async Task<IEnumerable<Reporte>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            // Devolver una lista vacía ya que la tabla no existe
            return new List<Reporte>();
        }

        public async Task<Reporte?> GetByIdAsync(long id)
        {
            // Devolver null ya que la tabla no existe
            return null;
        }

        public async Task<Reporte> UpdateAsync(Reporte reporte)
        {
            // No actualizar reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }
        
        // Nuevos métodos para obtener datos de reportes basados en cuentas y movimientos
        public async Task<IEnumerable<Cuenta>> GetCuentasConMovimientosAsync()
        {
            return await _context.Cuentas
                .Include(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Include(c => c.Movimientos)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Movimiento>> GetMovimientosByClienteIdAsync(long clienteId)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Where(m => m.Cuenta.ClienteId == clienteId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Movimiento>> GetMovimientosByClienteIdAndDateRangeAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Where(m => m.Cuenta.ClienteId == clienteId && m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .OrderBy(m => m.Fecha)
                .ToListAsync();
        }
        
        // Consultas avanzadas con más filtros para reportes
        public async Task<IEnumerable<Movimiento>> GetMovimientosByClienteConSaldoAsync(long clienteId)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Where(m => m.Cuenta.ClienteId == clienteId)
                .OrderBy(m => m.Fecha)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<dynamic>> GetResumenMovimientosPorClienteAsync()
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .GroupBy(m => new { 
                    ClienteId = m.Cuenta.ClienteId,
                    ClienteNombre = m.Cuenta.ClienteNavigation.Persona.Nombre,
                    CuentaNumero = m.Cuenta.NumeroCuenta
                })
                .Select(g => new {
                    ClienteId = g.Key.ClienteId,
                    Cliente = g.Key.ClienteNombre,
                    NumeroCuenta = g.Key.CuentaNumero,
                    TotalMovimientos = g.Count(),
                    TotalCreditos = g.Where(m => m.TipoMovimiento == Domain.Enums.TipoMovimiento.Credito).Sum(m => m.Valor),
                    TotalDebitos = g.Where(m => m.TipoMovimiento == Domain.Enums.TipoMovimiento.Debito).Sum(m => m.Valor),
                    SaldoActual = g.OrderByDescending(m => m.Fecha).FirstOrDefault().Saldo,
                    UltimoMovimiento = g.Max(m => m.Fecha)
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<dynamic>> GetReporteEstadoCuentaPorClienteAsync(long clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Where(m => m.Cuenta.ClienteId == clienteId);
                
            if (fechaInicio.HasValue)
                query = query.Where(m => m.Fecha >= fechaInicio.Value);
                
            if (fechaFin.HasValue)
                query = query.Where(m => m.Fecha <= fechaFin.Value);
                
            return await query
                .Select(m => new {
                    Fecha = m.Fecha,
                    TipoMovimiento = m.TipoMovimiento.ToString(),
                    Valor = m.Valor,
                    Saldo = m.Saldo,
                    Cliente = m.Cuenta.ClienteNavigation.Persona.Nombre,
                    NumeroCuenta = m.Cuenta.NumeroCuenta,
                    TipoCuenta = m.Cuenta.TipoCuenta.ToString()
                })
                .OrderBy(m => m.Fecha)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<dynamic>> GetReporteResumenPorTipoCuentaAsync()
        {
            return await _context.Cuentas
                .Include(c => c.ClienteNavigation)
                .ThenInclude(cli => cli.Persona)
                .Include(c => c.Movimientos)
                .GroupBy(c => c.TipoCuenta)
                .Select(g => new {
                    TipoCuenta = g.Key.ToString(),
                    TotalCuentas = g.Count(),
                    TotalClientes = g.Select(c => c.ClienteId).Distinct().Count(),
                    SaldoPromedio = g.Average(c => c.SaldoInicial),
                    TotalMovimientos = g.SelectMany(c => c.Movimientos).Count(),
                    MontoTotalTransacciones = g.SelectMany(c => c.Movimientos).Sum(m => m.Valor)
                })
                .ToListAsync();
        }
    }
}