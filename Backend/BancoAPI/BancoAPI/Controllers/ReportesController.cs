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
        /// Generar reporte de estado de cuenta especificando un rango de fechas y un cliente
        /// Example: /api/reportes?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31&formato=json
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<ReporteEstadoCuentaResponseDto>>> GenerarEstadoCuenta(
            [FromQuery] long clienteId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] string formato = "json")
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(ApiResponse<ReporteEstadoCuentaResponseDto>.Fail("La fecha de inicio no puede ser mayor que la fecha de fin"));
                }

                var reporte = await _reporteService.GenerarEstadoCuentaAsync(clienteId, fechaInicio, fechaFin);

                if (formato.ToLower() == "pdf")
                {
                    var pdfBytes = await _reporteService.GenerarEstadoCuentaPdfAsync(clienteId, fechaInicio, fechaFin);
                    reporte.PdfBase64 = Convert.ToBase64String(pdfBytes);
                }

                return Ok(ApiResponse<ReporteEstadoCuentaResponseDto>.CreateSuccess(reporte, "Estado de cuenta generado exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReporteEstadoCuentaResponseDto>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Descargar reporte de estado de cuenta en formato PDF
        /// Example: /api/reportes/pdf?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31
        /// </summary>
        [HttpGet("pdf")]
        public async Task<IActionResult> DescargarEstadoCuentaPdf(
            [FromQuery] long clienteId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(ApiResponse<object>.Fail("La fecha de inicio no puede ser mayor que la fecha de fin"));
                }

                var pdfBytes = await _reporteService.GenerarEstadoCuentaPdfAsync(clienteId, fechaInicio, fechaFin);
                var nombreArchivo = $"estado_cuenta_{clienteId}_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.pdf";

                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Descargar reporte de estado de cuenta en formato JSON
        /// Example: /api/reportes/json?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31
        /// </summary>
        [HttpGet("json")]
        public async Task<IActionResult> DescargarEstadoCuentaJson(
            [FromQuery] long clienteId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(ApiResponse<object>.Fail("La fecha de inicio no puede ser mayor que la fecha de fin"));
                }

                var jsonContent = await _reporteService.GenerarEstadoCuentaJsonAsync(clienteId, fechaInicio, fechaFin);
                var nombreArchivo = $"estado_cuenta_{clienteId}_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.json";

                return File(System.Text.Encoding.UTF8.GetBytes(jsonContent), "application/json", nombreArchivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"Error interno del servidor: {ex.Message}"));
            }
        }
    }
}