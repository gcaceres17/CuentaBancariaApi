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
        private readonly UserService userService;

        public AuthController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Autenticar([FromBody] UserAuthModel userModel)
        {
            try
            {
                var usuarioAutenticado = userService.AutenticarUsuario(userModel.name, userModel.password);

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
