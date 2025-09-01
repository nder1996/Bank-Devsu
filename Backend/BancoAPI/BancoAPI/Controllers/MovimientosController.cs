using BancoAPI.Application.DTOs;
using BancoAPI.Application.Exceptions;
using BancoAPI.Application.Services;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BancoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {

        private readonly IMovimientoService _movimientoService;

        public MovimientosController(IMovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }


        // GET: api/Movimientos
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MovimientoDto>>>> Get()
        {
            try
            {
                var movimientos = await _movimientoService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<MovimientoDto>>.CreateSuccess(movimientos, "Movimientos obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<MovimientoDto>>.Fail($"Error al obtener movimientos: {ex.Message}"));
            }
        }


        // POST: api/Movimientos
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MovimientoDto>>> Post([FromBody] MovimientoDto movimiento)
        {
            try
            {
                var nuevoMovimiento = await _movimientoService.CreateAsync(movimiento);
                return CreatedAtAction(nameof(Get), new { id = nuevoMovimiento.id }, 
                    ApiResponse<MovimientoDto>.CreateSuccess(nuevoMovimiento, "Movimiento creado exitosamente"));
            }
            catch (SaldoNoDisponibleException ex)
            {
                return BadRequest(ex.ToApiResponse());
            }
            catch (CupoDiarioExcedidoException ex)
            {
                return BadRequest(ex.ToApiResponse());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MovimientoDto>.Fail($"Error al crear movimiento: {ex.Message}"));
            }
        }

        // PUT: api/Movimientos/5
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<MovimientoDto>>> Put(long id, [FromBody] MovimientoDto movimiento)
        {
            try
            {
                if (id != movimiento.id)
                    return BadRequest(ApiResponse<MovimientoDto>.Fail("El ID del movimiento no coincide"));

                var movimientoActualizado = await _movimientoService.UpdateAsync(movimiento);
                return Ok(ApiResponse<MovimientoDto>.CreateSuccess(movimientoActualizado, "Movimiento actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MovimientoDto>.Fail($"Error al actualizar movimiento: {ex.Message}"));
            }
        }

        // DELETE: api/Movimientos/5
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(long id)
        {
            try
            {
                var resultado = await _movimientoService.DeleteAsync(id);
                if (!resultado)
                    return NotFound(ApiResponse<object>.Fail("Movimiento no encontrado"));
                
                return Ok(ApiResponse<object>.CreateSuccess(null, "Movimiento eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail($"Error al eliminar movimiento: {ex.Message}"));
            }
        }
    }
}
