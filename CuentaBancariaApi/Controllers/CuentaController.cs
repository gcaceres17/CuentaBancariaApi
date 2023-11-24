using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuentaBancariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {

        private CuentaService cuentaService;
        public CuentaController()
        {
            cuentaService = new CuentaService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialTres;");
        }

        // GET: api/<CuentaController>
        [HttpGet("ListAccounts")]
        public ActionResult Get()
        {
            var personas = cuentaService.obtenerCuentas();
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
            var persona = cuentaService.obtenerCuentasPorId(accountNumber);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }


        [HttpPost("create")]
        public IActionResult InsertarPersonaAccion([FromBody] Infraestructura.Modelos.CuentaInserModel cuenta)
        {
            cuentaService.InsertarCuenta(cuenta);
            return Ok();

        }

        [HttpPut("update")]
        public IActionResult ActualizarCuenta([FromBody] Infraestructura.Modelos.CuentaInserModel cuenta)
        {
            try
            {
                cuentaService.ActualizarCuenta(cuenta);
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
            cuentaService.EliminarCuenta(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }
    }
}
