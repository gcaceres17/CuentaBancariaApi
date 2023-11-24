using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuentaBancariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {

        private const string connectionString = "Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialTres;";
        private CiudadService servicio;

        public CiudadController()
        {
            servicio = new CiudadService(connectionString);
        }

        [HttpGet("ListCities")]
        public ActionResult Get()
        {
            var ciudad = servicio.obtenerCiudades();
            if (ciudad == null)
            {
                return NotFound();
            }
            return Ok(ciudad);
        }

        [HttpGet("findBy{id}")]
        public ActionResult GetCity(int id)
        {
            var ciudad = servicio.obtenerCiudadById(id);
            if (ciudad == null)
            {
                return NotFound();
            }
            return Ok(ciudad);
        }

        [HttpPost("create")]
        public IActionResult InsertarCiudadAccion([FromBody] Infraestructura.Modelos.CiudadModel ciudad)
        {
            servicio.insertarCiudad(ciudad);
            return Created("Se creo con exito!!", ciudad);
        }

        [HttpPut("update")]
        public IActionResult ModificarCiudadAccion([FromBody] Infraestructura.Modelos.CiudadModel ciudad)
        {
            servicio.modificarCiudad(ciudad);
            return Ok("Se actualizo con exito!!");
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            servicio.eliminarCiudad(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }
    }
}
