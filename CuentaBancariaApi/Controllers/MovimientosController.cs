using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuentaBancariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private MovimientoService movimientoService;
        public MovimientosController()
        {
            movimientoService = new MovimientoService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialDos;");
        }

        // GET: api/<CuentaController>
        [HttpGet("ListMovements")]
        public ActionResult Get()
        {
            var personas = movimientoService.obtenerMovimientos();
            if (personas == null)
            {
                return NotFound();
            }
            return Ok(personas);
        }

        // GET api/<CuentaController>/5
        [HttpGet("findBy{accountNumber}")]
        public IActionResult Get(string accountNumber)
        {
            var persona = movimientoService.obtenerMovimientosPorId(accountNumber);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        [HttpPost("create")]
        public IActionResult InsertarPersonaAccion([FromBody] Infraestructura.Modelos.MovimientosInsertModel movimiento)
        {
            movimientoService.InsertarMovimiento(movimiento);
            return Ok();

        }

        [HttpPut("update")]
        public IActionResult ActualizarCuenta([FromBody] Infraestructura.Modelos.MovimientosInsertModel movimiento)
        {
            try
            {
                movimientoService.ActualizarMovimiento(movimiento);
                return Ok("Se actualizó con éxito");
            }
            catch (Exception ex)
            {
                // Puedes registrar el error o realizar un manejo específico aquí
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            movimientoService.EliminarMovimiento(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }
    }
}
