using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BancoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }


        // POST: api/Clientes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteDto cliente)
        {
            var nuevoCliente = await _clienteService.CrearAsync(cliente);
            return CreatedAtAction(nameof(Get), new { id = nuevoCliente.id }, nuevoCliente);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteDto cliente)
        {
            if (cliente.id != id)
            {
                return BadRequest("El identificador no coincide");
            }

            var clienteActualizado = await _clienteService.ActualizarAsync(cliente);
            return Ok(clienteActualizado);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var eliminado = await _clienteService.EliminarAsync(id);
            if (eliminado)
            {
                return NoContent();
            }
            return BadRequest("No se pudo eliminar el cliente");
        }
    }
}
