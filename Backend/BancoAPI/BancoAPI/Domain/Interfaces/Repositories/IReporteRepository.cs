using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Repositories
{
    public interface IReporteRepository
    {
        Task<Reporte> CreateAsync(Reporte reporte);
        Task<Reporte?> GetByIdAsync(long id);
        Task<IEnumerable<Reporte>> GetAllAsync();
        Task<Reporte> UpdateAsync(Reporte reporte);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<Reporte>> GetByClienteIdAsync(long clienteId);
        Task<IEnumerable<Reporte>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin);
        
        // Nuevos métodos para obtener datos de reportes basados en cuentas y movimientos
        Task<IEnumerable<Cuenta>> GetCuentasConMovimientosAsync();
        Task<IEnumerable<Movimiento>> GetMovimientosByClienteIdAsync(long clienteId);
        Task<IEnumerable<Movimiento>> GetMovimientosByClienteIdAndDateRangeAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
        
        // Consultas avanzadas con filtros para reportes
        Task<IEnumerable<Movimiento>> GetMovimientosByClienteConSaldoAsync(long clienteId);
        Task<IEnumerable<dynamic>> GetResumenMovimientosPorClienteAsync();
        Task<IEnumerable<dynamic>> GetReporteEstadoCuentaPorClienteAsync(long clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<IEnumerable<dynamic>> GetReporteResumenPorTipoCuentaAsync();
    }
}