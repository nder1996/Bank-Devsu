using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        /// <summary>
        /// Obtiene todos los reportes disponibles basados en datos existentes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReporteListadoDto>>>> Get()
        {
            try
            {
                var reportes = await _reporteService.GetReportesFromExistingDataAsync();
                return Ok(ApiResponse<IEnumerable<ReporteListadoDto>>.CreateSuccess(reportes, "Reportes obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ReporteListadoDto>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtiene un reporte por su ID (no implementado ya que la tabla no existe)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Get(long id)
        {
            try
            {
                var reporte = await _reporteService.GetByIdAsync(id);
                if (reporte == null)
                {
                    return NotFound(ApiResponse<ReporteDto>.Fail("Reporte no encontrado"));
                }

                return Ok(ApiResponse<ReporteDto>.CreateSuccess(reporte, "Reporte obtenido exitosamente"));
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, ApiResponse<ReporteDto>.Fail("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtiene todos los reportes de un cliente específico usando datos existentes de movimientos
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MovimientoDto>>>> GetByClienteId(long clienteId)
        {
            try
            {
                var movimientos = await _reporteService.GetMovimientosByClienteAsync(clienteId);
                return Ok(ApiResponse<IEnumerable<MovimientoDto>>.CreateSuccess(movimientos, "Movimientos del cliente obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MovimientoDto>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Crea un nuevo reporte (no implementado ya que la tabla no existe)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Post([FromBody] ReporteRequestDto reporteRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return BadRequest(ApiResponse<ReporteDto>.Fail($"Datos inválidos: {errors}"));
                }

                var nuevoReporte = await _reporteService.CreateAsync(reporteRequest);
                return CreatedAtAction(nameof(Get), new { id = nuevoReporte.id }, 
                    ApiResponse<ReporteDto>.CreateSuccess(nuevoReporte, "Reporte creado exitosamente"));
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, ApiResponse<ReporteDto>.Fail("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualiza un reporte existente (no implementado ya que la tabla no existe)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Put(long id, [FromBody] ReporteDto reporte)
        {
            try
            {
                if (id != reporte.id)
                {
                    return BadRequest(ApiResponse<ReporteDto>.Fail("El ID del parámetro no coincide con el ID del reporte"));
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return BadRequest(ApiResponse<ReporteDto>.Fail($"Datos inválidos: {errors}"));
                }

                var reporteActualizado = await _reporteService.UpdateAsync(reporte);
                return Ok(ApiResponse<ReporteDto>.CreateSuccess(reporteActualizado, "Reporte actualizado exitosamente"));
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, ApiResponse<ReporteDto>.Fail("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Elimina un reporte (marcado como inactivo) (no implementado ya que la tabla no existe)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(long id)
        {
            try
            {
                var eliminado = await _reporteService.DeleteAsync(id);
                if (!eliminado)
                {
                    return NotFound(ApiResponse<object>.Fail("Reporte no encontrado"));
                }

                return Ok(ApiResponse<object>.CreateSuccess(null, "Reporte eliminado exitosamente"));
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, ApiResponse<object>.Fail("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Descarga el contenido de un reporte (no implementado ya que la tabla no existe)
        /// </summary>
        [HttpGet("{id}/download")]
        public async Task<ActionResult> DownloadReport(long id)
        {
            try
            {
                var reporte = await _reporteService.GetByIdAsync(id);
                if (reporte == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Reporte no encontrado"));
                }

                // Este método no está implementado ya que la tabla no existe
                throw new NotImplementedException("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, ApiResponse<object>.Fail("Funcionalidad no implementada: La tabla Reportes no existe en la base de datos"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Obtiene los movimientos de un cliente específico
        /// </summary>
        [HttpGet("movimientos/cliente/{clienteId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MovimientoDto>>>> GetMovimientosByCliente(long clienteId)
        {
            try
            {
                var movimientos = await _reporteService.GetMovimientosByClienteAsync(clienteId);
                return Ok(ApiResponse<IEnumerable<MovimientoDto>>.CreateSuccess(movimientos, "Movimientos del cliente obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MovimientoDto>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Obtiene los movimientos de un cliente en un rango de fechas
        /// </summary>
        [HttpGet("movimientos/cliente/{clienteId}/rango")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MovimientoDto>>>> GetMovimientosByClienteAndDateRange(
            long clienteId, 
            [FromQuery] DateTime fechaInicio, 
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                var movimientos = await _reporteService.GetMovimientosByClienteAndDateRangeAsync(clienteId, fechaInicio, fechaFin);
                return Ok(ApiResponse<IEnumerable<MovimientoDto>>.CreateSuccess(movimientos, "Movimientos del cliente en rango de fechas obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MovimientoDto>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Obtiene un resumen de movimientos agrupados por cliente
        /// </summary>
        [HttpGet("resumen/clientes")]
        public async Task<ActionResult<ApiResponse<IEnumerable<dynamic>>>> GetResumenMovimientosPorCliente()
        {
            try
            {
                var resumen = await _reporteService.GetResumenMovimientosPorClienteAsync();
                return Ok(ApiResponse<IEnumerable<dynamic>>.CreateSuccess(resumen, "Resumen de movimientos por cliente obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<dynamic>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Obtiene el estado de cuenta detallado de un cliente con filtros opcionales
        /// </summary>
        [HttpGet("estado-cuenta/cliente/{clienteId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<dynamic>>>> GetEstadoCuentaPorCliente(
            long clienteId,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            try
            {
                var estadoCuenta = await _reporteService.GetReporteEstadoCuentaPorClienteAsync(clienteId, fechaInicio, fechaFin);
                return Ok(ApiResponse<IEnumerable<dynamic>>.CreateSuccess(estadoCuenta, "Estado de cuenta del cliente obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<dynamic>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
        
        /// <summary>
        /// Obtiene un resumen de cuentas agrupadas por tipo
        /// </summary>
        [HttpGet("resumen/tipos-cuenta")]
        public async Task<ActionResult<ApiResponse<IEnumerable<dynamic>>>> GetResumenPorTipoCuenta()
        {
            try
            {
                var resumen = await _reporteService.GetReporteResumenPorTipoCuentaAsync();
                return Ok(ApiResponse<IEnumerable<dynamic>>.CreateSuccess(resumen, "Resumen por tipo de cuenta obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<dynamic>>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
    }
}