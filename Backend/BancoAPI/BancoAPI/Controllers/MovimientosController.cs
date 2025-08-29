using BancoAPI.Application.DTOs;
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
        public async Task<ActionResult<IEnumerable<MovimientoDto>>> Get()
        {
            var movimientos = await _movimientoService.GetAllAsync();
            return Ok(movimientos);
        }

        // GET: api/Movimientos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Movimiento>> Get(int id)
        {
            var movimiento = await _movimientoService.GetByIdAsync(id);
            if (movimiento == null)
                return NotFound();
            return Ok(movimiento);
        }

        // POST: api/Movimientos
        [HttpPost]
        public async Task<ActionResult<Movimiento>> Post([FromBody] MovimientoDto movimiento)
        {
            var nuevoMovimiento = await _movimientoService.CreateAsync(movimiento);
            return CreatedAtAction(nameof(Get), new { id = nuevoMovimiento.id }, nuevoMovimiento);
        }

        // PUT: api/Movimientos/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Movimiento>> Put(int id, [FromBody] MovimientoDto movimiento)
        {
            if (id != movimiento.id)
                return BadRequest("El ID del movimiento no coincide.");

            var movimientoActualizado = await _movimientoService.UpdateAsync(movimiento);
            return Ok(movimientoActualizado);
        }

        // DELETE: api/Movimientos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _movimientoService.DeleteAsync(id);
            if (!resultado)
                return NotFound();
            return NoContent();
        }
    }
}
