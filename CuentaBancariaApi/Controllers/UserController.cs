using Microsoft.AspNetCore.Mvc;
using Servicios.ContactosService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuentaBancariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserService userService;
        private readonly IConfiguration _configuration;
        public UserController() {

            userService = new UserService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialTres;", _configuration);
        }

        // GET api/<PersonaController>/5
        [HttpGet("findBy{id}")]
        public IActionResult Get(int id)
        {
            var user = userService.ObtenerUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/<PersonaController>
        [HttpGet("ListUsers")]
        public ActionResult Get()
        {
            var users = userService.getAllUser();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        // POST api/<PersonaController>/5
        [HttpPost("create")]
        public IActionResult InsertarUserAccion([FromBody] Infraestructura.Modelos.UserInsertModel user)
        {
            userService.InsertUser(user);
            return Ok();

        }

        [HttpPut("update")]
        public IActionResult ActualizarUser([FromBody] Infraestructura.Modelos.UserInsertModel user)
        {
            try
            {
                userService.UpdateUser(user);
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
            userService.DeleteUser(id);
            return Ok(new { message = "Registro eliminado exitosamente" });
        }
    }
}
