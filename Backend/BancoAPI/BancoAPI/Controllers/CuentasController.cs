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

        // GET: api/Cuentas/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Cuenta>> Get(long id)
        {
            var cuenta = await _cuentaService.ObtenerPorIdAsync(id);
            if (cuenta is null)
                return NotFound();

            return Ok(cuenta);
        }

        // POST: api/Cuentas
        [HttpPost]
        public async Task<ActionResult<Cuenta>> Post([FromBody] CuentaDto cuenta)
        {
            var nuevaCuenta = await _cuentaService.CrearAsync(cuenta);
            return CreatedAtAction(nameof(Get), new { id = nuevaCuenta.id }, nuevaCuenta);
        }

        // PUT: api/Cuentas/{id}/estado
        [HttpPut("{id:long}/estado")]
        public async Task<IActionResult> PutEstado(long id, [FromBody] bool nuevoEstado)
        {
            var resultado = await _cuentaService.ActualizarEstadoAsync(id, nuevoEstado);
            if (!resultado)
                return NotFound();

            return Ok();
        }

        // PUT: api/Cuentas/{id}/tipo
        [HttpPut("{id:long}/tipo")]
        public async Task<IActionResult> PutTipo(long id, [FromBody] string nuevoTipo)
        {
            var resultado = await _cuentaService.ActualizarTipoCuentaAsync(id, nuevoTipo);
            if (!resultado)
                return NotFound();

            return Ok();
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
