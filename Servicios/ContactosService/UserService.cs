using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infraestructura.Datos;
using Infraestructura.Modelos;
using Microsoft.IdentityModel.Tokens;

namespace Servicios.ContactosService
{
    public class UserService
    {
        private UserDatos userDatos;

        public UserService(string cadenaConexion)
        {
            userDatos = new UserDatos(cadenaConexion);
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
        

        public string ObtenerTokenAutenticacion(UserModel userModel)
        {
            if (userModel != null)
            {
                var token = CrearToken(userModel);
                return token;
            }

            return null;
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

        private string CrearToken(UserModel userModel)
        {
            var key = Encoding.ASCII.GetBytes("E@!knadkjbad45678ad.ci@456akjd|!45a");
            var handlerToken = new JwtSecurityTokenHandler();
            var descriptorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userModel.name),
                    new Claim(ClaimTypes.Role, userModel.level)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = handlerToken.CreateToken(descriptorToken);
            return handlerToken.WriteToken(token);
        }
    }
}
