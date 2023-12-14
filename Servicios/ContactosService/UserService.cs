using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infraestructura.Datos;
using Infraestructura.Modelos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace Servicios.ContactosService
{
    public class UserService
    {
        private UserDatos userDatos;
        private readonly IConfiguration _configuration;


        public UserService(string cadenaConexion, IConfiguration _configuration)
        {
            userDatos = new UserDatos(cadenaConexion);
            _configuration = _configuration;
        }

        public UserModel ObtenerUserById(int id)
        {
            return userDatos.obtenerUserById(id);
        }

        public UserModel AutenticarUsuario(string userName, string password)
        {
            try
            {
                
                var usuarioAutenticado = userDatos.AutenticarUsuario(userName, password);

                return usuarioAutenticado;
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al autenticar usuario: {ex.Message}");
                return null;
            }
        }

        public List<UserModel> getAllUser() {

            return userDatos.getAllUsers();

        }
        
        public void InsertUser(UserInsertModel user)
        {
            ValidarDatos(user);
            userDatos.InsertUser(user);
        }

        public void UpdateUser(UserInsertModel user)
        {
            ValidarDatos(user);
            userDatos.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            userDatos.DeleteUser(id);
        }

        private void ValidarDatos(UserInsertModel user)
        {
            if (user.name.Trim().Length == 0)
            {
                throw new Exception("Se debe cargar el nombre");
            }

            if (user.password.Trim().Length == 0)
            {
                throw new Exception("Se debe cargar la contraseña");
            }
        }

        public string ObtenerTokenAutenticacion(UserModel usuarioAutenticado)
        {
            if (usuarioAutenticado != null)
            {
                // Obtener más información del usuario (nombre, apellido, correo electrónico, etc.)
                string nombre = usuarioAutenticado.name; // Ajusta esto según la propiedad real en tu modelo
                string apellido = usuarioAutenticado.level; // Ajusta esto según la propiedad real en tu modelo

                // Generar el token con información adicional
                var token = CrearToken(usuarioAutenticado.name, nombre, apellido);

                return token;
            }

            return null;
        }

        private string CrearToken(string userName, string nombre, string apellido)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("nombre", nombre),
                new Claim("apellido", apellido),
        
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
