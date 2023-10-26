using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace personasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private ClienteService clienteService;
        public ClienteController()
        {
            clienteService = new ClienteService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialDos;");
        }

        // GET: api/<ClienteController>
        [HttpGet("ListSubject")]
        public ActionResult Get()
        {
            var personas = clienteService.obtenerClientes();
            if (personas == null)
            {
                return NotFound();
            }
            return Ok(personas);
        }

        // GET api/<ClienteController>/5
        [HttpGet("findBy{documentNumber}")]
        public IActionResult Get(string documentNumber)
        {
            var persona = clienteService.obtenerClientePorId(documentNumber);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        [HttpPost("create")]
        public IActionResult InsertarPersonaAccion([FromBody] Infraestructura.Modelos.ClienteInsertModel cliente)
        {
            clienteService.insertarCliente(cliente);
            return Ok();

        }

        [HttpPut("update")]
        public IActionResult ActualizarCliente([FromBody] Infraestructura.Modelos.ClienteInsertModel cliente)
        {
            try
            {
                clienteService.actualizarCliente(cliente);
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
            clienteService.eliminarCliente(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }

    }
}
