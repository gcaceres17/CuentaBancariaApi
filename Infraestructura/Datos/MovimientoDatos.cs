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
    public class MovimientoDatos
    {
        private ConexionDB ConexionDB;
        public MovimientoDatos(String cadenaConexion)
        {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public List<MovimientosModel> obtenerMovimientos()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from cuentas p join \"Movimientos\" c on p.\"idCuenta\" = c.\"idCuenta\"", conn);
            List<MovimientosModel> mov = new List<MovimientosModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    mov.Add(new MovimientosModel
                    {

                        idMovimiento = reader.GetInt32("idMovimiento"),
                        fechaMovimiento = reader.GetDateTime("fechaMovimiento"),
                        saldoActual = reader.GetDouble("saldoActual"),
                        saldoAnterior = reader.GetDouble("saldoAnterior"),
                        tipoMovimiento = reader.GetString("tipoMovimiento"),
                        MontoMovimiento = reader.GetDouble("MontoMovimiento"),
                        cuentaOrigen = reader.GetInt32("cuentaOrigen"),
                        cuentaDestino = reader.GetInt32("cuentaDestino"),

                        cuenta = new CuentaInserModel
                        {
                            idCuenta = reader.GetInt32("idCuenta"),
                            idCliente = reader.GetInt32("idCliente"),
                            nroCuenta = reader.GetString("nroCuenta"),
                            nroContrato = reader.GetString("nroContrato"),
                            fechaAlta = reader.GetDateTime("fechaAlta"),
                            tipoCuenta = reader.GetString("tipoCuenta"),
                            saldo = reader.GetDouble("saldo"),
                            costoMantenimiento = reader.GetDouble("costoMantenimiento"),
                            PromedioAcreditacion = reader.GetString("PromedioAcreditacion"),
                            moneda = reader.GetString("moneda"),
                            estado = reader.GetString("estado"),

                        },


                    });
                }
            }

            return mov;
        }

        public MovimientosModel obtenerMovimientosPorId(string nroCuenta)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"Select p.*, c.* from cuentas p join \"Movimientos\" c on p.\"idCuenta\" = c.\"idCuenta\" where \"nroCuenta\" = '{nroCuenta}'", conn);

            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new MovimientosModel
                {

                    idMovimiento = reader.GetInt32("idMovimiento"),
                    fechaMovimiento = reader.GetDateTime("fechaMovimiento"),
                    saldoActual = reader.GetDouble("saldoActual"),
                    saldoAnterior = reader.GetDouble("saldoAnterior"),
                    tipoMovimiento = reader.GetString("tipoMovimiento"),
                    MontoMovimiento = reader.GetDouble("MontoMovimiento"),
                    cuentaOrigen = reader.GetInt32("cuentaOrigen"),
                    cuentaDestino = reader.GetInt32("cuentaDestino"),

                    cuenta = new CuentaInserModel
                    {
                        idCuenta = reader.GetInt32("idCuenta"),
                        idCliente = reader.GetInt32("idCliente"),
                        nroCuenta = reader.GetString("nroCuenta"),
                        nroContrato = reader.GetString("nroContrato"),
                        fechaAlta = reader.GetDateTime("fechaAlta"),
                        tipoCuenta = reader.GetString("tipoCuenta"),
                        saldo = reader.GetDouble("saldo"),
                        costoMantenimiento = reader.GetDouble("costoMantenimiento"),
                        PromedioAcreditacion = reader.GetString("PromedioAcreditacion"),
                        moneda = reader.GetString("moneda"),
                        estado = reader.GetString("estado"),

                    },
                };
            }
            return null;
        }

        public void InsertarMovimiento(MovimientosInsertModel movimiento)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Insertar el movimiento en la tabla de Movimientos
                    var insertMovimientoSql = "INSERT INTO \"Movimientos\"( \"idCuenta\", \"fechaMovimiento\", \"tipoMovimiento\", \"saldoAnterior\", \"saldoActual\", \"montoMovimiento\", \"cuentaOrigen\", \"cuentaDestino\", canal) " +
                                              "VALUES ( @idCuenta, @fechaMovimiento, @tipoMovimiento, @saldoAnterior, @saldoActual, @montoMovimiento, @cuentaOrigen, @cuentaDestino, @canal);";

                    using (var insertMovimientoCommand = new Npgsql.NpgsqlCommand(insertMovimientoSql, conn))
                    {
                        
                        insertMovimientoCommand.Parameters.AddWithValue("@idCuenta", movimiento.idCuenta);
                        insertMovimientoCommand.Parameters.AddWithValue("@fechaMovimiento", movimiento.fechaMovimiento);
                        insertMovimientoCommand.Parameters.AddWithValue("@tipoMovimiento", movimiento.tipoMovimiento);
                        insertMovimientoCommand.Parameters.AddWithValue("@saldoAnterior", movimiento.saldoAnterior);
                        insertMovimientoCommand.Parameters.AddWithValue("@saldoActual", movimiento.saldoActual);
                        insertMovimientoCommand.Parameters.AddWithValue("@montoMovimiento", movimiento.MontoMovimiento);
                        insertMovimientoCommand.Parameters.AddWithValue("@cuentaOrigen", movimiento.cuentaOrigen);
                        insertMovimientoCommand.Parameters.AddWithValue("@cuentaDestino", movimiento.cuentaDestino);
                        insertMovimientoCommand.Parameters.AddWithValue("@canal", movimiento.canal);

                        insertMovimientoCommand.ExecuteNonQuery();
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

        public void ActualizarMovimiento(MovimientosInsertModel movimiento)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Actualizar el movimiento en la tabla de Movimientos
                    var updateMovimientoSql = "UPDATE \"Movimientos\" SET \"idCuenta\" = @idCuenta, \"fechaMovimiento\" = @fechaMovimiento, " +
                                              "\"tipoMovimiento\" = @tipoMovimiento, \"saldoAnterior\" = @saldoAnterior, \"saldoActual\" = @saldoActual, " +
                                              "\"montoMovimiento\" = @montoMovimiento, \"cuentaOrigen\" = @cuentaOrigen, \"cuentaDestino\" = @cuentaDestino, " +
                                              "canal = @canal WHERE \"idMovimiento\" = @idMovimiento;";

                    using (var updateMovimientoCommand = new Npgsql.NpgsqlCommand(updateMovimientoSql, conn))
                    {
                        updateMovimientoCommand.Parameters.AddWithValue("@idMovimiento", movimiento.idMovimiento);
                        updateMovimientoCommand.Parameters.AddWithValue("@idCuenta", movimiento.idCuenta);
                        updateMovimientoCommand.Parameters.AddWithValue("@fechaMovimiento", movimiento.fechaMovimiento);
                        updateMovimientoCommand.Parameters.AddWithValue("@tipoMovimiento", movimiento.tipoMovimiento);
                        updateMovimientoCommand.Parameters.AddWithValue("@saldoAnterior", movimiento.saldoAnterior);
                        updateMovimientoCommand.Parameters.AddWithValue("@saldoActual", movimiento.saldoActual);
                        updateMovimientoCommand.Parameters.AddWithValue("@montoMovimiento", movimiento.MontoMovimiento);
                        updateMovimientoCommand.Parameters.AddWithValue("@cuentaOrigen", movimiento.cuentaOrigen);
                        updateMovimientoCommand.Parameters.AddWithValue("@cuentaDestino", movimiento.cuentaDestino);
                        updateMovimientoCommand.Parameters.AddWithValue("@canal", movimiento.canal);

                        updateMovimientoCommand.ExecuteNonQuery();
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


        public void EliminarMovimiento(int idMovimiento)
        {
            using (var conn = ConexionDB.GetConexion())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // Eliminar el movimiento de la tabla de Movimientos
                    var deleteMovimientoSql = "DELETE FROM \"Movimientos\" WHERE \"idMovimiento\" = @idMovimiento;";

                    using (var deleteMovimientoCommand = new Npgsql.NpgsqlCommand(deleteMovimientoSql, conn))
                    {
                        deleteMovimientoCommand.Parameters.AddWithValue("@idMovimiento", idMovimiento);
                        deleteMovimientoCommand.ExecuteNonQuery();
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
