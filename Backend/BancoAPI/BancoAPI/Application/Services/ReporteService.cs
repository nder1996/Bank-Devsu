using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Enums;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;
using Newtonsoft.Json;

namespace BancoAPI.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly IMapper _mapper;

        public ReporteService(
            IReporteRepository reporteRepository,
            IClienteRepository clienteRepository,
            IMovimientoRepository movimientoRepository,
            IMapper mapper)
        {
            _reporteRepository = reporteRepository;
            _clienteRepository = clienteRepository;
            _movimientoRepository = movimientoRepository;
            _mapper = mapper;
        }

        public async Task<ReporteDto> CreateAsync(ReporteRequestDto reporteRequest)
        {
            // No crear reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // No eliminar reportes ya que la tabla no existe
            return false;
        }

        public async Task<IEnumerable<ReporteDto>> GetAllAsync()
        {
            // Devolver una lista vacía ya que la tabla no existe
            return new List<ReporteDto>();
        }

        public async Task<IEnumerable<ReporteDto>> GetByClienteIdAsync(long clienteId)
        {
            // Devolver una lista vacía ya que la tabla no existe
            return new List<ReporteDto>();
        }

        public async Task<ReporteDto?> GetByIdAsync(long id)
        {
            // Devolver null ya que la tabla no existe
            return null;
        }

        public async Task<ReporteDto> UpdateAsync(ReporteDto reporteDto)
        {
            // No actualizar reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }

        public async Task<byte[]> GenerateReportContentAsync(long reporteId)
        {
            // No generar contenido de reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }

        public async Task<string> DownloadReportAsync(long reporteId)
        {
            // No descargar reportes ya que la tabla no existe
            throw new NotImplementedException("La tabla Reportes no existe en la base de datos");
        }
        
        // Nuevo método para obtener datos de reportes basados en cuentas y movimientos existentes
        public async Task<IEnumerable<ReporteListadoDto>> GetReportesFromExistingDataAsync()
        {
            // Obtener todas las cuentas con sus movimientos y clientes
            var cuentas = await _reporteRepository.GetCuentasConMovimientosAsync();
            
            // Crear una lista de reportes basados en los datos existentes
            var reportes = new List<ReporteListadoDto>();
            
            foreach (var cuenta in cuentas)
            {
                if (cuenta.Movimientos.Any())
                {
                    // Obtener las fechas mínima y máxima de los movimientos
                    var fechaMinima = cuenta.Movimientos.Min(m => m.Fecha);
                    var fechaMaxima = cuenta.Movimientos.Max(m => m.Fecha);
                    
                    // Crear un reporte para esta cuenta
                    reportes.Add(new ReporteListadoDto
                    {
                        Id = cuenta.Id,
                        Cliente = cuenta.ClienteNavigation?.Persona?.Nombre ?? "Cliente desconocido",
                        Periodo = $"{fechaMinima:dd/MM/yy} - {fechaMaxima:dd/MM/yy}",
                        Formato = ReporteFormato.JSON // Por defecto JSON
                    });
                }
            }
            
            return reportes;
        }
        
        // Nuevo método para obtener movimientos de un cliente específico
        public async Task<IEnumerable<MovimientoDto>> GetMovimientosByClienteAsync(long clienteId)
        {
            var movimientos = await _reporteRepository.GetMovimientosByClienteIdAsync(clienteId);
            return _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);
        }
        
        // Nuevo método para obtener movimientos de un cliente en un rango de fechas
        public async Task<IEnumerable<MovimientoDto>> GetMovimientosByClienteAndDateRangeAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var movimientos = await _reporteRepository.GetMovimientosByClienteIdAndDateRangeAsync(clienteId, fechaInicio, fechaFin);
            return _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);
        }
        
        // Nuevos métodos con consultas avanzadas y filtros
        public async Task<IEnumerable<dynamic>> GetResumenMovimientosPorClienteAsync()
        {
            return await _reporteRepository.GetResumenMovimientosPorClienteAsync();
        }
        
        public async Task<IEnumerable<dynamic>> GetReporteEstadoCuentaPorClienteAsync(long clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _reporteRepository.GetReporteEstadoCuentaPorClienteAsync(clienteId, fechaInicio, fechaFin);
        }
        
        public async Task<IEnumerable<dynamic>> GetReporteResumenPorTipoCuentaAsync()
        {
            return await _reporteRepository.GetReporteResumenPorTipoCuentaAsync();
        }
    }
}