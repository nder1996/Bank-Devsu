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
    public class CuentasController : ControllerBase
    {


        private readonly ICuentaService _cuentaService;

        public CuentasController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        // GET: api/Cuentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaDto>>> Get()
        {
            var cuentas = await _cuentaService.ObtenerTodasAsync();
            return Ok(cuentas);
        }

        // POST: api/Cuentas
        [HttpPost]
        public async Task<ActionResult<Cuenta>> Post([FromBody] CuentaDto cuenta)
        {
            var nuevaCuenta = await _cuentaService.CrearAsync(cuenta);
            return CreatedAtAction(nameof(Get), new { id = nuevaCuenta.id }, nuevaCuenta);
        }

        // PUT: api/Cuentas/5
        [HttpPut("{id:long}")]
        public async Task<ActionResult<CuentaDto>> Put(long id, [FromBody] CuentaDto cuentaDto)
        {
            var cuentaActualizada = await _cuentaService.ActualizarAsync(id, cuentaDto);
            if (cuentaActualizada == null)
                return NotFound();

            return Ok(cuentaActualizada);
        }

        // DELETE: api/Cuentas/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var eliminado = await _cuentaService.EliminarAsync(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }

    }
}
