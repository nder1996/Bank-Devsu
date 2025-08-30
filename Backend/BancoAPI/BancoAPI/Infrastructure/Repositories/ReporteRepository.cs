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
            await _context.Reportes.AddAsync(reporte);
            await _context.SaveChangesAsync();
            return reporte;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null)
                return false;

            _context.Reportes.Remove(reporte);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Reporte>> GetAllAsync()
        {
            return await _context.Reportes
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .Where(r => r.Activo)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByClienteIdAsync(long clienteId)
        {
            return await _context.Reportes
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .Where(r => r.ClienteId == clienteId && r.Activo)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reporte>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Reportes
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .Where(r => r.FechaGeneracion >= fechaInicio && 
                           r.FechaGeneracion <= fechaFin && 
                           r.Activo)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        public async Task<Reporte?> GetByIdAsync(long id)
        {
            return await _context.Reportes
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.Activo);
        }

        public async Task<Reporte> UpdateAsync(Reporte reporte)
        {
            var existingReporte = await _context.Reportes.FindAsync(reporte.Id);
            if (existingReporte == null)
            {
                throw new Exception("Reporte not found");
            }

            // Actualizamos las propiedades
            existingReporte.ClienteId = reporte.ClienteId;
            existingReporte.FechaInicio = reporte.FechaInicio;
            existingReporte.FechaFin = reporte.FechaFin;
            existingReporte.Formato = reporte.Formato;
            existingReporte.RutaArchivo = reporte.RutaArchivo;
            existingReporte.NombreArchivo = reporte.NombreArchivo;
            existingReporte.Activo = reporte.Activo;

            _context.Reportes.Update(existingReporte);
            await _context.SaveChangesAsync();
            return existingReporte;
        }
    }
}