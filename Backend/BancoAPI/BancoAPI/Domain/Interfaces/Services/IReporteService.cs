using BancoAPI.Application.DTOs;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IReporteService
    {
        Task<ReporteDto> CreateAsync(ReporteRequestDto reporteRequest);
        Task<ReporteDto?> GetByIdAsync(long id);
        Task<IEnumerable<ReporteDto>> GetAllAsync();
        Task<ReporteDto> UpdateAsync(ReporteDto reporte);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<ReporteDto>> GetByClienteIdAsync(long clienteId);
        Task<byte[]> GenerateReportContentAsync(long reporteId);
        Task<string> DownloadReportAsync(long reporteId);
        
        // Nuevos métodos para obtener datos de reportes basados en cuentas y movimientos existentes
        Task<IEnumerable<ReporteListadoDto>> GetReportesFromExistingDataAsync();
        Task<IEnumerable<MovimientoDto>> GetMovimientosByClienteAsync(long clienteId);
        Task<IEnumerable<MovimientoDto>> GetMovimientosByClienteAndDateRangeAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin);
        
        // Métodos con consultas avanzadas y filtros
        Task<IEnumerable<dynamic>> GetResumenMovimientosPorClienteAsync();
        Task<IEnumerable<dynamic>> GetReporteEstadoCuentaPorClienteAsync(long clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<IEnumerable<dynamic>> GetReporteResumenPorTipoCuentaAsync();
    }
}