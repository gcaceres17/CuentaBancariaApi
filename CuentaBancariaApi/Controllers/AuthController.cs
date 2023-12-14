using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infraestructura.Modelos;
using Servicios.ContactosService;

namespace CuentaBancariaApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserService userService;
        private readonly IConfiguration _configuration;
        public AuthController()
        {

            userService = new UserService("Host=localhost;Port=5432;User Id=postgres;Password=gc.5435747;Database=parcialTres;", _configuration);
        }
        
        [HttpPost]
        public IActionResult Autenticar([FromBody] UserAuthModel userAuthModel)
        {
            try
            {
                var usuarioAutenticado = userService.AutenticarUsuario(userAuthModel.name, userAuthModel.password);

                if (usuarioAutenticado != null)
                {
                    var token = userService.ObtenerTokenAutenticacion(usuarioAutenticado);
                    return Ok(token);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al autenticar usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
