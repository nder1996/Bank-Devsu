using BancoAPI.Application.DTOs;
using BancoAPI.Application.Services;
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


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var movimientos = await _movimientoService.GetAllAsync();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

            // GET api/<ValuesController>/5
            /* [HttpGet("{id}")]
             public string Get(int id)
             {
                 return "value";
             }

             // POST api/<ValuesController>
             [HttpPost]
             public void Post([FromBody]string value)
             {
             }

             // PUT api/<ValuesController>/5
             [HttpPut("{id}")]
             public void Put(int id, [FromBody]string value)
             {
             }

             // DELETE api/<ValuesController>/5
             [HttpDelete("{id}")]
             public void Delete(int id)
             {
             }*/
        }
}
