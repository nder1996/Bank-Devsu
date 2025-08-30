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
    }
}
