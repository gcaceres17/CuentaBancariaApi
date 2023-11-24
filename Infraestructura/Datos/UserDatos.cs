using Infraestructura.Conexiones;
using Infraestructura.Modelos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Datos
{
    public class UserDatos
    {
        private ConexionDB ConexionDB;

        public UserDatos(String cadenaConexion) {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public UserModel obtenerUserById(int id)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT u.*, p.* FROM usuarios u JOIN persona p on p.\"idPersona\" = u.id_persona where id_usuario ={id};", conn);
            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new UserModel
                {
                    id_usuario = reader.GetInt32("id_usuario"),
                    name = reader.GetString("name"),
                    password = reader.GetString("password"),
                    level = reader.GetString("level"),
                    status = reader.GetString("status"),


                    persona = new PersonaInsertModel
                    {
                        idPersona = reader.GetInt32("idPersona"),
                        nombre = reader.GetString("nombre"),
                        apellido = reader.GetString("apellido"),
                        tipoDocumento = reader.GetString("tipoDocumento"),
                        nroDocumento = reader.GetString("nroDocumento"),
                        direccion = reader.GetString("direccion"),
                        email = reader.GetString("email"),
                        celular = reader.GetString("celular"),
                        estado = reader.GetString("estado"),
                        IdCiudad = reader.GetInt32("idCiudad"),
                    }
                };
            }
            return null;
        }

        public List<UserModel> getAllUsers()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT u.*, p.* FROM usuarios u JOIN persona p on p.\"idPersona\" = u.id_persona", conn);
            List<UserModel> usuarios = new List<UserModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuarios.Add(new UserModel
                    {

                        id_usuario = reader.GetInt32("id_usuario"),
                        name = reader.GetString("name"),
                        password = reader.GetString("password"),
                        level = reader.GetString("level"),
                        status = reader.GetString("status"),

                        persona = new PersonaInsertModel
                        {
                            idPersona = reader.GetInt32("idPersona"),
                            nombre = reader.GetString("nombre"),
                            apellido = reader.GetString("apellido"),
                            tipoDocumento = reader.GetString("tipoDocumento"),
                            nroDocumento = reader.GetString("nroDocumento"),
                            direccion = reader.GetString("direccion"),
                            email = reader.GetString("email"),
                            celular = reader.GetString("celular"),
                            estado = reader.GetString("estado"),
                            IdCiudad = reader.GetInt32("idCiudad"),

                        },


                    });
                }
            }

            return usuarios;

        }

        public void InsertUser(UserInsertModel user)
        {
            try
            {
                using var conn = ConexionDB.GetConexion();
               

                using var cmd = new NpgsqlCommand("INSERT INTO public.usuarios(id_persona, name, password, level) VALUES (@IdPersona, @Name, @Password, @Level);", conn);

                // Parámetros
                cmd.Parameters.AddWithValue("IdPersona", user.Idpersona);
                cmd.Parameters.AddWithValue("Name", user.name);
                cmd.Parameters.AddWithValue("Password", user.password);
                cmd.Parameters.AddWithValue("Level", user.level);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al insertar usuario: {ex.Message}");
            }
        }


        public void UpdateUser(UserInsertModel user)
        {
            try
            {
                using var conn = ConexionDB.GetConexion();
                

                using var cmd = new NpgsqlCommand("UPDATE public.usuarios " +
                                                "SET id_persona=@IdPersona, name=@Name, password=@Password, level=@Level, status=@Status " +
                                                "WHERE id_usuario=@IdUsuario;", conn);

                // Parámetros
                cmd.Parameters.AddWithValue("IdUsuario", user.id_usuario);
                cmd.Parameters.AddWithValue("IdPersona", user.Idpersona);
                cmd.Parameters.AddWithValue("Name", user.name);
                cmd.Parameters.AddWithValue("Password", user.password);
                cmd.Parameters.AddWithValue("Level", user.level);
                cmd.Parameters.AddWithValue("Status", user.status);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                using var conn = ConexionDB.GetConexion();

                using var cmd = new NpgsqlCommand("DELETE FROM public.usuarios WHERE id_usuario=@UserId;", conn);

                // Parámetro
                cmd.Parameters.AddWithValue("UserId", userId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            }
        }

        public UserModel AutenticarUsuario(string userName, string password)
        {
            try
            {
                using var conn = ConexionDB.GetConexion();

                using var cmd = new NpgsqlCommand("SELECT * FROM public.usuarios WHERE name=@UserName AND password=@Password;", conn);
                cmd.Parameters.AddWithValue("UserName", userName);
                cmd.Parameters.AddWithValue("Password", password);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new UserModel
                    {
                        id_usuario = reader.GetInt32("id_usuario"),
                        name = reader.GetString("name"),
                        password = reader.GetString("password"),
                        level = reader.GetString("level"),
                        status = reader.GetString("status"),

                    };
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al autenticar usuario: {ex.Message}");
            }

            return null; 
        }
    }

}
    
