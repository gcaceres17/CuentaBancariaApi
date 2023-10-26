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
   public class CiudadDatos
    {
        private ConexionDB ConexionDB;
        public CiudadDatos(String cadenaConexion)
        {
            ConexionDB = new ConexionDB(cadenaConexion);
        }

        public List<CiudadModel> obtenerCiudades()
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT * FROM public.ciudad", conn);

            List<CiudadModel> ciudades = new List<CiudadModel>();

            using (var reader = ps.ExecuteReader())
            {
                while (reader.Read())
                {
                    ciudades.Add(new CiudadModel
                    {
                        idCiudad = reader.GetInt32("idCiudad"),
                        ciudad = reader.GetString("ciudad"),
                        departamento = reader.GetString("departamento"),
                        codigo_postal = reader.GetInt32("codigo_postal")
                    });
                }
            }

            return ciudades;
        }

        public CiudadModel obtenerCiudadPorId(int id)
        {
            var conn = ConexionDB.GetConexion();
            var ps = new Npgsql.NpgsqlCommand($"SELECT * FROM public.ciudad where \"idCiudad\" = {id};", conn);
            using var reader = ps.ExecuteReader();
            if (reader.Read())
            {
                return new CiudadModel
                {
                    idCiudad = reader.GetInt32("idCiudad"),
                    ciudad = reader.GetString("ciudad"),
                    departamento = reader.GetString("departamento"),
                    codigo_postal = reader.GetInt32("codigo_postal")
                };
            }
            return null;
        }

        public void insertarCiudad(CiudadModel ciudad)
        {
            var conn = ConexionDB.GetConexion();
            var comando = new Npgsql.NpgsqlCommand("INSERT INTO ciudad(ciudad, departamento, codigo_postal)" +
                                                "VALUES( @ciudad, @departamento, @codigo_postal)", conn);
            comando.Parameters.AddWithValue("ciudad", ciudad.ciudad);
            comando.Parameters.AddWithValue("departamento", ciudad.departamento);
            comando.Parameters.AddWithValue("codigo_postal", ciudad.codigo_postal);

            comando.ExecuteNonQuery();
        }

        public void modificarCiudad(CiudadModel ciudad)
        {
            var conn = ConexionDB.GetConexion();
            var comando = new Npgsql.NpgsqlCommand($"UPDATE ciudad SET ciudad = '{ciudad.ciudad}', " +
                                                          $"departamento = '{ciudad.departamento}', " +
                                                          $"estado = '{ciudad.codigo_postal}' " +
                                                $" WHERE \"idCiudad\" = {ciudad.idCiudad}", conn);

            comando.ExecuteNonQuery();
        }

        public void eliminarCiudad(int id)
        {
            var conn = ConexionDB.GetConexion();
            var comando = new Npgsql.NpgsqlCommand($"DELETE FROM ciudad WHERE \"idCiudad\" = {id}", conn);

            comando.ExecuteNonQuery();
        }
    }
}
