using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace personasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private PersonaService personaService;
        public PersonaController()
        {
            personaService = new PersonaService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialTres;");
        }

        // GET: api/<PersonaController>
        [HttpGet("ListPersons")]
        public ActionResult Get()
        {
            var personas = personaService.obtenerPersonas();
            if (personas == null)
            {
                return NotFound();
            }
            return Ok(personas);
        }

        // GET api/<PersonaController>/5
        [HttpGet("findBy{documentNumber}")]
        public IActionResult Get(string documentNumber)
        {
            var persona = personaService.obtenerPersonaById(documentNumber);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        // POST api/<PersonaController>/5
        [HttpPost("create")]
        public IActionResult InsertarPersonaAccion([FromBody] Infraestructura.Modelos.PersonaInsertModel persona)
        {
            personaService.insertarPersona(persona);
            return Ok();
           
        }

        [HttpPut("update")]
        public IActionResult ActualizarPersona([FromBody] Infraestructura.Modelos.PersonaInsertModel persona)
        {
            try
            {
                personaService.actualizarPersona(persona);
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
            personaService.eliminarPersona(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }

    }
}
