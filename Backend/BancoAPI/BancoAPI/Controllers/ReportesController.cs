using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Interfaces.Services;
using BancoAPI.Infrastructure.Common;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReporteDto>>>> Get()
        {
            try
            {
                var reportes = await _reporteService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<ReporteDto>>.CreateSuccess(reportes, "Reportes obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ReporteDto>>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Get(long id)
        {
            try
            {
                var reporte = await _reporteService.GetByIdAsync(id);
                if (reporte == null)
                {
                    return NotFound(ApiResponse<ReporteDto>.CreateError("Reporte no encontrado"));
                }

                return Ok(ApiResponse<ReporteDto>.CreateSuccess(reporte, "Reporte obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReporteDto>>>> GetByClienteId(long clienteId)
        {
            try
            {
                var reportes = await _reporteService.GetByClienteIdAsync(clienteId);
                return Ok(ApiResponse<IEnumerable<ReporteDto>>.CreateSuccess(reportes, "Reportes del cliente obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ReporteDto>>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Post([FromBody] ReporteRequestDto reporteRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return BadRequest(ApiResponse<ReporteDto>.CreateError($"Datos inválidos: {errors}"));
                }

                var nuevoReporte = await _reporteService.CreateAsync(reporteRequest);
                return CreatedAtAction(nameof(Get), new { id = nuevoReporte.id }, 
                    ApiResponse<ReporteDto>.CreateSuccess(nuevoReporte, "Reporte creado exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<ReporteDto>.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ReporteDto>>> Put(long id, [FromBody] ReporteDto reporte)
        {
            try
            {
                if (id != reporte.id)
                {
                    return BadRequest(ApiResponse<ReporteDto>.CreateError("El ID del parámetro no coincide con el ID del reporte"));
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return BadRequest(ApiResponse<ReporteDto>.CreateError($"Datos inválidos: {errors}"));
                }

                var reporteActualizado = await _reporteService.UpdateAsync(reporte);
                return Ok(ApiResponse<ReporteDto>.CreateSuccess(reporteActualizado, "Reporte actualizado exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<ReporteDto>.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteDto>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(long id)
        {
            try
            {
                var eliminado = await _reporteService.DeleteAsync(id);
                if (!eliminado)
                {
                    return NotFound(ApiResponse<object>.CreateError("Reporte no encontrado"));
                }

                return Ok(ApiResponse<object>.CreateSuccess(null, "Reporte eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpGet("{id}/download")]
        public async Task<ActionResult> DownloadReport(long id)
        {
            try
            {
                var reporte = await _reporteService.GetByIdAsync(id);
                if (reporte == null)
                {
                    return NotFound(ApiResponse<object>.CreateError("Reporte no encontrado"));
                }

                var contenido = await _reporteService.GenerateReportContentAsync(id);
                var contentType = reporte.formato == Domain.Enums.ReporteFormato.PDF ? "application/pdf" : "application/json";
                
                return File(contenido, contentType, reporte.nombreArchivo);
            }
            catch (FileNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError("Archivo del reporte no encontrado"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.CreateError($"Error interno del servidor: {ex.Message}"));
            }
        }
    }
}
