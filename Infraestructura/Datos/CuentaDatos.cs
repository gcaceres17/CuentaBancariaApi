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
    public class CuentaDatos
    {
        private ConexionDB ConexionDB;
        public CuentaDatos(String cadenaConexion)
        {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public List<CuentaModel> obtenerCuentas()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from cuentas p join cliente c on p.\"idCliente\" = c.\"idCliente\"", conn);
            List<CuentaModel> cuentas = new List<CuentaModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    cuentas.Add(new CuentaModel
                    {
                        idCuenta = reader.GetInt32("idCuenta"),
                        nroCuenta = reader.GetString("nroCuenta"),
                        nroContrato = reader.GetString("nroContrato"),
                        fechaAlta = reader.GetDateTime("fechaAlta"),
                        tipoCuenta = reader.GetString("tipoCuenta"),
                        saldo = reader.GetDouble("saldo"),
                        costoMantenimiento = reader.GetDouble("costoMantenimiento"),
                        PromedioAcreditacion = reader.GetString("PromedioAcreditacion"),
                        moneda = reader.GetString("moneda"),
                        estado = reader.GetString("estado"),

                        cliente = new ClienteInsertModel
                        {
                            idCliente = reader.GetInt32("idCliente"),
                            fechaIngreso = reader.GetDateTime("fechaIngreso"),
                            calificacion = reader.GetString("calificacion"),
                            estado = reader.GetString("estado"),
                            IdPersona = reader.GetInt32("idPersona"),

                        },


                    });
                }
            }

            return cuentas;
        }

        public CuentaModel obtenerCuentasPorId(string nroCuenta)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from cuentas p join cliente c on p.\"idCliente\" = c.\"idCliente\" where \"nroCuenta\" = '{nroCuenta}'", conn);

            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new CuentaModel
                {
                    idCuenta = reader.GetInt32("idCuenta"),
                    nroCuenta = reader.GetString("nroCuenta"),
                    nroContrato = reader.GetString("nroContrato"),
                    fechaAlta = reader.GetDateTime("fechaAlta"),
                    tipoCuenta = reader.GetString("tipoCuenta"),
                    saldo = reader.GetDouble("saldo"),
                    costoMantenimiento = reader.GetDouble("costoMantenimiento"),
                    PromedioAcreditacion = reader.GetString("PromedioAcreditacion"),
                    moneda = reader.GetString("moneda"),
                    estado = reader.GetString("estado"),

                    cliente = new ClienteInsertModel
                    {
                        idCliente = reader.GetInt32("idCliente"),
                        fechaIngreso = reader.GetDateTime("fechaIngreso"),
                        calificacion = reader.GetString("calificacion"),
                        estado = reader.GetString("estado"),
                        IdPersona = reader.GetInt32("idPersona"),

                    },
                };
            }
            return null;
        }

        public void InsertarCuenta(CuentaInserModel cuenta)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Insertar la cuenta en la tabla de cuentas
                    var insertCuentaSql = "INSERT INTO cuentas( \"idCliente\", \"nroCuenta\", \"fechaAlta\", \"tipoCuenta\", saldo, \"nroContrato\", \"costoMantenimiento\", \"PromedioAcreditacion\", moneda, estado) " +
                                          "VALUES (@idCuenta, @idCliente, @nroCuenta, @fechaAlta, @tipoCuenta, @saldo, @nroContrato, @costoMantenimiento, @PromedioAcreditacion, @moneda, @estado);";

                    using (var insertCuentaCommand = new Npgsql.NpgsqlCommand(insertCuentaSql, conn))
                    {
                        
                        insertCuentaCommand.Parameters.AddWithValue("@idCliente", cuenta.idCliente);
                        insertCuentaCommand.Parameters.AddWithValue("@nroCuenta", cuenta.nroCuenta);
                        insertCuentaCommand.Parameters.AddWithValue("@fechaAlta", DateTime.Now); 
                        insertCuentaCommand.Parameters.AddWithValue("@tipoCuenta", cuenta.tipoCuenta);
                        insertCuentaCommand.Parameters.AddWithValue("@saldo", cuenta.saldo);
                        insertCuentaCommand.Parameters.AddWithValue("@nroContrato", cuenta.nroContrato);
                        insertCuentaCommand.Parameters.AddWithValue("@costoMantenimiento", cuenta.costoMantenimiento);
                        insertCuentaCommand.Parameters.AddWithValue("@PromedioAcreditacion", cuenta.PromedioAcreditacion);
                        insertCuentaCommand.Parameters.AddWithValue("@moneda", cuenta.moneda);
                        insertCuentaCommand.Parameters.AddWithValue("@estado", cuenta.estado);

                        insertCuentaCommand.ExecuteNonQuery();
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

        public void ActualizarCuenta(CuentaInserModel cuenta)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Actualizar la cuenta en la tabla de cuentas
                    var updateCuentaSql = "UPDATE cuentas SET \"idCliente\" = @idCliente, \"nroCuenta\" = @nroCuenta, " +
                                          "\"tipoCuenta\" = @tipoCuenta, saldo = @saldo, \"nroContrato\" = @nroContrato, " +
                                          "\"costoMantenimiento\" = @costoMantenimiento, \"PromedioAcreditacion\" = @PromedioAcreditacion, " +
                                          "moneda = @moneda, estado = @estado WHERE \"idCuenta\" = @idCuenta;";

                    using (var updateCuentaCommand = new Npgsql.NpgsqlCommand(updateCuentaSql, conn))
                    {
                        updateCuentaCommand.Parameters.AddWithValue("@idCuenta", cuenta.idCuenta);
                        updateCuentaCommand.Parameters.AddWithValue("@idCliente", cuenta.idCliente);
                        updateCuentaCommand.Parameters.AddWithValue("@nroCuenta", cuenta.nroCuenta);
                        updateCuentaCommand.Parameters.AddWithValue("@tipoCuenta", cuenta.tipoCuenta);
                        updateCuentaCommand.Parameters.AddWithValue("@saldo", cuenta.saldo);
                        updateCuentaCommand.Parameters.AddWithValue("@nroContrato", cuenta.nroContrato);
                        updateCuentaCommand.Parameters.AddWithValue("@costoMantenimiento", cuenta.costoMantenimiento);
                        updateCuentaCommand.Parameters.AddWithValue("@PromedioAcreditacion", cuenta.PromedioAcreditacion);
                        updateCuentaCommand.Parameters.AddWithValue("@moneda", cuenta.moneda);
                        updateCuentaCommand.Parameters.AddWithValue("@estado", cuenta.estado);

                        updateCuentaCommand.ExecuteNonQuery();
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

        public void EliminarCuenta(int idCuenta)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Eliminar la cuenta de la tabla de cuentas
                    var deleteCuentaSql = $"DELETE FROM cuentas WHERE \"idCuenta\" = {idCuenta};";

                    using (var deleteCuentaCommand = new Npgsql.NpgsqlCommand(deleteCuentaSql, conn))
                    {
                        deleteCuentaCommand.ExecuteNonQuery();
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
