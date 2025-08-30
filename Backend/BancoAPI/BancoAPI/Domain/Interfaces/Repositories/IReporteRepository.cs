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
    }
}