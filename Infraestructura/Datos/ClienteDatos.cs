using Infraestructura.Conexiones;
using Infraestructura.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Datos
{
    public class ClienteDatos
    {
        private ConexionDB ConexionDB;
        public ClienteDatos(String cadenaConexion)
        {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public List<ClienteModel> obtenerClientes()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from persona p join cliente c on p.\"idPersona\" = c.\"idPersona\" join ciudad r on r.\"idCiudad\" = p.\"idCiudad\"", conn);
            List<ClienteModel> clientes = new List<ClienteModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientes.Add(new ClienteModel
                    {

                        idCliente = reader.GetInt32("idCliente"),
                        fechaIngreso = reader.GetDateTime("fechaIngreso"),
                        calificacion = reader.GetString("calificacion"),
                        estado = reader.GetString("estado"),

                        persona = new PersonaInsertModel {
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

            return clientes;
        }

        public ClienteModel obtenerClientePorId(string documento)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from persona p join cliente c on p.\"idPersona\" = c.\"idPersona\" where p.\"nroDocumento\" = '{documento}';", conn);

            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new ClienteModel
                {
                    idCliente = reader.GetInt32("idCliente"),
                    fechaIngreso = reader.GetDateTime("fechaIngreso"),
                    calificacion = reader.GetString("calificacion"),
                    estado = reader.GetString("estado"),

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

        public void insertarCliente(ClienteInsertModel ciente)
        {
            var conn = ConexionDB.GetConexion();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Insertar la persona en la tabla de personas
                    var inserClienteCommand = new Npgsql.NpgsqlCommand("INSERT INTO public.cliente(" +
                        " \"idPersona\", calificacion, estado)" +
                        "VALUES (@idPersona ,@calificacion, @estado);", conn);
                    inserClienteCommand.Parameters.AddWithValue("@idPersona", ciente.IdPersona);
                    inserClienteCommand.Parameters.AddWithValue("@calificacion", ciente.calificacion);
                    inserClienteCommand.Parameters.AddWithValue("@estado", ciente.estado);

                    inserClienteCommand.ExecuteNonQuery();

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

        public void ActualizarCliente(ClienteInsertModel cliente)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Actualizar el cliente en la tabla de clientes
                    var updateClienteSql = "UPDATE cliente SET \"idPersona\" = @idPersona, calificacion = @calificacion, estado = @estado WHERE \"idCliente\" = @idCliente;";

                    using (var updateClienteCommand = new Npgsql.NpgsqlCommand(updateClienteSql, conn))
                    {
                        updateClienteCommand.Parameters.AddWithValue("@idCliente", cliente.idCliente);
                        updateClienteCommand.Parameters.AddWithValue("@idPersona", cliente.IdPersona);
                        updateClienteCommand.Parameters.AddWithValue("@calificacion", cliente.calificacion);
                        updateClienteCommand.Parameters.AddWithValue("@estado", cliente.estado);

                        updateClienteCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // En caso de error, puedes registrar el error o lanzar una excepción personalizada si lo deseas
                    throw ex; // Lanzar la excepción para que sea manejada en el código que llama a este método
                }
            }
        }

        public void EliminarCliente(int id)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Eliminar el cliente de la tabla de clientes
                    var deleteClienteSql = $"DELETE FROM cliente WHERE \"idCliente\" = {id};";

                    using (var deleteClienteCommand = new Npgsql.NpgsqlCommand(deleteClienteSql, conn))
                    {
                        deleteClienteCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // En caso de error, puedes registrar el error o lanzar una excepción personalizada si lo deseas
                    throw ex; // Lanzar la excepción para que sea manejada en el código que llama a este método
                }
            }
        }


    }
}
