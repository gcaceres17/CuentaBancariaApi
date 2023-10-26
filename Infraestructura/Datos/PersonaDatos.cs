using Infraestructura.Conexiones;
using Infraestructura.Modelos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Datos
{
    public class PersonasDatos
    {
        private ConexionDB ConexionDB;
        public PersonasDatos(String cadenaConexion)
        {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public PersonaModel obtenerPersonaPorId(string documento)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT p.* , c.* FROM persona p inner join ciudad c on c.\"idCiudad\" = p.\"idCiudad\" where p.\"nroDocumento\" = '{documento}';", conn);

            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new PersonaModel
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

                    ciudad = new CiudadModel
                    {

                        idCiudad = reader.GetInt32("idCiudad"),
                        ciudad = reader.GetString("ciudad"),
                        departamento = reader.GetString("departamento"),
                        codigo_postal = reader.GetInt32("codigo_postal")
                    }
                };
            }
            return null;
        }

        public List<PersonaModel> obtenerPersonas()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT p.* , c.* FROM persona p inner join ciudad c on c.\"idCiudad\" = p.\"idCiudad\"", conn);
            List<PersonaModel> personas = new List<PersonaModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    personas.Add(new PersonaModel
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

                        ciudad = new CiudadModel
                        {

                            idCiudad = reader.GetInt32("idCiudad"),
                            ciudad = reader.GetString("ciudad"),
                            departamento = reader.GetString("departamento"),
                            codigo_postal = reader.GetInt32("codigo_postal")
                        }

                    });
                }
            }

            return personas;
        }


        public void insertarPersona(PersonaInsertModel persona)
        {
            var conn = ConexionDB.GetConexion();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Insertar la persona en la tabla de personas
                    var insertPersonaCommand = new Npgsql.NpgsqlCommand("INSERT INTO public.persona (nombre, apellido, \"nroDocumento\", \"idCiudad\", " +
                        "\"tipoDocumento\", direccion, celular, email, estado) VALUES (@nombre, @apellido, @nroDocumento," + 
                        " @idCiudad, @tipoDocumento, @direccion, @celular, @email, @estado);", conn);
                    insertPersonaCommand.Parameters.AddWithValue("@nombre", persona.nombre);
                    insertPersonaCommand.Parameters.AddWithValue("@apellido", persona.apellido);
                    insertPersonaCommand.Parameters.AddWithValue("@nroDocumento", persona.nroDocumento);
                    insertPersonaCommand.Parameters.AddWithValue("@idCiudad", persona.IdCiudad);
                    insertPersonaCommand.Parameters.AddWithValue("@tipoDocumento", persona.tipoDocumento);
                    insertPersonaCommand.Parameters.AddWithValue("@direccion", persona.direccion);
                    insertPersonaCommand.Parameters.AddWithValue("@celular", persona.celular);
                    insertPersonaCommand.Parameters.AddWithValue("@email", persona.email);
                    insertPersonaCommand.Parameters.AddWithValue("@estado", persona.estado);
                    Console.WriteLine(insertPersonaCommand.ToString());
                    
                    insertPersonaCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // En caso de error, puedes manejarlo apropiadamente, como hacer un rollback de la transacción
                    transaction.Rollback();
                    throw ex; // Lanzar la excepción para que sea manejada en el código que llama a este método
                }
            }
        }


        public void ActualizarPersona(PersonaInsertModel persona)
        {
            var conn = ConexionDB.GetConexion();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Actualizar la persona en la tabla de personas
                    var updatePersonaSql = "UPDATE persona SET nombre = @nombre, apellido = @apellido, \"tipoDocumento\" = @tipoDocumento, \"nroDocumento\" = @nroDocumento, direccion = @direccion, email = @email, celular = @celular, estado = @estado, \"idCiudad\" = @idCiudad WHERE  \"idPersona\" = @idPersona;";

                    using (var updatePersonaCommand = new Npgsql.NpgsqlCommand(updatePersonaSql, conn))
                    {
                        updatePersonaCommand.Parameters.AddWithValue("@idPersona", persona.idPersona);
                        updatePersonaCommand.Parameters.AddWithValue("@nombre", persona.nombre);
                        updatePersonaCommand.Parameters.AddWithValue("@apellido", persona.apellido);
                        updatePersonaCommand.Parameters.AddWithValue("@tipoDocumento", persona.tipoDocumento);
                        updatePersonaCommand.Parameters.AddWithValue("@nroDocumento", persona.nroDocumento);
                        updatePersonaCommand.Parameters.AddWithValue("@direccion", persona.direccion);
                        updatePersonaCommand.Parameters.AddWithValue("@email", persona.email);
                        updatePersonaCommand.Parameters.AddWithValue("@celular", persona.celular);
                        updatePersonaCommand.Parameters.AddWithValue("@estado", persona.estado);
                        updatePersonaCommand.Parameters.AddWithValue("@idCiudad", persona.IdCiudad);

                        updatePersonaCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // En caso de error, puedes manejarlo apropiadamente, como hacer un rollback de la transacción
                    transaction.Rollback();
                    throw ex; // Lanzar la excepción para que sea manejada en el código que llama a este método
                }
            }
        }


        public void eliminarPersona(int id)
        {
            var conn = ConexionDB.GetConexion();
            var comando = new Npgsql.NpgsqlCommand($"DELETE FROM persona WHERE \"idPersona\" = {id}", conn);

            comando.ExecuteNonQuery();
        }


    }


}
