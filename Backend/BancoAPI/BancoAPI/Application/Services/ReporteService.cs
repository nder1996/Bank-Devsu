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
        private readonly string _reportesDirectory;

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
            _reportesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reportes");

            if (!Directory.Exists(_reportesDirectory))
            {
                Directory.CreateDirectory(_reportesDirectory);
            }
        }

        public async Task<ReporteDto> CreateAsync(ReporteRequestDto reporteRequest)
        {
            var cliente = await _clienteRepository.GetByIdAsync(reporteRequest.ClienteId);
            if (cliente == null)
            {
                throw new ArgumentException("El cliente especificado no existe");
            }

            if (reporteRequest.FechaInicio >= reporteRequest.FechaFin)
            {
                throw new ArgumentException("La fecha de inicio debe ser menor a la fecha de fin");
            }

            var reporte = new Reporte
            {
                ClienteId = reporteRequest.ClienteId,
                FechaInicio = reporteRequest.FechaInicio,
                FechaFin = reporteRequest.FechaFin,
                Formato = reporteRequest.Formato,
                FechaGeneracion = DateTime.Now,
                Activo = true
            };

            var nombreArchivo = GenerarNombreArchivo(cliente.Persona?.Nombre ?? "Cliente", reporte.Formato, reporte.FechaGeneracion);
            var rutaArchivo = Path.Combine(_reportesDirectory, nombreArchivo);

            reporte.NombreArchivo = nombreArchivo;
            reporte.RutaArchivo = rutaArchivo;

            await GenerarArchivoReporte(reporte);

            var reporteCreado = await _reporteRepository.CreateAsync(reporte);
            return _mapper.Map<ReporteDto>(reporteCreado);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var reporte = await _reporteRepository.GetByIdAsync(id);
            if (reporte == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(reporte.RutaArchivo) && File.Exists(reporte.RutaArchivo))
            {
                File.Delete(reporte.RutaArchivo);
            }

            return await _reporteRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReporteDto>> GetAllAsync()
        {
            var reportes = await _reporteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReporteDto>>(reportes);
        }

        public async Task<IEnumerable<ReporteDto>> GetByClienteIdAsync(long clienteId)
        {
            var reportes = await _reporteRepository.GetByClienteIdAsync(clienteId);
            return _mapper.Map<IEnumerable<ReporteDto>>(reportes);
        }

        public async Task<ReporteDto?> GetByIdAsync(long id)
        {
            var reporte = await _reporteRepository.GetByIdAsync(id);
            return reporte == null ? null : _mapper.Map<ReporteDto>(reporte);
        }

        public async Task<ReporteDto> UpdateAsync(ReporteDto reporteDto)
        {
            var reporteExistente = await _reporteRepository.GetByIdAsync(reporteDto.id);
            if (reporteExistente == null)
            {
                throw new ArgumentException("El reporte especificado no existe");
            }

            var cliente = await _clienteRepository.GetByIdAsync(reporteDto.clienteId);
            if (cliente == null)
            {
                throw new ArgumentException("El cliente especificado no existe");
            }

            if (reporteDto.fechaInicio >= reporteDto.fechaFin)
            {
                throw new ArgumentException("La fecha de inicio debe ser menor a la fecha de fin");
            }

            var reporte = _mapper.Map<Reporte>(reporteDto);
            
            bool necesitaRegeneracion = reporteExistente.ClienteId != reporteDto.clienteId ||
                                      reporteExistente.FechaInicio != reporteDto.fechaInicio ||
                                      reporteExistente.FechaFin != reporteDto.fechaFin ||
                                      reporteExistente.Formato != reporteDto.formato;

            if (necesitaRegeneracion)
            {
                if (!string.IsNullOrEmpty(reporteExistente.RutaArchivo) && File.Exists(reporteExistente.RutaArchivo))
                {
                    File.Delete(reporteExistente.RutaArchivo);
                }

                var nombreArchivo = GenerarNombreArchivo(cliente.Persona?.Nombre ?? "Cliente", reporte.Formato, DateTime.Now);
                var rutaArchivo = Path.Combine(_reportesDirectory, nombreArchivo);

                reporte.NombreArchivo = nombreArchivo;
                reporte.RutaArchivo = rutaArchivo;
                reporte.FechaGeneracion = DateTime.Now;

                await GenerarArchivoReporte(reporte);
            }

            var reporteActualizado = await _reporteRepository.UpdateAsync(reporte);
            return _mapper.Map<ReporteDto>(reporteActualizado);
        }

        public async Task<byte[]> GenerateReportContentAsync(long reporteId)
        {
            var reporte = await _reporteRepository.GetByIdAsync(reporteId);
            if (reporte == null || string.IsNullOrEmpty(reporte.RutaArchivo) || !File.Exists(reporte.RutaArchivo))
            {
                throw new FileNotFoundException("El archivo del reporte no existe");
            }

            return await File.ReadAllBytesAsync(reporte.RutaArchivo);
        }

        public async Task<string> DownloadReportAsync(long reporteId)
        {
            var reporte = await _reporteRepository.GetByIdAsync(reporteId);
            if (reporte == null || string.IsNullOrEmpty(reporte.RutaArchivo) || !File.Exists(reporte.RutaArchivo))
            {
                throw new FileNotFoundException("El archivo del reporte no existe");
            }

            return reporte.RutaArchivo;
        }

        private async Task GenerarArchivoReporte(Reporte reporte)
        {
            var movimientos = await _movimientoRepository.GetByClienteIdAndDateRangeAsync(
                reporte.ClienteId, reporte.FechaInicio, reporte.FechaFin);

            var movimientosDto = _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);

            var reporteData = new
            {
                Cliente = reporte.Cliente?.Persona?.Nombre ?? "Sin nombre",
                ClienteId = reporte.ClienteId,
                FechaInicio = reporte.FechaInicio,
                FechaFin = reporte.FechaFin,
                FechaGeneracion = reporte.FechaGeneracion,
                Formato = reporte.Formato.ToString(),
                TotalMovimientos = movimientosDto.Count(),
                Movimientos = movimientosDto.Select(m => new
                {
                    Fecha = m.fecha,
                    TipoMovimiento = m.tipoMovimiento.ToString(),
                    Valor = m.valor,
                    Saldo = m.saldo,
                    NumeroCuenta = m.cuenta.numeroCuenta
                }).OrderBy(m => m.Fecha)
            };

            switch (reporte.Formato)
            {
                case ReporteFormato.JSON:
                    var jsonContent = JsonConvert.SerializeObject(reporteData, Formatting.Indented);
                    await File.WriteAllTextAsync(reporte.RutaArchivo, jsonContent);
                    break;

                case ReporteFormato.PDF:
                    await GenerarReportePDF(reporteData, reporte.RutaArchivo);
                    break;

                default:
                    throw new NotSupportedException($"Formato de reporte no soportado: {reporte.Formato}");
            }
        }

        private async Task GenerarReportePDF(object reporteData, string rutaArchivo)
        {
            var jsonContent = JsonConvert.SerializeObject(reporteData, Formatting.Indented);
            var pdfContent = $"REPORTE BANCARIO\n\n{jsonContent}";
            await File.WriteAllTextAsync(rutaArchivo.Replace(".pdf", ".txt"), pdfContent);
        }

        private static string GenerarNombreArchivo(string nombreCliente, ReporteFormato formato, DateTime fechaGeneracion)
        {
            var nombreLimpio = string.Join("_", nombreCliente.Split(Path.GetInvalidFileNameChars()));
            var timestamp = fechaGeneracion.ToString("yyyyMMdd_HHmmss");
            var extension = formato == ReporteFormato.PDF ? "pdf" : "json";
            
            return $"Reporte_{nombreLimpio}_{timestamp}.{extension}";
        }
    }
}