using BancoAPI.Application.DTOs;
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

        public async Task<IEnumerable<EstadoCuentaDto>> GetEstadoCuentaAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var movimientos = await _context.Movimientos
                .Include(m => m.Cuenta)
                    .ThenInclude(c => c.ClienteNavigation)
                        .ThenInclude(cli => cli.Persona)
                .Where(m => m.Cuenta.ClienteId == clienteId && 
                           m.Fecha >= fechaInicio && 
                           m.Fecha <= fechaFin)
                .OrderBy(m => m.Fecha)
                .ToListAsync();

            return movimientos.Select(m => new EstadoCuentaDto
            {
                Fecha = m.Fecha.ToString("yyyy-MM-dd"),
                Cliente = m.Cuenta.ClienteNavigation?.Persona?.Nombre ?? "Cliente Desconocido",
                NumeroCuenta = m.Cuenta.NumeroCuenta,
                Tipo = m.Cuenta.TipoCuenta.ToString(),
                SaldoInicial = m.Cuenta.SaldoInicial,
                Estado = m.Cuenta.Estado,
                Movimiento = m.TipoMovimiento == Domain.Enums.TipoMovimiento.Credito ? m.Valor : -m.Valor,
                SaldoDisponible = m.Saldo
            });
        }

        public async Task<(decimal totalDebitos, decimal totalCreditos)> GetTotalesMovimientosAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var movimientos = await _context.Movimientos
                .Include(m => m.Cuenta)
                .Where(m => m.Cuenta.ClienteId == clienteId && 
                           m.Fecha >= fechaInicio && 
                           m.Fecha <= fechaFin)
                .ToListAsync();

            var totalDebitos = movimientos
                .Where(m => m.TipoMovimiento == Domain.Enums.TipoMovimiento.Debito)
                .Sum(m => m.Valor);

            var totalCreditos = movimientos
                .Where(m => m.TipoMovimiento == Domain.Enums.TipoMovimiento.Credito)
                .Sum(m => m.Valor);

            return (totalDebitos, totalCreditos);
        }
    }
}